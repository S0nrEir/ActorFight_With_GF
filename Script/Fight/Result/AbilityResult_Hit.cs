using GameFramework;
using UnityEngine;

namespace Aquila.Event
{
    /// <summary>
    /// 技能命中的参数
    /// </summary>
    public class AbilityResult_Hit : IReference
    {
        public void Clear()
        {
            _stateDescription = 0;
            _dealedDamage     = 0;
            _targetActorID    = -1;
            _castorActorID    = -1;
            _targetPosition   = GameEntry.GlobalVar.InvalidPosition;
        }

        /// <summary>
        /// 状态描述
        /// </summary>
        public int _stateDescription;

        /// <summary>
        /// 造成的伤害
        /// </summary>
        public int _dealedDamage;

        /// <summary>
        /// 目标ActorID
        /// </summary>
        public int _targetActorID;

        /// <summary>
        /// 施法者ActorID
        /// </summary>
        public int _castorActorID;

        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 _targetPosition;

        /// <summary>
        /// 获取一个AbilityResult_Hit实例
        /// </summary>
        public static AbilityResult_Hit Gen( int castorID, int targetID )
        {
            var result = ReferencePool.Acquire<AbilityResult_Hit>();
            result._dealedDamage = 0;
            result._stateDescription = 0;
            result._castorActorID = castorID;
            result._targetActorID = targetID;
            return result;
        }

        /// <summary>
        /// 释放一个AbilityResult_Hit实例
        /// </summary>
        public static void Release( AbilityResult_Hit result )
        {
            ReferencePool.Release( result );
        }
    }
}