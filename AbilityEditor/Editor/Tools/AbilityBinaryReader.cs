using System.IO;
using System.Text;
using UnityEditor;

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
                Aquila.Toolkit.Tools.Logger.Error($"[AbilityBinaryReader] File not found: {fullPath}");
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
                    Aquila.Toolkit.Tools.Logger.Error($"[AbilityBinaryReader] Invalid magic: {magic}, expected: {MAGIC}");
                    return;
                }


                if (version != VERSION)
                {
                    Aquila.Toolkit.Tools.Logger.Error($"[AbilityBinaryReader] Unsupported version: actual={version}, expected={VERSION}");
                    return;
                }

                _currentVersion = version;
                sb.AppendLine($"[Header] Magic: {magic}, Version: {version}");

                // Basic Info
                int abilityId = reader.ReadInt32();
                int costEffectId = reader.ReadInt32();
                int coolDownEffectId = reader.ReadInt32();
                int targetType = reader.ReadInt32();
                int selectType = reader.ReadInt32();
                float selectRadius = reader.ReadSingle();
                int timelineId = reader.ReadInt32();
                float timelineDuration = reader.ReadSingle();

                sb.AppendLine("[Basic Info]");
                sb.AppendLine($"  AbilityID: {abilityId}");
                sb.AppendLine($"  CostEffectID: {costEffectId}");
                sb.AppendLine($"  CoolDownEffectID: {coolDownEffectId}");
                sb.AppendLine($"  TargetType: {targetType}");
                sb.AppendLine($"  SelectType: {selectType}");
                sb.AppendLine($"  SelectRadius: {selectRadius}");
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

                ReadMontageEvents(reader, sb);
                ReadCueBindings(reader, sb);

                sb.AppendLine("========== End ==========");
                Aquila.Toolkit.Tools.Logger.Info(sb.ToString());
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

                default:
                    throw new InvalidDataException($"[AbilityBinaryReader] Unsupported clip type: {clipType}");
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
            int resolveTypeID = reader.ReadInt32();
            float duration = reader.ReadSingle();
            float period = reader.ReadSingle();
            ushort policy = reader.ReadUInt16();
            bool effectOnAwake = reader.ReadBoolean();
            
            sb.AppendLine($"{indent}EffectType: {effectType}");
            sb.AppendLine($"{indent}ModifierType: {modifierType}");
            sb.AppendLine($"{indent}AffectedAttribute: {affectedAttribute}");
            sb.AppendLine($"{indent}Target: {target}");
            sb.AppendLine($"{indent}ResolveTypeID: {resolveTypeID}");
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

            sb.AppendLine($"{indent}FormulaID: {reader.ReadInt32()}");
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
                case 4: return "Animation";
                case 5: return "Custom";
                default: return "Unknown";
            }
        }

        private static void ReadMontageEvents(BinaryReader reader, StringBuilder sb)
        {
            var count = reader.ReadInt32();
            sb.AppendLine($"[Montage Events] Count: {count}");
            for (var i = 0; i < count; i++)
            {
                var time = reader.ReadSingle();
                var sequence = reader.ReadInt32();
                var markerId = ReadString(reader);
                var eventTag = ReadString(reader);
                sb.AppendLine($"  [{i}] {time:F3}s #{sequence} {markerId} -> {eventTag}");
            }
        }

        private static void ReadCueBindings(BinaryReader reader, StringBuilder sb)
        {
            var count = reader.ReadInt32();
            sb.AppendLine($"[Cue Bindings] Count: {count}");
            for (var i = 0; i < count; i++)
            {
                var eventTag = ReadString(reader);
                var cueTag = ReadString(reader);
                var eventType = reader.ReadByte();
                var targetPolicy = reader.ReadByte();
                var locationPolicy = reader.ReadByte();
                var magnitude = reader.ReadSingle();
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = reader.ReadSingle();
                sb.AppendLine($"  [{i}] {eventTag} -> {cueTag}, event={eventType}, target={targetPolicy}, location={locationPolicy}, magnitude={magnitude}, offset=({x}, {y}, {z})");
            }
        }

        private const string MAGIC = "ABLT";
        private static byte _currentVersion = VERSION;
        private const byte VERSION = 0x05;
        private const string CONTEXT_MENU_PATH = "Assets/AbilityEditor/ReadBinaryAbilityData";
    }
}
