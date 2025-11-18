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

        public EffectClipData() : base()
        {
            _effectId = 0;
            _stackCount = 1;
            _canStack = false;
            ClipColor = new Color(0.8f, 0.4f, 0.8f); // 紫色
        }

        public EffectClipData( string clipName, float startTime, float endTime, int effectId)
            : base(clipName, startTime, endTime, new Color(0.8f, 0.4f, 0.8f))
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
            return $"Buff {_effectId}: {ClipName}{stackInfo} [{StartTime:F2}s - {EndTime:F2}s]";
        }

        #endregion
    }
}
