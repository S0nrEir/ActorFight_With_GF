using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// Effect 二进制读取工具
    /// 用于测试验证 .efct 文件内容
    /// </summary>
    public static class EffectBinaryReader
    {
        [MenuItem(CONTEXT_MENU_PATH, false, 100)]
        public static void ReadSelectedEfctFile()
        {
            var selected = Selection.activeObject;
            if (selected == null)
                return;

            string assetPath = AssetDatabase.GetAssetPath(selected);
            string fullPath = Path.GetFullPath(assetPath);

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"[EffectBinaryReader] File not found: {fullPath}");
                return;
            }

            ReadAndPrint(fullPath);
        }

        [MenuItem(CONTEXT_MENU_PATH, true)]
        public static bool ValidateReadSelectedEfctFile()
        {
            var selected = Selection.activeObject;
            if (selected == null)
                return false;

            string assetPath = AssetDatabase.GetAssetPath(selected);
            return !string.IsNullOrEmpty(assetPath) && Path.GetExtension(assetPath).Equals(".efct");
        }

        [MenuItem("Aquila/AbilityEditor/.efft Export|Import/Import .efct")]
        public static void ReadEfctFile()
        {
            string filePath = EditorUtility.OpenFilePanel("Select .efct File", "Assets/Res/Config/Effect", "efct");
            if (string.IsNullOrEmpty(filePath))
                return;

            ReadAndPrint(filePath);
        }

        public static void ReadAndPrint(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"========== Reading: {Path.GetFileName(filePath)} ==========");

                // Header
                string magic = Encoding.ASCII.GetString(reader.ReadBytes(6));
                byte version = reader.ReadByte();

                if (magic != MAGIC)
                {
                    Debug.LogError($"[EffectBinaryReader] Invalid magic: {magic}, expected: {MAGIC}");
                    return;
                }

                sb.AppendLine($"[Header] Magic: {magic}, Version: {version}");

                // Basic Info
                int id = reader.ReadInt32();
                int type = reader.ReadInt32();
                ushort modifierType = reader.ReadUInt16();
                bool effectOnAwake = reader.ReadBoolean();
                ushort policy = reader.ReadUInt16();
                float period = reader.ReadSingle();
                float duration = reader.ReadSingle();
                int target = reader.ReadInt32();
                int effectType = reader.ReadInt32();

                sb.AppendLine($"[Basic Info]");
                sb.AppendLine($"  ID: {id}");
                sb.AppendLine($"  Type: {type}");
                sb.AppendLine($"  ModifierType: {modifierType}");
                sb.AppendLine($"  EffectOnAwake: {effectOnAwake}");
                sb.AppendLine($"  Policy: {policy}");
                sb.AppendLine($"  Period: {period}s");
                sb.AppendLine($"  Duration: {duration}s");
                sb.AppendLine($"  Target: {target}");
                sb.AppendLine($"  EffectType (AffectedAttribute): {effectType}");

                // Extension Parameters
                float float1 = reader.ReadSingle();
                float float2 = reader.ReadSingle();
                float float3 = reader.ReadSingle();
                float float4 = reader.ReadSingle();
                int int1 = reader.ReadInt32();
                int int2 = reader.ReadInt32();
                int int3 = reader.ReadInt32();
                int int4 = reader.ReadInt32();

                sb.AppendLine($"[Extension Parameters]");
                sb.AppendLine($"  Float: ({float1}, {float2}, {float3}, {float4})");
                sb.AppendLine($"  Int: ({int1}, {int2}, {int3}, {int4})");

                // Derive Effects
                int deriveCount = reader.ReadInt32();
                if (deriveCount > 0)
                {
                    sb.Append($"[Derive Effects] Count: {deriveCount}, IDs: [");
                    for (int i = 0; i < deriveCount; i++)
                    {
                        if (i > 0) sb.Append(", ");
                        sb.Append(reader.ReadInt32());
                    }
                    sb.AppendLine("]");
                }
                else
                {
                    sb.AppendLine($"[Derive Effects] Count: 0");
                }

                // Awake Effects
                int awakeCount = reader.ReadInt32();
                if (awakeCount > 0)
                {
                    sb.Append($"[Awake Effects] Count: {awakeCount}, IDs: [");
                    for (int i = 0; i < awakeCount; i++)
                    {
                        if (i > 0) sb.Append(", ");
                        sb.Append(reader.ReadInt32());
                    }
                    sb.AppendLine("]");
                }
                else
                {
                    sb.AppendLine($"[Awake Effects] Count: 0");
                }

                sb.AppendLine("========== End ==========");
                Debug.Log(sb.ToString());
            }
        }

        private const string MAGIC = "EFFECT";
        private const string CONTEXT_MENU_PATH = "Assets/EffectEditor/ReadBinaryEffectData";
    }
}
