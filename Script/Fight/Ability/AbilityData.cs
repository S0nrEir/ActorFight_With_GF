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
        private readonly int _timelineID;
        private readonly float _timelineDuration;
        private readonly IReadOnlyList<EffectData> _effects;
        // private readonly IReadOnlyList<AudioData> _audios;
        // private readonly IReadOnlyList<VFXData> _vfxs;

        public AbilityData(
            int id,
            int costEffectID,
            int coolDownEffectID,
            AbilityTargetType targetType,
            int timelineID,
            float timelineDuration,
            EffectData[] effects)
            // AudioData[] audios,
            // VFXData[] vfxs)
        {
            _id = id;
            _costEffectID = costEffectID;
            _coolDownEffectID = coolDownEffectID;
            _targetType = targetType;
            _timelineID = timelineID;
            _timelineDuration = timelineDuration;
            _effects = effects?.ToArray() ?? System.Array.Empty<EffectData>();
            // _audios = audios?.ToArray() ?? System.Array.Empty<AudioData>();
            // _vfxs = vfxs?.ToArray() ?? System.Array.Empty<VFXData>();
        }

        // Getter 方法
        public int GetId() => _id;
        public int GetCostEffectID() => _costEffectID;
        public int GetCoolDownEffectID() => _coolDownEffectID;
        public AbilityTargetType GetTargetType() => _targetType;
        public int GetTimelineID() => _timelineID;
        public float GetTimelineDuration() => _timelineDuration;
        public IReadOnlyList<EffectData> GetEffects() => _effects;
        // public IReadOnlyList<AudioData> GetAudios() => _audios;
        // public IReadOnlyList<VFXData> GetVFXs() => _vfxs;
    }
}
