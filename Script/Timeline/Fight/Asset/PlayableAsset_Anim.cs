using Aquila.Toolkit;
using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.Timeline
{
    public class PlayableAsset_Anim : PlayableAsset
    {
        public override Playable CreatePlayable( PlayableGraph graph, GameObject owner )
        {
            var bhvr = new PlayableBhvr_Anim();
            bhvr._animName = _animName;
            //暂时先这样做
            bhvr._director = Tools.GetComponent<PlayableDirector>( owner );
            bhvr._animator = Tools.GetComponent<Animator>( owner );

            var playable = ScriptPlayable<PlayableBhvr_Anim>.Create( graph ,bhvr);
            return playable;
        }

        /// <summary>
        /// 动画名称
        /// </summary>
        [SerializeField] private string _animName = string.Empty;
    }
}
