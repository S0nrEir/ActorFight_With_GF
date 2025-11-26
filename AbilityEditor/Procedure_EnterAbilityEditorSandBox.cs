#if UNITY_EDITOR

using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

namespace Aquila.Procedure
{
    /// <summary>
    /// 进入技能编辑器沙盒流程
    /// </summary>
    public class Procedure_EnterAbilityEditorSandBox : ProcedureBase
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
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        /// 切换到运行沙盒流程
        /// </summary>
        public void SwitchToRunningState(IFsm<IProcedureManager> procedureOwner)
        {
            ChangeState<Procedure_RunningAbilityEditorSandBox>(procedureOwner);
        }
    }
}
#endif