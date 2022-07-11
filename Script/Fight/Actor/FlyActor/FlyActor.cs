using Aquila.Fight.Addon;
using GameFramework.Event;
using MRG.Fight.Addon;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 飞行类actor
    /// </summary>
    public abstract class FlyActor : TActorBase
    {
        public virtual void Setup (int actorID)
        {
            _targetActorID = actorID;
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

        protected override void InitAddons ()
        {
            base.InitAddons();
            _triggerAddon = AddAddon<ColliderTriggerAddon>();
            _moveAddon    = AddAddon<MoveAddon>();
        }

        protected override void OnRecycle ()
        {
            base.OnRecycle();
            UnRegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_COUNT_LMT );
            UnRegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_HIT );
            _targetActorID = -1;
        }

        public override void Reset ()
        {
            base.Reset();
            _moveAddon.Reset();

        }

        protected override void ResetData ()
        {
            base.ResetData();
        }

        /// <summary>
        /// 是否击中正确目标，是返回true,传入gameObject
        /// </summary>
        //public virtual bool HitCorrectTarget (object obj)
        //{
        //    return true;
        //}

        #endregion

        private void DoDamage ( ITakeDamageBehavior otherActor )
        {
            if (otherActor is null)
                return;

            //#TODO
            otherActor.TakeDamage( 0 );
        }

        //子弹只有一种状态，不用加fsm
        protected MoveAddon _moveAddon               = null;
        protected ColliderTriggerAddon _triggerAddon = null;

        /// <summary>
        /// 目标actorID
        /// </summary>
        protected int _targetActorID = -1;
    }

}