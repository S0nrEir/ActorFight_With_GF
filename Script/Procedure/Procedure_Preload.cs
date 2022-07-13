using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 预加载流程
    /// </summary>
    public class Procedure_Prelaod : ProcedureBase
    {
        protected override void OnInit( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnInit( procedureOwner );
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            //加载数据表
            if ( !GameEntry.DataTable.LoadDataTable() )
                Log.Error( "load data table faild!" );


        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            base.OnLeave( procedureOwner, isShutdown );
        }
    }

}
