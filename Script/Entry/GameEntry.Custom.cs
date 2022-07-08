using UnityGameFramework.Runtime;

namespace Aquila
{
    /// <summary>
    /// 自定义组件游戏入口
    /// </summary>
    public partial class GameEntry
    {
        /// <summary>
        /// 模块组件
        /// </summary>
        public static ModuleComponent Module { get; private set; }

        /// <summary>
        /// 初始化自定义组件
        /// </summary>
        private static void InitCustomComponents()
        {
            Module = UnityGameFramework.Runtime.GameEntry.GetComponent<ModuleComponent>();
        }
    }
}
