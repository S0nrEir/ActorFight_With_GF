using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquila.AbilityEditor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// 技能二进制导出工具
    /// 将AbilityData资产导出为.ablt二进制文件
    /// </summary>
    public static class AbilityBinaryExporter
    {
        [MenuItem("Aquila/AbilityEditor/.ablt Export|Import/Export all effect data(.ablt)")]
        public static void ExportAllAbilities()
        {
            if (!Directory.Exists(Misc.ABILITY_ASSET_BASE_PATH))
            {
                Debug.LogError($"[AbilityBinaryExporter] Source path not found: {Misc.ABILITY_ASSET_BASE_PATH}");
                return;
            }

            EnsureDirectoryExists(Misc.ABILITY_BIN_ASSET_PATH);

            string[] assetGuids = AssetDatabase.FindAssets("t:AbilityData", new[] { Misc.ABILITY_ASSET_BASE_PATH });
            int successCount = 0;
            int failCount = 0;

            foreach (string guid in assetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var abilityData = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(assetPath);
                if (abilityData == null)
                {
                    Debug.LogWarning($"[AbilityBinaryExporter] Failed to load: {assetPath}");
                    failCount++;
                    continue;
                }

                if (!abilityData.Validate(out string error))
                {
                    Debug.LogWarning($"[AbilityBinaryExporter] Validation failed for {assetPath}: {error}");
                    failCount++;
                    continue;
                }

                string outputFile = Path.Combine(Misc.ABILITY_BIN_ASSET_PATH, $"{abilityData.Id}.ablt");
                ExportAbility(abilityData, outputFile);
                successCount++;
            }

            AssetDatabase.Refresh();
            Debug.Log($"[AbilityBinaryExporter] Export complete. Success: {successCount}, Failed: {failCount}");
        }

        /// <summary>
        /// 导出单个AbilityData为.ablt文件
        /// </summary>
        public static void ExportAbility(AbilityEditorSOData data, string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(fs, Encoding.UTF8))
                {
                    //write Header
                    writer.Write(Encoding.ASCII.GetBytes(MAGIC));
                    writer.Write(VERSION);

                    //write Basic Info
                    writer.Write(data.Id);
                    writer.Write(data.CostEffectID);
                    writer.Write(data.CoolDownEffectID);
                    writer.Write((int)data.TargetType);
                    writer.Write(data.TimelineID);
                    writer.Write(data.TimelineDuration);

                    //write Tracks
                    var tracks = data.Tracks;
                    writer.Write(tracks?.Count ?? 0);

                    if (tracks != null)
                    {
                        foreach (var track in tracks)
                            WriteTrack(writer, track);
                    }
                }
            }
            Debug.Log($"[AbilityBinaryExporter] Exported: {outputPath}");
        }

        private static void WriteTrack(BinaryWriter writer, SerializedTrackData track)
        {
            // writer.Write(track.IsEnabled);
            // writer.Write(track.TrackColor.r);
            // writer.Write(track.TrackColor.g);
            // writer.Write(track.TrackColor.b);
            // writer.Write(track.TrackColor.a);

            var clips = track.Clips;
            //write clip count
            writer.Write(clips?.Count ?? 0);
            if (clips != null)
            {
                foreach (var clip in clips)
                    WriteClip(writer, clip);
            }
        }

        private static void WriteClip(BinaryWriter writer, TimelineClipData clip)
        {
            // ClipType
            writer.Write((int)clip.ClipType);
            // Common fields
            writer.Write(clip.StartTime);
            writer.Write(clip.EndTime);
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
                    Debug.LogWarning($"[AbilityBinaryExporter] Unknown clip type: {clip.ClipType}");
                    break;
            }
        }

        private static void WriteEffectClip(BinaryWriter writer, EffectClipData clip)
        {
            // Effect Clip只写入EffectId
            writer.Write(clip.EffectId);
        }

        private static void WriteAudioClip(BinaryWriter writer, AudioClipData clip)
        {
            WriteString(writer, clip.AudioPath);
            writer.Write(clip.Volume);
            writer.Write(clip.Loop);
            writer.Write(clip.FadeInDuration);
            writer.Write(clip.FadeOutDuration);
        }

        private static void WriteVFXClip(BinaryWriter writer, VFXClipData clip)
        {
            WriteString(writer, clip.VfxPath);
            WriteString(writer, clip.AttachPoint);
            // PositionOffset
            writer.Write(clip.PositionOffset.x);
            writer.Write(clip.PositionOffset.y);
            writer.Write(clip.PositionOffset.z);
            // RotationOffset
            writer.Write(clip.RotationOffset.x);
            writer.Write(clip.RotationOffset.y);
            writer.Write(clip.RotationOffset.z);
            // Scale
            writer.Write(clip.Scale.x);
            writer.Write(clip.Scale.y);
            writer.Write(clip.Scale.z);
            writer.Write(clip.FollowAttachPoint);
        }

        private static void WriteString(BinaryWriter writer, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                writer.Write(0);
                return;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes.Length);
            writer.Write(bytes);
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
        private const byte VERSION = 0x01;
    }
}
