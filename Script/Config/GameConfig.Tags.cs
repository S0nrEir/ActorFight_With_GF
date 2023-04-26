using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// 全局配置Tags类
        /// </summary>
        public static class Tags
        {
            /// <summary>
            /// actor
            /// </summary>
            public static string ACTOR = "Actor";

            /// <summary>
            /// 战斗地块根节点
            /// </summary>
            public static string TERRAIN_ROOT = "TerrainRoot";

            /// <summary>
            /// 地块节点
            /// </summary>
            public static string TERRAIN_BLOCK = "TerrainBlock";
        }
    }

}