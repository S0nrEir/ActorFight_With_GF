using System;
using System.Collections.Generic;
using System.Linq;
using Aquila.AbilityEditor;
using Cfg.Enum;

namespace Editor.AbilityEditor.Config
{
    /// <summary>
    /// Trigger data structure that groups effects by trigger time
    /// </summary>
    [Serializable]
    public class TriggerData
    {
        /// <summary>
        /// Trigger time in seconds
        /// </summary>
        public float TriggerTime { get; set; }

        /// <summary>
        /// List of effect IDs that trigger at this time
        /// </summary>
        public List<int> EffectIDs { get; set; }

        /// <summary>
        /// Source track name for debugging
        /// </summary>
        public string SourceTrack { get; set; }

        public TriggerData()
        {
            EffectIDs = new List<int>();
            SourceTrack = string.Empty;
        }

        public TriggerData(float triggerTime, List<int> effectIDs, string sourceTrack = "")
        {
            TriggerTime = triggerTime;
            EffectIDs = effectIDs ?? new List<int>();
            SourceTrack = sourceTrack ?? string.Empty;
        }

        public override string ToString()
        {
            var effectsStr = string.Join(", ", EffectIDs);
            return $"Trigger[Time={TriggerTime:F2}s, Effects=[{effectsStr}], Track={SourceTrack}]";
        }
    }

    /// <summary>
    /// Ability configuration container generated from the visual timeline editor
    /// Provides structured access to all ability data for custom export or runtime integration
    /// </summary>
    public class AbilityConfig
    {
        #region Basic Metadata

        /// <summary>
        /// Ability ID
        /// </summary>
        public int AbilityID { get; set; }

        /// <summary>
        /// Ability name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ability description
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Cost effect ID
        /// </summary>
        public int CostEffectID { get; set; }

        /// <summary>
        /// Cooldown effect ID
        /// </summary>
        public int CoolDownEffectID { get; set; }

        /// <summary>
        /// Target type
        /// </summary>
        public AbilityTargetType TargetType { get; set; }

        /// <summary>
        /// Timeline ID
        /// </summary>
        public int TimelineID { get; set; }

        /// <summary>
        /// Timeline duration in seconds
        /// </summary>
        public float TimelineDuration { get; set; }

        /// <summary>
        /// Data source for debugging ("AbilityData" or "EditorMemory")
        /// </summary>
        public string DataSource { get; set; }

        #endregion

        #region Clip Collections

        private readonly List<TriggerData> _triggers;
        private readonly List<EffectClipData> _effects;
        // private readonly List<SkillClipData> _skills;
        private readonly List<AudioClipData> _audios;
        private readonly List<VFXClipData> _vfxs;

        /// <summary>
        /// Triggers (effects grouped by time)
        /// </summary>
        public IReadOnlyList<TriggerData> Triggers => _triggers.AsReadOnly();

        /// <summary>
        /// All effect clips
        /// </summary>
        public IReadOnlyList<EffectClipData> Effects => _effects.AsReadOnly();

        /// <summary>
        /// All skill clips
        /// </summary>
        // public IReadOnlyList<SkillClipData> Skills => _skills.AsReadOnly();

        /// <summary>
        /// All audio clips
        /// </summary>
        public IReadOnlyList<AudioClipData> Audios => _audios.AsReadOnly();

        /// <summary>
        /// All VFX clips
        /// </summary>
        public IReadOnlyList<VFXClipData> VFXs => _vfxs.AsReadOnly();

        #endregion

        public AbilityConfig()
        {
            Name = string.Empty;
            Desc = string.Empty;
            DataSource = "EditorMemory"; // Default value
            _triggers = new List<TriggerData>();
            _effects = new List<EffectClipData>();
            // _skills = new List<SkillClipData>();
            _audios = new List<AudioClipData>();
            _vfxs = new List<VFXClipData>();
        }

