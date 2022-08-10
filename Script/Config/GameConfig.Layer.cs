namespace Aquila.Config
{
    public partial class GameConfig
    {

        /// <summary>
        /// 层级配置相关
        /// </summary>
        public static class Layer
        {
            /// <summary>
            /// 地块层级
            /// </summary>
            public const string LAYER_TERRAIN_BLOCK = "TerrainBlock";

            /// <summary>
            /// floor层级
            /// </summary>
            public const string LAYER_NAME_FLOOR = "Floor";

            /// <summary>
            /// 默认层级
            /// </summary>
            public const string LAYER_NAME_DEFAULT = "Default";

            /// <summary>
            /// 后处理层级
            /// </summary>
            public const string LAYER_NAME_POST_PROCESSING = "PostProcessing";
        }
    }
}