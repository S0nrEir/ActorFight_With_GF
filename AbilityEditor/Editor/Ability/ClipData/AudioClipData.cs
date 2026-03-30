using System;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// 音效Clip数据类
    /// 用于在timeline中表示音效播放
    /// </summary>
    [Serializable]
    public class AudioClipData : TimelineClipData
    {
        /// <summary>
        /// 音效资源路径或ID
        /// </summary>
        [SerializeField]
        private string _audioPath;

        /// <summary>
        /// 音量 (0-1)
        /// </summary>
        [SerializeField]
        private float _volume;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        [SerializeField]
        private bool _loop;

        /// <summary>
        /// 淡入时间（秒）
        /// </summary>
        [SerializeField]
        private float _fadeInDuration;

        /// <summary>
        /// 淡出时间（秒）
        /// </summary>
        [SerializeField]
        private float _fadeOutDuration;

        public override TimelineClipType ClipType => TimelineClipType.Audio;

        public AudioClipData()
        {
            _audioPath = string.Empty;
            _volume = 1f;
            _loop = false;
            _fadeInDuration = 0f;
            _fadeOutDuration = 0f;
            ClipColor = new Color(0.4f, 0.8f, 0.4f); // 绿色
        }

        public AudioClipData(string clipName, float startTime, float endTime, string audioPath)
            : base(clipName, startTime, endTime, new Color(0.4f, 0.8f, 0.4f))
        {
            _audioPath = audioPath;
            _volume = 1f;
            _loop = false;
            _fadeInDuration = 0f;
            _fadeOutDuration = 0f;
        }

        #region Properties

        public string AudioPath
        {
            get => _audioPath;
            set => _audioPath = value;
        }

        public float Volume
        {
            get => _volume;
            set => _volume = Mathf.Clamp01(value);
        }

        public bool Loop
        {
            get => _loop;
            set => _loop = value;
        }

        public float FadeInDuration
        {
            get => _fadeInDuration;
            set => _fadeInDuration = Mathf.Max(0, value);
        }

        public float FadeOutDuration
        {
            get => _fadeOutDuration;
            set => _fadeOutDuration = Mathf.Max(0, value);
        }

        #endregion

        #region Override Methods

        public override TimelineClipData Clone()
        {
            var clone = new AudioClipData();
            CopyBaseTo(clone);
            clone._audioPath = _audioPath;
            clone._volume = _volume;
            clone._loop = _loop;
            clone._fadeInDuration = _fadeInDuration;
            clone._fadeOutDuration = _fadeOutDuration;
            return clone;
        }

        public override bool Validate(out string errorMessage)
        {
            if (!base.Validate(out errorMessage))
                return false;

            if (string.IsNullOrEmpty(_audioPath))
            {
                errorMessage = "Audio path cannot be empty";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public override string GetDisplayInfo()
        {
            string loopInfo = _loop ? " [Loop]" : "";
            return $"Audio: {ClipName}{loopInfo} [{StartTime:F2}s - {EndTime:F2}s] Vol:{_volume:F2}";
        }

        #endregion
    }
}
