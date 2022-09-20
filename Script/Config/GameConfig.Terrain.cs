using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// 地块相关配置
        /// </summary>
        public static class Terrain
        {
            /// <summary>
            /// 地块物体挂点位置世界坐标偏移
            /// </summary>
            public static readonly Vector3 TERRAIN_HANG_POINT_OFFSET = new Vector3( 0, 0.9f, 0 );
        }
    }
}
