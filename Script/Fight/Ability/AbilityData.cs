using System;
using System.Collections.Generic;
using System.Linq;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 技能数据（不可变结构体）
    /// </summary>
    public readonly struct AbilityData
    {
        private readonly int _id;
        private readonly int _costEffectID;
        private readonly int _coolDownEffectID;
        private readonly AbilityTargetType _targetType;
        private readonly AbilitySelectType _selectType;
        private readonly float _selectRadius;
        private readonly int _timelineID;
        private readonly float _timelineDuration;
        private readonly IReadOnlyList<EffectData> _effects;
        private readonly IReadOnlyList<MontageEventData> _montageEvents;
        private readonly IReadOnlyList<AbilityCueBindingData> _cueBindings;
        // private readonly IReadOnlyList<AudioData> _audios;
        // private readonly IReadOnlyList<VFXData> _vfxs;

        public AbilityData(
            int id,
            int costEffectID,
            int coolDownEffectID,
            AbilityTargetType targetType,
            AbilitySelectType selectType,
            float selectRadius,
            int timelineID,
            float timelineDuration,
            EffectData[] effects,
            MontageEventData[] montageEvents = null,
            AbilityCueBindingData[] cueBindings = null)
            // AudioData[] audios,
            // VFXData[] vfxs)
        {
            _id = id;
            _costEffectID = costEffectID;
            _coolDownEffectID = coolDownEffectID;
            _targetType = targetType;
            _selectType = selectType;
            _selectRadius = selectRadius;
            _timelineID = timelineID;
            _timelineDuration = timelineDuration;
            _effects = effects?.ToArray() ?? Array.Empty<EffectData>();
            _montageEvents = montageEvents?.ToArray() ?? Array.Empty<MontageEventData>();
            _cueBindings = cueBindings?.ToArray() ?? Array.Empty<AbilityCueBindingData>();
            // _audios = audios?.ToArray() ?? System.Array.Empty<AudioData>();
            // _vfxs = vfxs?.ToArray() ?? System.Array.Empty<VFXData>();
        }

        // Getter 方法
        public int GetId() => _id;
        public int GetCostEffectID() => _costEffectID;
        public int GetCoolDownEffectID() => _coolDownEffectID;
        public AbilityTargetType GetTargetType() => _targetType;
        public AbilitySelectType GetSelectType() => _selectType;
        public float GetSelectRadius() => _selectRadius;
        public int GetTimelineID() => _timelineID;
        public float GetTimelineDuration() => _timelineDuration;
        public IReadOnlyList<EffectData> GetEffects() => _effects;
        public IReadOnlyList<MontageEventData> GetMontageEvents() => _montageEvents;
        public IReadOnlyList<AbilityCueBindingData> GetCueBindings() => _cueBindings;
        // public IReadOnlyList<AudioData> GetAudios() => _audios;
        // public IReadOnlyList<VFXData> GetVFXs() => _vfxs;
    }
}
