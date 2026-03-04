using System.IO;
using System.Text;
using Aquila.AbilityEditor;
using Aquila.AbilityEditor.Config;
using Aquila.Toolkit;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// Effect 二进制导出工具
    /// 将 EffectEditorSOData 资产导出为 .efct 二进制文件
    /// </summary>
    public static class EffectBinaryExporter
    {
        [MenuItem("Aquila/AbilityEditor/.efft Export|Import/Export all effect data(.efct)")]
        public static void ExportAllEffects()
        {
            if (!Directory.Exists(Misc.EFFECT_ASSET_BASE_PATH))
            {
                Debug.LogError($"[EffectBinaryExporter] Source path not found: {Misc.EFFECT_ASSET_BASE_PATH}");
                return;
            }

            EnsureDirectoryExists(Misc.EFFECT_BIN_ASSET_PATH);
            string[] assetGuids = AssetDatabase.FindAssets("t:EffectEditorSOData", new[] { Misc.EFFECT_ASSET_BASE_PATH });
            int successCount = 0;
            int failCount = 0;

            foreach (string guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var effectData = AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(assetPath);
                if (effectData == null)
                {
                    Debug.LogWarning($"[EffectBinaryExporter] Failed to load: {assetPath}");
                    failCount++;
                    continue;
                }

                // 基础验证
                if (effectData.id <= 0)
                {
                    Debug.LogWarning($"[EffectBinaryExporter] Invalid ID for {assetPath}: {effectData.id}");
                    failCount++;
                    continue;
                }

                string outputFile = Path.Combine(Misc.EFFECT_BIN_ASSET_PATH, $"{effectData.id}.efct");
                
                try
                {
                    ExportEffect(effectData, outputFile);
                    successCount++;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[EffectBinaryExporter] Failed to export {assetPath}: {ex.Message}");
                    failCount++;
                }
            }

            AssetDatabase.Refresh();
            Debug.Log($"[EffectBinaryExporter] Export complete. Success: {successCount}, Failed: {failCount}");
        }

        /// <summary>
        /// 导出单个 EffectEditorSOData 为 .efct 文件
        /// </summary>
        public static void ExportEffect(EffectEditorSOData data, string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                using (Aquila.Toolkit.Tools.ByteWriter writer = new Aquila.Toolkit.Tools.ByteWriter(fs))
                {
                    // Write Header
                    writer.WriteBytes(Encoding.ASCII.GetBytes(MAGIC));
                    writer.WriteByte(VERSION);

                    // Write Basic Info
                    writer.WriteInt32(data.id);
                    writer.WriteInt32((int)data.Type);
                    writer.WriteUInt16((ushort)data.ModifierType);
                    writer.WriteBoolean(data.EffectOnAwake);
                    writer.WriteUInt16((ushort)data.Policy);
                    writer.WriteSingle(data.Period);
                    writer.WriteSingle(data.Duration);
                    writer.WriteInt32(data.Target);
                    writer.WriteInt32((int)data.EffectType); // actor_attribute

                    // Write Extension Parameters
                    var extParam = data.ExtensionParam;
                    if (extParam != null)
                    {
                        writer.WriteSingle(extParam.float_1);
                        writer.WriteSingle(extParam.float_2);
                        writer.WriteSingle(extParam.float_3);
                        writer.WriteSingle(extParam.float_4);
                        writer.WriteInt32(extParam.int_1);
                        writer.WriteInt32(extParam.int_2);
                        writer.WriteInt32(extParam.int_3);
                        writer.WriteInt32(extParam.int_4);
                    }
                    else
                    {
                        // Write default values
                        writer.WriteSingle(0f);
                        writer.WriteSingle(0f);
                        writer.WriteSingle(0f);
                        writer.WriteSingle(0f);
                        writer.WriteInt32(0);
                        writer.WriteInt32(0);
                        writer.WriteInt32(0);
                        writer.WriteInt32(0);
                    }

                    // Write Derive Effects
                    var deriveEffects = data.DeriveEffects ?? new int[0];
                    writer.WriteInt32(deriveEffects.Length);
                    foreach (var effectId in deriveEffects)
                    {
                        writer.WriteInt32(effectId);
                    }

                    // Write Awake Effects
                    var awakeEffects = data.AwakeEffects ?? new int[0];
                    writer.WriteInt32(awakeEffects.Length);
                    foreach (var effectId in awakeEffects)
                    {
                        writer.WriteInt32(effectId);
                    }
                }
            }
            
            Debug.Log($"[EffectBinaryExporter] Exported: {outputPath}");
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }

        private const string MAGIC = "EFFECT";
        private const byte VERSION = 0x01;
    }
}
