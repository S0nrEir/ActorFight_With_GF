using Aquila.Fight.Addon;
using UnityEngine;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 飞行类actor
    /// </summary>
    public abstract class Actor_Fly : Actor_Base
    {
        public override void Setup( int actorID )
        {
            _targetActorID = actorID;
        }

        private void Hide()
        {
            SetWorldPosition( Vector3.zero );
            GameEntry.Entity.HideEntity( ActorID );
        }

        #region override

        protected virtual void OnColliderTriggerCountLmt( int evnetID, object[] param )
        {
            if ( param == null || param.Length == 0 )
                return;

            Hide();
        }

        protected virtual void ColliderTriggerHit( int evnetID, object[] param )
        {
            if ( param == null || param.Length == 0 )
                return;

            Hide();
        }

        protected override void OnShow( object userData )
        {
            base.OnShow( userData );
            //RegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_COUNT_LMT, OnColliderTriggerCountLmt );
            //RegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_HIT, ColliderTriggerHit );
        }

        // protected override void InitAddons (object user_data)
        // {
        //     base.InitAddons(user_data);
        // }

        protected override void AddAddon()
        {
            base.AddAddon();
            _triggerAddon = AddAddon<Addon_ColliderTrigger>();
            _moveAddon = AddAddon<Addon_Move>();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            //UnRegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_COUNT_LMT );
            //UnRegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_HIT );
            _targetActorID = -1;
        }

        public override void Reset()
        {
            base.Reset();
            _moveAddon.Reset();
        }

        #endregion

        //子弹只有一种状态，不用加fsm
        protected Addon_Move _moveAddon = null;
        protected Addon_ColliderTrigger _triggerAddon = null;

        /// <summary>
        /// 目标actorID
        /// </summary>
        protected int _targetActorID = -1;
    }

}