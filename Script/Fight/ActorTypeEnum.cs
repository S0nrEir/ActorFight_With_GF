using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 阵营类型
    /// </summary>
    public enum ForceTypeEnum
    {
        //无效
        Invalid = -1,
        //区域
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        
        //阵营
        Eight,
        Nine,
        Maximun,
    }


    public enum ActorTypeEnum
    {
        INVALID = -1,

        /// <summary>
        /// 生物，用于表示基类型
        /// </summary>
        DYNAMIC,

        /// <summary>
        /// 非生物，用于表示基类型
        /// </summary>
        STATIC,

        /// <summary>
        /// 英雄
        /// </summary>
        HERO,

        /// <summary>
        /// 随从
        /// </summary>
        MINION,

        /// <summary>
        /// 防御塔
        /// </summary>
        Tower,

        /// <summary>
        /// 子弹
        /// </summary>
        Bullet,

        /// <summary>
        /// 投射物
        /// </summary>
        Projectile,

        /// <summary>
        /// 触发类actor
        /// </summary>
        Trigger,
    }

    /// <summary>
    /// roleBase表格npcType枚举
    /// </summary>
    public enum RoleBaseNpcTypeEnum
    {
        /// <summary>
        /// 英雄
        /// </summary>
        HERO = 0,

        /// <summary>
        /// 随从
        /// </summary>
        MINION = 1,

        /// <summary>
        /// 防御塔
        /// </summary>
        TOWER = 2,

        /// <summary>
        /// 子弹
        /// </summary>
        BULLET = 3,
    }

    /// <summary>
    /// roleBase子类型枚举
    /// </summary>
    public class RoleBaseSubTypeEnum
    {
        /// <summary>
        /// 随从类
        /// </summary>
        public class Minion
        {
            /// <summary>
            /// 魔兽
            /// </summary>
            public const int MONSTER = 0;

            /// <summary>
            /// 元素
            /// </summary>
            public const int ELEMENTS = 1;

            /// <summary>
            /// 蜘蛛
            /// </summary>
            public const int SPIDER = 2;
        }

        /// <summary>
        /// 子弹/投射类
        /// </summary>
        public class Bullet
        {
            /// <summary>
            /// 直线贯通型
            /// </summary>
            public const int STRAIGHT_LINE = 0;
        }
    }
}