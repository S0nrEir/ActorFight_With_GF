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
        /// 导出所有技能资产配置为.ablt文件
        /// </summary>
        public static void ExportToConfig()
        {
            
        }

        // 导出配置为 AbilityData 资产文件
        // config: 生成的配置
        // tracks: 编辑器中的轨道数据
        public static void ExportToAsset(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            string assetPath = $"{Misc.ABILITY_ASSET_BASE_PATH}/{config.AbilityID}.asset";
            EnsureDirectoryExists( Misc.ABILITY_ASSET_BASE_PATH );
            var existingAsset = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(assetPath);
            bool isOverwrite = existingAsset != null;
            AbilityEditorSOData abilityData;
            if (isOverwrite)
            {
                //覆盖旧资产 / overwrite old asset
                abilityData = existingAsset;
                UpdateAbilityData(abilityData, config, tracks);
                EditorUtility.SetDirty(abilityData);
            }
            else
            {
                //新资产 / new asset
                abilityData = CreateAbilityData(config, tracks);
                AssetDatabase.CreateAsset(abilityData, assetPath);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            string action = isOverwrite ? "已覆盖" : "已创建";
            Debug.Log($"[AbilityDataExporter] {action}配置资产: {assetPath}");
        }

        /// <summary>
        /// 导出配置为沙盒测试用的 AbilityData 资产文件（固定路径和文件名）
        /// </summary>
        public static void ExportToSandBox(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            string assetPath = $"{Misc.SANDBOX_ABILITY_PATH}/{Misc.SANDBOX_ABILITY_FILENAME}";
            EnsureDirectoryExists(Misc.SANDBOX_ABILITY_PATH);

            var existingAsset = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(assetPath);
            bool isOverwrite = existingAsset != null;

            AbilityEditorSOData abilityData;
            if (isOverwrite)
            {
                // 覆盖旧资产 / overwrite old asset
                abilityData = existingAsset;
                UpdateAbilityData(abilityData, config, tracks);
                EditorUtility.SetDirty(abilityData);
            }
            else
            {
                // 新资产 / new asset
                abilityData = CreateAbilityData(config, tracks);
                AssetDatabase.CreateAsset(abilityData, assetPath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string action = isOverwrite ? "已覆盖" : "已创建";
            Debug.Log($"[AbilityDataExporter] {action}沙盒测试配置: {assetPath}");
        }

        // 从 AbilityConfig 和 Tracks 创建新的 AbilityData
        private static AbilityEditorSOData CreateAbilityData(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            var abilityData = ScriptableObject.CreateInstance<AbilityEditorSOData>();
            UpdateAbilityData(abilityData, config, tracks);
            return abilityData;
        }

        // 更新现有 AbilityData 的数据
        private static void UpdateAbilityData(AbilityEditorSOData abilityData, AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            // 复制元数据
            abilityData.Id = config.AbilityID;
            abilityData.Name = config.Name;
            abilityData.Desc = config.Desc;
            abilityData.CostEffectID = config.CostEffectID;
            abilityData.CoolDownEffectID = config.CoolDownEffectID;
            abilityData.TargetType = config.TargetType;
            abilityData.TimelineID = config.TimelineID;
            abilityData.TimelineAssetPath = config.TimelineAssetPath;
            abilityData.TimelineDuration = config.TimelineDuration;

            // 转换 Tracks 数据
            var serializedTracks = ConvertTracksToSerialized(tracks);
            abilityData.SetTracks(serializedTracks);
        }

        // 将 TimelineTrackItem 转换为 SerializedTrackData
        private static List<SerializedTrackData> ConvertTracksToSerialized(List<TimelineTrackItem> tracks)
        {
            var serialized = new List<SerializedTrackData>();

            if (tracks == null)
                return serialized;

            foreach (var track in tracks)
            {
                if (track != null)
                {
                    var trackData = new SerializedTrackData(track);
                    serialized.Add(trackData);
                }
            }

            return serialized;
        }

        // 确保目录存在
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
