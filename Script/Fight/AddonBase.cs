using Aquila.Fight.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// addon基类 by yhc 
    /// </summary>
    public abstract partial class AddonBase
    {
        public abstract AddonTypeEnum AddonType { get; }

        public virtual void Init ( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            Actor = actor;
            TargetGameObject = targetGameObject;
            TargetTransform = targetTransform;
        }

        public virtual void OnUpdate ( float elapseSeconds, float realElapseSeconds )
        {
            
        }

        /// <summary>
        /// 重置addon状态
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

        //#封装do方法，统一处理
        /// <summary>
        /// 开关
        /// </summary>
        public bool Enable => _enable;

        /// <summary>
        /// 开关默认开
        /// </summary>
        protected bool _enable = true;

        /// <summary>
        /// 组件添加
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
        public virtual uint Valid () 
        {
            return AddonValidErrorCodeEnum.NONE;
        }

        public abstract void SetEnable ( bool enable );

        /// <summary>
        /// 通知actor一个event
        /// </summary>
        //public void NotifyActor ( ActorEventEnum type, object[] param )
        //{
        //    var intType = (int)type;
        //}
    }

}
