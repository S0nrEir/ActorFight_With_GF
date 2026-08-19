using GameFramework.Sound;
using UnityEngine;

namespace Aquila.Fight
{
    [CreateAssetMenu(fileName = "GameplayCueAudioNotify", menuName = "Aquila/GameplayCue/Audio Notify")]
    public class GameplayCueAudioNotify : GameplayCueNotifyBase
    {
        public override void Execute(in GameplayCueParameters parameters)
        {
            var assetPath = ResolveAssetPath(_soundEffectId);
            if (string.IsNullOrWhiteSpace(assetPath))
            {
                Aquila.Toolkit.Tools.Logger.Error(
                    $"[GameplayCueAudioNotify] Sound effect lookup failed, SoundEffectId={_soundEffectId}, CueTag={CueTag}");
                return;
            }

            Play(assetPath, _soundGroup, _volume, parameters.Location);
        }

        protected virtual string ResolveAssetPath(int soundEffectId)
        {
            var soundEffectMap = GameEntry.LuBan?.Tables?.SoundEffectMap;
            return soundEffectMap?.GetOrDefault(soundEffectId)?.asset_path;
        }

        protected virtual void Play(string assetPath, string soundGroup, float volume, Vector3 location)
        {
            var playParams = PlaySoundParams.Create();
            playParams.VolumeInSoundGroup = volume;
            GameEntry.Sound.PlaySound(assetPath, soundGroup, 0, playParams, location);
        }

        [SerializeField] private int _soundEffectId;
        [SerializeField] private string _soundGroup = "Effect";
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
    }
}
