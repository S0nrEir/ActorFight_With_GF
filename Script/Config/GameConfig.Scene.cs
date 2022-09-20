using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// 场景相关
        /// </summary>
        public class Scene
        {
            /// <summary>
            /// 主相机默认世界空间坐标位置//#todo解决excel中数据首字符不能为'-'的情况
            /// </summary>
            /// </summary>
            public static Vector3 MAIN_CAMERA_DEFAULT_POSITION { get; } = new Vector3( -2.75f, 5.95f, -3.91f );
        }
    }
}