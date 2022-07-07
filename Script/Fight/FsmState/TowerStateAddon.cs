using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 防御塔状态
    /// </summary>
    public class TowerStateAddon : FSMAddon
    {
        public override List<ActorStateBase> StateList => new List<ActorStateBase>
        {
            new TowerIdleState((int)ActorStateTypeEnum.IDLE_STATE),
            new TowerAbilityState((int)ActorStateTypeEnum.ABILITY_STATE),
            new TowerDieState((int)ActorStateTypeEnum.DIE_STATE)
        };

        public override void Reset ()
        {
            base.Reset();
        }

        public override void Dispose ()
        {
            base.Dispose();
        }
    }

    /// <summary>
    /// 待机状态
    /// </summary>
    public class TowerIdleState : ActorIdleStateBase
    {
        public TowerIdleState ( int stateID ) : base( stateID )
        { }
    }

    /// <summary>
    /// 攻击状态
    /// </summary>
    public class TowerAbilityState : ActorAbilityStateBase
    {
        //#todo防御塔的技能逻辑和别的不一样
        public TowerAbilityState ( int stateID ) : base( stateID )
        { }

        public override void OnEnter ( params object[] param )
        {
            Debug.Log("<color=white>TowerState---->OnEnter</color>");
            //base.OnEnter( param );

        }

        public override void OnLeave ( params object[] param )
        {
            //base.OnLeave( param );
            Debug.Log( "<color=white>TowerState---->OnLeave</color>" );
        }

        public override void OnUpdate ( float deltaTime )
        {
            //base.OnUpdate( deltaTime );
            Debug.Log( "<color=white>TowerState---->OnLeave</color>" );
        }

    }

    /// <summary>
    /// 待机状态
    /// </summary>
    public class TowerDieState : ActorDieStateBase
    {
        public TowerDieState ( int stateID ) : base( stateID )
        {
        }

        public override void OnEnter ( params object[] param )
        {
            base.OnEnter( param );
            //#todo防御塔的死亡状态暂时弄成这样的，因为动画的问题需要先跟美术同步
            //Utils.SetActive( _actor.gameObject, false );
            if (!_actor.TryGetAddon<InfoBoardAddon>( out var sliderAddon ))
            {
                sliderAddon.ChangeHPValue( 0f );
                sliderAddon.SetEnable( false );
            }

            if (!_actor.TryGetAddon<DataAddon>( out var dataAddon ))
            {
                var hp = dataAddon.GetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, -1 );
                if (hp != -1)
                    dataAddon.GetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, 0 );
            }
            Debug.Log( $"<color=white>TowerActor.EnterDie</color>" );
        }
    }
}