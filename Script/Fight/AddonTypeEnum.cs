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
            /// 行为
            /// </summary>
            BEHAVIOUR,

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
    }
}
