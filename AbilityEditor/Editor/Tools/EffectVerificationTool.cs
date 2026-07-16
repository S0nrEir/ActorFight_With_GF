using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquila.AbilityEditor;
using Aquila.AbilityEditor.Config;
using Cfg.Enum;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// Effect 二进制导出验证工具
    /// 验证导出的 .efct 文件与原始配置的一致性
    /// </summary>
    public static class EffectVerificationTool
    {
        private const float FLOAT_TOLERANCE = 0.0001f;
        private const string MAGIC = "EFFECT";
        private const byte EXPECTED_VERSION = 0x01;

        /// <summary>
        /// 执行完整的验证流程
        /// </summary>
        public static VerificationResult VerifyAllEffects(string tempExportPath, string logPath)
        {
            var result = new VerificationResult();
            var cachedData = new Dictionary<int, CachedEffectData>();
            
            if (!LoadAndCacheEffects(cachedData, result))
            {
                result.ErrorMessage = "Failed to load effects";
                return result;
            }

            EnsureDirectoryExists(tempExportPath);
            if (!ExportEffectsToTemp(cachedData, tempExportPath, result))
            {
                result.ErrorMessage = "Failed to export effects";
                return result;
            }
            VerifyExportedFiles(cachedData, tempExportPath, result);
            GenerateReport(result, logPath);
            CleanupTempFiles(tempExportPath);
            
            return result;
        }

        private static bool LoadAndCacheEffects(Dictionary<int, CachedEffectData> cache, VerificationResult result)
        {
            string[] assetGuids = AssetDatabase.FindAssets("t:EffectEditorSOData", new[] { Misc.EFFECT_ASSET_BASE_PATH });

            if (assetGuids.Length == 0)
            {
                Aquila.Toolkit.Tools.Logger.Warning($"[EffectVerificationTool] No effects found in {Misc.EFFECT_ASSET_BASE_PATH}");
                return false;
            }

            foreach (string guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var effectData = AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(assetPath);

                if (effectData == null)
                {
                    Aquila.Toolkit.Tools.Logger.Warning($"[EffectVerificationTool] Failed to load: {assetPath}");
                    continue;
                }

                if (effectData.id <= 0)
                {
                    Aquila.Toolkit.Tools.Logger.Warning($"[EffectVerificationTool] Invalid ID for {assetPath}: {effectData.id}");
                    continue;
                }

                var cached = CacheEffectData(effectData);
                cache[cached.Id] = cached;
                result.TotalEffects++;
            }

            Aquila.Toolkit.Tools.Logger.Info($"[EffectVerificationTool] Loaded and cached {cache.Count} effects");
            return cache.Count > 0;
        }

        private static CachedEffectData CacheEffectData(EffectEditorSOData data)
        {
            var cached = new CachedEffectData
            {
                Id = data.id,
                Type = data.Type,
                ModifierType = data.ModifierType,
                EffectOnAwake = data.EffectOnAwake,
                Policy = data.Policy,
                Period = data.Period,
                Duration = data.Duration,
                Target = data.Target,
                EffectType = data.AffectedAttribute
            };

            if (data.ExtensionParam != null)
            {
                cached.Float1 = data.ExtensionParam.float_1;
                cached.Float2 = data.ExtensionParam.float_2;
                cached.Float3 = data.ExtensionParam.float_3;
                cached.Float4 = data.ExtensionParam.float_4;
                cached.Int1 = data.ExtensionParam.int_1;
                cached.Int2 = data.ExtensionParam.int_2;
                cached.Int3 = data.ExtensionParam.int_3;
                cached.Int4 = data.ExtensionParam.int_4;
            }

            cached.DeriveEffects = data.DeriveEffects != null ? (int[])data.DeriveEffects.Clone() : new int[0];
            cached.AwakeEffects = data.AwakeEffects != null ? (int[])data.AwakeEffects.Clone() : new int[0];

            return cached;
        }

        private static bool ExportEffectsToTemp(Dictionary<int, CachedEffectData> cache, string tempPath, VerificationResult result)
        {
            foreach (var kvp in cache)
            {
                int effectId = kvp.Key;
                string assetPath = $"{Misc.EFFECT_ASSET_BASE_PATH}/{effectId}.asset";
                var effectData = AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(assetPath);

                if (effectData == null)
                {
                    Aquila.Toolkit.Tools.Logger.Warning($"[EffectVerificationTool] Failed to reload effect {effectId} for export");
                    continue;
                }

                string outputFile = Path.Combine(tempPath, $"{effectId}.efct");

                try
                {
                    EffectBinaryExporter.ExportEffect(effectData, outputFile);
                }
                catch (Exception ex)
                {
                    Aquila.Toolkit.Tools.Logger.Error($"[EffectVerificationTool] Failed to export effect {effectId}: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        private static void VerifyExportedFiles(Dictionary<int, CachedEffectData> cache, string tempPath, VerificationResult result)
        {
            foreach (var kvp in cache)
            {
                int effectId = kvp.Key;
                var cached = kvp.Value;
                string binaryFile = Path.Combine(tempPath, $"{effectId}.efct");

                if (!File.Exists(binaryFile))
                {
                    var failure = new EffectVerificationFailure
                    {
                        EffectId = effectId,
                        ErrorMessage = "Binary file not found"
                    };
                    result.Failures.Add(failure);
                    continue;
                }

                try
                {
                    var readData = ReadBinaryFile(binaryFile);
                    var differences = CompareEffectData(cached, readData);

                    if (differences.Count > 0)
                    {
                        var failure = new EffectVerificationFailure
                        {
                            EffectId = effectId,
                            Differences = differences
                        };
                        result.Failures.Add(failure);
                    }
                    else
                    {
                        result.SuccessfulEffects.Add(effectId);
                    }
                }
                catch (Exception ex)
                {
                    var failure = new EffectVerificationFailure
                    {
                        EffectId = effectId,
                        ErrorMessage = $"Failed to read/compare: {ex.Message}"
                    };
                    result.Failures.Add(failure);
                }
            }
        }

        internal static CachedEffectData ReadBinaryFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            using (Aquila.Toolkit.Tools.ByteReader reader = new Aquila.Toolkit.Tools.ByteReader(fs))
            {
                // Read Header
                string magic = reader.ReadFixedString(6);
                byte version = reader.ReadByte();

                if (magic != MAGIC)
                    throw new InvalidDataException($"Invalid magic: {magic}");
                if (version != EXPECTED_VERSION)
                    throw new InvalidDataException($"Unsupported version: {version}");

                // Read Basic Info
                var data = new CachedEffectData
                {
                    Id = reader.ReadInt32(),
                    Type = (EffectType)reader.ReadInt32(),
                    ModifierType = (NumricModifierType)reader.ReadUInt16(),
                    EffectOnAwake = reader.ReadBoolean(),
                    Policy = (DurationPolicy)reader.ReadUInt16(),
                    Period = reader.ReadSingle(),
                    Duration = reader.ReadSingle(),
                    Target = reader.ReadInt32(),
                    EffectType = (actor_attribute)reader.ReadInt32(),
                    Float1 = reader.ReadSingle(),
                    Float2 = reader.ReadSingle(),
                    Float3 = reader.ReadSingle(),
                    Float4 = reader.ReadSingle(),
                    Int1 = reader.ReadInt32(),
                    Int2 = reader.ReadInt32(),
                    Int3 = reader.ReadInt32(),
                    Int4 = reader.ReadInt32()
                };

                // Read Derive Effects
                int deriveCount = reader.ReadInt32();
                data.DeriveEffects = new int[deriveCount];
                for (int i = 0; i < deriveCount; i++)
                {
                    data.DeriveEffects[i] = reader.ReadInt32();
                }

                // Read Awake Effects
                int awakeCount = reader.ReadInt32();
                data.AwakeEffects = new int[awakeCount];
                for (int i = 0; i < awakeCount; i++)
                {
                    data.AwakeEffects[i] = reader.ReadInt32();
                }

                return data;
            }
        }

        private static List<string> CompareEffectData(CachedEffectData expected, CachedEffectData actual)
        {
            var differences = new List<string>();

            if (expected.Id != actual.Id)
                differences.Add($"Field: Id | Expected: {expected.Id} | Actual: {actual.Id}");

            if (expected.Type != actual.Type)
                differences.Add($"Field: Type | Expected: {expected.Type} | Actual: {actual.Type}");

            if (expected.ModifierType != actual.ModifierType)
                differences.Add($"Field: ModifierType | Expected: {expected.ModifierType} | Actual: {actual.ModifierType}");

            if (expected.EffectOnAwake != actual.EffectOnAwake)
                differences.Add($"Field: EffectOnAwake | Expected: {expected.EffectOnAwake} | Actual: {actual.EffectOnAwake}");

            if (expected.Policy != actual.Policy)
                differences.Add($"Field: Policy | Expected: {expected.Policy} | Actual: {actual.Policy}");

            if (!FloatEquals(expected.Period, actual.Period))
                differences.Add($"Field: Period | Expected: {expected.Period} | Actual: {actual.Period}");

            if (!FloatEquals(expected.Duration, actual.Duration))
                differences.Add($"Field: Duration | Expected: {expected.Duration} | Actual: {actual.Duration}");

            if (expected.Target != actual.Target)
                differences.Add($"Field: Target | Expected: {expected.Target} | Actual: {actual.Target}");

            if (expected.EffectType != actual.EffectType)
                differences.Add($"Field: EffectType | Expected: {expected.EffectType} | Actual: {actual.EffectType}");

            // Extension params
            if (!FloatEquals(expected.Float1, actual.Float1))
                differences.Add($"Field: Float1 | Expected: {expected.Float1} | Actual: {actual.Float1}");

            if (!FloatEquals(expected.Float2, actual.Float2))
                differences.Add($"Field: Float2 | Expected: {expected.Float2} | Actual: {actual.Float2}");

            if (!FloatEquals(expected.Float3, actual.Float3))
                differences.Add($"Field: Float3 | Expected: {expected.Float3} | Actual: {actual.Float3}");

            if (!FloatEquals(expected.Float4, actual.Float4))
                differences.Add($"Field: Float4 | Expected: {expected.Float4} | Actual: {actual.Float4}");

            if (expected.Int1 != actual.Int1)
                differences.Add($"Field: Int1 | Expected: {expected.Int1} | Actual: {actual.Int1}");

            if (expected.Int2 != actual.Int2)
                differences.Add($"Field: Int2 | Expected: {expected.Int2} | Actual: {actual.Int2}");

            if (expected.Int3 != actual.Int3)
                differences.Add($"Field: Int3 | Expected: {expected.Int3} | Actual: {actual.Int3}");

            if (expected.Int4 != actual.Int4)
                differences.Add($"Field: Int4 | Expected: {expected.Int4} | Actual: {actual.Int4}");

            // Arrays
            if (!ArrayEquals(expected.DeriveEffects, actual.DeriveEffects))
                differences.Add($"Field: DeriveEffects | Expected: [{string.Join(", ", expected.DeriveEffects)}] | Actual: [{string.Join(", ", actual.DeriveEffects)}]");

            if (!ArrayEquals(expected.AwakeEffects, actual.AwakeEffects))
                differences.Add($"Field: AwakeEffects | Expected: [{string.Join(", ", expected.AwakeEffects)}] | Actual: [{string.Join(", ", actual.AwakeEffects)}]");

            return differences;
        }

        private static bool FloatEquals(float a, float b)
        {
            return Mathf.Abs(a - b) < FLOAT_TOLERANCE;
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
            sb.AppendLine("========== Effect Export Verification Report ==========");
            sb.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Total Effects: {result.TotalEffects}");
            sb.AppendLine($"Verified: {result.SuccessfulEffects.Count}");
            sb.AppendLine($"Failed: {result.Failures.Count}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                sb.AppendLine($"[ERROR] {result.ErrorMessage}");
                sb.AppendLine();
            }

            foreach (var failure in result.Failures)
            {
                sb.AppendLine($"[FAILED] Effect ID: {failure.EffectId}");

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

            if (result.SuccessfulEffects.Count > 0)
            {
                sb.AppendLine($"[SUCCESS] Effect IDs: {string.Join(", ", result.SuccessfulEffects)}");
            }

            sb.AppendLine("========== End of Report ==========");

            string reportContent = sb.ToString();

            // Write to log file
            EnsureDirectoryExists(Path.GetDirectoryName(logPath));
            File.WriteAllText(logPath, reportContent);

            // Also log to console
            Aquila.Toolkit.Tools.Logger.Info($"[EffectVerificationTool] Report:\n{reportContent}");
        }

        private static void CleanupTempFiles(string tempPath)
        {
            try
            {
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                    Aquila.Toolkit.Tools.Logger.Info($"[EffectVerificationTool] Cleaned up temp directory: {tempPath}");
                }
            }
            catch (Exception ex)
            {
                Aquila.Toolkit.Tools.Logger.Warning($"[EffectVerificationTool] Failed to cleanup temp files: {ex.Message}");
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
            public int TotalEffects;
            public List<int> SuccessfulEffects = new List<int>();
            public List<EffectVerificationFailure> Failures = new List<EffectVerificationFailure>();
            public string ErrorMessage;

            public bool IsSuccess => Failures.Count == 0 && string.IsNullOrEmpty(ErrorMessage);
        }

        public class EffectVerificationFailure
        {
            public int EffectId;
            public string ErrorMessage;
            public List<string> Differences = new List<string>();
        }

        internal class CachedEffectData
        {
            public int Id;
            public EffectType Type;
            public NumricModifierType ModifierType;
            public bool EffectOnAwake;
            public DurationPolicy Policy;
            public float Period;
            public float Duration;
            public int Target;
            public actor_attribute EffectType;
            public float Float1;
            public float Float2;
            public float Float3;
            public float Float4;
            public int Int1;
            public int Int2;
            public int Int3;
            public int Int4;
            public int[] DeriveEffects;
            public int[] AwakeEffects;
        }

        #endregion
    }
}
