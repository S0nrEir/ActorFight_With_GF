using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// ActorEvent枚举
    /// </summary>
    public enum AddonEventTypeEnum
    {
        /// <summary>
        /// 使用技能
        /// </summary>
        USE_ABILITY,

        /// <summary>
        /// 取消行为
        /// </summary>
        CANCEL_BHVR,

        /// <summary>
        /// 抵达追踪目标
        /// </summary>
        TRACING_ARRIVE,

        /// <summary>
        /// 抵达目标点
        /// </summary>
        POSITION_ARRIVE,
    }
}
