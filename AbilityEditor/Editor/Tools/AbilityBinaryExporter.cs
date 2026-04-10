using System.IO;
using System.Text;
using Aquila.AbilityEditor;
using UnityEditor;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// 技能二进制导出工具
    /// 将AbilityData资产导出为.ablt二进制文件
    /// </summary>
    public static class AbilityBinaryExporter
    {
        [MenuItem("Aquila/AbilityEditor/.ablt Export|Import/Export all ability data(.ablt)")]
        public static void ExportAllAbilities()
        {
            if (!Directory.Exists(Misc.ABILITY_ASSET_BASE_PATH))
            {
                Aquila.Toolkit.Tools.Logger.Error($"[AbilityBinaryExporter] Source path not found: {Misc.ABILITY_ASSET_BASE_PATH}");
                return;
            }

            EnsureDirectoryExists(Misc.ABILITY_BIN_ASSET_PATH);
            string[] assetGuids = AssetDatabase.FindAssets("t:AbilityEditorSOData", new[] { Misc.ABILITY_ASSET_BASE_PATH });
            int successCount = 0;
            int failCount = 0;

            foreach (string guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var abilityData = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(assetPath);
                if (abilityData == null)
                {
                    Aquila.Toolkit.Tools.Logger.Warning($"[AbilityBinaryExporter] Failed to load: {assetPath}");
                    failCount++;
                    continue;
                }

                if (!abilityData.Validate(out string error))
                {
                    Aquila.Toolkit.Tools.Logger.Warning($"[AbilityBinaryExporter] Validation failed for {assetPath}: {error}");
                    failCount++;
                    continue;
                }

                string outputFile = Path.Combine(Misc.ABILITY_BIN_ASSET_PATH, $"{abilityData.Id}.ablt");
                ExportAbility(abilityData, outputFile);
                successCount++;
            }

            AssetDatabase.Refresh();
            Aquila.Toolkit.Tools.Logger.Info($"[AbilityBinaryExporter] Export complete. Success: {successCount}, Failed: {failCount}");
        }

        /// <summary>
        /// 导出单个AbilityData为.ablt文件
        /// </summary>
        public static void ExportAbility(AbilityEditorSOData data, string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                using (Aquila.Toolkit.Tools.ByteWriter writer = new Aquila.Toolkit.Tools.ByteWriter(fs))
                {
                    //write Header
                    writer.WriteBytes(Encoding.ASCII.GetBytes(MAGIC));
                    writer.WriteByte(VERSION);

                    //write Basic Info
                    writer.WriteInt32(data.Id);
                    writer.WriteInt32(data.CostEffectID);
                    writer.WriteInt32(data.CoolDownEffectID);
                    writer.WriteInt32((int)data.TargetType);
                    writer.WriteInt32(data.TimelineID);
                    writer.WriteSingle(data.TimelineDuration);

                    //write Tracks
                    var tracks = data.Tracks;
                    writer.WriteInt32(tracks?.Count ?? 0);

                    if (tracks != null)
                    {
                        foreach (var track in tracks)
                            WriteTrack(writer, track);
                    }
                }
            }
            Aquila.Toolkit.Tools.Logger.Info($"[AbilityBinaryExporter] Exported: {outputPath}");
        }

        private static void WriteTrack(Aquila.Toolkit.Tools.ByteWriter writer, SerializedTrackData track)
        {
            var clips = track.Clips;
            //write clip count
            writer.WriteInt32(clips?.Count ?? 0);
            if (clips != null)
            {
                foreach (var clip in clips)
                    WriteClip(writer, clip);
            }
        }

        private static void WriteClip(Aquila.Toolkit.Tools.ByteWriter writer, TimelineClipData clip)
        {
            // ClipType
            writer.WriteInt32((int)clip.ClipType);
            // Common fields
            writer.WriteSingle(clip.StartTime);
            writer.WriteSingle(clip.EndTime);
            // writer.Write(clip.IsEnabled);

            // Type-specific data
            switch (clip)
            {
                case EffectClipData effectClip:
                    WriteEffectClip(writer, effectClip);
                    break;
                
                case AudioClipData audioClip:
                    WriteAudioClip(writer, audioClip);
                    break;
                
                case VFXClipData vfxClip:
                    WriteVFXClip(writer, vfxClip);
                    break;
                
                default:
                    Aquila.Toolkit.Tools.Logger.Warning($"[AbilityBinaryExporter] Unknown clip type: {clip.ClipType}");
                    break;
            }
        }

        private static void WriteEffectClip(Aquila.Toolkit.Tools.ByteWriter writer, EffectClipData clip)
        {
            // Effect Clip 基础字段
            writer.WriteInt32(clip.EffectId);
            writer.WriteInt32(clip.StackCount);
            writer.WriteBoolean(clip.CanStack);
            
            // Effect 配置字段
            writer.WriteInt32((int)clip.EffectType);
            writer.WriteUInt16((ushort)clip.ModifierType);
            writer.WriteInt32((int)clip.AffectedAttribute);
            writer.WriteInt32(clip.Target);
            writer.WriteInt32(clip.ResolveTypeID);
            writer.WriteSingle(clip.Duration);
            writer.WriteSingle(clip.Period);
            writer.WriteUInt16((ushort)clip.Policy);
            writer.WriteBoolean(clip.EffectOnAwake);
            
            // 扩展参数
            var extParam = clip.ExtensionParam;
            writer.WriteSingle(extParam.FloatParam_1);
            writer.WriteSingle(extParam.FloatParam_2);
            writer.WriteSingle(extParam.FloatParam_3);
            writer.WriteSingle(extParam.FloatParam_4);
            writer.WriteInt32(extParam.IntParam_1);
            writer.WriteInt32(extParam.IntParam_2);
            writer.WriteInt32(extParam.IntParam_3);
            writer.WriteInt32(extParam.IntParam_4);
            
            // 派生效果数组
            var deriveEffects = clip.DeriveEffects ?? new int[0];
            writer.WriteInt32(deriveEffects.Length);
            foreach (var effectId in deriveEffects)
                writer.WriteInt32(effectId);
            
            // 唤醒效果数组
            var awakeEffects = clip.AwakeEffects ?? new int[0];
            writer.WriteInt32(awakeEffects.Length);
            foreach (var effectId in awakeEffects)
                writer.WriteInt32(effectId);
        }

        private static void WriteAudioClip(Aquila.Toolkit.Tools.ByteWriter writer, AudioClipData clip)
        {
            writer.WriteInt32(clip.AudioId);
            writer.WriteSingle(clip.Volume);
            writer.WriteBoolean(clip.Loop);
            writer.WriteSingle(clip.FadeInDuration);
            writer.WriteSingle(clip.FadeOutDuration);
        }

        private static void WriteVFXClip(Aquila.Toolkit.Tools.ByteWriter writer, VFXClipData clip)
        {
            writer.WriteString(clip.VfxPath);
            writer.WriteString(clip.AttachPoint);
            // PositionOffset
            writer.WriteVector3(clip.PositionOffset);
            // RotationOffset
            writer.WriteVector3(clip.RotationOffset);
            // Scale
            writer.WriteVector3(clip.Scale);
            writer.WriteBoolean(clip.FollowAttachPoint);
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }
        
        private const string MAGIC = "ABLT";
        private const byte VERSION = 0x02;
    }
}
