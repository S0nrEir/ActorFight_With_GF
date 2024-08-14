using UnityEngine.Playables;
using UnityGameFramework.Runtime;

namespace Aquila.Timeline
{
    public class PlayableBehaviour_Base : PlayableBehaviour
    {
        public PlayableBehaviour_Base()
        {
            Log.Info($"<color=white>PlayableBhvr_Base</color>");
        }
        
        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            _asset = null;
            Log.Info($"<color=white>PlayableBhvr_Base.OnPlayableDestroy</color>");
        }
        
        /// <summary>
        /// 设置playable asset给behaviour
        /// </summary>
        // public void SetAsset(PlayableAsset asset)
        // {
        //     _asset = asset;
        // }

        public PlayableAsset _asset = null;
    }
}