using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aquila.AbilityEditor;
using Cfg.Enum;
using UnityEngine.UIElements;

namespace Editor.AbilityEditor.Config
{
    /// <summary>
    /// 技能配置生成器
    /// </summary>
    public static class AbilityConfigGenerator
    {
        /// <summary>
        /// Generate config from AbilityEditorWindow (legacy method)
        /// </summary>
        public static AbilityConfig Generate(AbilityEditorWindow editor)
        {
            if (editor == null)
                throw new ArgumentNullException(nameof(editor), "Editor window cannot be null");

            //从UI字段转换元数据 / Converting metadata from UI fields
            var metadata = ParseMetadata(editor);
            //收集轨道上的所有clips / Collect all clips on the timeline
            var clipCollections = CollectClips(editor);
            //从effect clip收集数据 / Collect data from effect clips.
            var triggers = GenerateTriggers(clipCollections.Effects);
            AbilityConfigValidator.ValidateAll(
                metadata.AbilityID,
                metadata.Name,
                metadata.TimelineID,
                metadata.TimelineDuration,
                clipCollections.Effects,
                clipCollections.Audios,
                clipCollections.VFXs,
                triggers);

            //create config
            var config = new AbilityConfig
            {
                AbilityID = metadata.AbilityID,
                Name = metadata.Name,
                Desc = metadata.Desc,
                CostEffectID = metadata.CostEffectID,
                CoolDownEffectID = metadata.CoolDownEffectID,
                TargetType = metadata.TargetType,
                TimelineID = metadata.TimelineID,
                TimelineAssetPath = metadata.TimelineAssetPath,
                TimelineDuration = metadata.TimelineDuration,
                DataSource = "EditorMemory"
            };

            config.Initialize(
                triggers,
                clipCollections.Effects,
                clipCollections.Audios,
                clipCollections.VFXs);

            Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Successfully generated config: {config}");
            return config;
        }

        /// <summary>
        /// Generate config from AbilityData with optional editor tracks fallback
        /// </summary>
        public static AbilityConfig Generate(
            AbilityEditorSOData sourceData,
            List<TimelineTrackItem> editorTracks = null)
        {
            if (sourceData == null)
                throw new ArgumentNullException(nameof(sourceData), "AbilityData cannot be null");

            // Determine data source priority: AbilityData.Tracks > editorTracks
            ClipCollections clipCollections;
            string dataSource;

            if (sourceData.Tracks != null && sourceData.Tracks.Count > 0)
            {
                // Primary: Load from AbilityData.Tracks
                Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Generating config from AbilityData: {sourceData.name}");
                clipCollections = CollectClipsFromSerializedTracks(sourceData.Tracks);
                dataSource = "AbilityData";
            }
            else if (editorTracks != null && editorTracks.Count > 0)
            {
                // Fallback: Load from editor memory
                Aquila.Toolkit.Tools.Logger.Warning("[AbilityConfigGenerator] AbilityData.Tracks is empty. Using editor memory state.");
                clipCollections = CollectClipsFromTracks(editorTracks);
                dataSource = "EditorMemory";
            }
            else
            {
                // No data available
                throw new ArgumentException("No track data available. Either provide AbilityData.Tracks or editorTracks parameter.");
            }

            // Extract metadata from AbilityData
            var metadata = new AbilityMetadata
            {
                AbilityID = sourceData.Id,
                Name = !string.IsNullOrWhiteSpace(sourceData.Name) ? sourceData.Name : $"Ability_{sourceData.Id}",
                Desc = sourceData.Desc ?? string.Empty,
                CostEffectID = sourceData.CostEffectID,
                CoolDownEffectID = sourceData.CoolDownEffectID,
                TargetType = sourceData.TargetType,
                TimelineID = sourceData.TimelineID,
                TimelineAssetPath = sourceData.TimelineAssetPath ?? string.Empty,
                TimelineDuration = sourceData.TimelineDuration
            };

            // Generate triggers
            var triggers = GenerateTriggers(clipCollections.Effects);

            // Validate
            AbilityConfigValidator.ValidateAll(
                metadata.AbilityID,
                metadata.Name,
                metadata.TimelineID,
                metadata.TimelineDuration,
                clipCollections.Effects,
                clipCollections.Audios,
                clipCollections.VFXs,
                triggers);

            // Create config
            var config = new AbilityConfig
            {
                AbilityID = metadata.AbilityID,
                Name = metadata.Name,
                Desc = metadata.Desc,
                CostEffectID = metadata.CostEffectID,
                CoolDownEffectID = metadata.CoolDownEffectID,
                TargetType = metadata.TargetType,
                TimelineID = metadata.TimelineID,
                TimelineAssetPath = metadata.TimelineAssetPath,
                TimelineDuration = metadata.TimelineDuration,
                DataSource = dataSource
            };

            config.Initialize(
                triggers,
                clipCollections.Effects,
                clipCollections.Audios,
                clipCollections.VFXs);

            Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Successfully generated config: {config}");
            return config;
        }
        
        

