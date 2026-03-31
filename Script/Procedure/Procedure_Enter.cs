using GameFramework.Fsm;
using GameFramework.Procedure;

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

            //#todo:改掉procedure preload
            // if ( Procedure_PreloadResource.StartWith<Procedure_Prelaod>( procedureOwner, _resourcePreloadPathList ) )
            // {
            //     ChangeState<Procedure_PreloadResource>( procedureOwner );
            //     return;
            // }

            ChangeState<Procedure_Prelaod>( procedureOwner );
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            base.OnLeave( procedureOwner, isShutdown );
        }

        private static readonly string[] _resourcePreloadPathList =
        {
            @"Assets/Res/Prefab/UI/Item/HPBar.prefab",
            @"Assets/Res/Prefab/UI/Item/DamageNumber.prefab"
        };
    }
}