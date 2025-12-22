#if UNITY_EDITOR

using Aquila.AbilityEditor;
using Aquila.Module;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
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

            // _playerInstance = GameEntry.Module.GetModule<Module_ActorMgr>().Get(Misc.PLYAER_META_ROLE_ID);
            // _dummyInstance = GameEntry.Module.GetModule<Module_ActorMgr>().Get(Misc.DUMMY_META_ROLE_ID);
            // GameEntry.UI.OpenForm();
            GameEntry.AbilityEditorSandBox.Init();
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
        
        // private Module_ProxyActor.ActorInstance _playerInstance = null;
        // private Module_ProxyActor.ActorInstance _dummyInstance = null;
    }
}
#endif