        private struct AbilityMetadata
        {
            public int AbilityID;
            public string Name;
            public string Desc;
            public int CostEffectID;
            public int CoolDownEffectID;
            public AbilityTargetType TargetType;
            public int TimelineID;
            public string TimelineAssetPath;
            public float TimelineDuration;
        }

        /// <summary>
        /// Parse metadata from editor UI fields using reflection
        /// </summary>
        private static AbilityMetadata ParseMetadata(AbilityEditorWindow editor)
        {
            var metadata = new AbilityMetadata();

            // Use reflection to access private fields
            var editorType = editor.GetType();

            // Parse Ability ID
            var abilityIDField = editorType.GetField("_abilityIDTextField",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var abilityIDTextField = abilityIDField?.GetValue(editor) as TextField;
            if (abilityIDTextField != null && int.TryParse(abilityIDTextField.value, out int abilityID))
                metadata.AbilityID = abilityID;
            else
                throw new ArgumentException("Ability ID is empty or invalid");

            // Parse Ability Name (use ID as name if description field is empty)
            var descField = editorType.GetField("_abilityDescTextField", BindingFlags.NonPublic | BindingFlags.Instance);
            var descTextField = descField?.GetValue(editor) as TextField;

            metadata.Name = !string.IsNullOrWhiteSpace(descTextField?.value)
                ? descTextField.value
                : $"Ability_{metadata.AbilityID}";
            metadata.Desc = descTextField?.value ?? string.Empty;

            // Parse Cost Effect ID
            var costField = editorType.GetField("_costIDTextField",BindingFlags.NonPublic | BindingFlags.Instance);
            var costTextField = costField?.GetValue(editor) as TextField;
            if (costTextField != null && int.TryParse(costTextField.value, out int costID))
                metadata.CostEffectID = costID;
            else
                metadata.CostEffectID = -1;

            // Parse CoolDown Effect ID
            var coolDownField = editorType.GetField("_coolDownIDTextField", BindingFlags.NonPublic | BindingFlags.Instance);
            var coolDownTextField = coolDownField?.GetValue(editor) as TextField;
            if (coolDownTextField != null && int.TryParse(coolDownTextField.value, out int coolDownID))
                metadata.CoolDownEffectID = coolDownID;
            else
                metadata.CoolDownEffectID = -1;
            
            var timelineIDField = editorType.GetField("_timelineIDTextField",BindingFlags.NonPublic | BindingFlags.Instance);
            var timelineIDTextField = timelineIDField?.GetValue(editor) as TextField;
            if (timelineIDTextField != null && int.TryParse(timelineIDTextField.value, out int timelineID))
                metadata.TimelineID = timelineID;
            else
                metadata.TimelineID = -1;

            // Parse Timeline Asset Path
            var timelineAssetPathField = editorType.GetField("_timelineAssetPathTxtField", BindingFlags.NonPublic | BindingFlags.Instance);
            var timelineAssetPathTextField = timelineAssetPathField?.GetValue(editor) as TextField;
            metadata.TimelineAssetPath = timelineAssetPathTextField?.value ?? string.Empty;

            // Parse Target Type
            var targetTypeField = editorType.GetField("_targetTypeDropdown",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var targetTypeDropdown = targetTypeField?.GetValue(editor) as DropdownField;

            if (targetTypeDropdown != null && Enum.TryParse<AbilityTargetType>(targetTypeDropdown.value, out var targetType))
            {
                metadata.TargetType = targetType;
            }

            // Parse Duration
            var durationField = editorType.GetField("_durationTextField",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var durationTextField = durationField?.GetValue(editor) as TextField;

            if (durationTextField != null && float.TryParse(durationTextField.value, out float duration))
            {
                metadata.TimelineDuration = duration;
            }
            else
            {
                throw new ArgumentException("Timeline duration is empty or invalid");
            }

            return metadata;
        }

        #region Clip Collection

        /// <summary>
        /// clip集合
        /// </summary>
        private struct ClipCollections
        {
            public List<EffectClipData> Effects;
            // public List<SkillClipData> Skills;
            public List<AudioClipData> Audios;
            public List<VFXClipData> VFXs;
        }

        /// <summary>
        /// 收集所有已启用轨道中所有已启用的clip /  Collect all enabled clips from all enabled tracks
        /// </summary>
        private static ClipCollections CollectClips(AbilityEditorWindow editor)
        {
            var collections = new ClipCollections
            {
                Effects = new List<EffectClipData>(),
                Audios = new List<AudioClipData>(),
                VFXs = new List<VFXClipData>()
            };

            // Access _timelineTrackItems using reflection
            var editorType = editor.GetType();
            var trackItemsField = editorType.GetField("_timelineTrackItems",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var trackItems = trackItemsField?.GetValue(editor) as List<TimelineTrackItem>;

            if (trackItems == null || trackItems.Count == 0)
            {
                Aquila.Toolkit.Tools.Logger.Warning("[AbilityConfigGenerator] No tracks found in editor");
                return collections;
            }

            // Iterate through all tracks
            foreach (var track in trackItems)
            {
                if (!track.IsEnabled)
                {
                    Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Skipping disabled track: {track.Name}");
                    continue;
                }

                // Get clips from track
                var clips = track.Clips;
                if (clips == null || clips.Count == 0)
                    continue;
                
                foreach (var clip in clips)
                {
                    if (!clip.IsEnabled)
                    {
                        Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Skipping disabled clip: {clip.ClipName}");
                        continue;
                    }
                    
                    switch (clip)
                    {
                        case EffectClipData effectClip:
                            collections.Effects.Add(effectClip);
                            break;

                        // case SkillClipData skillClip:
                        //     collections.Skills.Add(skillClip);
                        //     break;

                        case AudioClipData audioClip:
                            collections.Audios.Add(audioClip);
                            break;

                        case VFXClipData vfxClip:
                            collections.VFXs.Add(vfxClip);
                            break;

                        default:
                            Aquila.Toolkit.Tools.Logger.Warning($"[AbilityConfigGenerator] Unknown clip type: {clip.GetType().Name}");
                            break;
                    }
                }
            }

            Aquila.Toolkit.Tools.Logger.Info("[AbilityConfigGenerator] Collected clips: " +
                                             $"Effects={collections.Effects.Count}, " +
                                             $"Audios={collections.Audios.Count}, " +
                                             $"VFXs={collections.VFXs.Count}");

            return collections;
        }

        /// <summary>
        /// Collect clips from SerializedTrackData (from AbilityData.Tracks)
        /// </summary>
        private static ClipCollections CollectClipsFromSerializedTracks(IReadOnlyList<SerializedTrackData> tracks)
        {
            var collections = new ClipCollections
            {
                Effects = new List<EffectClipData>(),
                // Skills = new List<SkillClipData>(),
                Audios = new List<AudioClipData>(),
                VFXs = new List<VFXClipData>()
            };

            if (tracks == null || tracks.Count == 0)
            {
                Aquila.Toolkit.Tools.Logger.Warning("[AbilityConfigGenerator] No serialized tracks found");
                return collections;
            }

            foreach (var track in tracks)
            {
                if (track == null)
                {
                    Aquila.Toolkit.Tools.Logger.Warning("[AbilityConfigGenerator] Null track in serialized tracks");
                    continue;
                }

                if (!track.IsEnabled)
                {
                    Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Skipping disabled track: {track.TrackName}");
                    continue;
                }

                var clips = track.Clips;
                if (clips == null || clips.Count == 0)
                    continue;

                foreach (var clip in clips)
                {
                    if (clip == null)
                    {
                        Aquila.Toolkit.Tools.Logger.Warning($"[AbilityConfigGenerator] Null clip in track: {track.TrackName}");
                        continue;
                    }

                    if (!clip.IsEnabled)
                    {
                        Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Skipping disabled clip: {clip.ClipName}");
                        continue;
                    }

                    // Validate clip before adding
                    if (!clip.Validate(out string error))
                    {
                        Aquila.Toolkit.Tools.Logger.Warning($"[AbilityConfigGenerator] Skipping invalid clip in track '{track.TrackName}': {error}");
                        continue;
                    }

                    switch (clip)
                    {
                        case EffectClipData effectClip:
                            collections.Effects.Add(effectClip);
                            break;

                        // case SkillClipData skillClip:
                        //     collections.Skills.Add(skillClip);
                        //     break;

                        case AudioClipData audioClip:
                            collections.Audios.Add(audioClip);
                            break;

                        case VFXClipData vfxClip:
                            collections.VFXs.Add(vfxClip);
                            break;

                        default:
                            Aquila.Toolkit.Tools.Logger.Warning($"[AbilityConfigGenerator] Unknown clip type: {clip.GetType().Name}");
                            break;
                    }
                }
            }

            Aquila.Toolkit.Tools.Logger.Info("[AbilityConfigGenerator] Collected clips from serialized tracks: " +
                                             $"Effects={collections.Effects.Count}, " +
                                             // $"Skills={collections.Skills.Count}, " +
                                             $"Audios={collections.Audios.Count}, " +
                                             $"VFXs={collections.VFXs.Count}");

            return collections;
        }

        /// <summary>
        /// Collect clips from TimelineTrackItem (from editor memory)
        /// </summary>
        private static ClipCollections CollectClipsFromTracks(List<TimelineTrackItem> trackItems)
        {
            var collections = new ClipCollections
            {
                Effects = new List<EffectClipData>(),
                // Skills = new List<SkillClipData>(),
                Audios = new List<AudioClipData>(),
                VFXs = new List<VFXClipData>()
            };

            if (trackItems == null || trackItems.Count == 0)
            {
                Aquila.Toolkit.Tools.Logger.Warning("[AbilityConfigGenerator] No track items found");
                return collections;
            }

            foreach (var track in trackItems)
            {
                if (track == null)
                {
                    Aquila.Toolkit.Tools.Logger.Warning("[AbilityConfigGenerator] Null track in track items");
                    continue;
                }

                if (!track.IsEnabled)
                {
                    Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Skipping disabled track: {track.Name}");
                    continue;
                }

                var clips = track.Clips;
                if (clips == null || clips.Count == 0)
                    continue;

                foreach (var clip in clips)
                {
                    if (clip == null)
                    {
                        Aquila.Toolkit.Tools.Logger.Warning($"[AbilityConfigGenerator] Null clip in track: {track.Name}");
                        continue;
                    }

                    if (!clip.IsEnabled)
                    {
                        Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Skipping disabled clip: {clip.ClipName}");
                        continue;
                    }

                    switch (clip)
                    {
                        case EffectClipData effectClip:
                            collections.Effects.Add(effectClip);
                            break;

                        // case SkillClipData skillClip:
                        //     collections.Skills.Add(skillClip);
                        //     break;

                        case AudioClipData audioClip:
                            collections.Audios.Add(audioClip);
                            break;

                        case VFXClipData vfxClip:
                            collections.VFXs.Add(vfxClip);
                            break;

                        default:
                            Aquila.Toolkit.Tools.Logger.Warning($"[AbilityConfigGenerator] Unknown clip type: {clip.GetType().Name}");
                            break;
                    }
                }
            }

            Aquila.Toolkit.Tools.Logger.Info("[AbilityConfigGenerator] Collected clips from track items: " +
                                             $"Effects={collections.Effects.Count}, " +
                                             // $"Skills={collections.Skills.Count}, " +
                                             $"Audios={collections.Audios.Count}, " +
                                             $"VFXs={collections.VFXs.Count}");

            return collections;
        }

        #endregion


        /// <summary>
        /// Generate triggers from effect clips, grouping by time and merging IDs
        /// </summary>
        private static List<TriggerData> GenerateTriggers(List<EffectClipData> effects)
        {
            if (effects == null || effects.Count == 0)
            {
                Aquila.Toolkit.Tools.Logger.Warning("[AbilityConfigGenerator] No effect clips to generate triggers from");
                return new List<TriggerData>();
            }
            
            var groupedEffects = effects
                .GroupBy(e => RoundTriggerTime(e.TriggerTime))
                .OrderBy(g => g.Key)
                .ToList();

            var triggers = new List<TriggerData>();

            foreach (var group in groupedEffects)
            {
                var effectIDs = group.Select(e => e.EffectId).ToList();
                var sourceTrack = string.Join(", ", group.Select(e => e.ClipName).Distinct());

                var trigger = new TriggerData(
                    group.Key,
                    effectIDs,
                    sourceTrack);

                triggers.Add(trigger);

                Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Created trigger at {group.Key:F2}s with {effectIDs.Count} effects");
            }

            Aquila.Toolkit.Tools.Logger.Info($"[AbilityConfigGenerator] Generated {triggers.Count} triggers total");

            return triggers;
        }

        /// <summary>
        /// 将触发时间四舍五入到指定精度，以避免浮点问题 / Round trigger time to specified precision to avoid floating point issues
        /// </summary>
        private static float RoundTriggerTime(float time)
        {
            return (float)Math.Round(time / Misc.MIN_CLIP_DURATION) * Misc.MIN_CLIP_DURATION;
        }
    }
}
