using System;
using Cfg.Enum;
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
        /// Effect扩展参数（嵌套类）
        /// </summary>
        [Serializable]
        public class EffectExtensionParam
        {
            [SerializeField] public float FloatParam_1 = -1f;
            [SerializeField] public float FloatParam_2 = -1f;
            [SerializeField] public float FloatParam_3 = -1f;
            [SerializeField] public float FloatParam_4 = -1f;
            [SerializeField] public int IntParam_1 = -1;
            [SerializeField] public int IntParam_2 = -1;
            [SerializeField] public int IntParam_3 = -1;
            [SerializeField] public int IntParam_4 = -1;

            public EffectExtensionParam Clone()
            {
                return new EffectExtensionParam
                {
                    FloatParam_1 = FloatParam_1,
                    FloatParam_2 = FloatParam_2,
                    FloatParam_3 = FloatParam_3,
                    FloatParam_4 = FloatParam_4,
                    IntParam_1 = IntParam_1,
                    IntParam_2 = IntParam_2,
                    IntParam_3 = IntParam_3,
                    IntParam_4 = IntParam_4
                };
            }
        }

        /// <summary>
        /// Effect ID（关联到配置表中的效果）
        /// </summary>
        [SerializeField]
        private int _effectId;

        /// <summary>
        /// 结算类型ID（必须大于0）
        /// </summary>
        [SerializeField]
        private int _resolveTypeID = 1;

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

        #region Effect配置字段

        /// <summary>
        /// Effect类型
        /// </summary>
        [SerializeField]
        private EffectType _effectType = EffectType.Instant_Cost;

        /// <summary>
        /// 数值修改器类型
        /// </summary>
        [SerializeField]
        private NumricModifierType _modifierType = NumricModifierType.None;

        /// <summary>
        /// 影响的属性类型
        /// </summary>
        [SerializeField]
        private actor_attribute _affectedAttribute = actor_attribute.Curr_HP;

        /// <summary>
        /// 目标类型（0=我方，1=敌方）
        /// </summary>
        [SerializeField]
        private int _target;

        /// <summary>
        /// 持续时间（秒），-1表示瞬时
        /// </summary>
        [SerializeField]
        private float _duration = -1f;

        /// <summary>
        /// 生效周期（秒）
        /// </summary>
        [SerializeField]
        private float _period;

        /// <summary>
        /// 生效策略
        /// </summary>
        [SerializeField]
        private DurationPolicy _policy = DurationPolicy.Instant;

        /// <summary>
        /// 是否立即生效
        /// </summary>
        [SerializeField]
        private bool _effectOnAwake = true;

        /// <summary>
        /// 扩展参数
        /// </summary>
        [SerializeField]
        private EffectExtensionParam _extensionParam = new EffectExtensionParam();

        /// <summary>
        /// 派生效果ID列表
        /// </summary>
        [SerializeField]
        private int[] _deriveEffects = new int[0];

        /// <summary>
        /// 唤醒效果ID列表
        /// </summary>
        [SerializeField]
        private int[] _awakeEffects = new int[0];

        #endregion

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

        public EffectClipData()
        {
            _effectId = 0;
            _resolveTypeID = -1;
            _stackCount = 1;
            _canStack = false;
            ClipColor = new Color(0.8f, 0.4f, 0.8f); // 紫色
            // 确保EndTime等于StartTime
            EndTime = StartTime;
        }

        public EffectClipData(string clipName, float triggerTime, int effectId)
            : base(clipName, triggerTime, triggerTime, new Color(0.8f, 0.4f, 0.8f))
        {
            _effectId = effectId;
            _resolveTypeID = -1;
            _stackCount = 1;
            _canStack = false;
        }

        public int EffectId
        {
            get => _effectId;
            set => _effectId = value;
        }

        public int ResolveTypeID
        {
            get => _resolveTypeID;
            set => _resolveTypeID = value;
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

        #region Effect配置属性

        public EffectType EffectType
        {
            get => _effectType;
            set => _effectType = value;
        }

        public NumricModifierType ModifierType
        {
            get => _modifierType;
            set => _modifierType = value;
        }

        public actor_attribute AffectedAttribute
        {
            get => _affectedAttribute;
            set => _affectedAttribute = value;
        }

        public int Target
        {
            get => _target;
            set => _target = value;
        }

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        public float Period
        {
            get => _period;
            set => _period = value;
        }

        public DurationPolicy Policy
        {
            get => _policy;
            set => _policy = value;
        }

        public bool EffectOnAwake
        {
            get => _effectOnAwake;
            set => _effectOnAwake = value;
        }

        public EffectExtensionParam ExtensionParam
        {
            get => _extensionParam;
            set => _extensionParam = value;
        }

        public int[] DeriveEffects
        {
            get => _deriveEffects;
            set => _deriveEffects = value;
        }

        public int[] AwakeEffects
        {
            get => _awakeEffects;
            set => _awakeEffects = value;
        }

        #endregion

        #region Override Methods

        public override TimelineClipData Clone()
        {
            var clone = new EffectClipData();
            CopyBaseTo(clone);
            clone._effectId = _effectId;
            clone._resolveTypeID = _resolveTypeID;
            clone._stackCount = _stackCount;
            clone._canStack = _canStack;

            // 复制Effect配置字段
            clone._effectType = _effectType;
            clone._modifierType = _modifierType;
            clone._affectedAttribute = _affectedAttribute;
            clone._target = _target;
            clone._duration = _duration;
            clone._period = _period;
            clone._policy = _policy;
            clone._effectOnAwake = _effectOnAwake;
            clone._extensionParam = _extensionParam?.Clone() ?? new EffectExtensionParam();
            clone._deriveEffects = _deriveEffects != null ? (int[])_deriveEffects.Clone() : new int[0];
            clone._awakeEffects = _awakeEffects != null ? (int[])_awakeEffects.Clone() : new int[0];

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

            if (_resolveTypeID <= 0)
            {
                errorMessage = "ResolveTypeID must be greater than 0";
                return false;
            }

            if (_stackCount < 1)
            {
                errorMessage = "Stack count must be at least 1";
                return false;
            }

            // 验证Duration
            if (_duration < -1)
            {
                errorMessage = "Duration cannot be less than -1";
                return false;
            }

            // 验证Period
            if (_period < 0)
            {
                errorMessage = "Period cannot be negative";
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