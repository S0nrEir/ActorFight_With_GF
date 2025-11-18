using System;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Buff/Effect Clip数据类
    /// 用于在timeline中表示效果/Buff的应用
    /// </summary>
    [Serializable]
    public class EffectClipData : TimelineClipData
    {
        /// <summary>
        /// Effect ID（关联到配置表中的效果）
        /// </summary>
        [SerializeField]
        private int _effectId;

        /// <summary>
        /// 效果强度/层数
        /// </summary>
        [SerializeField]
        private int _stackCount;

        /// <summary>
        /// 是否可堆叠
        /// </summary>
        [SerializeField]
        private bool _canStack;

        public override TimelineClipType ClipType => TimelineClipType.Buff;

        /// <summary>
        /// EffectClip是即时触发的，不需要持续时间
        /// </summary>
        public override bool IsInstantClip => true;

        /// <summary>
        /// Effect触发时间（即时clip只有触发时间，没有持续时间）
        /// </summary>
        public float TriggerTime
        {
            get => StartTime;
            set
            {
                StartTime = value;
                EndTime = value; // EndTime始终等于StartTime
            }
        }

        public EffectClipData() : base()
        {
            _effectId = 0;
            _stackCount = 1;
            _canStack = false;
            ClipColor = new Color(0.8f, 0.4f, 0.8f); // 紫色
            // 确保EndTime等于StartTime
            EndTime = StartTime;
        }

        public EffectClipData( string clipName, float triggerTime, int effectId)
            : base(clipName, triggerTime, triggerTime, new Color(0.8f, 0.4f, 0.8f))
        {
            _effectId = effectId;
            _stackCount = 1;
            _canStack = false;
        }

        #region Properties

        public int EffectId
        {
            get => _effectId;
            set => _effectId = value;
        }

        public int StackCount
        {
            get => _stackCount;
            set => _stackCount = Mathf.Max(1, value);
        }

        public bool CanStack
        {
            get => _canStack;
            set => _canStack = value;
        }

        #endregion

        #region Override Methods

        public override TimelineClipData Clone()
        {
            var clone = new EffectClipData();
            CopyBaseTo(clone);
            clone._effectId = _effectId;
            clone._stackCount = _stackCount;
            clone._canStack = _canStack;
            return clone;
        }

        public override bool Validate(out string errorMessage)
        {
            if (!base.Validate(out errorMessage))
                return false;

            if (_effectId <= 0)
            {
                errorMessage = "Effect ID must be greater than 0";
                return false;
            }

            if (_stackCount < 1)
            {
                errorMessage = "Stack count must be at least 1";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public override string GetDisplayInfo()
        {
            string stackInfo = _canStack ? $" (x{_stackCount})" : "";
            return $"Effect {_effectId}: {ClipName}{stackInfo} [Trigger at {TriggerTime:F2}s]";
        }

        #endregion
    }
}
