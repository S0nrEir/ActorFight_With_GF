using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Aquila.Fight
{

    /// <summary>
    /// 技能结果
    /// </summary>
    public enum AbilityResultDescTypeEnum
    {
        /// <summary>
        /// 无效
        /// </summary>
        INVALID = -1,
        
        /// <summary>
        /// 命中
        /// </summary>
        HIT = 0,
        
        /// <summary>
        /// 施法者不存在
        /// </summary>
        NO_CASTOR,
        
        /// <summary>
        /// 没有目标
        /// </summary>
        NO_TARGET,
        
        /// <summary>
        /// 技能冷却中
        /// </summary>
        CD_NOT_OK,
        
        /// <summary>
        /// 无法使用
        /// </summary>
        CANT_USE,
        
        /// <summary>
        /// 技能消耗不够
        /// </summary>
        COST_NOT_ENOUGH,
        
        /// <summary>
        /// 造成了暴击
        /// </summary>
        CRITICAL,
    }
}
