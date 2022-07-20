namespace Aquila.Module
{
    /// <summary>
    /// 战斗模块
    /// </summary>
    public class Module_Fight : GameFrameworkModuleBase, IUpdate
    {
        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            _fight_flag = true;
            _actor_module = GameFrameworkModule.GetModule<Module_Actor>();
        }

        public override void OnClose()
        {
            _fight_flag = false;
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            _fight_flag = false;
        }


        /// <summary>
        /// 刷帧处理选定逻辑
        /// </summary>
        public void OnUpdate( float deltaTime )
        {

        }



        /// <summary>
        /// actor模块
        /// </summary>
        private Module_Actor _actor_module = null;

        /// <summary>
        /// 开始标记
        /// </summary>
        private bool _fight_flag = false;
    }

}
