using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Numric
{
    /// <summary>
    /// 属性类型枚举
    /// </summary>
    public enum Attribute_Type_Enum
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        //Un_Valid = -1,

        /// <summary>
        /// 当前HP
        /// </summary>
        Curr_HP = 0,

        /// <summary>
        /// HP上限
        /// </summary>
        Max_HP,

        /// <summary>
        /// 攻击力
        /// </summary>
        ATK,

        /// <summary>
        /// 防御力
        /// </summary>
        DEF,

        /// <summary>
        /// 速度
        /// </summary>
        SPD,

        /// <summary>
        /// 上限，不要将新的属性枚举添加在后面
        /// </summary>
        Max,
    }
}
