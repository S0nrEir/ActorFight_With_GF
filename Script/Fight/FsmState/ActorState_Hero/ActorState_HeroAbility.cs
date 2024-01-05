using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Fight;
using GameFramework;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;

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
                Log.Warning( "<color=yellow>HeroStateAddon.IsAbilityDataValid()--->param is null || param.Length == 0</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_TIMELINE_META, true );
                return false;
            }
            var result = param as AbilityResult_Use;
            _abilityMeta = GameEntry.LuBan.Tables.Ability.Get( result._abilityID );
            if ( _abilityMeta is null )
            {
                Log.Warning( "<color=yellow>HeroStateAddon.IsAbilityDataValid()--->_abilityMeta is null</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_ABILITY_META, true );
            }

            _timelineMeta = GameEntry.LuBan.Tables.AbilityTimeline.Get( _abilityMeta.Timeline );
            if ( _timelineMeta is null )
            {
                Log.Warning( "<color=yellow>HeroStateAddon.IsAbilityDataValid()--->timeline meta is null</color>" );
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_TIMELINE_META, true );
            }
            //检查CD和消耗
            var abilityAddon = _fsm.ActorInstance().GetAddon<Addon_Ability>();
            if ( abilityAddon is null )
                state = Tools.SetBitValue( state, ( int ) AbilityUseResultTypeEnum.NONE_PARAM, true );

            var canUseFlag = abilityAddon.CanUseAbility( _abilityMeta.id );
            if ( canUseFlag != 0 )
                state = Tools.SetBitValue( state, ( ushort ) canUseFlag, true );

            //到最后设置succ
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
        /// 尝试使用技能，到触发时间使用技能
        /// </summary>
        private void TryUseAbility( float deltaTime )
        {
            _time += deltaTime;
            if (_abilityFinishFlag)
                return;

            if (_time >= _timelineMeta.Duration)
                return;

            if (_currTriggerIndex >= _abilityMeta.Triggers.Length)
                return;

            var nextTrigger = _abilityMeta.Triggers[_currTriggerIndex];
            if (_time >= nextTrigger.TriggerTime)
            {
                if (Tools.GetBitValue(_result._stateDescription, (int)AbilityUseResultTypeEnum.IS_TARGET_AS_POSITION))
                {
                    GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility(_currTriggerIndex, _castorID, -1, _abilityMeta.id, _result._targetPosition );
                }
                else
                {
                    foreach ( var targetID in _result._targetIDArr )
                        GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility(_currTriggerIndex, _castorID, targetID, _abilityMeta.id, _result._targetPosition );
                }
                GameEntry.Event.Fire( _fsm.ActorInstance(), EventArg_OnUseAblity.Create( _result ) );
                _currTriggerIndex++;
                if (_currTriggerIndex >= _abilityMeta.Triggers.Length)
                    _abilityFinishFlag = true;
            }
            // if ( !_abilityFinishFlag && _time >= _timelineMeta.TriggerTime )
            // {
            //     if ( Tools.GetBitValue( _result._stateDescription, ( int ) AbilityUseResultTypeEnum.IS_TARGET_AS_POSITION ) )
            //     {
            //         //以位置作为依据的，不需要目标actorID
            //         GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility( _castorID, -1, _abilityMeta.id, _result._targetPosition );
            //     }
            //     else
            //     {
            //         foreach ( var targetID in _result._targetIDArr )
            //             GameEntry.Module.GetModule<Module_ProxyActor>().AffectAbility( _castorID, targetID, _abilityMeta.id, _result._targetPosition );
            //     }
            //     GameEntry.Event.Fire( _fsm.ActorInstance(), EventArg_OnUseAblity.Create( _result ) );
            //     _abilityFinishFlag = true;
            // }
        }

        /// <summary>
        /// 技能完成检查
        /// </summary>
        private void FinishAbility()
        {
            if(_abilityFinishFlag)
                _fsm.SwitchTo((int)ActorStateTypeEnum.IDLE_STATE,null,null);
            
            // if ( _time >= _timelineMeta.Duration )
            //     _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );
        }

        public override void OnEnter( object param )
        {
            base.OnEnter( param );
            if ( !IsAbilityDataValid( param ) )
            {
                _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );
                return;
            }
            //技能消耗
            //可以释放技能后，先扣除消耗和计算CD，不要等timeline开始在做
            _fsm.ActorInstance().Actor.Notify( ( int ) AddonEventTypeEnum.USE_ABILITY, new AddonParam_OnUseAbility() { _abilityID = _abilityMeta.id } );
            _time = 0f;
            _abilityFinishFlag = false;
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
            //#todo施法结束回调
            _timelineMeta     = null;
            _abilityMeta      = null;
            _castorID         = -1;
            _currTriggerIndex = -1;
            
            if ( _result != null )
                ReferencePool.Release( _result );

            _result = null;
        }
        public ActorState_HeroAbility( int state_id ) : base( state_id )
        {

        }

        /// <summary>
        /// 施法者ActorID
        /// </summary>
        private int _castorID = -1;

        /// <summary>；
        /// 
        /// 使用状态结果
        /// </summary>
        private AbilityResult_Use _result = null;

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

        /// <summary>
        /// 当前走到的触发器节点索引
        /// </summary>
        private int _currTriggerIndex = -1;
    }
}