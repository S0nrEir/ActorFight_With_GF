﻿using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 进入游戏流程
    /// </summary>
    public class Procedure_Enter : ProcedureBase
    {
        protected override void OnInit( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnInit( procedureOwner );
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            Log.Info( "Procedure--->OnEnter", LogColorTypeEnum.White );
            ChangeState<Procedure_Prelaod>( procedureOwner );
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            base.OnLeave( procedureOwner, isShutdown );
        }
    }

}