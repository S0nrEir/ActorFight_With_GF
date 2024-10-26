using Aquila.Extension;
using Aquila.Fight.Impact;

namespace Aquila
{
    /// <summary>
    /// 自定义组件游戏入口
    /// </summary>
    public partial class GameEntry
    {
        /// <summary>
        /// 数据表
        /// </summary>
        public static Component_LuBan LuBan
        {
            get;
            private set;
        }

        /// <summary>
        /// 计时器组件
        /// </summary>
        public static Component_Timer Timer
        {
            get;
            private set;
        }

        /// <summary>
        /// 脚本组件
        /// </summary>
        public static Component_Lua Lua
        {
            get;
            private set;
        }

        /// <summary>
        /// 模块组件
        /// </summary>
        public static Component_Module Module
        {
            get;
            private set;
        }

        /// <summary>
        /// 时间轮组件
        /// </summary>
        public static Component_TimeWheel TimeWheel
        {
            get;
            private set;
        }

        /// <summary>
        /// 信息板组件
        /// </summary>
        public static Component_InfoBoard InfoBoard
        {
            get;
            private set;
        }

        /// <summary>
        /// 全局实例和变量组件
        /// </summary>
        public static Component_GlobalVar GlobalVar
        {
            get;
            private set;
        }

        /// <summary>
        /// Timeline组件
        /// </summary>
        public static Component_Timeline Timeline
        {
            get;
            private set;
        }

        /// <summary>
        /// Impact组件
        /// </summary>
        public static Component_Impact Impact
        {
            get;
            private set;
        }

        /// <summary>
        /// UI组件
        /// </summary>
        public static Component_UI UI
        {
            get;
            private set;
        }

        public static Component_Async Async
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private static void InitCustomComponents()
        {
            Timer       = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_Timer>();
            LuBan       = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_LuBan>();
            Lua         = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_Lua>();
            Module      = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_Module>();
            //TimeWheel = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_TimeWheel>();
            InfoBoard   = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_InfoBoard>();
            GlobalVar   = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_GlobalVar>();
            Timeline    = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_Timeline>();
            Impact      = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_Impact>();
            UI          = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_UI>();
            Async       = UnityGameFramework.Runtime.GameEntry.GetComponent<Component_Async>();
        }
    }
}
