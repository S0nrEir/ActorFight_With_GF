using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRG.Fight
{
    /// <summary>
    /// 枚举类型表示的方向
    /// </summary>
    public enum DirectionEnum
    {
        /// <summary>
        /// 无效
        /// </summary>
        UnValid,

        /// <summary>
        /// 右
        /// </summary>
        Right = 0,

        /// <summary>
        /// 右上
        /// </summary>
        UpRight,

        /// <summary>
        /// 上
        /// </summary>
        Up,

        /// <summary>
        /// 左上
        /// </summary>
        UpLeft,

        /// <summary>
        /// 左边
        /// </summary>
        Left,

        /// <summary>
        ///左下
        /// </summary>
        DownLeft,

        /// <summary>
        /// 下
        /// </summary>
        Down,

        /// <summary>
        /// 右下
        /// </summary>
        DownRight = 7,

        /// <summary>
        /// 最大值范围，必须小于他
        /// </summary>
        Max = 8
    }
}
