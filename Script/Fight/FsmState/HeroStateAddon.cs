using Aquila.Fight.Addon;
using System.Collections.Generic;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 英雄状态addon by yhc
    /// </summary>
    public class HeroStateAddon : FSMAddon
    {
        public override List<ActorStateBase> StateList => new List<ActorStateBase>
        {
            new HeroIdleState((int)ActorStateTypeEnum.IDLE_STATE),
            new HeroMoveState((int)ActorStateTypeEnum.MOVE_STATE),
            new HeroAbilityState((int)ActorStateTypeEnum.ABILITY_STATE),
            new HeroDieState((int)ActorStateTypeEnum.DIE_STATE),
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

    /// <summary>
    /// 待机状态
    /// </summary>
    public class HeroIdleState : ActorIdleStateBase
    {
        public HeroIdleState( int stateID ) : base( stateID )
        { }
    }

    /// <summary>
    /// 移动状态
    /// </summary>
    public class HeroMoveState : ActorMoveStateBase
    {
        public HeroMoveState( int stateID ) : base( stateID )
        {

        }
    }

    /// <summary>
    /// 技能状态
    /// </summary>
    public class HeroAbilityState : ActorAbilityStateBase
    {
        public HeroAbilityState( int stateID ) : base( stateID )
        {
        }
    }

    /// <summary>
    /// 英雄死亡状态
    /// </summary>
    public class HeroDieState : ActorDieStateBase
    {
        public HeroDieState( int stateID ) : base( stateID )
        {
        }
    }
}