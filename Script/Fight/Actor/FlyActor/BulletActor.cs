using UnityEngine;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 子弹类actor
    /// </summary>
    public class BulletActor : FlyActor
    , ITriggerHitBehavior
    {
        public bool HitCorrectTarget( object obj )
        {
            return true;
        }

        public void SetTriggerLmt( int lmt )
        {
            if ( _triggerAddon is null )
                return;

            _triggerAddon.SetTriggerLmt( lmt );
        }

        protected override void ColliderTriggerHit( int evnetID, object[] param )
        {
            //_readyFlag = false;
        }

        protected override void OnColliderTriggerCountLmt( int evnetID, object[] param )
        {
            //_readyFlag = false;
        }

        /// <summary>
        /// 设定bullet的方向，速度默认从dataAddon拿,方向归一化
        /// </summary>
        public void SetDirection( Vector3 direction )
        {
            var x = direction.x * DIRECTION_OFFSET;
            var z = direction.z * DIRECTION_OFFSET;
            //var speed = _dataAddon.GetNumricValue( Addon.DataAddonFieldTypeEnum.NUM_MOVE_SPEED, 1000f );
            //_moveAddon.SetSpeed( speed / ( float ) 1000 );
            //var radius = _dataAddon.GetNumricValue( Addon.DataAddonFieldTypeEnum.NUM_RADIUS, 1f );
            //_triggerAddon.SetSize( new Vector3( radius, radius, radius ) );
            _targetPos = CachedTransform.position + new Vector3( x, 0, z );
            _readyFlag = true;
        }

        public override ActorTypeEnum ActorType => ActorTypeEnum.Bullet;

        protected override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            base.OnUpdate( elapseSeconds, realElapseSeconds );
            if ( !_readyFlag || _targetPos == Vector3.zero )
                return;

            _moveAddon.MoveTo( CachedTransform.position, _targetPos, elapseSeconds );
        }

        protected override void InitAddons( object user_data )
        {
            base.InitAddons( user_data );
        }

        public override void Reset()
        {
            _readyFlag = false;
            _targetPos = Vector3.zero;
            base.Reset();
        }

        protected override void OnShow( object userData )
        {
            base.OnShow( userData );
            Reset();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
        }

        private bool _readyFlag = false;

        /// <summary>
        /// 方向
        /// </summary>
        private Vector3 _targetPos = Vector3.zero;

        /// <summary>
        /// 方向偏移
        /// </summary>
        private const float DIRECTION_OFFSET = 999f;
    }

    public class BulletActorEntityData : EntityData
    {
        public BulletActorEntityData( int entityID ) : base( entityID, typeof( BulletActor ).GetHashCode() )
        {

        }
    }

}