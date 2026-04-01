using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// 模块配置
        /// </summary>
        public static class Misc
        {
            /// <summary>
            /// 预加载的form配置
            /// </summary>
            public static readonly string[] DataTableConfigs = new string[]
            {
                "UIForm",
                "Preload"
            };
        }
    }
}