#if UNITY_EDITOR

using Aquial.UI;
using Aquila.AbilityEditor;
using Aquila.Fight.Addon;
using Aquila.Module;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using Form_AbilitySandBox = Aquila.AbilityEditor.Form_AbilitySandBox;
using Module_ActorMgr = Aquila.Module.Module_ActorMgr;

namespace Aquila.Procedure
{
    public class Procedure_RunningAbilityEditorSandBox : ProcedureBase
    {
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            var playerIdVar = procedureOwner.GetData(Misc.KEY_PLAYER_ENTITY_ID) as UnityGameFramework.Runtime.VarInt32;
            var dummyIdVar  = procedureOwner.GetData(Misc.KEY_DUMMY_ENTITY_ID) as UnityGameFramework.Runtime.VarInt32;

            if (playerIdVar == null || dummyIdVar == null)
            {
                UnityGameFramework.Runtime.Log.Error("[RunningAbilityEditorSandBox] actor ID data not found in FSM");
                return;
            }

            int playerEntityId = playerIdVar.Value;
            int dummyEntityId  = dummyIdVar.Value;

            ReferencePool.Release(playerIdVar);
            ReferencePool.Release(dummyIdVar);
            procedureOwner.RemoveData(Misc.KEY_PLAYER_ENTITY_ID);
            procedureOwner.RemoveData(Misc.KEY_DUMMY_ENTITY_ID);

            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            _playerInstance = actorMgr.Get(playerEntityId);
            _dummyInstance  = actorMgr.Get(dummyEntityId);

            if (_playerInstance == null)
            {
                UnityGameFramework.Runtime.Log.Error($"[RunningAbilityEditorSandBox] player instance not found, id={playerEntityId}");
                return;
            }

            if (GameEntry.AbilityEditorSandBox.TryGetSandBoxAbility(out var abilityData))
            {
                var abilityAddon = _playerInstance.GetAddon<Addon_Ability>();
                if (abilityAddon != null)
                    abilityAddon.GiveAbility(abilityData);
                else
                    UnityGameFramework.Runtime.Log.Error("[RunningAbilityEditorSandBox] player has no Addon_Ability");
            }
            else
            {
                UnityGameFramework.Runtime.Log.Error("[RunningAbilityEditorSandBox] no sandbox ability data found");
            }

            GameEntry.UI.OpenForm(FormIdEnum.AbilitySandBoxForm, new Form_AbilitySandBox.AbilitySandBoxForm_Param());
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            _playerInstance = null;
            _dummyInstance  = null;
        }

        private Module_ProxyActor.ActorInstance _playerInstance = null;
        private Module_ProxyActor.ActorInstance _dummyInstance  = null;
    }
}
#endif