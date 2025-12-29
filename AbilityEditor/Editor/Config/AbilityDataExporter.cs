using System.Collections.Generic;
using System.IO;
using Aquila.AbilityEditor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Config
{
    // 配置导出工具类，负责将 AbilityConfig 导出为 AbilityData 资产文件
    public static class AbilityDataExporter
    {
        /// <summary>
        /// 导出配置为 AbilityData 资产文件 / Export configuration as AbilityData asset file
        /// </summary>
        public static void ExportToAsset(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            string assetPath = Path.Combine(Application.dataPath , Misc.ABILITY_CFG_GEN_PATH , $"{config.AbilityID}.asset");
            if(File.Exists(assetPath))
            {
                throw new System.InvalidOperationException($"资产文件已存在: {assetPath}。请先删除现有文件或使用不同 的技能ID。");
            }
            EnsureDirectoryExists(assetPath);
            var abilityData = CreateAbilityData(config, tracks);
            
            AssetDatabase.CreateAsset(abilityData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[AbilityDataExporter] 已导出配置到: {assetPath}");
        }
        
        /// <summary>
        /// 从 AbilityConfig 和 Tracks 创建 AbilityData / Create AbilityData from AbilityConfig and Tracks
        /// </summary>
        private static AbilityData CreateAbilityData(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            var abilityData = ScriptableObject.CreateInstance<AbilityData>();
            
            abilityData.Id = config.AbilityID;
            abilityData.Name = config.Name;
            abilityData.Desc = config.Desc;
            abilityData.CostEffectID = config.CostEffectID;
            abilityData.CoolDownEffectID = config.CoolDownEffectID;
            abilityData.TargetType = config.TargetType;
            abilityData.TimelineID = config.TimelineID;
            abilityData.TimelineDuration = config.TimelineDuration;

            // 转换 Tracks 数据 / convert track data
            var serializedTracks = ConvertTracksToSerialized(tracks);
            abilityData.SetTracks(serializedTracks);

            return abilityData;
        }
        
        /// <summary>
        /// 将 TimelineTrackItem 转换为 SerializedTrackData / convert TimelineTrackItem to SerializedTrackData
        /// </summary>
        private static List<SerializedTrackData> ConvertTracksToSerialized(List<TimelineTrackItem> tracks)
        {
            var serialized = new List<SerializedTrackData>();

            foreach (var track in tracks)
            {
                var trackData = new SerializedTrackData(track);
                // 不保存轨道颜色，使用默认白色
                trackData.TrackColor = Color.white;
                serialized.Add(trackData);
            }

            return serialized;
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }
    }
}
