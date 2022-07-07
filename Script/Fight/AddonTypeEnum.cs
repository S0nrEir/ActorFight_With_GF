using System;
using System.Collections.Generic;

namespace Aquila.Fight.Addon
{
    public abstract partial class AddonBase
    {
        /// <summary>
        /// addon类型枚举
        /// </summary>
        public enum AddonTypeEnum
        {
            /// <summary>
            /// 动画
            /// </summary>
            ANIM = 0,

            /// <summary>
            /// 移动
            /// </summary>
            MOVE = 1,

            /// <summary>
            /// 导航
            /// </summary>
            NAV = 2,

            /// <summary>
            /// 状态
            /// </summary>
            STATE_MATCHINE = 3,

            /// <summary>
            /// 流程处理器
            /// </summary>
            PROCESSOR = 4,

            /// <summary>
            /// 数据
            /// </summary>
            DATA = 5,

            /// <summary>
            /// HP
            /// </summary>
            HEALTH_BAR = 6,

            /// <summary>
            /// 寻路
            /// </summary>
            PATH_FINDER = 7,

            /// <summary>
            /// Actor事件
            /// </summary>
            EVENT = 8,

            /// <summary>
            /// 触发器
            /// </summary>
            TRIGGER = 9,

            /// <summary>
            /// 特效
            /// </summary>
            EFFECT = 10,

            /// <summary>
            /// 地图组件
            /// </summary>
            //MAP = 8,

            /// <summary>
            /// 基础类型
            /// </summary>
            BASE = 999,
        }

        /// <summary>
        /// addon错误码
        /// </summary>
        public class AddonValidErrorCodeEnum
        {
            public const ushort NONE            = 0b0000_0000_0000_0000;//正常
            public const ushort ZERO_DATA_COUNT = 0b0000_0000_0000_0001;//1数据组件数据为空
            public const ushort NONE_EVENT      = 0b0000_0000_0000_0010;//事件为空

            ///// <summary>
            ///// 没有该状态
            ///// </summary>
            //public const ushort HAVE_NO_STATE = 0b0000_0000_0000_0100;

            private static Dictionary<uint, string> _erroCodeMap = new Dictionary<uint, string>
            {
                { ZERO_DATA_COUNT,"数据组组件为空!" },
            };

            /// <summary>
            /// 错误码的string形式
            /// </summary>
            public static string ErrCode2String ( uint errCode )
            {
                if (!_erroCodeMap.TryGetValue( errCode, out var errMsg ))
                    return $"cant find errCode {errCode} msg";

                return errMsg;
            }
        }
    }
}
