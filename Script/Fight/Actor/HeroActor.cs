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

            _EffectAddon?.Hide( actorEffect );
        }

        #endregion

        #region impl
        public void Die()
        {
            SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
        }

        public void TakeDamage( int dmg )
        {
            //var currHp = _dataAddon.GetNumricValue( DataAddonFieldTypeEnum.NUM_CURR_HP, 0 );
            //currHp -= dmg;
            ////写入当前hp
            //_dataAddon.SetNumricValue( DataAddonFieldTypeEnum.NUM_CURR_HP, currHp );
            //if ( currHp <= 0 )
            //{
            //    SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
            //    return;
            //}
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
            _FsmAddon.SwitchTo( stateType, enterParam, existParam );
        }

        /// <summary>
        /// 使用ability行动
        /// </summary>
        public void DoAbilityAction()
        {
        }

        #endregion

        public ActorStateTypeEnum CurrState => _FsmAddon.CurrState;

        #region public methods

        #endregion


        #region override

        public override ActorTypeEnum ActorType => ActorTypeEnum.HERO;

        protected override void InitAddons()
        {
            base.InitAddons();
            _FsmAddon          = AddAddon<Addon_HeroState>();
            //_ProcessorAddon    = AddAddon<ProcessorAddon>();
            _AnimAddon         = AddAddon<Addon_Anim>();
            _MoveAddon         = AddAddon<Addon_Move>();
            _HPAddon           = AddAddon<Addon_InfoBoard>();
            _NavAddon          = AddAddon<Addon_Nav>();
            _EffectAddon       = AddAddon<Addon_Effect>();
            //_MapAddon        = AddAddon<MapAddon>();

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
            _FsmAddon?.OnUpdateDate( elapseSeconds, realElapseSeconds );
        }

        /// <summary>
        /// 检查
        /// </summary>
        protected override bool OnPreAbilityAction( int abilityID )
        {
            Debug.Log( "OnPreAbilityAction hero Actor" );
            //目前只有idle才可以放技能
            if ( _FsmAddon.CurrState != ActorStateTypeEnum.IDLE_STATE )
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

        protected override void ResetData()
        {
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

        private Addon_FSM _FsmAddon { get; set; } = null;
        //private ProcessorAddon _ProcessorAddon { get; set; } = null;
        private Addon_Anim _AnimAddon { get; set; } = null;
        private Addon_Move _MoveAddon { get; set; } = null;
        private Addon_InfoBoard _HPAddon { get; set; } = null;
        private Addon_Nav _NavAddon { get; set; } = null;
        private Addon_Effect _EffectAddon { get; set; } = null;
        private Addon_Data _DataAddon { get; set; } = null;
        //private MapAddon _MapAddon { get; set; } = null;

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