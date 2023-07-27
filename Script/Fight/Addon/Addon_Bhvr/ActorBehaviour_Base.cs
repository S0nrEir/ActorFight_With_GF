using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight
{
    /// <summary>
    /// actor行为类
    /// </summary>
    public abstract class ActorBehaviour_Base : IActorBehaviour
    {
        public ActorBehaviour_Base( ActorInstance instance )
        {
            _instance = instance;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public virtual void Dispose()
        {
            _instance = null;
        }

        /// <summary>
        /// 执行行为
        /// </summary>
        public virtual void Exec( object param )
        {
        }

        /// <summary>
        /// 行为类型
        /// </summary>
        public virtual ActorBehaviourTypeEnum BehaviourType => ActorBehaviourTypeEnum.INVALID;

        /// <summary>
        /// actor实例
        /// </summary>
        protected ActorInstance _instance = null;
    }

}
