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
        /// 无效
        /// </summary>
        INVALID,
    }
}