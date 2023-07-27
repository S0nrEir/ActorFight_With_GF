using GameFramework;

namespace Aquila.Event
{
    /// <summary>
    /// 技能命中的参数
    /// </summary>
    public class AbilityResult_Hit : IReference
    {
        public void Clear()
        {

        }

        /// <summary>
        /// 状态描述
        /// </summary>
        public int _stateDescription = 0;

        /// <summary>
        /// 造成的伤害
        /// </summary>
        public int _dealedDamage = 0;

        /// <summary>
        /// 目标ActorID
        /// </summary>
        public int _targetActorID = -1;

        /// <summary>
        /// 施法者ActorID
        /// </summary>
        public int _castorActorID = -1;

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