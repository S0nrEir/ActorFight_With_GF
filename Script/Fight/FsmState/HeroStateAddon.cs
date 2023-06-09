using Aquila.Fight.Addon;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;
using Aquila.Toolkit;
using Cfg.Fight;
using Aquila.Event;
using Aquila.Module;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 英雄状态addon by yhc
    /// </summary>
    public class Addon_HeroState : Addon_FSM
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
    public class HeroIdleState : ActorStateBase
    {
        public HeroIdleState( int stateID ) : base( stateID )
        { }
    }

    /// <summary>
    /// 移动状态
    /// </summary>
    public class HeroMoveState : ActorStateBase
    {
        public HeroMoveState( int stateID ) : base( stateID )
        {

        }
    }

    /// <summary>
    /// 使用技能状态
    /// </summary>
    public class HeroAbilityState : ActorStateBase
    {
        /// <summary>
        /// 检查技能数据是否合法
        /// </summary>
        private bool IsAbilityDataValid( object[] param )
        {
            int state = 0;
            var succ = true;
            if ( param is null || param.Length == 0 )
            {
                Log.Warning( "<color=yellow>HeroStateAddon.OnEnter()--->param is null || param.Length == 0</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_TIMELINE_META, true );
                return false;
            }
            var result = param[0] as AbilityResult_Use;
            _abilityMeta = GameEntry.DataTable.Tables.Ability.Get(result._abilityID);
            if ( _abilityMeta is null )
            {
                Log.Warning( "<color=yellow>HeroStateAddon.OnEnter()--->_abilityMeta is null</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_ABILITY_META, true );
            }

            _timelineMeta = GameEntry.DataTable.Tables.AbilityTimeline.Get( _abilityMeta.Timeline );
            if ( _timelineMeta is null )
            {
                Log.Warning( "<color=yellow>HeroStateAddon.OnEnter()--->timeline meta is null</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_TIMELINE_META, true );
            }
            //检查CD和消耗
            var abilityAddon = _fsm.GetActorInstance().GetAddon<Addon_Ability>();
            if ( abilityAddon is null )
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_PARAM, true );
            

            var canUseFlag = abilityAddon.CanUseAbility( _abilityMeta.id );
            if( canUseFlag != 0)
                state = Tools.SetBitValue( state, (ushort)canUseFlag , true );
            //#todo检查施法者和目标

            //到最后设置succ
            result._stateDescription = state;
            if ( result.StateFlagIsClean() )
            {
                result._stateDescription = Tools.SetBitValue( result._stateDescription, ( int ) AbilityUseResultTypeEnum.SUCC, true );
                result._succ = true;
            }
            else
            {
                result._succ = false;
            }

            _castorID = result._castorID;
            _targetID = result._targetID;
            return result._succ;
        }

        /// <summary>
        /// 尝试使用技能，到触发时间使用技能
        /// </summary>
        private void TryUseAbility(float deltaTime)
        {
            _time += deltaTime;
            if ( !_abilityFinishFlag && _time >= _timelineMeta.TriggerTime )
            {
                var abilityAddon = _fsm.GetActorInstance().GetAddon<Addon_Ability>();
                if ( abilityAddon is null )
                {
                    Log.Warning( "<color=yellow>HeroStateAddon.OnUpdate--->abilityAddon is null </color>" );
                    return;
                }
                //continue:这里走公用接口，将效果施加到actor上
                GameEntry.Module.GetModule<Module_ProxyActor>().ApplyEffect2Actor( _castorID, _targetID, _abilityMeta.id );
                _abilityFinishFlag = true;
            }
        }

        /// <summary>
        /// 技能完成检查代码
        /// </summary>
        private void FinishAbility()
        {
            if ( _time >= _timelineMeta.Duration )
                _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );
        }

        public override void OnEnter(params object[] param)
        {
            base.OnEnter(param);
            if ( !IsAbilityDataValid( param ) )
                _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );

            _time = 0f;
            _abilityFinishFlag = false;
            GameEntry.Timeline.Play( _timelineMeta.AssetPath, Tools.GetComponent<PlayableDirector>( _actor.transform ) );
        }

        public override void OnUpdate( float deltaTime )
        {
            base.OnUpdate( deltaTime );
            TryUseAbility(deltaTime);
            FinishAbility();
        }
        public override void OnLeave(params object[] param)
        {
            base.OnLeave(param);
            //#todo施法结束回调
            _timelineMeta = null;
            _abilityMeta = null;
            _castorID = -1;
            _targetID = -1;
        }

        
        public HeroAbilityState( int state_id ) : base( state_id )
        {
            
        }

        /// <summary>
        /// 施法者ActorID
        /// </summary>
        private int _castorID = -1;

        /// <summary>
        /// 目标ActorID
        /// </summary>
        private int _targetID = -1;

        /// <summary>
        /// 技能数据
        /// </summary>
        private Table_AbilityBase _abilityMeta = null;

        /// <summary>
        /// 技能timeline配置
        /// </summary>
        private Table_AbilityTimeline _timelineMeta = null;

        /// <summary>
        /// 技能完成标记
        /// </summary>
        private bool _abilityFinishFlag = false;

        /// <summary>
        /// 进入该状态时间
        /// </summary>
        private float _time = 0f;
    }

    /// <summary>
    /// 英雄死亡状态
    /// </summary>
    public class HeroDieState : ActorStateBase
    {
        public HeroDieState( int state_id ) : base( state_id )
        {
        }
    }
}