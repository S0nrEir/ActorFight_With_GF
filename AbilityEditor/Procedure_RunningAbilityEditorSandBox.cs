#if UNITY_EDITOR

using Aquila.UI;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
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

            var playerIdVar = procedureOwner.GetData(Procedure_EnterAbilityEditorSandBox.KEY_PLAYER_ENTITY_ID) as VarInt32;
            var dummyIdVar  = procedureOwner.GetData(Procedure_EnterAbilityEditorSandBox.KEY_DUMMY_ENTITY_ID) as VarInt32;

            if (playerIdVar == null || dummyIdVar == null)
            {
                Tools.Logger.Error("[RunningAbilityEditorSandBox] actor ID data not found in FSM");
                return;
            }

            int playerEntityId = playerIdVar.Value;
            int dummyEntityId  = dummyIdVar.Value;

            procedureOwner.RemoveData(Procedure_EnterAbilityEditorSandBox.KEY_PLAYER_ENTITY_ID);
            procedureOwner.RemoveData(Procedure_EnterAbilityEditorSandBox.KEY_DUMMY_ENTITY_ID);

            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            _playerInstance = actorMgr.Get(playerEntityId);
            _dummyInstance  = actorMgr.Get(dummyEntityId);

            if (_playerInstance == null)
            {
                Tools.Logger.Error($"[RunningAbilityEditorSandBox] player instance not found, id={playerEntityId}");
                return;
            }

            int sandBoxAbilityId = -1;
            if (GameEntry.AbilityEditorSandBox.TryGetSandBoxAbility(out var abilityData))
            {
                sandBoxAbilityId = abilityData.GetId();
                var abilityAddon = _playerInstance.GetAddon<Addon_Ability>();
                if (abilityAddon != null)
                    abilityAddon.GiveAbility(abilityData);
                else
                    Tools.Logger.Error("[RunningAbilityEditorSandBox] player has no Addon_Ability");
            }
            else
            {
                Tools.Logger.Error("[RunningAbilityEditorSandBox] no sandbox ability data found");
            }

            _playerInstance.Actor.SetWorldPosition( new Vector3( -4.4f, -5.782828f, 15.1f ) );
            _playerInstance.Actor.SetRotation( new Vector3( 0f, 295.011993f, 0f ) );

            if ( _dummyInstance != null )
            {
                _dummyInstance.Actor.SetWorldPosition( new Vector3( -21.6f, -5.782828f, 19.1f ) );
                _dummyInstance.Actor.SetRotation( new Vector3( 0f, 98.6860046f, 0f ) );
            }

            GameEntry.UI.OpenForm(FormIdEnum.AbilitySandBoxForm, new Form_AbilitySandBox.AbilitySandBoxForm_Param
            {
                _playerID = playerEntityId,
                _dummyID = dummyEntityId,
                _abilityID = sandBoxAbilityId
            });
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            _playerInstance = null;
            _dummyInstance  = null;
        }

        private Module_ProxyActor.ActorInstance _playerInstance;
        private Module_ProxyActor.ActorInstance _dummyInstance;
    }
}
#endif

