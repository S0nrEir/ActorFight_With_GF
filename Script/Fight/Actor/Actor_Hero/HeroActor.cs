using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.Module;

namespace Aquila.Fight.Actor
{
    public partial class Actor_Hero : Actor_Base
    {
        public ActorStateTypeEnum CurrState => _fsmAddon.CurrState;
        public override ActorTypeEnum ActorType => ActorTypeEnum.HERO;
        
        protected override void OnInitActor(object userData)
        {
            if( userData is HeroActorEntityData)
                Setup(( userData as HeroActorEntityData )._roleMetaID );
        }
        
        protected override void AddAddon()
        {
            base.AddAddon();
            _baseAttrAddon      = AddAddon<Addon_BaseAttrNumric>();
            _abilityAddon       = AddAddon<Addon_Ability>();
            _fsmAddon           = AddAddon<Addon_HeroFSM>();
            //_anim_addon       = AddAddon<Addon_Anim>();
            // _move_addon      = AddAddon<Addon_Move>();
            // _info_addon      = AddAddon<Addon_InfoBoard>();
            //_nav_addon        = AddAddon<Addon_Nav>();
            _fxAddon            = AddAddon<Addon_FX>();
            _hpAddon            = AddAddon<Addon_HP>();
            _timelineAddon      = AddAddon<Addon_Timeline>();
        }
        
        protected override void InitAddons( Module_ProxyActor.ActorInstance instance)
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
            _fsmAddon?.OnUpdate( elapseSeconds, realElapseSeconds );
            //更新技能数据（CD之类的）
            _abilityAddon?.OnUpdate(elapseSeconds,realElapseSeconds);
            //信息板位置更新
            _hpAddon?.OnUpdate(elapseSeconds,realElapseSeconds);
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
        private Addon_FSM _fsmAddon { get; set; } = null;

        /// <summary>
        /// 动画组件
        /// </summary>
        //private Addon_Anim _anim_addon { get; set; } = null;

        /// <summary>
        /// 移动组件
        /// </summary>
        private Addon_Move _moveAddon { get; set; } = null;

        /// <summary>
        /// 信息板组件
        /// </summary>
        // private Addon_InfoBoard _info_addon { get; set; } = null;
        
        /// <summary>
        /// 导航组件
        /// </summary>
        private Addon_Nav _navAddon { get; set; } = null;

        /// <summary>
        /// 特效组件
        /// </summary>
        private Addon_FX _fxAddon { get; set; } = null;

        /// <summary>
        /// 基础属性数值组件
        /// </summary>
        private Addon_BaseAttrNumric _baseAttrAddon { get; set; } = null;

        /// <summary>
        /// 技能组件
        /// </summary>
        private Addon_Ability _abilityAddon = null;

        /// <summary>
        /// 血条显示组件
        /// </summary>
        private Addon_HP _hpAddon = null;

        /// <summary>
        /// timeline组件
        /// </summary>
        private Addon_Timeline _timelineAddon = null;
    }

    public class HeroActorEntityData : EntityData
    {
        public HeroActorEntityData( int entityID ) : base( entityID, typeof( Actor_Hero ).GetHashCode() )
        {
        }

        /// <summary>
        /// 角色role meta表id
        /// </summary>
        public int _roleMetaID = -1;
    }
}