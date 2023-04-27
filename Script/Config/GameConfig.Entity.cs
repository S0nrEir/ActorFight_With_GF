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
            public const string GROUP_HERO_ACTOR = "HeroActor";

            /// <summary>
            /// 特效类actor组
            /// </summary>
            public const string GROUP_ACTOR_FX = "ActorFX";

            /// <summary>
            /// 投射物
            /// </summary>
            public const string GROUP_PROJECTILE = "ProjectileActor";

            /// <summary>
            /// Trigger类actor组
            /// </summary>
            public const string GROUP_TRIGGER = "TriggerActor";

            /// <summary>
            /// 其他
            /// </summary>
            public const string GROUP_OTHER = "Other";

            /// <summary>
            /// actor
            /// </summary>
            public const int PRIORITY_ACTOR = 1;

            /// <summary>
            /// 特效
            /// </summary>
            public const int PRIORITY_FX = 2;


        }
    }
}