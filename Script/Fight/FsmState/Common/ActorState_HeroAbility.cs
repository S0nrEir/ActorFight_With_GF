using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Fight;
using GameFramework;
using UnityEngine.Playables;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 使用技能状态
    /// </summary>
    public class ActorState_HeroAbility : ActorState_Base
    {
        /// <summary>
        /// 检查技能数据是否合法
        /// </summary>
        private bool IsAbilityDataValid( object param )
        {
            int state = 0;
            if ( param is null || param is not AbilityResult_Use )
            {
                Tools.Logger.Warning( "<color=yellow>HeroStateAddon.IsAbilityDataValid()--->param is null || param.Length == 0</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_TIMELINE_META, true );
                return false;
            }
            var result = param as AbilityResult_Use;
            
            if (!GameEntry.AbilityPool.TryGetAbility(result._abilityID, out _abilityData))
            {
                Tools.Logger.Warning($"<color=yellow>HeroStateAddon.IsAbilityDataValid()--->Ability {result._abilityID} not found in pool</color>");
                state = Tools.SetBitValue(state, (int)AbilityUseResultTypeEnum.NONE_ABILITY_META, true);
            }

            _timelineMeta = GameEntry.LuBan.Tables.AbilityTimeline.Get( _abilityData.GetTimelineID() );
            if ( _timelineMeta is null )
            {
                Tools.Logger.Warning( "<color=yellow>HeroStateAddon.IsAbilityDataValid()--->timeline meta is null</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_TIMELINE_META, true );
            }
            // 检查 CD 与消耗
            var abilityAddon = _fsm.ActorInstance().GetAddon<Addon_Ability>();
            if ( abilityAddon is null )
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_PARAM, true );

            var canUseFlag = abilityAddon.CanUseAbility( _abilityData.GetId() );
            if ( canUseFlag != 0 )
                state = Tools.SetBitValue( state, ( ushort ) canUseFlag, true );

            // 最后设置成功标记
            result._stateDescription = state;
            result._succ = result.StateFlagIsClean();
            if ( result._succ )
                result._stateDescription = Tools.SetBitValue( result._stateDescription, ( int ) AbilityUseResultTypeEnum.SUCC, true );

            _castorID = result._castorID;
            // _targetIDArr = result._targetIDArr;
            _result = result;
            return result._succ;
        }

        /// <summary>
        /// 尝试在触发时间点释放技能效果
        /// </summary>
        private void TryUseAbility( float deltaTime )
        {
            _time += deltaTime;
            if ( _abilityFinishFlag )
                return;

            if ( _time > _timelineMeta.Duration )
                return;

            var effects = _abilityData.GetEffects();
            if ( effects is null || effects.Count == 0 )
                return;
            
            while ( _currTriggerIndex < effects.Count )
            {
                var nextEffect = effects[_currTriggerIndex];
                if ( _time < nextEffect.GetStartTime() )
                    break;

                if ( Tools.GetBitValue( _result._stateDescription, ( int ) AbilityUseResultTypeEnum.IS_TARGET_AS_POSITION ) )
                {
                    GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility( _currTriggerIndex, _castorID, -1, _abilityData.GetId(), _result._targetPosition );
                }
                else
                {
                    foreach ( var targetID in _result._targetIDArr )
                        GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility( _currTriggerIndex, _castorID, targetID, _abilityData.GetId(), _result._targetPosition );
                }

                if ( !_onUseEventFired )
                {
                    GameEntry.Event.FireNow( _fsm.ActorInstance(), EventArg_OnUseAblity.Create( _result ) );
                    _onUseEventFired = true;
                }

                _currTriggerIndex++;
            }
        }

        /// <summary>
        /// 技能完成检查
        /// </summary>
        private void FinishAbility()
        {
            if ( _abilityFinishFlag )
                return;

            if ( _time < _timelineMeta.Duration )
                return;

            _abilityFinishFlag = true;
            _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );
        }
        public override void OnEnter( object param )
        {
            base.OnEnter( param );
            if ( !IsAbilityDataValid( param ) )
            {
                _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );
                return;
            }
            // 技能消耗
            // 技能释放时先扣消耗并开始计算 CD，不等待 timeline 结束
            _fsm.ActorInstance().Actor.Notify( ( int ) AddonEventTypeEnum.USE_ABILITY, new AddonParam_OnUseAbility { _abilityID = _abilityData.GetId() } );
            _time = 0f;
            _abilityFinishFlag = false;
            _currTriggerIndex = 0;
            _onUseEventFired = false;
            GameEntry.Timeline.Play( _timelineMeta.AssetPath, Tools.GetComponent<PlayableDirector>( _actor.transform ) );
        }

        public override void OnUpdate( float deltaTime )
        {
            base.OnUpdate( deltaTime );
            TryUseAbility( deltaTime );
            FinishAbility();
        }
        public override void OnLeave( object param )
        {
            base.OnLeave( param );
            //#todo 施法结束回调
            _timelineMeta     = null;
            _abilityData      = default;
            _castorID         = -1;
            _currTriggerIndex = -1;
            _onUseEventFired = false;
            
            if ( _result != null )
                ReferencePool.Release( _result );

            _result = null;
        }
        public ActorState_HeroAbility( int state_id ) : base( state_id )
        {

        }

        /// <summary>
        /// 施法者 ActorID
        /// </summary>
        private int _castorID = -1;

        /// <summary>
        /// 施法结果
        /// </summary>
        private AbilityResult_Use _result;

        /// <summary>
        /// 技能数据
        /// </summary>
        private AbilityData _abilityData;

        /// <summary>
        /// 技能 Timeline 配置
        /// </summary>
        private Table_AbilityTimeline _timelineMeta;

        /// <summary>
        /// 技能完成标记
        /// </summary>
        private bool _abilityFinishFlag;

        /// <summary>
        /// 进入该状态后的累计时间
        /// </summary>
        private float _time;

        /// <summary>
        /// 当前触发到的效果索引
        /// </summary>
        private int _currTriggerIndex = -1;

        /// <summary>
        /// 单次施法的 OnUse 事件触发标记
        /// </summary>
        private bool _onUseEventFired;
    }
}