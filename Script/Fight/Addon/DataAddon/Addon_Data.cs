using Aquila.Fight.Actor;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 数据组件 by yhc 
    /// </summary>
    public class Addon_Data : AddonBase
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.DATA;

        public override void OnAdd ()
        {
        }

        public override void Dispose ()
        {
            base.Dispose();
        }

        public override uint Valid ()
        {
            return ( uint ) AddonValidErrorCodeEnum.NONE;
        }

        public override void Reset ()
        {
            base.Reset();
        }

        public override void Init ( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
        }

        //#todo_数据分成四个部分，基础数值，装备，buff，总值

    }

    /// <summary>
    /// 数据字段定义类型
    /// </summary>
    public enum DataAddonFieldTypeEnum
    {
        /// <summary>
        /// 血量上限
        /// </summary>
        NUM_MAX_HP = 1,

        /// <summary>
        /// 当前血量
        /// </summary>
        NUM_CURR_HP,

        /// <summary>
        /// 当前血量
        /// </summary>
        NUM_CURR_MP,

        /// <summary>
        /// 当前血量
        /// </summary>
        NUM_MAX_MP,

        /// <summary>
        /// 移速
        /// </summary>
        NUM_MOVE_SPEED,

        /// <summary>
        /// 攻
        /// </summary>
        NUM_ACK,

        /// <summary>
        /// 防
        /// </summary>
        NUM_DEF,

        /// <summary>
        /// 护盾
        /// </summary>
        NUM_SHIELD,

        /// <summary>
        /// 表id
        /// </summary>
        NUM_META_ID,

        /// <summary>
        /// 警戒范围
        /// </summary>
        NUM_ALERT_RADIUS,

        /// <summary>
        /// 伤害半径
        /// </summary>
        NUM_RADIUS,

        /// <summary>
        /// ROLEBase表数据
        /// </summary>
        OBJ_META_ROLEBASE,
    }
}

