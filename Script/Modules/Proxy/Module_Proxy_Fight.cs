using Aquila.Extension;

namespace Aquila.Module
{

    /// <summary>
    /// 战斗代理组件
    /// </summary>
    public partial class Module_Proxy_Fight : GameFrameworkModuleBase
    {
        #region pub



        #endregion

        #region override

        public override void Start( object param )
        {
            base.Start( param );
            MgrStart();
        }

        public override void End()
        {
            MgrEnd();
            base.End();
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            MgrEnsureInit();
        }

        protected override bool Contains_Sub_Module => false;

        #endregion
    }

}
