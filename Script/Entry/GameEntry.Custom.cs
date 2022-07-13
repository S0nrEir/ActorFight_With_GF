using Aquila.Extension;

namespace Aquila
{
    /// <summary>
    /// 自定义组件游戏入口
    /// </summary>
    public partial class GameEntry
    {
        public static LuBanCompoent DataTable
        {
            get;
            private set;
        }

        /// <summary>
        /// 计时器组件
        /// </summary>
        public static TimerComponent Timer
        {
            get;
            private set;
        }

        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private static void InitCustomComponents()
        {
            Timer = UnityGameFramework.Runtime.GameEntry.GetComponent<TimerComponent>();
            DataTable = UnityGameFramework.Runtime.GameEntry.GetComponent<LuBanCompoent>();
        }
    }
}
