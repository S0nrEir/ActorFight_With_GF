using GameFramework.Sound;
using UnityEngine;

namespace Aquila.Fight
{
    [CreateAssetMenu(fileName = "GameplayCueAudioNotify", menuName = "Aquila/GameplayCue/Audio Notify")]
    public class GameplayCueAudioNotify : GameplayCueNotifyBase
    {
        public override void Execute(in GameplayCueParameters parameters)
        {
            Play(_assetPath, _soundGroup, _volume, parameters.Location);
        }

        protected virtual void Play(string assetPath, string soundGroup, float volume, Vector3 location)
        {
            var playParams = PlaySoundParams.Create();
            playParams.VolumeInSoundGroup = volume;
            GameEntry.Sound.PlaySound(assetPath, soundGroup, 0, playParams, location);
        }

        [SerializeField] private string _assetPath;
        [SerializeField] private string _soundGroup = "Effect";
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
    }
}
