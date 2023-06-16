using Cfg.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Impact
{
    public partial class Component_Impact
    {
        /// <summary>
        /// 被动效果数据
        /// </summary>
        internal struct ImpactData
        {
            /// <summary>
            /// 施加者ActorID
            /// </summary>
            public int _castorActorID;

            /// <summary>
            /// 目标castorID
            /// </summary>
            public int _targetActorID;

            /// <summary>
            /// effect实例索引ID
            /// </summary>
            public int _effectIndex;

            /// <summary>
            /// 生效周期(多长时间生效一次)
            /// </summary>
            public float _period;

            /// <summary>
            /// 持续时长
            /// </summary>
            public float _duration;

            /// <summary>
            /// 生效策略（周期or持续）
            /// </summary>
            public DurationPolicy _policy;

            /// <summary>
            /// 施加后立生效
            /// </summary>
            public bool _effectOnAwake;
        }
    }
}
