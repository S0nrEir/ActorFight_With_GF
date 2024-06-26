using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight
{
    /// <summary>
    /// 追踪transform类行为
    /// </summary>
    public class ActorBehaviour_TracingTransform : ActorBehaviour_Base, IProjectileBehaviour
    {
        /// <summary>
        /// 准备就绪
        /// </summary>
        public void GetReady( Transform targetTransform )
        {
            if ( _radius <= 0f )
            {
                Log.Warning( $"<color=yellow>ActorBehaviour_TracingTransform.GetReady()--->_radius <= 0f</color>" );
                return;
            }

            if ( _targetActorID == -1 || _onHitabilityID == -1 )
            {
                Log.Warning( $"<color=yellow>ActorBehaviour_TracingTransform.GetReady()--->_targetActorID == -1 || abilityID == -1,targetActorID:{_targetActorID},_onHitabilityID:{_onHitabilityID}</color>" );
                return;
            }

            _targetTransform = targetTransform;
            _readyFlag = true;
        }

        //-------------------- override --------------------
        public override ActorBehaviourTypeEnum BehaviourType => ActorBehaviourTypeEnum.TRACING_TRANSFORM;


        public override void Exec( object param )
        {
            base.Exec( param );
        }

        public override void Update( float elapsed, float realElapsed )
        {
            base.Update( elapsed, realElapsed );

            if ( _arriveFlag || !_readyFlag)
                return;

            if ( Distance() <= _radius )
            {
                _arriveFlag = true;
                _instance.Actor.Notify( ( int ) AddonEventTypeEnum.TRACING_ARRIVE, null );
                //在这里直接使用技能，走module接口，需要自己的id，对反的id，技能id
                if (_onHitabilityID >= 0)
                {
                    // var fsm = _instance.GetAddon<Addon_FSM_Orb>();
                    // if (fsm != null) 
                    // {
                    //     //现在改成进入状态，然后由状态处理
                    //     var param = ReferencePool.Acquire<OrbAbilityStateParam>();
                    //     param.AbilityID = _onHitabilityID;
                    //     param.CastorActorID = _instance.Actor.ActorID;
                    //     param.TargetActorID = _targetActorID;
                    //     param.Position = _targetTransform.position;
                    //     fsm.SwitchTo((int)ActorStateTypeEnum.ABILITY_STATE,param,null);
                    // }
                    var module = GameEntry.Module.GetModule<Module_ProxyActor>();
                    //#todo:投射物默认启用第一个trigger，有需要再改
                    module.AffectAbility( 0,_instance.Actor.ActorID, _targetActorID, _onHitabilityID, GameEntry.GlobalVar.InvalidPosition );
                }
                // GameEntry.Entity.HideEntity( _instance.Actor.ActorID );
                return;
            }

            //move
            var dir = _targetTransform.position - _cachedActorTransform.position;
            _cachedActorTransform.position = _cachedActorTransform.position + dir.normalized * _defaultSpeed * elapsed;
            _cachedActorTransform.rotation = Quaternion.Slerp
                (
                    _cachedActorTransform.rotation,
                    Quaternion.LookRotation(dir),
                    .8f
                );
        }

        private float Distance()
        {
            return Tools.DistanceSQR( _cachedActorTransform.position, _targetTransform.position );
        }

        public ActorBehaviour_TracingTransform( ActorInstance instance ) : base( instance )
        {
            _cachedActorTransform = instance.Actor.CachedTransform;
            _targetTransform      = null;
            _arriveFlag           = false;
            _readyFlag            = false;
            _radius               = 0f;
            _targetActorID        = -1;
            _onHitabilityID       = -1;
        }

        /// <summary>
        /// 目标transform
        /// </summary>
        private Transform _targetTransform = null;

        /// <summary>
        /// 是否抵达目标点
        /// </summary>
        private bool _arriveFlag = false;

        /// <summary>
        /// 就绪标记
        /// </summary>
        private bool _readyFlag = false;

        /// <summary>
        /// 范围
        /// </summary>
        public float _radius = 0f;

        /// <summary>
        /// 缓存actor的transform
        /// </summary>
        private Transform _cachedActorTransform = null;

        /// <summary>
        /// 目标ActorID
        /// </summary>
        public int _targetActorID = -1;

        /// <summary>
        /// 要使用的技能ID
        /// </summary>
        public int _onHitabilityID = -1;

        /// <summary>
        /// 默认速度
        /// </summary>
        public float _defaultSpeed = 1f;
    }
}
