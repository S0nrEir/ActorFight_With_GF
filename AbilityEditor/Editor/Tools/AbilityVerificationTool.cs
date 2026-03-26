using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquila.AbilityEditor;
using Aquila.Toolkit;
using Cfg.Enum;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// Ability 二进制导出验证工具
    /// 验证导出的 .ablt 文件与原始配置的一致性
    /// </summary>
    public static class AbilityVerificationTool
    {
        private const float FLOAT_TOLERANCE = 0.0001f;
        private const string MAGIC = "ABLT";
        private const byte EXPECTED_VERSION = 0x01;

        /// <summary>
        /// 执行完整的验证流程
        /// </summary>
        public static VerificationResult VerifyAllAbilities(string tempExportPath, string logPath)
        {
            var result = new VerificationResult();
            var cachedData = new Dictionary<int, CachedAbilityData>();
            
            if (!LoadAndCacheAbilities(cachedData, result))
            {
                result.ErrorMessage = "Failed to load abilities";
                return result;
            }
            EnsureDirectoryExists(tempExportPath);
            if (!ExportAbilitiesToTemp(cachedData, tempExportPath, result))
            {
                result.ErrorMessage = "Failed to export abilities";
                return result;
            }
            VerifyExportedFiles(cachedData, tempExportPath, result);
            GenerateReport(result, logPath);
            CleanupTempFiles(tempExportPath);
            
            return result;
        }

        private static bool LoadAndCacheAbilities(Dictionary<int, CachedAbilityData> cache, VerificationResult result)
        {
            string[] assetGuids = AssetDatabase.FindAssets("t:AbilityEditorSOData", new[] { Misc.ABILITY_ASSET_BASE_PATH });
            
            if (assetGuids.Length == 0)
            {
                Debug.LogWarning($"[AbilityVerificationTool] No abilities found in {Misc.ABILITY_ASSET_BASE_PATH}");
                return false;
            }

            foreach (string guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var abilityData = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(assetPath);
                
                if (abilityData == null)
                {
                    Debug.LogWarning($"[AbilityVerificationTool] Failed to load: {assetPath}");
                    continue;
                }

                if (!abilityData.Validate(out string error))
                {
                    Debug.LogWarning($"[AbilityVerificationTool] Validation failed for {assetPath}: {error}");
                    continue;
                }

                var cached = CacheAbilityData(abilityData);
                cache[cached.Id] = cached;
                result.TotalAbilities++;
            }

            Debug.Log($"[AbilityVerificationTool] Loaded and cached {cache.Count} abilities");
            return cache.Count > 0;
        }

        private static CachedAbilityData CacheAbilityData(AbilityEditorSOData data)
        {
            var cached = new CachedAbilityData
            {
                Id = data.Id,
                CostEffectID = data.CostEffectID,
                CoolDownEffectID = data.CoolDownEffectID,
                TargetType = data.TargetType,
                TimelineID = data.TimelineID,
                TimelineDuration = data.TimelineDuration,
                Tracks = new List<CachedTrackData>()
            };

            if (data.Tracks != null)
            {
                foreach (var track in data.Tracks)
                {
                    var cachedTrack = new CachedTrackData { Clips = new List<CachedClipData>() };
                    
                    if (track.Clips != null)
                    {
                        foreach (var clip in track.Clips)
                        {
                            CachedClipData cachedClip = null;

                            if (clip is EffectClipData effectClip)
                            {
                                cachedClip = new CachedEffectClipData
                                {
                                    ClipType = TimelineClipType.Buff,
                                    StartTime = effectClip.StartTime,
                                    EndTime = effectClip.EndTime,
                                    EffectId = effectClip.EffectId,
                                    StackCount = effectClip.StackCount,
                                    CanStack = effectClip.CanStack,
                                    EffectType = effectClip.EffectType,
                                    ModifierType = effectClip.ModifierType,
                                    AffectedAttribute = effectClip.AffectedAttribute,
                                    Target = effectClip.Target,
                                    Duration = effectClip.Duration,
                                    Period = effectClip.Period,
                                    Policy = effectClip.Policy,
                                    EffectOnAwake = effectClip.EffectOnAwake,
                                    FloatParam1 = effectClip.ExtensionParam.FloatParam_1,
                                    FloatParam2 = effectClip.ExtensionParam.FloatParam_2,
                                    FloatParam3 = effectClip.ExtensionParam.FloatParam_3,
                                    FloatParam4 = effectClip.ExtensionParam.FloatParam_4,
                                    IntParam1 = effectClip.ExtensionParam.IntParam_1,
                                    IntParam2 = effectClip.ExtensionParam.IntParam_2,
                                    IntParam3 = effectClip.ExtensionParam.IntParam_3,
                                    IntParam4 = effectClip.ExtensionParam.IntParam_4,
                                    DeriveEffects = effectClip.DeriveEffects != null ? (int[])effectClip.DeriveEffects.Clone() : new int[0],
                                    AwakeEffects = effectClip.AwakeEffects != null ? (int[])effectClip.AwakeEffects.Clone() : new int[0]
                                };
                            }
                            else if (clip is AudioClipData audioClip)
                            {
                                cachedClip = new CachedAudioClipData
                                {
                                    ClipType = TimelineClipType.Audio,
                                    StartTime = audioClip.StartTime,
                                    EndTime = audioClip.EndTime,
                                    AudioPath = audioClip.AudioPath,
                                    Volume = audioClip.Volume,
                                    Loop = audioClip.Loop,
                                    FadeInDuration = audioClip.FadeInDuration,
                                    FadeOutDuration = audioClip.FadeOutDuration
                                };
                            }
                            else if (clip is VFXClipData vfxClip)
                            {
                                cachedClip = new CachedVFXClipData
                                {
                                    ClipType = TimelineClipType.VFX,
                                    StartTime = vfxClip.StartTime,
                                    EndTime = vfxClip.EndTime,
                                    VfxPath = vfxClip.VfxPath,
                                    AttachPoint = vfxClip.AttachPoint,
                                    PositionOffset = vfxClip.PositionOffset,
                                    RotationOffset = vfxClip.RotationOffset,
                                    Scale = vfxClip.Scale,
                                    FollowAttachPoint = vfxClip.FollowAttachPoint
                                };
                            }

                            if (cachedClip != null)
                            {
                                cachedTrack.Clips.Add(cachedClip);
                            }
                        }
                    }

                    cached.Tracks.Add(cachedTrack);
                }
            }

            return cached;
        }

        private static bool ExportAbilitiesToTemp(Dictionary<int, CachedAbilityData> cache, string tempPath, VerificationResult result)
        {
            foreach (var kvp in cache)
            {
                int abilityId = kvp.Key;
                string assetPath = $"{Misc.ABILITY_ASSET_BASE_PATH}/{abilityId}.asset";
                var abilityData = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(assetPath);
                
                if (abilityData == null)
                {
                    Debug.LogWarning($"[AbilityVerificationTool] Failed to reload ability {abilityId} for export");
                    continue;
                }

                string outputFile = Path.Combine(tempPath, $"{abilityId}.ablt");
                AbilityBinaryExporter.ExportAbility(abilityData, outputFile);
            }

            return true;
        }

        private static void VerifyExportedFiles(Dictionary<int, CachedAbilityData> cache, string tempPath, VerificationResult result)
        {
            foreach (var kvp in cache)
            {
                int abilityId = kvp.Key;
                var cached = kvp.Value;
                string binaryFile = Path.Combine(tempPath, $"{abilityId}.ablt");

                if (!File.Exists(binaryFile))
                {
                    var failure = new AbilityVerificationFailure
                    {
                        AbilityId = abilityId,
                        ErrorMessage = "Binary file not found"
                    };
                    result.Failures.Add(failure);
                    continue;
                }
                
                var readData = ReadBinaryFile(binaryFile);
                var differences = CompareAbilityData(cached, readData);

                if (differences.Count > 0)
                {
                    var failure = new AbilityVerificationFailure
                    {
                        AbilityId = abilityId,
                        Differences = differences
                    };
                    result.Failures.Add(failure);
                }
                else
                {
                    result.SuccessfulAbilities.Add(abilityId);
                }
            }
        }

        internal static CachedAbilityData ReadBinaryFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            using (Aquila.Toolkit.Tools.ByteReader reader = new Aquila.Toolkit.Tools.ByteReader(fs))
            {
                // Read Header
                string magic = reader.ReadFixedString(4);
                byte version = reader.ReadByte();

                if (magic != MAGIC)
                    throw new InvalidDataException($"Invalid magic: {magic}");
                if (version != EXPECTED_VERSION)
                    throw new InvalidDataException($"Unsupported version: {version}");

                // Read Basic Info
                var data = new CachedAbilityData
                {
                    Id = reader.ReadInt32(),
                    CostEffectID = reader.ReadInt32(),
                    CoolDownEffectID = reader.ReadInt32(),
                    TargetType = (AbilityTargetType)reader.ReadInt32(),
                    TimelineID = reader.ReadInt32(),
                    TimelineDuration = reader.ReadSingle(),
                    Tracks = new List<CachedTrackData>()
                };

                // Read Tracks
                int trackCount = reader.ReadInt32();
                for (int t = 0; t < trackCount; t++)
                    data.Tracks.Add(ReadTrack(reader));

                return data;
            }
        }

        private static CachedTrackData ReadTrack(Aquila.Toolkit.Tools.ByteReader reader)
        {
            var track = new CachedTrackData { Clips = new List<CachedClipData>() };
            int clipCount = reader.ReadInt32();

            for (int c = 0; c < clipCount; c++)
                track.Clips.Add(ReadClip(reader));

            return track;
        }

        private static CachedClipData ReadClip(Aquila.Toolkit.Tools.ByteReader reader)
        {
            int clipType = reader.ReadInt32();
            float startTime = reader.ReadSingle();
            float endTime = reader.ReadSingle();

            CachedClipData clip = null;

            switch (clipType)
            {
                case 1: // Effect/Buff
                    clip = new CachedEffectClipData
                    {
                        ClipType = TimelineClipType.Buff,
                        StartTime = startTime,
                        EndTime = endTime,
                        EffectId = reader.ReadInt32(),
                        StackCount = reader.ReadInt32(),
                        CanStack = reader.ReadBoolean(),
                        EffectType = (Cfg.Enum.EffectType)reader.ReadInt32(),
                        ModifierType = (Cfg.Enum.NumricModifierType)reader.ReadUInt16(),
                        AffectedAttribute = (Cfg.Enum.actor_attribute)reader.ReadInt32(),
                        Target = reader.ReadInt32(),
                        Duration = reader.ReadSingle(),
                        Period = reader.ReadSingle(),
                        Policy = (Cfg.Enum.DurationPolicy)reader.ReadUInt16(),
                        EffectOnAwake = reader.ReadBoolean(),
                        FloatParam1 = reader.ReadSingle(),
                        FloatParam2 = reader.ReadSingle(),
                        FloatParam3 = reader.ReadSingle(),
                        FloatParam4 = reader.ReadSingle(),
                        IntParam1 = reader.ReadInt32(),
                        IntParam2 = reader.ReadInt32(),
                        IntParam3 = reader.ReadInt32(),
                        IntParam4 = reader.ReadInt32()
                    };
                    
                    // Read DeriveEffects array
                    int deriveCount = reader.ReadInt32();
                    ((CachedEffectClipData)clip).DeriveEffects = new int[deriveCount];
                    for (int i = 0; i < deriveCount; i++)
                    {
                        ((CachedEffectClipData)clip).DeriveEffects[i] = reader.ReadInt32();
                    }
                    
                    // Read AwakeEffects array
                    int awakeCount = reader.ReadInt32();
                    ((CachedEffectClipData)clip).AwakeEffects = new int[awakeCount];
                    for (int i = 0; i < awakeCount; i++)
                    {
                        ((CachedEffectClipData)clip).AwakeEffects[i] = reader.ReadInt32();
                    }
                    break;

                case 2: // Audio
                    clip = new CachedAudioClipData
                    {
                        ClipType = TimelineClipType.Audio,
                        StartTime = startTime,
                        EndTime = endTime,
                        AudioPath = reader.ReadString(),
                        Volume = reader.ReadSingle(),
                        Loop = reader.ReadBoolean(),
                        FadeInDuration = reader.ReadSingle(),
                        FadeOutDuration = reader.ReadSingle()
                    };
                    break;

                case 3: // VFX
                    clip = new CachedVFXClipData
                    {
                        ClipType = TimelineClipType.VFX,
                        StartTime = startTime,
                        EndTime = endTime,
                        VfxPath = reader.ReadString(),
                        AttachPoint = reader.ReadString(),
                        PositionOffset = reader.ReadVector3(),
                        RotationOffset = reader.ReadVector3(),
                        Scale = reader.ReadVector3(),
                        FollowAttachPoint = reader.ReadBoolean()
                    };
                    break;

                default:
                    throw new InvalidDataException($"Unknown clip type: {clipType}");
            }

            return clip;
        }

        private static List<string> CompareAbilityData(CachedAbilityData expected, CachedAbilityData actual)
        {
            var differences = new List<string>();

            // Compare basic fields
            if (expected.Id != actual.Id)
                differences.Add($"Field: Id | Expected: {expected.Id} | Actual: {actual.Id}");
            
            if (expected.CostEffectID != actual.CostEffectID)
                differences.Add($"Field: CostEffectID | Expected: {expected.CostEffectID} | Actual: {actual.CostEffectID}");
            
            if (expected.CoolDownEffectID != actual.CoolDownEffectID)
                differences.Add($"Field: CoolDownEffectID | Expected: {expected.CoolDownEffectID} | Actual: {actual.CoolDownEffectID}");
            
            if (expected.TargetType != actual.TargetType)
                differences.Add($"Field: TargetType | Expected: {expected.TargetType} | Actual: {actual.TargetType}");
            
            if (expected.TimelineID != actual.TimelineID)
                differences.Add($"Field: TimelineID | Expected: {expected.TimelineID} | Actual: {actual.TimelineID}");
            
            if (!FloatEquals(expected.TimelineDuration, actual.TimelineDuration))
                differences.Add($"Field: TimelineDuration | Expected: {expected.TimelineDuration} | Actual: {actual.TimelineDuration}");

            // Compare tracks
            if (expected.Tracks.Count != actual.Tracks.Count)
            {
                differences.Add($"Field: Track Count | Expected: {expected.Tracks.Count} | Actual: {actual.Tracks.Count}");
                return differences; // Can't compare further if track counts differ
            }

            for (int t = 0; t < expected.Tracks.Count; t++)
            {
                var expectedTrack = expected.Tracks[t];
                var actualTrack = actual.Tracks[t];

                if (expectedTrack.Clips.Count != actualTrack.Clips.Count)
                {
                    differences.Add($"Track[{t}] Clip Count | Expected: {expectedTrack.Clips.Count} | Actual: {actualTrack.Clips.Count}");
                    continue;
                }

                for (int c = 0; c < expectedTrack.Clips.Count; c++)
                {
                    var clipDiffs = CompareClipData(expectedTrack.Clips[c], actualTrack.Clips[c], t, c);
                    differences.AddRange(clipDiffs);
                }
            }

            return differences;
        }

        private static List<string> CompareClipData(CachedClipData expected, CachedClipData actual, int trackIndex, int clipIndex)
        {
            var differences = new List<string>();
            string prefix = $"Track[{trackIndex}] Clip[{clipIndex}]";

            if (expected.ClipType != actual.ClipType)
            {
                differences.Add($"{prefix} ClipType | Expected: {expected.ClipType} | Actual: {actual.ClipType}");
                return differences; // Can't compare further if types differ
            }

            if (!FloatEquals(expected.StartTime, actual.StartTime))
                differences.Add($"{prefix} StartTime | Expected: {expected.StartTime} | Actual: {actual.StartTime}");
            
            if (!FloatEquals(expected.EndTime, actual.EndTime))
                differences.Add($"{prefix} EndTime | Expected: {expected.EndTime} | Actual: {actual.EndTime}");

            // Type-specific comparisons
            if (expected is CachedEffectClipData expectedEffect && actual is CachedEffectClipData actualEffect)
            {
                if (expectedEffect.EffectId != actualEffect.EffectId)
                    differences.Add($"{prefix} (EffectClip) EffectId | Expected: {expectedEffect.EffectId} | Actual: {actualEffect.EffectId}");
                
                if (expectedEffect.StackCount != actualEffect.StackCount)
                    differences.Add($"{prefix} (EffectClip) StackCount | Expected: {expectedEffect.StackCount} | Actual: {actualEffect.StackCount}");
                
                if (expectedEffect.CanStack != actualEffect.CanStack)
                    differences.Add($"{prefix} (EffectClip) CanStack | Expected: {expectedEffect.CanStack} | Actual: {actualEffect.CanStack}");
                
                if (expectedEffect.EffectType != actualEffect.EffectType)
                    differences.Add($"{prefix} (EffectClip) EffectType | Expected: {expectedEffect.EffectType} | Actual: {actualEffect.EffectType}");
                
                if (expectedEffect.ModifierType != actualEffect.ModifierType)
                    differences.Add($"{prefix} (EffectClip) ModifierType | Expected: {expectedEffect.ModifierType} | Actual: {actualEffect.ModifierType}");
                
                if (expectedEffect.AffectedAttribute != actualEffect.AffectedAttribute)
                    differences.Add($"{prefix} (EffectClip) AffectedAttribute | Expected: {expectedEffect.AffectedAttribute} | Actual: {actualEffect.AffectedAttribute}");
                
                if (expectedEffect.Target != actualEffect.Target)
                    differences.Add($"{prefix} (EffectClip) Target | Expected: {expectedEffect.Target} | Actual: {actualEffect.Target}");
                
                if (!FloatEquals(expectedEffect.Duration, actualEffect.Duration))
                    differences.Add($"{prefix} (EffectClip) Duration | Expected: {expectedEffect.Duration} | Actual: {actualEffect.Duration}");
                
                if (!FloatEquals(expectedEffect.Period, actualEffect.Period))
                    differences.Add($"{prefix} (EffectClip) Period | Expected: {expectedEffect.Period} | Actual: {actualEffect.Period}");
                
                if (expectedEffect.Policy != actualEffect.Policy)
                    differences.Add($"{prefix} (EffectClip) Policy | Expected: {expectedEffect.Policy} | Actual: {actualEffect.Policy}");
                
                if (expectedEffect.EffectOnAwake != actualEffect.EffectOnAwake)
                    differences.Add($"{prefix} (EffectClip) EffectOnAwake | Expected: {expectedEffect.EffectOnAwake} | Actual: {actualEffect.EffectOnAwake}");
                
                // Extension params
                if (!FloatEquals(expectedEffect.FloatParam1, actualEffect.FloatParam1))
                    differences.Add($"{prefix} (EffectClip) FloatParam1 | Expected: {expectedEffect.FloatParam1} | Actual: {actualEffect.FloatParam1}");
                
                if (!FloatEquals(expectedEffect.FloatParam2, actualEffect.FloatParam2))
                    differences.Add($"{prefix} (EffectClip) FloatParam2 | Expected: {expectedEffect.FloatParam2} | Actual: {actualEffect.FloatParam2}");
                
                if (!FloatEquals(expectedEffect.FloatParam3, actualEffect.FloatParam3))
                    differences.Add($"{prefix} (EffectClip) FloatParam3 | Expected: {expectedEffect.FloatParam3} | Actual: {actualEffect.FloatParam3}");
                
                if (!FloatEquals(expectedEffect.FloatParam4, actualEffect.FloatParam4))
                    differences.Add($"{prefix} (EffectClip) FloatParam4 | Expected: {expectedEffect.FloatParam4} | Actual: {actualEffect.FloatParam4}");
                
                if (expectedEffect.IntParam1 != actualEffect.IntParam1)
                    differences.Add($"{prefix} (EffectClip) IntParam1 | Expected: {expectedEffect.IntParam1} | Actual: {actualEffect.IntParam1}");
                
                if (expectedEffect.IntParam2 != actualEffect.IntParam2)
                    differences.Add($"{prefix} (EffectClip) IntParam2 | Expected: {expectedEffect.IntParam2} | Actual: {actualEffect.IntParam2}");
                
                if (expectedEffect.IntParam3 != actualEffect.IntParam3)
                    differences.Add($"{prefix} (EffectClip) IntParam3 | Expected: {expectedEffect.IntParam3} | Actual: {actualEffect.IntParam3}");
                
                if (expectedEffect.IntParam4 != actualEffect.IntParam4)
                    differences.Add($"{prefix} (EffectClip) IntParam4 | Expected: {expectedEffect.IntParam4} | Actual: {actualEffect.IntParam4}");
                
                // Arrays
                if (!ArrayEquals(expectedEffect.DeriveEffects, actualEffect.DeriveEffects))
                    differences.Add($"{prefix} (EffectClip) DeriveEffects | Expected: [{string.Join(", ", expectedEffect.DeriveEffects)}] | Actual: [{string.Join(", ", actualEffect.DeriveEffects)}]");
                
                if (!ArrayEquals(expectedEffect.AwakeEffects, actualEffect.AwakeEffects))
                    differences.Add($"{prefix} (EffectClip) AwakeEffects | Expected: [{string.Join(", ", expectedEffect.AwakeEffects)}] | Actual: [{string.Join(", ", actualEffect.AwakeEffects)}]");
            }
            else if (expected is CachedAudioClipData expectedAudio && actual is CachedAudioClipData actualAudio)
            {
                if (expectedAudio.AudioPath != actualAudio.AudioPath)
                    differences.Add($"{prefix} (AudioClip) AudioPath | Expected: {expectedAudio.AudioPath} | Actual: {actualAudio.AudioPath}");
                
                if (!FloatEquals(expectedAudio.Volume, actualAudio.Volume))
                    differences.Add($"{prefix} (AudioClip) Volume | Expected: {expectedAudio.Volume} | Actual: {actualAudio.Volume}");
                
                if (expectedAudio.Loop != actualAudio.Loop)
                    differences.Add($"{prefix} (AudioClip) Loop | Expected: {expectedAudio.Loop} | Actual: {actualAudio.Loop}");
                
                if (!FloatEquals(expectedAudio.FadeInDuration, actualAudio.FadeInDuration))
                    differences.Add($"{prefix} (AudioClip) FadeInDuration | Expected: {expectedAudio.FadeInDuration} | Actual: {actualAudio.FadeInDuration}");
                
                if (!FloatEquals(expectedAudio.FadeOutDuration, actualAudio.FadeOutDuration))
                    differences.Add($"{prefix} (AudioClip) FadeOutDuration | Expected: {expectedAudio.FadeOutDuration} | Actual: {actualAudio.FadeOutDuration}");
            }
            else if (expected is CachedVFXClipData expectedVFX && actual is CachedVFXClipData actualVFX)
            {
                if (expectedVFX.VfxPath != actualVFX.VfxPath)
                    differences.Add($"{prefix} (VFXClip) VfxPath | Expected: {expectedVFX.VfxPath} | Actual: {actualVFX.VfxPath}");
                
                if (expectedVFX.AttachPoint != actualVFX.AttachPoint)
                    differences.Add($"{prefix} (VFXClip) AttachPoint | Expected: {expectedVFX.AttachPoint} | Actual: {actualVFX.AttachPoint}");
                
                if (!Vector3Equals(expectedVFX.PositionOffset, actualVFX.PositionOffset))
                    differences.Add($"{prefix} (VFXClip) PositionOffset | Expected: {expectedVFX.PositionOffset} | Actual: {actualVFX.PositionOffset}");
                
                if (!Vector3Equals(expectedVFX.RotationOffset, actualVFX.RotationOffset))
                    differences.Add($"{prefix} (VFXClip) RotationOffset | Expected: {expectedVFX.RotationOffset} | Actual: {actualVFX.RotationOffset}");
                
                if (!Vector3Equals(expectedVFX.Scale, actualVFX.Scale))
                    differences.Add($"{prefix} (VFXClip) Scale | Expected: {expectedVFX.Scale} | Actual: {actualVFX.Scale}");
                
                if (expectedVFX.FollowAttachPoint != actualVFX.FollowAttachPoint)
                    differences.Add($"{prefix} (VFXClip) FollowAttachPoint | Expected: {expectedVFX.FollowAttachPoint} | Actual: {actualVFX.FollowAttachPoint}");
            }

            return differences;
        }

        private static bool FloatEquals(float a, float b)
        {
            return Mathf.Abs(a - b) < FLOAT_TOLERANCE;
        }

        private static bool Vector3Equals(Vector3 a, Vector3 b)
        {
            return FloatEquals(a.x, b.x) && FloatEquals(a.y, b.y) && FloatEquals(a.z, b.z);
        }

        private static bool ArrayEquals(int[] a, int[] b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            
            return true;
        }

        private static void GenerateReport(VerificationResult result, string logPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("========== Ability Export Verification Report ==========");
            sb.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Total Abilities: {result.TotalAbilities}");
            sb.AppendLine($"Verified: {result.SuccessfulAbilities.Count}");
            sb.AppendLine($"Failed: {result.Failures.Count}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                sb.AppendLine($"[ERROR] {result.ErrorMessage}");
                sb.AppendLine();
            }

            foreach (var failure in result.Failures)
            {
                sb.AppendLine($"[FAILED] Ability ID: {failure.AbilityId}");
                
                if (!string.IsNullOrEmpty(failure.ErrorMessage))
                {
                    sb.AppendLine($"  Error: {failure.ErrorMessage}");
                }
                
                foreach (var diff in failure.Differences)
                {
                    sb.AppendLine($"  {diff}");
                }
                
                sb.AppendLine();
            }

            if (result.SuccessfulAbilities.Count > 0)
            {
                sb.AppendLine($"[SUCCESS] Ability IDs: {string.Join(", ", result.SuccessfulAbilities)}");
            }

            sb.AppendLine("========== End of Report ==========");

            string reportContent = sb.ToString();
            
            // Write to log file
            EnsureDirectoryExists(Path.GetDirectoryName(logPath));
            File.WriteAllText(logPath, reportContent);
            
            // Also log to console
            Debug.Log($"[AbilityVerificationTool] Report:\n{reportContent}");
        }

        private static void CleanupTempFiles(string tempPath)
        {
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
                Debug.Log($"[AbilityVerificationTool] Cleaned up temp directory: {tempPath}");
            }
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #region Data Structures

        public class VerificationResult
        {
            public int TotalAbilities;
            public List<int> SuccessfulAbilities = new List<int>();
            public List<AbilityVerificationFailure> Failures = new List<AbilityVerificationFailure>();
            public string ErrorMessage;

            public bool IsSuccess => Failures.Count == 0 && string.IsNullOrEmpty(ErrorMessage);
        }

        public class AbilityVerificationFailure
        {
            public int AbilityId;
            public string ErrorMessage;
            public List<string> Differences = new List<string>();
        }

        internal class CachedAbilityData
        {
            public int Id;
            public int CostEffectID;
            public int CoolDownEffectID;
            public AbilityTargetType TargetType;
            public int TimelineID;
            public float TimelineDuration;
            public List<CachedTrackData> Tracks;
        }

        internal class CachedTrackData
        {
            public List<CachedClipData> Clips;
        }

        internal abstract class CachedClipData
        {
            public TimelineClipType ClipType;
            public float StartTime;
            public float EndTime;
        }

        internal class CachedEffectClipData : CachedClipData
        {
            public int EffectId;
            public int StackCount;
            public bool CanStack;
            public Cfg.Enum.EffectType EffectType;
            public Cfg.Enum.NumricModifierType ModifierType;
            public Cfg.Enum.actor_attribute AffectedAttribute;
            public int Target;
            public float Duration;
            public float Period;
            public Cfg.Enum.DurationPolicy Policy;
            public bool EffectOnAwake;
            public float FloatParam1;
            public float FloatParam2;
            public float FloatParam3;
            public float FloatParam4;
            public int IntParam1;
            public int IntParam2;
            public int IntParam3;
            public int IntParam4;
            public int[] DeriveEffects;
            public int[] AwakeEffects;
        }

        internal class CachedAudioClipData : CachedClipData
        {
            public string AudioPath;
            public float Volume;
            public bool Loop;
            public float FadeInDuration;
            public float FadeOutDuration;
        }

        internal class CachedVFXClipData : CachedClipData
        {
            public string VfxPath;
            public string AttachPoint;
            public Vector3 PositionOffset;
            public Vector3 RotationOffset;
            public Vector3 Scale;
            public bool FollowAttachPoint;
        }

        #endregion
    }
}
