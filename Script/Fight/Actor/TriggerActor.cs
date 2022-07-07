﻿using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using MRG.Fight.Addon;
using MRG.Fight.FSM;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 触发类actor，用于陷阱
    /// </summary>
    public class TriggerActor :
        TActorBase,
        ISwitchStateBehavior,
        ITriggerHitBehavior
    {
        /// <summary>
        /// 触发
        /// </summary>
        public void Trigger (Tab_Effect meta)
        {
            _effectAddon.ShowBuffEffectAsync
                (
                    ACTOR_ID_POOL.Gen(),
                    meta,
                    ( effectMeta, effect ) => Utils.Fight.SetEffectActorTran( effectMeta, effect )
                ) ;
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void Leave ()
        {
            _effectAddon.HideAll();
        }

        /// <summary>
        /// 别的object进入trigger
        /// </summary>
        private void ColliderTriggerHit ( int evnetID, object[] param )
        {
            Log.Info( $"<color=white>TriggerActor---->ColliderTriggerHit<color=white>" );
        }

        /// <summary>
        /// 特效时间到了
        /// </summary>
        private void OnEffectTimsUp ( int eventID, object[] param )
        {
            Log.Info( $"<color=white>TriggerActor---->OnEffectTimsUp<color=white>" );
        }

        #region impl
        public bool HitCorrectTarget ( object obj )
        {
            return true;
        }
        
        /// <summary>
        /// 状态切换
        /// </summary>
        public void SwitchTo ( ActorStateTypeEnum stateType, object[] enterParam, object[] existParam )
        {
            Log.Info( $"<color=white>TriggerActor--->SwitchTo,ActorID:{ActorID}<color>" );
        }

        #endregion

        #region override

        /// <summary>
        /// 设置信息
        /// </summary>
        public void Setup
            (
                string tag,
                int index,
                int actorID,
                ulong hostID,
                int forceType,
                int dataID,
                (float x,float z) wh
            ) 
        {
            Setup( tag, index, actorID, hostID, forceType, dataID );
            _triggerAddon.SetSize( new Vector3( wh.x, 1f, wh.z ) );
        }

        public void SetSize ( (float x, float z) wh )
        {
            if (_triggerAddon != null)
                return;

            _triggerAddon.SetSize( new Vector3( wh.x, 1f, wh.z ) );
        }

        public override ActorTypeEnum ActorType => ActorTypeEnum.Trigger;

        protected override void OnShow ( object userData )
        {
            base.OnShow( userData );
        }

        protected override void OnRecycle ()
        {
            base.OnRecycle();
        }

        protected override void Register ()
        {
            base.Register();
            RegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_HIT, ColliderTriggerHit );
            RegisterActorEvent( ActorEventEnum.EFFECT_TIMES_UP, OnEffectTimsUp );
        }

        protected override void UnRegister ()
        {
            base.UnRegister();
            UnRegisterActorEvent( ActorEventEnum.COLLIDER_TRIGGER_HIT );
            UnRegisterActorEvent( ActorEventEnum.EFFECT_TIMES_UP );
        }

        public override void Reset ()
        {
            base.Reset();
            _triggerAddon.SetTriggerLmt( int.MaxValue );
        }

        protected override void InitAddons ()
        {
            base.InitAddons();
            _triggerAddon = AddAddon<ColliderTriggerAddon>();
            _effectAddon = AddAddon<EffectAddon>();
        }

        #endregion

        #region fields

        /// <summary>
        /// 碰撞
        /// </summary>
        private ColliderTriggerAddon _triggerAddon = null;

        /// <summary>
        /// 特效组件
        /// </summary>
        private EffectAddon _effectAddon = null;

        #endregion
    }

    public class TriggerActorEntityData : EntityData
    {
        public Vector3 centerPos = Vector3.zero;
        public int index = -1;
        public (float x, float y) stepWH;




        public TriggerActorEntityData ( int entityID ) : base ( entityID,typeof ( TriggerActor ).GetHashCode() )
        { 
        }
    }

}