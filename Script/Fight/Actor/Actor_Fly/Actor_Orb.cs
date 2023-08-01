using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
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
        }

        protected override void InitAddons( Module_ProxyActor.ActorInstance instance )
        {
            base.InitAddons( instance );
            _behaviourAddon.AddBehaviour( ActorBehaviourTypeEnum.TRACING_TRANSFORM );
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
            if ( userData is Actor_Orb_EntityData )
            {
                var param = ( userData as Actor_Orb_EntityData );
                _targetActorID = param._targetActorID;
                Setup( param._roleMetaID );
            }
            else
            {
                Log.Warning( "<color=yellow>Actor_Orb.OnInitActor()---></color>" );
            }
        }

        public override ActorTypeEnum ActorType => ActorTypeEnum.Orb;

        //-------------------- field --------------------
        /// <summary>
        /// 行为组件
        /// </summary>
        private Addon_Behaviour _behaviourAddon = null;

        /// <summary>
        /// 目标actorID
        /// </summary>
        private int _targetActorID = -1;
    }

    public class Actor_Orb_EntityData : EntityData
    {
        public Actor_Orb_EntityData( int entity_id ) : base( entity_id, typeof( Actor_Orb ).GetHashCode() )
        {
        }

        /// <summary>
        /// 目标actorID
        /// </summary>
        public int _targetActorID = -1;

        /// <summary>
        /// actor表ID
        /// </summary>
        public int _roleMetaID = -1;
    }
}