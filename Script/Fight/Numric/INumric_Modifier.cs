using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Numric
{
    /// <summary>
    /// 数值修改器类型接口
    /// </summary>
    public interface INumric_Modifier
    {

    }

    /// <summary>
    /// Numric数值修改方式枚举
    /// </summary>
    public enum Numric_Modify_Type_Enum
    {
        /// <summary>
        /// 无，无效
        /// </summary>
        None = 0,

        /// <summary>
        /// 加
        /// </summary>
        Add = 1,

        /// <summary>
        /// 百分比，变为原先的百分之多少
        /// </summary>
        Percent,
    }
}