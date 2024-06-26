using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.Toolkit;
using Cfg.Enum;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 触发类actor，用于陷阱
    /// </summary>
    public class Actor_Trigger :
        Actor_Base,
        ISwitchStateBehavior,
        ITriggerHitBehavior
    {
        /// <summary>
        /// 触发
        /// </summary>
        public void Trigger( string asset_path, float duration )
        {
            _effectAddon.ShowEffectAsync
                (
                    ActorIDPool.Gen(),
                    asset_path,
                    duration,
                    ( effectEntityData, effect ) => Tools.Fight.BindEffect( effectEntityData, effect )
                );
        }

        /// <summary>
        /// 取消
        /// </summary>
        public void Leave()
        {
            _effectAddon.HideAll();
        }

        /// <summary>
        /// 别的object进入trigger
        /// </summary>
        private void ColliderTriggerHit( int evnetID, object[] param )
        {
            Log.Info( $"<color=white>TriggerActor---->ColliderTriggerHit<color=white>" );
        }

        /// <summary>
        /// 特效时间到了
        /// </summary>
        private void OnEffectTimsUp( int eventID, object[] param )
        {
            Log.Info( $"<color=white>TriggerActor---->OnEffectTimsUp<color=white>" );
        }

        #region impl
        public bool HitCorrectTarget( object obj )
        {
            return true;
        }

        /// <summary>
        /// 状态切换
        /// </summary>
        public void SwitchTo( ActorStateTypeEnum stateType, object enterParam, object existParam )
        {
            Log.Info( $"<color=white>TriggerActor--->SwitchTo,ActorID:{ActorID}<color>" );
        }

        #endregion

        #region override

        /// <summary>
        /// 设置信息
        /// </summary>
        //public void Setup
        //    (
        //        string tag,
        //        int actor_id,
        //        (float x, float z) wh
        //    )
        //{
        //    Setup( tag );
        //    _triggerAddon.SetSize( new Vector3( wh.x, 1f, wh.z ) );
        //}

        public void SetSize( (float x, float z) wh )
        {
            if ( _triggerAddon != null )
                return;

            _triggerAddon.SetSize( new Vector3( wh.x, 1f, wh.z ) );
        }

        public override RoleType ActorType => RoleType.Hero;

        protected override void OnShow( object userData )
        {
            base.OnShow( userData );
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        public override void Reset()
        {
            base.Reset();
            _triggerAddon.SetTriggerLmt( int.MaxValue );
        }

        // protected override void InitAddons( object user_data )
        // {
        //     base.InitAddons( user_data );
        // }

        protected override void AddAddon()
        {
            base.AddAddon();
            _triggerAddon = AddAddon<Addon_ColliderTrigger>();
            _effectAddon = AddAddon<Addon_FX>();
        }

        #endregion

        #region fields

        /// <summary>
        /// 碰撞
        /// </summary>
        private Addon_ColliderTrigger _triggerAddon = null;

        /// <summary>
        /// 特效组件
        /// </summary>
        private Addon_FX _effectAddon = null;

        #endregion
    }

    public class TriggerActorEntityData : EntityData
    {
        public Vector3 centerPos = Vector3.zero;
        public int index = -1;
        public (float x, float y) stepWH;

        public TriggerActorEntityData( int entityID ) : base( entityID, typeof( Actor_Trigger ).GetHashCode() )
        {
        }
    }

}