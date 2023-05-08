using Aquila.Fight.Addon;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 飞行类actor
    /// </summary>
    public abstract class FlyActor : TActorBase
    {
        public override void Setup (int actor_id)
        {
            _target_actor_id = actor_id;
        }

        private void Hide ()
        {
            SetWorldPosition( Vector3.zero );
            GameEntry.Entity.HideEntity( ActorID );
        }

        #region override

        protected virtual void OnColliderTriggerCountLmt ( int evnetID, object[] param )
        {
            if (param == null || param.Length == 0)
                return;

            //DoDamage( param[0] as ITakeDamageBehavior );
            Hide();
        }

        protected virtual void ColliderTriggerHit ( int evnetID, object[] param )
        {
            if (param == null || param.Length == 0)
                return;

            //DoDamage( param[0] as ITakeDamageBehavior );
            Hide();
        }

        protected override void OnShow ( object userData )
        {
            base.OnShow( userData );
            RegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_COUNT_LMT, OnColliderTriggerCountLmt );
            RegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_HIT, ColliderTriggerHit );
        }

        protected override void InitAddons (object user_data)
        {
            base.InitAddons(user_data);
            _triggerAddon = AddAddon<Addon_ColliderTrigger>();
            _moveAddon    = AddAddon<Addon_Move>();
        }

        protected override void OnRecycle ()
        {
            base.OnRecycle();
            UnRegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_COUNT_LMT );
            UnRegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_HIT );
            _target_actor_id = -1;
        }

        public override void Reset ()
        {
            base.Reset();
            _moveAddon.Reset();

        }

        /// <summary>
        /// 是否击中正确目标，是返回true,传入gameObject
        /// </summary>
        //public virtual bool HitCorrectTarget (object obj)
        //{
        //    return true;
        //}

        #endregion

        private void DoDamage ( ITakeDamageBehavior other_actor )
        {
            if (other_actor is null)
                return;

            //#TODO
            other_actor.TakeDamage( 0 );
        }

        //子弹只有一种状态，不用加fsm
        protected Addon_Move _moveAddon               = null;
        protected Addon_ColliderTrigger _triggerAddon = null;

        /// <summary>
        /// 目标actorID
        /// </summary>
        protected int _target_actor_id = -1;
    }

}