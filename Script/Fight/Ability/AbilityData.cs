using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 技能数据
    /// </summary>
    public struct AbilityData
    {
        private int _id;
        // private string _name;
        // private string _desc;
        private int _costEffectID;
        private int _coolDownEffectID;
        private AbilityTargetType _targetType;
        private int _timelineID;
        // private string _timelineAssetPath;
        private float _timelineDuration;
        private EffectData[] _effects;
        private AudioData[] _audios;
        private VFXData[] _vfxs;

        public AbilityData(
            int id,
            string name,
            string desc,
            int costEffectID,
            int coolDownEffectID,
            AbilityTargetType targetType,
            int timelineID,
            string timelineAssetPath,
            float timelineDuration,
            EffectData[] effects,
            AudioData[] audios,
            VFXData[] vfxs)
        {
            _id = id;
            // _name = name;
            // _desc = desc;
            _costEffectID = costEffectID;
            _coolDownEffectID = coolDownEffectID;
            _targetType = targetType;
            _timelineID = timelineID;
            // _timelineAssetPath = timelineAssetPath;
            _timelineDuration = timelineDuration;
            _effects = effects;
            _audios = audios;
            _vfxs = vfxs;
        }

        public int GetId() => _id;
        // public string GetName() => _name;
        // public string GetDesc() => _desc;
        public int GetCostEffectID() => _costEffectID;
        public int GetCoolDownEffectID() => _coolDownEffectID;
        public AbilityTargetType GetTargetType() => _targetType;
        public int GetTimelineID() => _timelineID;
        // public string GetTimelineAssetPath() => _timelineAssetPath;
        public float GetTimelineDuration() => _timelineDuration;
        public EffectData[] GetEffects() => _effects;
        public AudioData[] GetAudios() => _audios;
        public VFXData[] GetVFXs() => _vfxs;

        public void SetId(int value) => _id = value;
        // public void SetName(string value) => _name = value;
        // public void SetDesc(string value) => _desc = value;
        public void SetCostEffectID(int value) => _costEffectID = value;
        public void SetCoolDownEffectID(int value) => _coolDownEffectID = value;
        public void SetTargetType(AbilityTargetType value) => _targetType = value;
        public void SetTimelineID(int value) => _timelineID = value;
        // public void SetTimelineAssetPath(string value) => _timelineAssetPath = value;
        public void SetTimelineDuration(float value) => _timelineDuration = value;
        public void SetEffects(EffectData[] value) => _effects = value;
        public void SetAudios(AudioData[] value) => _audios = value;
        public void SetVFXs(VFXData[] value) => _vfxs = value;
    }
}
