using Aquila.Fight.Addon;
using System.Collections.Generic;
using Aquila.Timeline;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityGameFramework.Runtime;
using Aquila.Toolkit;

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
        public override void OnEnter(params object[] param)
        {
            base.OnEnter(param);
            if (param is null || param.Length == 0)
            {
                Log.Warning("<color=yellow>HeroStateAddon.OnEnter()--->param is null || param.Length == 0</color>");
                return;
            }

            var timeline = _director.playableAsset as TimelineAsset;
            if ( timeline is null )
            {
                Log.Warning( "<color=yellow>HeroStateAddon.OnEnter()--->timeline is null</color>" );
                return;
            }

            _asset = Tools.GetFirstClipAssetFromTrack<PlayableAsset_Anim>( Tools.GetTrackFromTimeline<PlayableTrack_Anim>( timeline ) );
            if ( _asset is null )
            {
                Log.Warning( "<color=yellow>HeroStateAddon.OnEnter()--->clip is null</color>" );
                return;
            }

            _time = 0f;
            _abilityFinishFlag = false;
            //播放timeline开始计时
            //根据时间节点来做相应的行为
            _director.Play();
        }

        public override void OnLeave(params object[] param)
        {
            _director = null;
            _asset = null;
            base.OnLeave(param);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            _time += deltaTime;

            if ( _abilityFinishFlag )
            {
                _fsm.SwitchTo( ( int ) ActorStateTypeEnum.IDLE_STATE, null, null );
                return;
            }
            
            //在对应的hurtPoint施加buff
            if (_director.time >= _asset._triggerTime && !_asset._triggerFlag)
            {
                _asset._triggerFlag = true;
                //apply effect
            }

            if ( _director.time >= _time )
                _abilityFinishFlag = true;
        }
        
        public HeroAbilityState( int state_id ) : base( state_id )
        {
            
        }

        /// <summary>
        /// 技能完成标记
        /// </summary>
        private bool _abilityFinishFlag = false;

        /// <summary>
        /// 进入该状态时间
        /// </summary>
        private float _time = 0f;

        /// <summary>
        /// 轨道资源
        /// </summary>
        private PlayableAsset_Anim _asset = null;
        
        /// <summary>
        /// 这里从playableDirector组件拿
        /// </summary>
        private PlayableDirector _director = null;
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