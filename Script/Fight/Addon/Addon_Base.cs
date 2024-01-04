using Aquila.Fight.Actor;
using Aquila.Module;
using GameFramework.ObjectPool;
using ICSharpCode.SharpZipLib.Core;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// addon基类 by yhc 
    /// </summary>
    public abstract partial class Addon_Base
    {
        /// <summary>
        /// addon的初始化，addon的数据初始化在这里做
        /// </summary>
        public virtual void Init( Module_ProxyActor.ActorInstance instance)
        {
            _actorInstance = instance;
            Actor          = instance.Actor;
        }

        public virtual void OnUpdate ( float elapseSeconds, float realElapseSeconds )
        {
            
        }

        /// <summary>
        /// 重置addon状态，Actor在Reset时调用
        /// </summary>
        public virtual void Reset ()
        {
        }

        /// <summary>
        /// 目标处理的Transform
        /// </summary>
        public Transform TargetTransform { get; private set; }

        /// <summary>
        /// 该addon附件的actor
        /// </summary>
        public GameObject TargetGameObject { get; private set;  }

        public Actor_Base Actor { get; private set; }

        /// <summary>
        /// actor持有的addon
        /// </summary>
        protected Addon_Base[] _actor_addons = null;

        /// <summary>
        /// 持有的actor实例
        /// </summary>
        protected Module_ProxyActor.ActorInstance _actorInstance = null;

        /// <summary>
        /// 当组件被添加到actor上
        /// </summary>
        public abstract void OnAdd ();

        /// <summary>
        /// 组件被移除
        /// </summary>
        public virtual void OnRemove ()
        {
        }
        
        /// <summary>
        /// 清理资源，所属actor回收时调用
        /// </summary>
        public virtual void Dispose ()
        {
            Actor          = null;
            _actorInstance = null;
        }
        
        /// <summary>
        /// 组件类型
        /// </summary>
        public abstract AddonTypeEnum AddonType { get; }

    }

}
