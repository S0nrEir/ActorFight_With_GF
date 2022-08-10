using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// ʵ���������
        /// </summary>
        public static class Entity
        {
            /// <summary>
            /// Ӣ����actor��
            /// </summary>
            public const string GROUP_HeroActor = "HeroActor";

            /// <summary>
            /// ��Ч��actor��
            /// </summary>
            public const string GROUP_ActorEffect = "ActorEffect";

            /// <summary>
            /// Ͷ����
            /// </summary>
            public const string GROUP_Projectile = "ProjectileActor";

            /// <summary>
            /// Trigger��actor��
            /// </summary>
            public const string GROUP_Trigger = "TriggerActor";

            /// <summary>
            /// ����
            /// </summary>
            public const string GROUP_Other = "Other";

            /// <summary>
            /// actor
            /// </summary>
            public const int Priority_Actor = 1;

            /// <summary>
            /// ��Ч
            /// </summary>
            public const int Priority_Effect = 2;
        }
    }
}