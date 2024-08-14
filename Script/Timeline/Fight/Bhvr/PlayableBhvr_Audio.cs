using UnityEngine.Playables;

namespace Aquila.Timeline
{
    public class PlayableBhvr_Audio : PlayableBehaviour_Base
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
            var asset = _asset as PlayableAsset_Audio;
            if (asset != null)
                _soundID = GameEntry.Sound.PlaySound
                    (
                        asset._audioPath,
                        "Effect"
                    );
        }

        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            GameEntry.Sound.StopSound(_soundID);
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
        }

        public override void OnBehaviourPause( Playable playable, FrameData info )
        {
            base.OnBehaviourPause( playable, info );
        }
        
        /// <summary>
        /// 音频ID
        /// </summary>
        private int _soundID = -1;
    }
}
