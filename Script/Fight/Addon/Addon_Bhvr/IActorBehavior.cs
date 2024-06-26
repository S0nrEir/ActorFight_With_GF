using Aquila.Fight.FSM;
using System.Collections.Generic;
using UnityEngine;

//接口类型，实现相应行为的actor实现此接口,用于规范Actor的行为
namespace Aquila.Fight
{
    public interface IActorBehaviour
    {
        /// <summary>
        /// 执行行为
        /// </summary>
        public void Exec( object param );
    }

    /// <summary>
    /// trigger触发
    /// </summary>
    public interface ITriggerHitBehavior
    {
        /// <summary>
        /// 是否击中了正确的target
        /// </summary>
        bool HitCorrectTarget ( object obj );
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public interface IDieBehavior
    {
        void Die ();
    }

    /// <summary>
    /// 路点移动
    /// </summary>
    public interface IPathMoveBehavior
    {
        void Move ( IList<float> xList, IList<float> zList );
    }

    /// <summary>
    /// 移动
    /// </summary>
    public interface INavMoveBehavior
    {
        /// <summary>
        /// 移动至
        /// </summary>
        void MoveTo ( float targetX, float targetZ );
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    public interface ISwitchStateBehavior
    {
        /// <summary>
        /// 切换状态
        /// </summary>
        void SwitchTo ( ActorStateTypeEnum stateType, object enterParam, object existParam );
    }

    public interface IProjectileBehaviour
    {
        void GetReady( Transform transform );
    }
}