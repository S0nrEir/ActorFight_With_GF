using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 事件组件优先级枚举
    /// </summary>
    public  enum EventAddonPrioerityTypeEnum
    {
        /// <summary>
        /// 响应effect逻辑
        /// </summary>
        EFFECT_SPEC,
        
        /// <summary>
        /// 基础属性-数值组件
        /// </summary>
        ADDON_NUMRIC_BASEATTR = 0,
        /// <summary>
        /// 技能组件
        /// </summary>
        ADDON_ABILITY,

        /// <summary>
        /// 状态
        /// </summary>
        ADDON_STATE_MATCHINE,
    }
}
