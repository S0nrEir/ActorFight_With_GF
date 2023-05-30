using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.Toolkit;
using GameFramework.Event;
using System.Collections.Generic;
using Aquila.Module;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    public partial class HeroActor : TActorBase
    {
        public ActorStateTypeEnum CurrState => _fsm_addon.CurrState;
        public override ActorTypeEnum ActorType => ActorTypeEnum.HERO;
        
        protected override void OnInitActor(object user_data)
        {
            if(user_data is HeroActorEntityData)
                Setup((user_data as HeroActorEntityData)._role_meta_id);
        }
        
        protected override void AddAddon()
        {
            base.AddAddon();
            _base_attr_addon = AddAddon<Addon_BaseAttrNumric>();
            _data_addon      = AddAddon<Addon_Data>();
            _ability_addon   = AddAddon<Addon_Ability>();
            _fsm_addon       = AddAddon<Addon_HeroState>();
            _anim_addon      = AddAddon<Addon_Anim>();
            _move_addon      = AddAddon<Addon_Move>();
            // _info_addon      = AddAddon<Addon_InfoBoard>();
            //_nav_addon        = AddAddon<Addon_Nav>();
            _fx_addon        = AddAddon<Addon_FX>();
            _hp_addon        = AddAddon<Addon_HP>();
        }
        
        protected override void InitAddons(Module_Proxy_Actor.ActorInstance instance)
        {
            base.InitAddons(instance);
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
            //更新状态机
            _fsm_addon?.OnUpdate( elapseSeconds, realElapseSeconds );
            //更新技能系统数据（CD等）
            _ability_addon?.OnUpdate(elapseSeconds,realElapseSeconds);
            //信息板位置更新
            _hp_addon?.OnUpdate(elapseSeconds,realElapseSeconds);
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
        // private Addon_InfoBoard _info_addon { get; set; } = null;
        
        /// <summary>
        /// 导航组件
        /// </summary>
        private Addon_Nav _nav_addon { get; set; } = null;

        /// <summary>
        /// 特效组件
        /// </summary>
        private Addon_FX _fx_addon { get; set; } = null;

        /// <summary>
        /// 数据组件
        /// </summary>
        private Addon_Data _data_addon { get; set; } = null;

        /// <summary>
        /// 基础属性数值组件
        /// </summary>
        private Addon_BaseAttrNumric _base_attr_addon { get; set; } = null;

        /// <summary>
        /// 技能组件
        /// </summary>
        private Addon_Ability _ability_addon = null;

        /// <summary>
        /// 血条显示组件
        /// </summary>
        private Addon_HP _hp_addon = null;
    }

    public class HeroActorEntityData : EntityData
    {
        public HeroActorEntityData( int entity_id ) : base( entity_id, typeof( HeroActor ).GetHashCode() )
        {
        }

        /// <summary>
        /// 角色role meta表id
        /// </summary>
        public int _role_meta_id = -1;
    }
}