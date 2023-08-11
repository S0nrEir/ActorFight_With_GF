using Cfg.Enum;
using System;

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
            /// 该impact持有的effect实例的hash索引
            /// </summary>
            public int _effectHash;

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
            /// 经过时长
            /// </summary>
            public float _elapsed;

            /// <summary>
            /// 生效时长
            /// </summary>
            public float _interval;

            /// <summary>
            /// 施加后立生效
            /// </summary>
            public bool _effectOnAwake;

            /// <summary>
            /// 叠加层数
            /// </summary>
            public int _stackCount;

            /// <summary>
            /// 叠加层数上限
            /// </summary>
            public int _stackLimit;

            /// <summary>
            /// 覆盖时重置时间
            /// </summary>
            public bool _resetDurationWhenOverride;
        }
    }
}
