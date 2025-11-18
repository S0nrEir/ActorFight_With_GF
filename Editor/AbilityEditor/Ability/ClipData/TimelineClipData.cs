using System;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Timeline轨道子项的基类
    /// 用于在timeline中表示可拖动、可调整时长的元素（技能、Buff、音效、特效、动画等）
    /// </summary>
    [Serializable]
    public abstract class TimelineClipData
    {
        /// <summary>
        /// 子项唯一ID
        /// </summary>
        [SerializeField]
        private string _clipId;

        /// <summary>
        /// 子项显示名称
        /// </summary>
        [SerializeField]
        private string _clipName;

        /// <summary>
        /// 开始时间（秒）
        /// </summary>
        [SerializeField]
        private float _startTime;

        /// <summary>
        /// 结束时间（秒）
        /// </summary>
        [SerializeField]
        private float _endTime;

        /// <summary>
        /// 子项颜色
        /// </summary>
        [SerializeField]
        private Color _clipColor;

        /// <summary>
        /// 是否启用
        /// </summary>
        [SerializeField]
        private bool _isEnabled;

        /// <summary>
        /// 子项类型（用于区分不同类型的clip）
        /// </summary>
        public abstract TimelineClipType ClipType { get; }

        /// <summary>
        /// 是否为即时clip（时间点触发，而非持续时间）
        /// </summary>
        public virtual bool IsInstantClip => false;

        public TimelineClipData()
        {
            _clipId = Guid.NewGuid().ToString();
            _clipName = "New Clip";
            _startTime = 0f;
            _endTime = 1f;
            _clipColor = Color.white;
            _isEnabled = true;
        }

        public TimelineClipData(string clipName, float startTime, float endTime, Color clipColor)
        {
            _clipId = Guid.NewGuid().ToString();
            _clipName = clipName;
            _startTime = Mathf.Max(0, startTime);
            _endTime = Mathf.Max(_startTime, endTime);
            _clipColor = clipColor;
            _isEnabled = true;
        }

        #region Properties

        public string ClipId => _clipId;
        public string ClipName
        {
            get => _clipName;
            set => _clipName = value;
        }

        public float StartTime
        {
            get => _startTime;
            set => _startTime = Mathf.Max(0, value);
        }

        public float EndTime
        {
            get => _endTime;
            set => _endTime = Mathf.Max(_startTime, value);
        }

        public float Duration => _endTime - _startTime;

        public Color ClipColor
        {
            get => _clipColor;
            set => _clipColor = value;
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 设置时间范围（会自动约束endTime >= startTime）
        /// </summary>
        public void SetTimeRange(float startTime, float endTime)
        {
            _startTime = Mathf.Max(0, startTime);
            _endTime = Mathf.Max(_startTime, endTime);
        }

        /// <summary>
        /// 移动clip到新的开始时间（保持时长不变）
        /// </summary>
        public void MoveTo(float newStartTime)
        {
            float duration = Duration;
            _startTime = Mathf.Max(0, newStartTime);
            _endTime = _startTime + duration;
        }

        /// <summary>
        /// 调整clip的持续时间
        /// </summary>
        public void SetDuration(float duration)
        {
            _endTime = _startTime + Mathf.Max(0, duration);
        }

        /// <summary>
        /// 验证clip的时间范围是否在timeline的有效范围内
        /// </summary>
        public bool IsValidInTimelineRange(float timelineStartTime, float timelineEndTime)
        {
            return _startTime >= timelineStartTime && _endTime <= timelineEndTime;
        }

        /// <summary>
        /// 约束clip的时间范围到timeline的有效范围内
        /// </summary>
        public void ClampToTimelineRange(float timelineStartTime, float timelineEndTime)
        {
            _startTime = Mathf.Clamp(_startTime, timelineStartTime, timelineEndTime);
            _endTime = Mathf.Clamp(_endTime, _startTime, timelineEndTime);
        }

        /// <summary>
        /// 检查是否与另一个clip在时间上重叠
        /// </summary>
        public bool OverlapsWith(TimelineClipData other)
        {
            if (other == null)
                return false;

            return !(_endTime <= other._startTime || _startTime >= other._endTime);
        }

        /// <summary>
        /// 克隆clip数据
        /// </summary>
        public abstract TimelineClipData Clone();

        /// <summary>
        /// 验证clip数据的有效性
        /// </summary>
        public virtual bool Validate(out string errorMessage)
        {
            if (string.IsNullOrEmpty(_clipName))
            {
                errorMessage = "Clip name cannot be empty";
                return false;
            }

            // 即时clip允许EndTime等于StartTime
            if (!IsInstantClip && _endTime <= _startTime)
            {
                errorMessage = "End time must be greater than start time";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// 获取clip的详细信息（用于编辑器显示）
        /// </summary>
        public virtual string GetDisplayInfo()
        {
            return $"{_clipName} [{_startTime:F2}s - {_endTime:F2}s]";
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 复制基础数据到另一个clip（用于Clone实现）
        /// </summary>
        protected void CopyBaseTo(TimelineClipData target)
        {
            if (target == null)
                return;

            target._clipId = Guid.NewGuid().ToString(); // 生成新ID
            target._clipName = _clipName;
            target._startTime = _startTime;
            target._endTime = _endTime;
            target._clipColor = _clipColor;
            target._isEnabled = _isEnabled;
        }

        #endregion
    }

    /// <summary>
    /// Timeline子项类型枚举
    /// </summary>
    public enum TimelineClipType
    {
        /// <summary>
        /// 技能/能力
        /// </summary>
        Ability,

        /// <summary>
        /// Buff/效果
        /// </summary>
        Buff,

        /// <summary>
        /// 音效
        /// </summary>
        Audio,

        /// <summary>
        /// 特效/VFX
        /// </summary>
        VFX,

        /// <summary>
        /// 动画
        /// </summary>
        Animation,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom
    }
}
