using System;
using System.Collections.Generic;
using System.Linq;
using Aquila.AbilityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Config
{
    /// <summary>
    /// Validation logic for ability configuration data
    /// Ensures data integrity before config generation
    /// </summary>
    public static class AbilityConfigValidator
    {
        private const float TRIGGER_COLLISION_THRESHOLD = 0.01f;

        #region Metadata Validation

        /// <summary>
        /// Validate basic ability metadata
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when metadata is invalid</exception>
        public static void ValidateAbilityMetadata(int abilityID, string name, int timelineID)
        {
            if (abilityID <= 0)
            {
                throw new ArgumentException("Ability ID must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Ability Name cannot be empty");
            }

            if (timelineID < 0)
            {
                throw new ArgumentException("Timeline ID cannot be negative");
            }
        }

        #endregion

        #region Timeline Validation

        /// <summary>
        /// Validate timeline duration and clip bounds
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when timeline is invalid</exception>
        public static void ValidateTimeline(
            float timelineDuration,
            List<EffectClipData> effects,
            List<SkillClipData> skills,
            List<AudioClipData> audios,
            List<VFXClipData> vfxs)
        {
            if (timelineDuration <= 0)
            {
                throw new InvalidOperationException("Timeline duration must be greater than 0");
            }

            // Check effect clips
            foreach (var effect in effects)
            {
                if (effect.TriggerTime > timelineDuration)
                {
                    throw new InvalidOperationException(
                        $"Effect clip '{effect.ClipName}' at {effect.TriggerTime:F2}s exceeds timeline duration {timelineDuration:F2}s");
                }
            }

            // Check skill clips
            foreach (var skill in skills)
            {
                if (skill.StartTime > timelineDuration)
                {
                    throw new InvalidOperationException(
                        $"Skill clip '{skill.ClipName}' at {skill.StartTime:F2}s exceeds timeline duration {timelineDuration:F2}s");
                }
            }

            // Check audio clips
            foreach (var audio in audios)
            {
                if (audio.StartTime > timelineDuration)
                {
                    throw new InvalidOperationException(
                        $"Audio clip '{audio.ClipName}' at {audio.StartTime:F2}s exceeds timeline duration {timelineDuration:F2}s");
                }
            }

            // Check VFX clips
            foreach (var vfx in vfxs)
            {
                if (vfx.StartTime > timelineDuration)
                {
                    throw new InvalidOperationException(
                        $"VFX clip '{vfx.ClipName}' at {vfx.StartTime:F2}s exceeds timeline duration {timelineDuration:F2}s");
                }
            }
        }

        #endregion

        #region Effect ID Validation

        /// <summary>
        /// Validate all effect IDs are non-negative
        /// </summary>
        /// <exception cref="System.IO.InvalidDataException">Thrown when effect ID is invalid</exception>
        public static void ValidateEffectIDs(List<EffectClipData> effects)
        {
            foreach (var effect in effects)
            {
                if (effect.EffectId < 0)
                {
                    throw new System.IO.InvalidDataException(
                        $"Effect ID cannot be negative. Clip '{effect.ClipName}' has EffectID={effect.EffectId}");
                }
            }
        }

        /// <summary>
        /// Validate skill effect IDs array
        /// </summary>
        /// <exception cref="System.IO.InvalidDataException">Thrown when skill effect ID is invalid</exception>
        public static void ValidateSkillEffectIDs(List<SkillClipData> skills)
        {
            foreach (var skill in skills)
            {
                if (skill.EffectIds != null)
                {
                    for (int i = 0; i < skill.EffectIds.Length; i++)
                    {
                        if (skill.EffectIds[i] < 0)
                        {
                            throw new System.IO.InvalidDataException(
                                $"Skill clip '{skill.ClipName}' has negative EffectID at index {i}: {skill.EffectIds[i]}");
                        }
                    }
                }
            }
        }

        #endregion

        #region Trigger Collision Detection

        /// <summary>
        /// Check for trigger time collisions and log warnings
        /// </summary>
        public static void CheckTriggerCollisions(List<TriggerData> triggers)
        {
            if (triggers == null || triggers.Count <= 1)
                return;

            for (int i = 0; i < triggers.Count - 1; i++)
            {
                float timeDiff = triggers[i + 1].TriggerTime - triggers[i].TriggerTime;

                if (timeDiff > 0 && timeDiff < TRIGGER_COLLISION_THRESHOLD)
                {
                    Debug.LogWarning(
                        $"[AbilityConfig] Triggers at {triggers[i].TriggerTime:F3}s and {triggers[i + 1].TriggerTime:F3}s " +
                        $"are very close ({timeDiff * 1000:F1}ms apart). Consider merging or adjusting timing.");
                }
            }
        }

        #endregion

        #region Placeholder Warnings

        /// <summary>
        /// Log warnings for incomplete VFX and Audio data
        /// </summary>
        public static void WarnIncompletePlaceholders(List<AudioClipData> audios, List<VFXClipData> vfxs)
        {
            // Check audio clips
            foreach (var audio in audios)
            {
                if (string.IsNullOrWhiteSpace(audio.AudioPath))
                {
                    Debug.LogWarning(
                        $"[AbilityConfig] Audio clip '{audio.ClipName}' at {audio.StartTime:F2}s has no asset path. " +
                        $"Remember to assign Audio assets later.");
                }
            }

            // Check VFX clips
            foreach (var vfx in vfxs)
            {
                if (string.IsNullOrWhiteSpace(vfx.VfxPath))
                {
                    Debug.LogWarning(
                        $"[AbilityConfig] VFX clip '{vfx.ClipName}' at {vfx.StartTime:F2}s has no asset path. " +
                        $"Remember to assign VFX assets later.");
                }
            }
        }

        #endregion

        #region Comprehensive Validation

        /// <summary>
        /// Run all validation checks on collected data
        /// </summary>
        public static void ValidateAll(
            int abilityID,
            string name,
            int timelineID,
            float timelineDuration,
            List<EffectClipData> effects,
            List<SkillClipData> skills,
            List<AudioClipData> audios,
            List<VFXClipData> vfxs,
            List<TriggerData> triggers)
        {
            // Metadata validation (throws exceptions on failure)
            ValidateAbilityMetadata(abilityID, name, timelineID);

            // Timeline validation (throws exceptions on failure)
            ValidateTimeline(timelineDuration, effects, skills, audios, vfxs);

            // Effect ID validation (throws exceptions on failure)
            if (effects != null && effects.Count > 0)
            {
                ValidateEffectIDs(effects);
            }

            // Skill effect ID validation (throws exceptions on failure)
            if (skills != null && skills.Count > 0)
            {
                ValidateSkillEffectIDs(skills);
            }

            // Trigger collision detection (warning only)
            if (triggers != null && triggers.Count > 0)
            {
                CheckTriggerCollisions(triggers);
            }

            // Placeholder warnings (warning only)
            WarnIncompletePlaceholders(
                audios ?? new List<AudioClipData>(),
                vfxs ?? new List<VFXClipData>());
        }

        #endregion
    }
}
