using Aquila.Fight.Addon;
using Aquila.Toolkit;
using Cfg.Fight;
using GameFramework;
using UnityEngine.Playables;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 使用技能状态：进入后播放技能 Timeline，时长到达后停止并退出到待机状态。
    /// </summary>
    public class ActorState_HeroAbility : ActorState_Base
    {
        public override void OnEnter(object param)
        {
            base.OnEnter(param);

            if (!(param is AbilityResult_Use abilityParam))
                throw new GameFrameworkException("ActorState_HeroAbility.OnEnter param must be AbilityResult_Use.");

            if (!GameEntry.AbilityPool.GetAbility(abilityParam._abilityID, out var abilityData))
                throw new GameFrameworkException($"ActorState_HeroAbility.OnEnter ability not found, id={abilityParam._abilityID}.");

            _timelineMeta = GameEntry.LuBan.Tables.AbilityTimeline.GetOrDefault(abilityData.GetTimelineID());
            if (_timelineMeta == null)
                throw new GameFrameworkException($"ActorState_HeroAbility.OnEnter timeline meta not found, timelineID={abilityData.GetTimelineID()}.");

            if (string.IsNullOrEmpty(_timelineMeta.AssetPath))
                throw new GameFrameworkException($"ActorState_HeroAbility.OnEnter timeline asset path is empty, timelineID={_timelineMeta.id}.");

            _director = Tools.GetComponent<PlayableDirector>(_actor.transform);

            _elapsed = 0f;
            _isTimelinePlaying = true;
            GameEntry.Timeline.Play(_timelineMeta.AssetPath, _director);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (!_isTimelinePlaying || _timelineMeta == null)
                return;

            if (deltaTime > 0f)
                _elapsed += deltaTime;

            if (_elapsed < _timelineMeta.Duration)
                return;

            StopTimeline();
            _fsm.SwitchTo((int)ActorStateTypeEnum.IDLE_STATE, null, null);
        }

        public override void OnLeave(object param)
        {
            base.OnLeave(param);

            StopTimeline();
            _timelineMeta = null;
            _director = null;
            _elapsed = 0f;
            _isTimelinePlaying = false;
        }

        public ActorState_HeroAbility(int state_id) : base(state_id)
        {
        }

        private void StopTimeline()
        {
            if (!_isTimelinePlaying)
                return;

            if (_director != null)
                _director.Stop();

            _isTimelinePlaying = false;
        }

        /// <summary>
        /// 技能 Timeline 配置。
        /// </summary>
        private Table_AbilityTimeline _timelineMeta;

        /// <summary>
        /// 当前状态累计时间。
        /// </summary>
        private float _elapsed;

        /// <summary>
        /// 当前状态持有的 Timeline 播放组件。
        /// </summary>
        private PlayableDirector _director;

        /// <summary>
        /// 当前状态是否在播放 Timeline。
        /// </summary>
        private bool _isTimelinePlaying;
    }
}