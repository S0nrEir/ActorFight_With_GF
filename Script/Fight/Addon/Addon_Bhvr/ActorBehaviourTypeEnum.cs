using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// actor行为枚举
    /// </summary>
    public enum ActorBehaviourTypeEnum
    {
        /// <summary>
        /// 死亡
        /// </summary>
        DIE,

        /// <summary>
        /// 使用技能
        /// </summary>
        ABILITY,

        /// <summary>
        /// 追踪行为，在生成后会控制actor自动朝目标transform行进
        /// </summary>
        TRACING_TRANSFORM,

        /// <summary>
        /// 向目标点行进行为
        /// </summary>
        TARGETING_POSITION,

        /// <summary>
        /// 无效
        /// </summary>
        INVALID,
    }
}