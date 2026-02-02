using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 音频数据
    /// </summary>
    public struct AudioData
    {
        private float _startTime;
        private float _endTime;
        private string _audioPath;
        private float _volume;
        private bool _loop;
        private float _fadeInDuration;
        private float _fadeOutDuration;

        public AudioData(
            float startTime,
            float endTime,
            string audioPath,
            float volume,
            bool loop,
            float fadeInDuration,
            float fadeOutDuration)
        {
            _startTime = startTime;
            _endTime = endTime;
            _audioPath = audioPath;
            _volume = volume;
            _loop = loop;
            _fadeInDuration = fadeInDuration;
            _fadeOutDuration = fadeOutDuration;
        }

        public float GetStartTime() => _startTime;
        public float GetEndTime() => _endTime;
        public string GetAudioPath() => _audioPath;
        public float GetVolume() => _volume;
        public bool GetLoop() => _loop;
        public float GetFadeInDuration() => _fadeInDuration;
        public float GetFadeOutDuration() => _fadeOutDuration;

        public void SetStartTime(float value) => _startTime = value;
        public void SetEndTime(float value) => _endTime = value;
        public void SetAudioPath(string value) => _audioPath = value;
        public void SetVolume(float value) => _volume = value;
        public void SetLoop(bool value) => _loop = value;
        public void SetFadeInDuration(float value) => _fadeInDuration = value;
        public void SetFadeOutDuration(float value) => _fadeOutDuration = value;
    }
}
