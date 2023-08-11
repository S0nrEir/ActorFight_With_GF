using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.Module;
using Cfg.Enum;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    public partial class Actor_Hero : Actor_Base
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        public ActorStateTypeEnum CurrState => _fsmAddon.CurrState;

        public override RoleType ActorType => RoleType.Hero;

        protected override void AddAddon()
        {
            base.AddAddon();
            _baseAttrAddon  = AddAddon<Addon_BaseAttrNumric>();
            _abilityAddon   = AddAddon<Addon_Ability>();
            _fsmAddon       = AddAddon<Addon_HeroFSM>();
            _hpAddon        = AddAddon<Addon_HP>();
            _timelineAddon  = AddAddon<Addon_Timeline>();
            _behaviourAddon = AddAddon<Addon_Behaviour>();
        }

        protected override void InitAddons( Module_ProxyActor.ActorInstance instance )
        {
            base.InitAddons( instance );

            _behaviourAddon.AddBehaviour( ActorBehaviourTypeEnum.ABILITY );
            _behaviourAddon.AddBehaviour( ActorBehaviourTypeEnum.DIE );
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

        protected override void OnTagChange( long tag, long changedTag, bool isADD )
        {
            Log.Info( $"<color=green>tag changed!,tag:{tag},changedTag:{changedTag},is add:{isADD}</color>" );
        }

        //----------------addon----------------
        /// <summary>
        /// 状态机组件
        /// </summary>
        private Addon_FSM _fsmAddon { get; set; } = null;

        /// <summary>
        /// 移动组件
        /// </summary>
        private Addon_Move _moveAddon { get; set; } = null;

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

        /// <summary>
        /// 行为组件
        /// </summary>
        private Addon_Behaviour _behaviourAddon = null;
    }

    public class HeroActorEntityData : Actor_Base_EntityData
    {
        public HeroActorEntityData( int entityID ) : base( entityID, typeof( Actor_Hero ).GetHashCode() )
        {
        }
    }
}