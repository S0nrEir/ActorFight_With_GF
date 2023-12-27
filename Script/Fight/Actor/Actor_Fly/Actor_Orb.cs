using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 法球类actor
    /// </summary>
    public class Actor_Orb : Actor_Base
    {
        //----------------------- pub -----------------------
        

        /// <summary>
        /// 设置目标位置并且准备继续
        /// </summary>
        public void SetTargetPositionAndReady(Vector3 position)
        {
            //替换追踪组件为目标点组件
            _behaviourAddon.RemoveBehaviour( ActorBehaviourTypeEnum.TRACING_TRANSFORM );
            var bhvr = _behaviourAddon.AddBehaviour( ActorBehaviourTypeEnum.TARGETING_POSITION ) as ActorBehaviour_TargetingPosition;
            bhvr._onHitAbilityID = DefaultOnHitAbilityID();
            bhvr._radius = 1f;
            bhvr.GetReady( position );
        }
        
        /// <summary>
        /// 设置目标点并准备就绪
        /// </summary>
        public void SetTargetPositionAndReady( Transform target )
        {
            var onHitAbilityID = DefaultOnHitAbilityID();
            if ( onHitAbilityID < 0 )
            {
                Log.Warning( $"Actor_Orb.SetTargetPositionAndReady()--->abilityID < 0,abilityID:{onHitAbilityID},roleMetaID:{RoleMetaID}" );
                return;
            }

            //替换追踪组件为目标点组件
            _behaviourAddon.RemoveBehaviour( ActorBehaviourTypeEnum.TRACING_TRANSFORM );
            var bhvr = _behaviourAddon.AddBehaviour( ActorBehaviourTypeEnum.TARGETING_POSITION ) as ActorBehaviour_TargetingPosition;
            bhvr._onHitAbilityID = onHitAbilityID;
            bhvr._radius = 1f;
            bhvr.GetReady( target );
        }

        /// <summary>
        /// 设置跟随目标并准备就绪
        /// </summary>
        public void SetTargetTransformAndReady( Transform targetTransform )
        {
            var onHitAbilityID = DefaultOnHitAbilityID();
            if ( onHitAbilityID < 0 )
            {
                Log.Warning( $"Actor_Orb.SetTargetTransformAndReady()--->abilityID < 0,abilityID:{onHitAbilityID},roleMetaID:{RoleMetaID}" );
                return;
            }

            var bhvr = _behaviourAddon.GetBehaviour( ActorBehaviourTypeEnum.TRACING_TRANSFORM ) as ActorBehaviour_TracingTransform;
            if ( bhvr != null )
            {
                //#todo这里改成配表，为了测试暂时写死的
                bhvr._radius = 1f;
                bhvr._onHitabilityID = onHitAbilityID;
                bhvr._targetActorID = _targetActorID;
                bhvr.GetReady( targetTransform/*, _targetActorID, onHitAbilityID*/ );
            }
        }

        /// <summary>
        /// 当actor死亡
        /// </summary>
        private void OnActorDie(object sender, GameEventArgs e)
        {
            var arg = e as EventArg_OnActorDie;
            if (arg is null)
                return;
            
            var bhvr = _behaviourAddon.GetBehaviour( ActorBehaviourTypeEnum.TRACING_TRANSFORM ) as ActorBehaviour_TracingTransform;
            if (arg._actorID != bhvr._targetActorID)
                return;

            var lastPosition = GameEntry.Module.GetModule<Module_ProxyActor>().GetPosition(arg._actorID);
            SetTargetPositionAndReady(lastPosition);
        }

        /// <summary>
        /// 获取法球的默认命中技能ID
        /// </summary>
        private int DefaultOnHitAbilityID()
        {
            return Tools.Actor.DefaultOrbOnHitAbilityID( GameEntry.LuBan.Tables.RoleMeta.Get( RoleMetaID ).AbilityBaseID );
        }

        protected override void AddAddon()
        {
            base.AddAddon();
            _behaviourAddon = AddAddon<Addon_Behaviour>();
            AddAddon<Addon_Ability>();
        }

        protected override void InitAddons( Module_ProxyActor.ActorInstance instance )
        {
            base.InitAddons( instance );
            _behaviourAddon.AddBehaviour( ActorBehaviourTypeEnum.TRACING_TRANSFORM );
        }

        protected override void OnInitActor( object userData )
        {
            base.OnInitActor( userData );
            
            if ( userData is Actor_Orb_EntityData data )
                _targetActorID = data._targetActorID;
            else
                Log.Warning( "<color=yellow>Actor_Orb.OnInitActor()---></color>" );
            
            GameEntry.Event.Subscribe(EventArg_OnActorDie.EventID,OnActorDie);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(EventArg_OnActorDie.EventID,OnActorDie);
            base.OnHide(isShutdown, userData);
        }

        protected override void OnRecycle()
        {
            _behaviourAddon = null;
            base.OnRecycle();
        }

        public override RoleType ActorType => RoleType.Orb;

        //-------------------- field --------------------

        /// <summary>
        /// 行为组件
        /// </summary>
        private Addon_Behaviour _behaviourAddon = null;

        /// <summary>
        /// 目标actorID
        /// </summary>
        private int _targetActorID = -1;
        
        /// <summary>
        /// 当前的目标位置
        /// </summary>
        private Vector3 _currTargetPosition = Vector3.zero;
    }

    public class Actor_Orb_EntityData : Actor_Base_EntityData
    {
        public Actor_Orb_EntityData( int entity_id ) : base( entity_id, typeof( Actor_Orb ).GetHashCode() )
        {
        }

        /// <summary>
        /// 召唤者ID
        /// </summary>
        public int _callerID = -1;

        /// <summary>
        /// 目标actorID
        /// </summary>
        public int _targetActorID = -1;
    }
}