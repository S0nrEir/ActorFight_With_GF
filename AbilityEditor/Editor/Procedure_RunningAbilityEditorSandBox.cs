#if UNITY_EDITOR

using Aquial.UI;
using Aquila.AbilityEditor;
using Aquila.Module;
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

            
            GameEntry.UI.OpenForm(FormIdEnum.AbilitySandBoxForm,new Form_AbilitySandBox.AbilitySandBoxForm_Param());
            GameEntry.AbilityEditorSandBox.Init();
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
        
        private Module_ProxyActor.ActorInstance _playerInstance = null;
        private Module_ProxyActor.ActorInstance _dummyInstance = null;
    }
    
}
#endif