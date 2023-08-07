using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight
{
    public class ActorBehaviour_TargetingPosition : ActorBehaviour_Base, IProjectileBehaviour
    {
        /// <summary>
        /// 就绪
        /// </summary>
        public void GetReady( Transform transform )
        {
            if ( _radius <= 0f )
            {
                Log.Warning( $"<color=yellow>ActorBehaviour_TargetingPosition.GetReady()--->_radius <= 0f </color>" );
                return;
            }

            _targetPosition = transform.position;
            _readyFlag = true;
        }

        public override void Update( float elapsed, float realElapsed )
        {
            base.Update( elapsed, realElapsed );

            if ( !_readyFlag )
                return;

            if ( Tools.DistanceSQR( _cachedActorTransform.position, _targetPosition ) <= _radius )
            {
                _instance.Actor.Notify( ( int ) AddonEventTypeEnum.POSITION_ARRIVE, null );

                //#todo点地或者以坐标点为目标命中后，技能是不需要目标actor的，这里以后再写
                //if(_onHitAbilityID > 0)
                //    GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility(_instance.Actor.ActorID,targetid)

                GameEntry.Entity.HideEntity( _instance.Actor.ActorID );
                return;
            }

            //move
            var currPosition = _cachedActorTransform.position;
            var dir = ( _targetPosition - currPosition ).normalized;
            _cachedActorTransform.position = dir * elapsed * _defaultSpeed + currPosition;
        }

        public ActorBehaviour_TargetingPosition( ActorInstance instance ) : base( instance )
        {
            _cachedActorTransform = instance.Actor.CachedTransform;
            _targetPosition       = GameEntry.GlobalVar.InvalidPosition;
            _onHitAbilityID       = -1;
            _radius               = 0f;
            _readyFlag            = false;
        }

        /// <summary>
        /// 触发的技能ID
        /// </summary>
        public int _onHitAbilityID = -1;

        /// <summary>
        /// 缓存actor transform
        /// </summary>
        private Transform _cachedActorTransform = null;

        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 _targetPosition = GameEntry.GlobalVar.InvalidPosition;

        /// <summary>
        /// 就绪标记
        /// </summary>
        public bool _readyFlag = false;

        /// <summary>
        /// 范围
        /// </summary>
        public float _radius = 0f;

        /// <summary>
        /// 默认速度
        /// </summary>
        public float _defaultSpeed = 0.05f;
    }

}
