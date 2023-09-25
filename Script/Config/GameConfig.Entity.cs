using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// 实体配置相关
        /// </summary>
        public static class Entity
        {
            /// <summary>
            /// 英雄类actor组
            /// </summary>
            public const string GROUP_HERO_ACTOR = "Hero";

            /// <summary>
            /// 投射物
            /// </summary>
            public const string GROUP_PROJECTILE = "Projectile";

            /// <summary>
            /// 特效entity组
            /// </summary>
            public const string GROUP_FX = "FX";

            /// <summary>
            /// actor 优先级
            /// </summary>
            public const int PRIORITY_ACTOR = 1;

            /// <summary>
            /// 特效 优先级
            /// </summary>
            public const int PRIORITY_FX = 3;


        }
    }
}