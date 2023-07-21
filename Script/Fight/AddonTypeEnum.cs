using System;
using System.Collections.Generic;

namespace Aquila.Fight.Addon
{
    public abstract partial class Addon_Base
    {
        /// <summary>
        /// addon类型枚举
        /// </summary>
        public enum AddonTypeEnum
        {
            /// <summary>
            /// 基础属性-数值组件
            /// </summary>
            NUMRIC_BASEATTR = 1,

            /// <summary>
            /// 数据
            /// </summary>
            DATA,

            /// <summary>
            /// 技能组件
            /// </summary>
            ABILITY,

            /// <summary>
            /// 状态
            /// </summary>
            STATE_MATCHINE,

            /// <summary>
            /// 事件
            /// </summary>
            EVENT,

            /// <summary>
            /// 动画
            /// </summary>
            ANIM,

            /// <summary>
            /// 移动
            /// </summary>
            MOVE,

            /// <summary>
            /// 导航
            /// </summary>
            NAV,

            /// <summary>
            /// 流程处理器
            /// </summary>
            PROCESSOR,

            /// <summary>
            /// 信息板组件
            /// </summary>
            INFO_BOARD,

            /// <summary>
            /// 寻路组件
            /// </summary>
            PATH_FINDER,

            /// <summary>
            /// 触发器
            /// </summary>
            TRIGGER,

            /// <summary>
            /// 特效
            /// </summary>
            EFFECT,

            /// <summary>
            /// 精灵图类型
            /// </summary>
            SPRITE,

            /// <summary>
            /// 地图组件
            /// </summary>
            //MAP = 8,
            
            /// <summary>
            /// HP显示组件
            /// </summary>
            HP,
            
            /// <summary>
            /// TIMELINE组件
            /// </summary>
            TIMELINE,

            /// <summary>
            /// 基础类型
            /// </summary>
            Max,
        }

        /// <summary>
        /// addon错误码
        /// </summary>
        // public class AddonValidErrorCodeEnum
        // {
        //     public const ushort NONE            = 0b0000_0000_0000_0000;//正常
        //     public const ushort ZERO_DATA_COUNT = 0b0000_0000_0000_0001;//1数据组件数据为空
        //     public const ushort NONE_EVENT      = 0b0000_0000_0000_0010;//事件为空
        //
        //     ///// <summary>
        //     ///// 没有该状态
        //     ///// </summary>
        //     //public const ushort HAVE_NO_STATE = 0b0000_0000_0000_0100;
        //
        //     private static Dictionary<uint, string> _err_code_map = new Dictionary<uint, string>
        //     {
        //         { ZERO_DATA_COUNT,"数据组组件为空!" },
        //     };
        //
        //     /// <summary>
        //     /// 错误码的string形式
        //     /// </summary>
        //     public static string ErrCode2String ( uint errCode )
        //     {
        //         if (!_err_code_map.TryGetValue( errCode, out var errMsg ))
        //             return $"cant find errCode {errCode} msg";
        //
        //         return errMsg;
        //     }
        // }
    }
}
