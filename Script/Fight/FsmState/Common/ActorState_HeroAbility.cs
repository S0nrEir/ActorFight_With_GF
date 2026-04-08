using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Fight;
using GameFramework;
using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 使用技能状态
    /// </summary>
    public class ActorState_HeroAbility : ActorState_Base
    {
        /// <summary>
        /// 尝试在触发时间点释放技能效果
        /// </summary>
        // private void TryUseAbility( float deltaTime )
        // {
        //     _time += deltaTime;
        //     if ( _abilityFinishFlag )
        //         return;
        //
        //     if ( _time > _timelineMeta.Duration )
        //         return;
        //
        //     var effects = _abilityData.GetEffects();
        //     if ( effects is null || effects.Count == 0 )
        //         return;
        //     
        //     while ( _currTriggerIndex < effects.Count )
        //     {
        //         var nextEffect = effects[_currTriggerIndex];
        //         if ( _time < nextEffect.GetStartTime() )
        //             break;
        //
        //         if ( Tools.GetBitValue( _result._stateDescription, ( int ) AbilityUseResultTypeEnum.IS_TARGET_AS_POSITION ) )
        //         {
        //             GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility( _currTriggerIndex, _castorID, -1, _abilityData.GetId(), _result._targetPosition );
        //         }
        //         else
        //         {
        //             foreach ( var targetID in _result._targetIDArr )
        //                 GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility( _currTriggerIndex, _castorID, targetID, _abilityData.GetId(), _result._targetPosition );
        //         }
        //
        //         if ( !_onUseEventFired )
        //         {
        //             GameEntry.Event.FireNow( _fsm.ActorInstance(), EventArg_OnUseAblity.Create( _result ) );
        //             _onUseEventFired = true;
        //         }
        //
        //         _currTriggerIndex++;
        //     }
        // }

        /// <summary>
        /// 技能完成检查
        /// </summary>
        // private void FinishAbility()
        // {
        //     if ( _abilityFinishFlag )
        //         return;
        //
        //     if ( _time < _timelineMeta.Duration )
        //         return;
        //
        //     _abilityFinishFlag = true;
        //     _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );
        // }
        
        public override void OnEnter( object param )
        {
            base.OnEnter( param );
            var abilityParam = param as AbilityResult_Use;
            if (abilityParam is null)
            {
                Tools.Logger.Error("ActorState_HeroAbility::OnEnter AbilityParam is null");
                return;
            }

            if (!GameEntry.AbilityPool.GetAbility(abilityParam._abilityID, out var ability))
            {
                Tools.Logger.Error("ActorState_HeroAbility::OnEnter AbilityParam is null");
                return;
            }

            _timelineMeta = GameEntry.LuBan.Tables.AbilityTimeline.Get(ability.GetTimelineID());
            if (_timelineMeta is null)
            {
                Tools.Logger.Error("ActorState_HeroAbility::OnEnter AbilityParam is null");
                return;
            }
            
            
        }

        public override void OnUpdate( float deltaTime )
        {
            base.OnUpdate( deltaTime );
            
        }
        public override void OnLeave( object param )
        {
            base.OnLeave( param );
            // //#todo 施法结束回调
            _timelineMeta     = null;
            // _abilityData      = default;
            // _castorID         = -1;
            // _currTriggerIndex = -1;
            // _onUseEventFired = false;
            //
            // if ( _result != null )
            //     ReferencePool.Release( _result );
            //
            // _result = null;
        }
        public ActorState_HeroAbility( int state_id ) : base( state_id )
        {

        }

        // /// <summary>
        // /// 施法者 ActorID
        // /// </summary>
        // private int _castorID = -1;
        //
        // /// <summary>
        // /// 施法结果
        // /// </summary>
        // private AbilityResult_Use _result;
        //
        // /// <summary>
        // /// 技能数据
        // /// </summary>
        // private AbilityData _abilityData;
        //
        // /// <summary>
        // /// 技能 Timeline 配置
        // /// </summary>
        private Table_AbilityTimeline _timelineMeta;
        //
        // /// <summary>
        // /// 技能完成标记
        // /// </summary>
        // private bool _abilityFinishFlag;
        //
        // /// <summary>
        // /// 进入该状态后的累计时间
        // /// </summary>
        // private float _time;
        //
        // /// <summary>
        // /// 当前触发到的效果索引
        // /// </summary>
        // private int _currTriggerIndex = -1;
        //
        // /// <summary>
        // /// 单次施法的 OnUse 事件触发标记
        // /// </summary>
        // private bool _onUseEventFired;
    }
}