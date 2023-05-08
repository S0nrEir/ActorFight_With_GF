// using Aquila.Module;
// using UnityEngine;
//
// namespace Aquila.Fight.Actor
// {
//     /// <summary>
//     /// 投射物actor
//     /// </summary>
//     public class ProjectileActor : FlyActor,
//         INavMoveBehavior,
//         ITriggerHitBehavior
//     {
//         #region public methods
//
//         public void SetTarget( Transform targetTransform, int targetID )
//         {
//             SetReadyFlag( true, targetTransform );
//             Setup( targetID );
//         }
//
//         #endregion
//
//         #region private methods
//
//         /// <summary>
//         /// 设置检查状态
//         /// </summary>
//         private void SetReadyFlag( bool ready, Transform targetTran = null )
//         {
//             _readyFlag = ready;
//             _targetTransform = _readyFlag ? targetTran : null;
//         }
//
//         #endregion
//
//         #region impl
//
//         public void MoveTo( float targetX, float targetZ )
//         {
//             _moveAddon.MoveTo( CachedTransform.position, _targetTransform.position, _elapsedSeconds );
//         }
//
//         #endregion
//
//         protected override void ColliderTriggerHit( int evnetID, object[] param )
//         {
//             SetReadyFlag( false );
//             base.ColliderTriggerHit( evnetID, param );
//         }
//
//         protected override void OnColliderTriggerCountLmt( int evnetID, object[] param )
//         {
//             SetReadyFlag( false );
//             base.OnColliderTriggerCountLmt( evnetID, param );
//         }
//
//         //刷位置
//         protected override void OnUpdate( float elapseSeconds, float realElapseSeconds )
//         {
//             base.OnUpdate( elapseSeconds, realElapseSeconds );
//             if ( !ReadyFlag )
//                 return;
//
//             _elapsedSeconds = elapseSeconds;
//             MoveTo( _targetTransform.position.x, _targetTransform.position.z );
//         }
//
//         #region override
//         public override ActorTypeEnum ActorType => ActorTypeEnum.Projectile;
//
//         protected override void InitAddons( Module_Proxy_Actor.ActorInstance instance)
//         {
//             base.InitAddons( instance );
//             _triggerAddon.SetTriggerLmt( 1 );
//             _moveAddon.SetSpeed( 5f );
//         }
//
//         public override void Reset()
//         {
//             base.Reset();
//             SetReadyFlag( false, null );
//             _elapsedSeconds = 0f;
//         }
//
//         protected override void OnShow( object userData )
//         {
//             base.OnShow( userData );
//         }
//
//         protected override void OnRecycle()
//         {
//             Reset();
//             base.OnRecycle();
//         }
//
//         protected override void OnInit( object userData )
//         {
//             base.OnInit( userData );
//         }
//
//         public virtual bool HitCorrectTarget( object obj )
//         {
//             var go = obj as GameObject;
//             if ( go == null )
//                 return false;
//
//             //return _targetTransform != null && Vector3.Distance( _targetTransform.position , CachedTransform.position ) <= .1f;
//             return go.transform == _targetTransform;
//         }
//
//         /// <summary>
//         /// 准备状态，要求标记位为true且目标transform不为空
//         /// </summary>
//         public bool ReadyFlag => _readyFlag && _targetTransform != null && _target_actor_id != -1;
//
//         /// <summary>
//         /// 准备标记，完成后才会开始移动
//         /// </summary>
//         private bool _readyFlag = false;
//
//         /// <summary>
//         /// 目标transform
//         /// </summary>
//         private Transform _targetTransform = null;
//
//         /// <summary>
//         /// 流逝时间
//         /// </summary>
//         private float _elapsedSeconds = 0f;
//         #endregion
//     }
//
//     public class ProjectileActorEntityData : EntityData
//     {
//         public ProjectileActorEntityData( int entity_id ) : base( entity_id, typeof( ProjectileActor ).GetHashCode() )
//         {
//         }
//     }
// }