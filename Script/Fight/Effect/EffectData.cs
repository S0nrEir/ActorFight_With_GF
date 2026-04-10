using System;
using System.Collections.Generic;
using System.Linq;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// Effect数据（不可变结构体）
    /// </summary>
    public readonly struct EffectData
    {
        public EffectData(
            int effectId,
            int stackLimit,
            bool canStack,
            float startTime,
            float endTime,
            EffectType effectType,
            NumricModifierType modifierType,
            actor_attribute affectedAttribute,
            int target,
            float duration,
            float period,
            DurationPolicy policy,
            bool effectOnAwake,
            int[] deriveEffects,
            int[] awakeEffects,
            float floatParam1 = 0f,
            float floatParam2 = 0f,
            float floatParam3 = 0f,
            float floatParam4 = 0f,
            int intParam1 = 0,
            int intParam2 = 0,
            int intParam3 = 0,
            int intParam4 = 0,
            int resolveTypeID = -1)
        {
            _effectId = effectId;
            _stackLimit = stackLimit;
            _canStack = canStack;
            _startTime = startTime;
            _endTime = endTime;
            _effectType = effectType;
            _modifierType = modifierType;
            _affectedAttribute = affectedAttribute;
            _target = target;
            _duration = duration;
            _period = period;
            _policy = policy;
            _effectOnAwake = effectOnAwake;
            _deriveEffects = deriveEffects?.ToArray() ?? Array.Empty<int>();
            _awakeEffects = awakeEffects?.ToArray() ?? Array.Empty<int>();
            _floatParam1 = floatParam1;
            _floatParam2 = floatParam2;
            _floatParam3 = floatParam3;
            _floatParam4 = floatParam4;
            _intParam1 = intParam1;
            _intParam2 = intParam2;
            _intParam3 = intParam3;
            _intParam4 = intParam4;
            _resolveTypeID = resolveTypeID;
        }

        // Getter 方法
        public int GetEffectId() => _effectId;
        public int GetStackLimit() => _stackLimit;
        public bool GetCanStack() => _canStack;
        public float GetStartTime() => _startTime;
        public float GetEndTime() => _endTime;
        public EffectType GetEffectType() => _effectType;
        public NumricModifierType GetModifierType() => _modifierType;
        public actor_attribute GetAffectedAttribute() => _affectedAttribute;
        public int GetTarget() => _target;
        public float GetDuration() => _duration;
        public float GetPeriod() => _period;
        public DurationPolicy GetPolicy() => _policy;
        public bool GetEffectOnAwake() => _effectOnAwake;
        public IReadOnlyList<int> GetDeriveEffects() => _deriveEffects;
        public IReadOnlyList<int> GetAwakeEffects() => _awakeEffects;
        public float GetFloatParam1() => _floatParam1;
        public float GetFloatParam2() => _floatParam2;
        public float GetFloatParam3() => _floatParam3;
        public float GetFloatParam4() => _floatParam4;
        public int GetIntParam1() => _intParam1;
        public int GetIntParam2() => _intParam2;
        public int GetIntParam3() => _intParam3;
        public int GetIntParam4() => _intParam4;
        public int GetResolveTypeID() => _resolveTypeID;
        
        
        // 基础字段
        private readonly int _effectId;
        private readonly int _stackLimit;
        private readonly bool _canStack;
        
        // 时间字段
        private readonly float _startTime;
        private readonly float _endTime;
        
        // Effect 配置
        private readonly EffectType _effectType;
        private readonly NumricModifierType _modifierType;
        private readonly actor_attribute _affectedAttribute;
        private readonly int _target;
        private readonly float _duration;
        private readonly float _period;
        private readonly DurationPolicy _policy;
        private readonly bool _effectOnAwake;
        
        // 扩展参数
        private readonly float _floatParam1;
        private readonly float _floatParam2;
        private readonly float _floatParam3;
        private readonly float _floatParam4;
        private readonly int _intParam1;
        private readonly int _intParam2;
        private readonly int _intParam3;
        private readonly int _intParam4;
        private readonly int _resolveTypeID;
        
        // 数组（不可变）
        private readonly IReadOnlyList<int> _deriveEffects;
        private readonly IReadOnlyList<int> _awakeEffects;
    }
}