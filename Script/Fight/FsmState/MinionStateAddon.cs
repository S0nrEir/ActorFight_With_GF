using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace MRG.Fight.FSM
{
    /// <summary>
    /// 随从类状态机插件
    /// </summary>
    public class MinionStateAddon : FSMAddon
    {
        public override List<ActorStateBase> StateList => new List<ActorStateBase>
        {
            new MinionIdleState((int)ActorStateTypeEnum.IDLE_STATE),
            new MinionAtkState((int)ActorStateTypeEnum.ABILITY_STATE),
            new MinionMoveState((int)ActorStateTypeEnum.MOVE_STATE)
        };

        public override void Reset()
        {
            base.Reset();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }

    #region 移动

    public class MinionMoveState : ActorMoveStateBase
    {
        public MinionMoveState( int stateID ) : base( stateID )
        { }
    }

    #endregion

    #region 攻击

    public class MinionAtkState : ActorAbilityStateBase
    {
        public MinionAtkState( int stateID ) : base( stateID )
        { }
    }

    #endregion

    #region 索敌

    public class MinionSearchingState : ActorStateBase
    {
        public MinionSearchingState( int stateID ) : base( stateID )
        { }

        public override void Init( ActorFSM fsm, TActorBase actor )
        {
            base.Init( fsm, actor );
        }

        public override void OnEnter( params object[] param )
        {
            base.OnEnter( param );
            if ( param is null )
                return;

            if ( !_actor.TryGetAddon( out _navAddon ) )
                throw new GameFrameworkException( "minion state addon _navAddon == null" );

            if ( !_actor.TryGetAddon<DataAddon>( out var dataAddon ) )
                throw new GameFrameworkException( "minion state addon dataAddon == null" );

            _targetTransform = param[0] as Transform;
            if ( _targetTransform == null )
                throw new GameFrameworkException( "minion state addon _targetTransform == null" );

            _navAddon.SetDestination( _targetTransform.position );
            _radius = dataAddon.GetFloatDataValue( DataAddonFieldTypeEnum.FLOAT_ALERT_RADIUS, 0f );
            if ( _radius <= 0f )
                Debug.LogError( "minion state addon radius == 0f" );
        }

        public override void OnLeave( params object[] param )
        {
            base.OnLeave( param );

            _targetTransform = null;
            _navAddon = null;
            _radius = 0f;
        }

        public override void OnUpdate( float deltaTime )
        {
            base.OnUpdate( deltaTime );
            if ( Utils.Fight.IsEnterRadius( _actor.CachedTransform.position, _targetTransform.position, _radius ) )
                _actor.Trigger( ActorEventEnum.NAV_ARRIVE_TARGET, null );
        }

        private float _radius = 0f;

        /// <summary>
        /// 索敌目标
        /// </summary>
        private Transform _targetTransform = null;
        private NavAddon _navAddon = null;
    }

    #endregion

    #region 待机
    public class MinionIdleState : ActorIdleStateBase
    {
        public MinionIdleState( int stateID ) : base( stateID )
        { }
    }
    #endregion

    #region 胜利

    public class MinionWinState : ActorStateBase
    {
        public MinionWinState( int stateID ) : base( stateID )
        { }

        public override void Init( ActorFSM fsm, TActorBase actor )
        {
            base.Init( fsm, actor );
        }

        public override void OnEnter( params object[] param )
        {
            base.OnEnter( param );

        }

        public override void OnLeave( params object[] param )
        {
            base.OnLeave( param );
        }

        public override void OnUpdate( float deltaTime )
        {
            base.OnUpdate( deltaTime );
        }
    }

    #endregion

}
