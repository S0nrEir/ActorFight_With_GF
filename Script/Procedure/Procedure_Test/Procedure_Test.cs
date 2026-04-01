using GameFramework.Fsm;
using GameFramework.Procedure;

namespace Aquila.Procedure
{
    /// <summary>
    /// 测试流程
    /// </summary>
    public partial class Procedure_Test : ProcedureBase
    {
        protected override void OnUpdate( IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds )
        {
            //base.OnUpdate( procedureOwner, elapseSeconds, realElapseSeconds );
            //RunInput();
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            //ImpactTest();
            // LoadScript();
            //InputTest();
            //TimeWheelTest();
            TestAsync();
        }
    }

}
