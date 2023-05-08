using GameFramework;
using Aquila.Fight.Actor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using Aquila.Fight.Addon;
using Aquila.Fight;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 碰撞检测触发类addon，比较特殊的addon，因为要依赖于monobehavior的回调做碰撞检测。
    /// </summary>
    public class Addon_ColliderTrigger : Addon_Base
    {
        /// <summary>
        /// 设置碰撞大小
        /// </summary>
        public void SetSize ( Vector3 size )
        {
            if (_triggerBhvr != null)
                return;

            _triggerBhvr.SetColliderSize( size );
        }

        /// <summary>
        /// 设置触发次数
        /// </summary>
        public void SetTriggerLmt ( int lmt )
        {
            if (lmt <= 0)
                return;

            _triggerCounterLmt = lmt;
        }

        private void OnTriggerEnter ( object actor )
        {
            var go = actor as GameObject;
            if (go == null)
                return;

            //持有者的HitCorrectTarget调用检查
            if (!_typedActor.HitCorrectTarget( go ))
                return;

            _triggerCounter++;
            //触发次数满了，销毁
            Actor.Trigger( _triggerCounter >= _triggerCounterLmt ? ActorEventEnum.COLLIDER_TRIGGER_COUNT_LMT : ActorEventEnum.COLLIDER_TRIGGER_HIT, actor );
        }

        /// <summary>
        /// 初始化triggerBhvr
        /// </summary>
        private void InitTriggerBhvr ()
        {
            if (_triggerBhvr == null)
                _triggerBhvr = TargetGameObject.GetOrAddComponent<TriggerAddonBehavior>();

            if (!_triggerBhvr.enabled)
                _triggerBhvr.enabled = true;

            _triggerBhvr.Init( this, OnTriggerEnter );
        }


        public override AddonTypeEnum AddonType => AddonTypeEnum.TRIGGER;

        public override void OnAdd ()
        {
            _triggerdActorIDSet = new HashSet<int>();
            InitTriggerBhvr();
            _typedActor = Actor as ITriggerHitBehavior;
            if (_typedActor is null)
                Log.Error( $"_typedActor is null" );
        }

        public override void Reset ()
        {
            base.Reset();
            _triggerdActorIDSet?.Clear();
            _triggerCounter = 0;
        }
        
        public override void Dispose ()
        {
            base.Dispose();
            _triggerBhvr.Dispose();
            _triggerBhvr = null;
            _triggerdActorIDSet = null;
            _typedActor = null;
        }

        /// <summary>
        /// 盒碰撞触发器
        /// </summary>
        private TriggerAddonBehavior _triggerBhvr = null;

        /// <summary>
        /// 触发过的actorID集合
        /// </summary>
        private HashSet<int> _triggerdActorIDSet;

        /// <summary>
        /// 触发次数限制
        /// </summary>
        private int _triggerCounterLmt = 1;

        /// <summary>
        /// trigger命中次数
        /// </summary>
        private int _triggerCounter = 0;

        /// <summary>
        /// 类型化的actor方便拿取
        /// </summary>
        private ITriggerHitBehavior _typedActor = null;
    }

    /// <summary>
    /// 对于TriggerAddonBehavior的要求是：只能做回调检测的触发并且给到addon，不包含任何逻辑
    /// </summary>
    internal class TriggerAddonBehavior : MonoBehaviour
    {
        /// <summary>
        /// 设置trigger大小
        /// </summary>
        public void SetColliderSize(Vector3 size)
        {
            var boxCollider = _collider as BoxCollider;
            if (boxCollider is null)
                return;

            boxCollider.size = size;
            //boxCollider.center = Vector3.zero;
        }

        public void Init ( Addon_Base addon ,Action<object> on_trigger_enter)
        {
            if (addon == null || addon.Actor == null)
                return;

            if (addon.GetType() != typeof( Addon_ColliderTrigger ) )
                return;

            _collider = addon.TargetGameObject.GetOrAddComponent<BoxCollider>();
            _collider.isTrigger = true;
            _on_trigger_enter = on_trigger_enter;
            InitFlag = true;
        }

        public void Dispose()
        {
            _collider = null;
            _on_trigger_enter = null;
        }

        private void OnTriggerEnter ( Collider other )
        {
            if (!InitFlag || _on_trigger_enter == null)
                return;

            //#TODO Tag检查，enable检查
            //var actor = other.gameObject.GetComponent<TActorBase>();
            //if (actor == null)
            //    throw new GameFrameworkException( "actor == null!" );

            _on_trigger_enter.Invoke( other.gameObject );
        }

        /// <summary>
        /// 初始化标
        /// </summary>
        public bool InitFlag { get; private set; } = false;

        /// <summary>
        /// 碰撞回调
        /// </summary>
        private Action<object> _on_trigger_enter = null;

        private Collider _collider = null;
    }
}