        /// <summary>
        /// Initialize config with collections
        /// </summary>
        public void Initialize(
            List<TriggerData> triggers,
            List<EffectClipData> effects,
            List<AudioClipData> audios,
            List<VFXClipData> vfxs)
        {
            if (triggers != null)
                _triggers.AddRange(triggers);
            
            if (effects != null) 
                _effects.AddRange(effects);
            
            if (audios != null) 
                _audios.AddRange(audios);
            
            if (vfxs != null)
                _vfxs.AddRange(vfxs);
        }


        /// <summary>
        /// Get all clips at specific time (within 0.01s tolerance)
        /// </summary>
        public List<TimelineClipData> GetClipsAtTime(float time)
        {
            const float tolerance = 0.01f;
            var clips = new List<TimelineClipData>();

            clips.AddRange(_effects.Where(c => Math.Abs(c.TriggerTime - time) < tolerance));
            // clips.AddRange(_skills.Where(c => Math.Abs(c.StartTime - time) < tolerance));
            clips.AddRange(_audios.Where(c => Math.Abs(c.StartTime - time) < tolerance));
            clips.AddRange(_vfxs.Where(c => Math.Abs(c.StartTime - time) < tolerance));

            return clips;
        }

        /// <summary>
        /// Get clips of specific type
        /// </summary>
        public List<T> GetClipsByType<T>() where T : TimelineClipData
        {
            if (typeof(T) == typeof(EffectClipData))
                return _effects.Cast<T>().ToList();
            // if (typeof(T) == typeof(SkillClipData))
            //     return _skills.Cast<T>().ToList();
            if (typeof(T) == typeof(AudioClipData))
                return _audios.Cast<T>().ToList();
            if (typeof(T) == typeof(VFXClipData))
                return _vfxs.Cast<T>().ToList();

            return new List<T>();
        }

        /// <summary>
        /// Get all trigger times sorted ascending
        /// </summary>
        public List<float> GetAllTriggerTimes()
        {
            return _triggers.Select(t => t.TriggerTime).OrderBy(t => t).ToList();
        }

        /// <summary>
        /// Get clips within time range (inclusive)
        /// </summary>
        public List<TimelineClipData> GetClipsInRange(float start, float end)
        {
            var clips = new List<TimelineClipData>();

            clips.AddRange(_effects.Where(c => c.TriggerTime >= start && c.TriggerTime <= end));
            clips.AddRange(_audios.Where(c => c.StartTime >= start && c.StartTime <= end));
            clips.AddRange(_vfxs.Where(c => c.StartTime >= start && c.StartTime <= end));

            return clips.OrderBy(c => c.StartTime).ToList();
        }
        
        #region Validation

        /// <summary>
        /// Validate internal consistency
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (AbilityID <= 0)
            {
                errorMessage = "Ability ID must be greater than 0";
                return false;
            }

            if (TimelineDuration <= 0)
            {
                errorMessage = "Timeline duration must be greater than 0";
                return false;
            }
            
            foreach (var effect in _effects)
            {
                if (effect.TriggerTime > TimelineDuration)
                {
                    errorMessage = $"Effect clip '{effect.ClipName}' at {effect.TriggerTime}s exceeds timeline duration {TimelineDuration}s";
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Optional Mutators

        /// <summary>
        /// Add a trigger (for advanced scenarios)
        /// </summary>
        public void AddTrigger(TriggerData trigger)
        {
            if (trigger != null)
                _triggers.Add(trigger);
        }

        /// <summary>
        /// Remove triggers at specific time
        /// </summary>
        public void RemoveTriggersAtTime(float time)
        {
            _triggers.RemoveAll(t => Math.Abs(t.TriggerTime - time) < 0.01f);
        }

        #endregion
        public override string ToString()
        {
            return $"AbilityConfig[ID={AbilityID}, Name={Name}, DataSource={DataSource}, Duration={TimelineDuration}s, " +
                   $"Triggers={_triggers.Count}, Effects={_effects.Count}," +
                   $"Audios={_audios.Count}, VFXs={_vfxs.Count}]";
        }
    }
}
