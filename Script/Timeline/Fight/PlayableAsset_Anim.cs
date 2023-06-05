using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.Timeline
{
    public class PlayableAsset_Anim : PlayableAsset
    {
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var bhvr = new PlayableBhvr_Anim();
            return ScriptPlayable<PlayableBhvr_Anim>.Create(graph,bhvr);
        }

        /// <summary>
        /// 触发标记
        /// </summary>
        public bool _triggerFlag = false;
        
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
