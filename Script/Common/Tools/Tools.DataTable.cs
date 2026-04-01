using Cfg.Single;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// 数据表工具类
        /// </summary>
        public static class Table
        {
            /// <summary>
            /// 获取SceneConfig表实例
            /// </summary>
            public static SceneConfig GetSceneConfig()
            {
                return GameEntry.LuBan.Tables.SceneConfig;
            }
        }
    }
}
