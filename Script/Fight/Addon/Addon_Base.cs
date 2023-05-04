using Aquila.Fight.Actor;
using System.Collections;
using System.Collections.Generic;
using Aquila.Module;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// addon基类 by yhc 
    /// </summary>
    public abstract partial class Addon_Base
    {
        /// <summary>
        /// 让该addon持有该actor的所有其他addon
        /// </summary>
        public void SetActorAddons(Addon_Base[] addons)
        {
            _actor_addons = addons;
        }
        
        /// <summary>
        /// 设置玩家的actor实例
        /// </summary>
        public void SetActorInstace(Module_Proxy_Actor.ActorInstance instance)
        {
            _actor_instance = instance;
        }

        public abstract AddonTypeEnum AddonType { get; }

        public virtual void Init ( TActorBase actor, GameObject target_go, Transform target_transform )
        {
            Actor = actor;
            TargetGameObject = target_go;
            TargetTransform = target_transform;
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

        public TActorBase Actor { get; private set; }

        /// <summary>
        /// actor持有的addon
        /// </summary>
        protected Addon_Base[] _actor_addons = null;

        /// <summary>
        /// 持有的actor实例
        /// </summary>
        protected Module_Proxy_Actor.ActorInstance _actor_instance = null;

        /// <summary>
        /// 当组件被添加到actor上
        /// </summary>
        public abstract void OnAdd ();

        /// <summary>
        /// 组件移除
        /// </summary>
        public virtual void OnRemove ()
        {
            
        }

        /// <summary>
        /// 清理资源，所属actor回收会调用
        /// </summary>
        public virtual void Dispose ()
        {
            
        }

        /// <summary>
        /// 检查，返回addon错误码
        /// </summary>
        // public virtual uint Valid () 
        // {
        //     return AddonValidErrorCodeEnum.NONE;
        // }

        /// <summary>
        /// 通知actor一个event
        /// </summary>
        //public void NotifyActor ( ActorEventEnum type, object[] param )
        //{
        //    var intType = (int)type;
        //}
    }

}
