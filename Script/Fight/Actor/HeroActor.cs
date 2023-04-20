using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.ToolKit;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    public class HeroActor :
        TActorBase,
        ISwitchStateBehavior,
        IDieBehavior
    {

        #region impl
        public void Die()
        {
            SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
        }

        public void TakeDamage( int dmg )
        {
        }

        /// <summary>
        /// switch state
        /// </summary>
        public void SwitchTo( ActorStateTypeEnum stateType, object[] enterParam, object[] existParam )
        {
            _fsm_addon.SwitchTo( stateType, enterParam, existParam );
        }

        #endregion

        public ActorStateTypeEnum CurrState => _fsm_addon.CurrState;

        #region public methods

        #endregion


        #region override

        public override ActorTypeEnum ActorType => ActorTypeEnum.HERO;

        protected override void InitAddons(object user_data)
        {
            base.InitAddons( user_data );
            if ( !( user_data is HeroActorEntityData ) )
            {
                Log.Warning( "user_data is not HeroActorEntityData" );
                return;
            }
            var data = user_data as HeroActorEntityData;
            Setup( data._role_meta_id );

            _base_attr_addon    = AddAddon<Addon_BaseAttrNumric>();
            _data_addon         = AddAddon<Addon_Data>();
            _fsm_addon          = AddAddon<Addon_HeroState>();
            _anim_addon         = AddAddon<Addon_Anim>();
            _move_addon         = AddAddon<Addon_Move>();
            _hp_addon           = AddAddon<Addon_InfoBoard>();
            //_nav_addon          = AddAddon<Addon_Nav>();
            _effect_addon       = AddAddon<Addon_Effect>();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        /// <summary>
        /// 轮询每帧做战斗状态组件更新
        /// </summary>
        protected override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            base.OnUpdate( elapseSeconds, realElapseSeconds );
            _fsm_addon?.OnUpdateDate( elapseSeconds, realElapseSeconds );
        }

        protected override void Register()
        {
            base.Register();
        }

        protected override void UnRegister()
        {
            base.UnRegister();

        }

        public override void Reset()
        {
            base.Reset();
        }
        protected override void OnShow( object userData )
        {
            base.OnShow( userData );
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            base.OnHide( isShutdown, userData );
        }

        #endregion
        
        //----------------addon----------------
        /// <summary>
        /// 状态机组件
        /// </summary>
        private Addon_FSM _fsm_addon { get; set; } = null;

        /// <summary>
        /// 动画组件
        /// </summary>
        private Addon_Anim _anim_addon { get; set; } = null;

        /// <summary>
        /// 移动组件
        /// </summary>
        private Addon_Move _move_addon { get; set; } = null;

        /// <summary>
        /// 信息板组件
        /// </summary>
        private Addon_InfoBoard _hp_addon { get; set; } = null;

        /// <summary>
        /// 导航组件
        /// </summary>
        private Addon_Nav _nav_addon { get; set; } = null;

        /// <summary>
        /// 特效组件
        /// </summary>
        private Addon_Effect _effect_addon { get; set; } = null;

        /// <summary>
        /// 数据组件
        /// </summary>
        private Addon_Data _data_addon { get; set; } = null;

        /// <summary>
        /// 基础属性数值组件
        /// </summary>
        private Addon_BaseAttrNumric _base_attr_addon { get; set; } = null;
    }

    public class HeroActorEntityData : EntityData
    {
        public HeroActorEntityData( int entityId ) : base( entityId, typeof( HeroActor ).GetHashCode() )
        {
        }

        /// <summary>
        /// 角色role meta表id
        /// </summary>
        public int _role_meta_id = -1;
    }
}