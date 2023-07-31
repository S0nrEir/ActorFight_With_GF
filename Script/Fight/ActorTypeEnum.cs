using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Actor
{
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
        /// 法球
        /// </summary>
        Orb,

        /// <summary>
        /// 触发类actor
        /// </summary>
        Trigger,
    }
}