using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.Timeline
{
    public class PlayableBhvr_Anim : PlayableBehaviour
    {
        public override void OnGraphStart(Playable playable)
        {
            base.OnGraphStart(playable);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
        }
        
        /// <summary>
        /// 触发时间
        /// </summary>
        public float _triggerTime = 0f;

        /// <summary>
        /// 动画名称
        /// </summary>
        public string _animName = string.Empty;
    }
}