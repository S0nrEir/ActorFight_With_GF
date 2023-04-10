using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.ToolKit;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Actor
{
    public class HeroActor :
        TActorBase,
        INavMoveBehavior,
        ISwitchStateBehavior,
        IDoAbilityBehavior,
        ITakeDamageBehavior,
        IPathMoveBehavior,
        IDieBehavior
    {
        #region event

        /// <summary>
        /// 导航到达目标点
        /// </summary>
        private void OnNavArriveTarget( int eventID, object[] param )
        {
            SwitchTo( ActorStateTypeEnum.IDLE_STATE, null, null );
        }

        /// <summary>
        /// 移动到了最后一个路点
        /// </summary>
        private void OnArriveFinalPoint( int eventID, object[] param )
        {
            SwitchTo( ActorStateTypeEnum.IDLE_STATE, null, null );
        }

        /// <summary>
        /// 技能完成
        /// </summary>
        private void OnAbilityFinish( int eventID, object[] param )
        {
            SwitchTo( ActorStateTypeEnum.IDLE_STATE, null, null );
        }

        /// <summary>
        /// 特效完成
        /// </summary>
        private void OnEffectTimsUp( int eventID, object[] param )
        {
            if ( param is null || param.Length == 0 )
                return;

            var actorEffect = param[0] as ActorEffect;
            if ( actorEffect is null )
                return;

            _effect_addon?.Hide( actorEffect );
        }

        #endregion

        #region impl
        public void Die()
        {
            SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
        }

        public void TakeDamage( int dmg )
        {
        }

        /// <summary>
        /// 路点移动，使用moveAddon不用navMesh了
        /// </summary>
        public void Move( IList<float> xList, IList<float> zList )
        {
            if ( xList is null || zList is null )
            {
                Debug.LogError( "xList is null || zList is null" );
                return;
            }

            SwitchTo( ActorStateTypeEnum.MOVE_STATE, new object[] { xList, zList }, null );
        }

        public void MoveTo( float targetX, float targetZ )
        {
            //#TODO参数缓存列表后续更新
            SwitchTo
                (
                    ActorStateTypeEnum.MOVE_STATE,
                    new object[]
                    {
                        new Vector3
                        (
                            targetX,
                            Tools.Fight.TerrainPositionY(string.Empty,targetX,targetZ,0) ,//#todo修改layer
                            targetZ
                        )
                    },
                    null
                );
        }

        /// <summary>
        /// switch state
        /// </summary>
        public void SwitchTo( ActorStateTypeEnum stateType, object[] enterParam, object[] existParam )
        {
            _fsm_addon.SwitchTo( stateType, enterParam, existParam );
        }

        /// <summary>
        /// 使用ability行动
        /// </summary>
        public void DoAbilityAction()
        {
        }

        #endregion

        public ActorStateTypeEnum CurrState => _fsm_addon.CurrState;

        #region public methods

        #endregion


        #region override

        public override ActorTypeEnum ActorType => ActorTypeEnum.HERO;

        protected override void InitAddons()
        {
            base.InitAddons();
            _fsm_addon          = AddAddon<Addon_HeroState>();
            _anim_addon         = AddAddon<Addon_Anim>();
            _move_addon         = AddAddon<Addon_Move>();
            _hp_addon           = AddAddon<Addon_InfoBoard>();
            _nav_addon          = AddAddon<Addon_Nav>();
            _effect_addon       = AddAddon<Addon_Effect>();
            _base_attr_addon    = AddAddon<Addon_BaseAttrNumric>();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();

            UnRegisterActorEvent( ActorEventEnum.NAV_ARRIVE_TARGET );
            UnRegisterActorEvent( ActorEventEnum.MOVE_TO_FINAL_POINT );
            UnRegisterActorEvent( ActorEventEnum.ABILITY_FINISH );
            UnRegisterActorEvent( ActorEventEnum.EFFECT_TIMES_UP );
        }

        /// <summary>
        /// 轮询每帧做战斗状态组件更新
        /// </summary>
        protected override void OnUpdate( float elapseSeconds, float realElapseSeconds )
        {
            base.OnUpdate( elapseSeconds, realElapseSeconds );
            _fsm_addon?.OnUpdateDate( elapseSeconds, realElapseSeconds );
        }

        /// <summary>
        /// 检查
        /// </summary>
        protected override bool OnPreAbilityAction( int abilityID )
        {
            Debug.Log( "OnPreAbilityAction hero Actor" );
            if ( _fsm_addon.CurrState != ActorStateTypeEnum.IDLE_STATE )
                return false;

            return true;
        }

        protected override bool OnAfterAbilityAction()
        {
            return base.OnAfterAbilityAction();
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
            RegisterActorEvent( ActorEventEnum.NAV_ARRIVE_TARGET, OnNavArriveTarget );
            RegisterActorEvent( ActorEventEnum.MOVE_TO_FINAL_POINT, OnArriveFinalPoint );
            RegisterActorEvent( ActorEventEnum.ABILITY_FINISH, OnAbilityFinish );
            RegisterActorEvent( ActorEventEnum.EFFECT_TIMES_UP, OnEffectTimsUp );
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            base.OnHide( isShutdown, userData );
        }

        #endregion

        #region addon

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

        #endregion

        #region message

        /// <summary>
        /// 地图数据更新
        /// </summary>
        private void OnMapUpdate( object sender, GameEventArgs e )
        {

        }

        #endregion
    }

    public class HeroActorEntityData : EntityData
    {
        public HeroActorEntityData( int entityId ) : base( entityId, typeof( HeroActor ).GetHashCode() )
        {
        }
    }
}