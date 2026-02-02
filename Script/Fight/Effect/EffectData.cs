using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// Effect数据
    /// </summary>
    public struct EffectData
    {
        private float _triggerTime;
        private int _effectId;
        private int _stackCount;
        private bool _canStack;
        private EffectType _effectType;
        private ushort _modifierType;
        private actor_attribute _affectedAttribute;
        private int _target;
        private float _duration;
        private float _period;
        private DurationPolicy _policy;
        private bool _effectOnAwake;
        private int[] _deriveEffects;
        private int[] _awakeEffects;
        private float _floatParam1;
        private float _floatParam2;
        private float _floatParam3;
        private float _floatParam4;
        private int _intParam1;
        private int _intParam2;
        private int _intParam3;
        private int _intParam4;

        public EffectData(
            float triggerTime,
            int effectId,
            int stackCount,
            bool canStack,
            EffectType effectType,
            ushort modifierType,
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
            int intParam4 = 0)
        {
            _triggerTime = triggerTime;
            _effectId = effectId;
            _stackCount = stackCount;
            _canStack = canStack;
            _effectType = effectType;
            _modifierType = modifierType;
            _affectedAttribute = affectedAttribute;
            _target = target;
            _duration = duration;
            _period = period;
            _policy = policy;
            _effectOnAwake = effectOnAwake;
            _deriveEffects = deriveEffects;
            _awakeEffects = awakeEffects;
            _floatParam1 = floatParam1;
            _floatParam2 = floatParam2;
            _floatParam3 = floatParam3;
            _floatParam4 = floatParam4;
            _intParam1 = intParam1;
            _intParam2 = intParam2;
            _intParam3 = intParam3;
            _intParam4 = intParam4;
        }

        public float GetTriggerTime() => _triggerTime;
        public int GetEffectId() => _effectId;
        public int GetStackCount() => _stackCount;
        public bool GetCanStack() => _canStack;
        public EffectType GetEffectType() => _effectType;
        public ushort GetModifierType() => _modifierType;
        public actor_attribute GetAffectedAttribute() => _affectedAttribute;
        public int GetTarget() => _target;
        public float GetDuration() => _duration;
        public float GetPeriod() => _period;
        public DurationPolicy GetPolicy() => _policy;
        public bool GetEffectOnAwake() => _effectOnAwake;
        public int[] GetDeriveEffects() => _deriveEffects;
        public int[] GetAwakeEffects() => _awakeEffects;
        public float GetFloatParam1() => _floatParam1;
        public float GetFloatParam2() => _floatParam2;
        public float GetFloatParam3() => _floatParam3;
        public float GetFloatParam4() => _floatParam4;
        public int GetIntParam1() => _intParam1;
        public int GetIntParam2() => _intParam2;
        public int GetIntParam3() => _intParam3;
        public int GetIntParam4() => _intParam4;

        public void SetTriggerTime(float value) => _triggerTime = value;
        public void SetEffectId(int value) => _effectId = value;
        public void SetStackCount(int value) => _stackCount = value;
        public void SetCanStack(bool value) => _canStack = value;
        public void SetEffectType(EffectType value) => _effectType = value;
        public void SetModifierType(ushort value) => _modifierType = value;
        public void SetAffectedAttribute(actor_attribute value) => _affectedAttribute = value;
        public void SetTarget(int value) => _target = value;
        public void SetDuration(float value) => _duration = value;
        public void SetPeriod(float value) => _period = value;
        public void SetPolicy(DurationPolicy value) => _policy = value;
        public void SetEffectOnAwake(bool value) => _effectOnAwake = value;
        public void SetDeriveEffects(int[] value) => _deriveEffects = value;
        public void SetAwakeEffects(int[] value) => _awakeEffects = value;
        public void SetFloatParam1(float value) => _floatParam1 = value;
        public void SetFloatParam2(float value) => _floatParam2 = value;
        public void SetFloatParam3(float value) => _floatParam3 = value;
        public void SetFloatParam4(float value) => _floatParam4 = value;
        public void SetIntParam1(int value) => _intParam1 = value;
        public void SetIntParam2(int value) => _intParam2 = value;
        public void SetIntParam3(int value) => _intParam3 = value;
        public void SetIntParam4(int value) => _intParam4 = value;
    }
}