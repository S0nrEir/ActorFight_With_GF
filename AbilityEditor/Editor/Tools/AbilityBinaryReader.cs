using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// 技能二进制读取工具
    /// 用于测试验证.ablt文件内容
    /// </summary>
    public static class AbilityBinaryReader
    {
        [MenuItem(CONTEXT_MENU_PATH, false, 100)]
        public static void ReadSelectedAbltFile()
        {
            var selected = Selection.activeObject;
            if (selected == null)
                return;

            string assetPath = AssetDatabase.GetAssetPath(selected);
            string fullPath = Path.GetFullPath(assetPath);

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"[AbilityBinaryReader] File not found: {fullPath}");
                return;
            }

            ReadAndPrint(fullPath);
        }

        [MenuItem(CONTEXT_MENU_PATH, true)]
        public static bool ValidateReadSelectedAbltFile()
        {
            var selected = Selection.activeObject;
            if (selected == null)
                return false;

            string assetPath = AssetDatabase.GetAssetPath(selected);
            return !string.IsNullOrEmpty(assetPath) && Path.GetExtension(assetPath).Equals(".ablt");
        }

        [MenuItem("Aquila/AbilityEditor/.ablt Export|Import/Importe .abl")]
        public static void ReadAbltFile()
        {
            string filePath = EditorUtility.OpenFilePanel("Select .ablt File", "Assets/Res/Config/Ability", "ablt");
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
                string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
                byte version = reader.ReadByte();

                if (magic != MAGIC)
                {
                    Debug.LogError($"[AbilityBinaryReader] Invalid magic: {magic}, expected: {MAGIC}");
                    return;
                }

                sb.AppendLine($"[Header] Magic: {magic}, Version: {version}");

                // Basic Info
                int abilityId = reader.ReadInt32();
                int costEffectId = reader.ReadInt32();
                int coolDownEffectId = reader.ReadInt32();
                int targetType = reader.ReadInt32();
                int timelineId = reader.ReadInt32();
                float timelineDuration = reader.ReadSingle();

                sb.AppendLine($"[Basic Info]");
                sb.AppendLine($"  AbilityID: {abilityId}");
                sb.AppendLine($"  CostEffectID: {costEffectId}");
                sb.AppendLine($"  CoolDownEffectID: {coolDownEffectId}");
                sb.AppendLine($"  TargetType: {targetType}");
                sb.AppendLine($"  TimelineID: {timelineId}");
                sb.AppendLine($"  TimelineDuration: {timelineDuration}s");

                // Tracks
                int trackCount = reader.ReadInt32();
                sb.AppendLine($"[Tracks] Count: {trackCount}");

                for (int t = 0; t < trackCount; t++)
                {
                    sb.AppendLine($"  Track[{t}]:");
                    ReadTrack(reader, sb, "    ");
                }

                sb.AppendLine("========== End ==========");
                Debug.Log(sb.ToString());
            }
        }

        private static void ReadTrack(BinaryReader reader, StringBuilder sb, string indent)
        {
            // bool isEnabled = reader.ReadBoolean();
            // float r = reader.ReadSingle();
            // float g = reader.ReadSingle();
            // float b = reader.ReadSingle();
            // float a = reader.ReadSingle();

            sb.AppendLine($"{indent}IsEnabled: {true}");
            // sb.AppendLine($"{indent}Color: ({r:F2}, {g:F2}, {b:F2}, {a:F2})");
            
            //read clip count
            int clipCount = reader.ReadInt32();
            sb.AppendLine($"{indent}ClipCount: {clipCount}");

            for (int c = 0; c < clipCount; c++)
            {
                sb.AppendLine($"{indent}Clip[{c}]:");
                ReadClip(reader, sb, indent + "  ");
            }
        }

        private static void ReadClip(BinaryReader reader, StringBuilder sb, string indent)
        {
            int clipType = reader.ReadInt32();
            float startTime = reader.ReadSingle();
            float endTime = reader.ReadSingle();
            // bool isEnabled = reader.ReadBoolean();

            string clipTypeName = GetClipTypeName(clipType);
            sb.AppendLine($"{indent}Type: {clipTypeName} ({clipType})");
            sb.AppendLine($"{indent}Time: {startTime:F2}s - {endTime:F2}s");
            sb.AppendLine($"{indent}IsEnabled: {true}");

            switch (clipType)
            {
                case 1: // Buff/Effect
                    ReadEffectClip(reader, sb, indent);
                    break;
                
                case 2: // Audio
                    ReadAudioClip(reader, sb, indent);
                    break;
                
                case 3: // VFX
                    ReadVFXClip(reader, sb, indent);
                    break;
                
                default:
                    sb.AppendLine($"{indent}[Unknown clip type data]");
                    break;
            }
        }

        private static void ReadEffectClip(BinaryReader reader, StringBuilder sb, string indent)
        {
            // 基础字段
            int effectId = reader.ReadInt32();
            int stackCount = reader.ReadInt32();
            bool canStack = reader.ReadBoolean();
            
            sb.AppendLine($"{indent}EffectId: {effectId}");
            sb.AppendLine($"{indent}StackCount: {stackCount}");
            sb.AppendLine($"{indent}CanStack: {canStack}");
            
            // Effect 配置字段
            int effectType = reader.ReadInt32();
            ushort modifierType = reader.ReadUInt16();
            int affectedAttribute = reader.ReadInt32();
            int target = reader.ReadInt32();
            float duration = reader.ReadSingle();
            float period = reader.ReadSingle();
            ushort policy = reader.ReadUInt16();
            bool effectOnAwake = reader.ReadBoolean();
            
            sb.AppendLine($"{indent}EffectType: {effectType}");
            sb.AppendLine($"{indent}ModifierType: {modifierType}");
            sb.AppendLine($"{indent}AffectedAttribute: {affectedAttribute}");
            sb.AppendLine($"{indent}Target: {target}");
            sb.AppendLine($"{indent}Duration: {duration}");
            sb.AppendLine($"{indent}Period: {period}");
            sb.AppendLine($"{indent}Policy: {policy}");
            sb.AppendLine($"{indent}EffectOnAwake: {effectOnAwake}");
            
            // 扩展参数
            float floatParam1 = reader.ReadSingle();
            float floatParam2 = reader.ReadSingle();
            float floatParam3 = reader.ReadSingle();
            float floatParam4 = reader.ReadSingle();
            int intParam1 = reader.ReadInt32();
            int intParam2 = reader.ReadInt32();
            int intParam3 = reader.ReadInt32();
            int intParam4 = reader.ReadInt32();
            
            sb.AppendLine($"{indent}ExtensionParams: F({floatParam1}, {floatParam2}, {floatParam3}, {floatParam4}) I({intParam1}, {intParam2}, {intParam3}, {intParam4})");
            
            // 派生效果数组
            int deriveCount = reader.ReadInt32();
            if (deriveCount > 0)
            {
                sb.Append($"{indent}DeriveEffects: [");
                for (int i = 0; i < deriveCount; i++)
                {
                    if (i > 0) sb.Append(", ");
                    sb.Append(reader.ReadInt32());
                }
                sb.AppendLine("]");
            }
            
            // 唤醒效果数组
            int awakeCount = reader.ReadInt32();
            if (awakeCount > 0)
            {
                sb.Append($"{indent}AwakeEffects: [");
                for (int i = 0; i < awakeCount; i++)
                {
                    if (i > 0) sb.Append(", ");
                    sb.Append(reader.ReadInt32());
                }
                sb.AppendLine("]");
            }
        }

        private static void ReadAudioClip(BinaryReader reader, StringBuilder sb, string indent)
        {
            string audioPath = ReadString(reader);
            float volume = reader.ReadSingle();
            bool loop = reader.ReadBoolean();
            float fadeIn = reader.ReadSingle();
            float fadeOut = reader.ReadSingle();

            sb.AppendLine($"{indent}AudioPath: {audioPath}");
            sb.AppendLine($"{indent}Volume: {volume:F2}");
            sb.AppendLine($"{indent}Loop: {loop}");
            sb.AppendLine($"{indent}FadeIn: {fadeIn:F2}s, FadeOut: {fadeOut:F2}s");
        }

        private static void ReadVFXClip(BinaryReader reader, StringBuilder sb, string indent)
        {
            string vfxPath = ReadString(reader);
            string attachPoint = ReadString(reader);
            float posX = reader.ReadSingle();
            float posY = reader.ReadSingle();
            float posZ = reader.ReadSingle();
            float rotX = reader.ReadSingle();
            float rotY = reader.ReadSingle();
            float rotZ = reader.ReadSingle();
            float scaleX = reader.ReadSingle();
            float scaleY = reader.ReadSingle();
            float scaleZ = reader.ReadSingle();
            bool followAttach = reader.ReadBoolean();

            sb.AppendLine($"{indent}VfxPath: {vfxPath}");
            sb.AppendLine($"{indent}AttachPoint: {attachPoint}");
            sb.AppendLine($"{indent}Position: ({posX:F2}, {posY:F2}, {posZ:F2})");
            sb.AppendLine($"{indent}Rotation: ({rotX:F2}, {rotY:F2}, {rotZ:F2})");
            sb.AppendLine($"{indent}Scale: ({scaleX:F2}, {scaleY:F2}, {scaleZ:F2})");
            sb.AppendLine($"{indent}FollowAttachPoint: {followAttach}");
        }

        private static string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length <= 0)
                return string.Empty;
            byte[] bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        private static string GetClipTypeName(int clipType)
        {
            switch (clipType)
            {
                case 0: return "Ability";
                case 1: return "Buff/Effect";
                case 2: return "Audio";
                case 3: return "VFX";
                case 4: return "Animation";
                case 5: return "Custom";
                default: return "Unknown";
            }
        }

        private const string MAGIC = "ABLT";
        private const string CONTEXT_MENU_PATH = "Assets/AbilityEditor/ReadBinaryAbilityData";
    }
}
