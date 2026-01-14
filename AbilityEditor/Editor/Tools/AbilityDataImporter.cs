using System;
using System.Collections.Generic;
using System.IO;
using Aquila.AbilityEditor;
using Cfg.Enum;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    // AbilityData 导入工具
    // 从 JSON 文件导入 AbilityData 资产
    public static class AbilityDataImporter
    {
        private const string ASSET_BASE_PATH = "Assets/AbilityEditor/Editor/Config/Ability";

        [MenuItem("Aquila/AbilityEditor/Tools/Import AbilityData from JSON")]
        public static void ImportFromJSON()
        {
            // 选择 JSON 文件
            string jsonPath = EditorUtility.OpenFilePanel("Select AbilityData JSON", Application.dataPath, "json");
            if (string.IsNullOrEmpty(jsonPath))
            {
                Debug.Log("[AbilityDataImporter] Import cancelled");
                return;
            }

            try
            {
                // 读取 JSON
                string jsonContent = File.ReadAllText(jsonPath);
                var abilities = ParseJSON(jsonContent);

                if (abilities == null || abilities.Count == 0)
                {
                    Debug.LogError("[AbilityDataImporter] No abilities found in JSON file");
                    return;
                }

                // 确保目标目录存在
                EnsureDirectoryExists(ASSET_BASE_PATH);

                // 导入所有 abilities
                int successCount = 0;
                int skippedCount = 0;

                foreach (var abilityJson in abilities)
                {
                    bool success = ImportAbility(abilityJson, out bool skipped);
                    if (success)
                        successCount++;
                    if (skipped)
                        skippedCount++;
                }

                // 刷新资产数据库
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // 显示结果
                string message = $"导入完成!\n成功: {successCount}\n跳过: {skippedCount}";
                EditorUtility.DisplayDialog("AbilityData Import", message, "OK");
                Debug.Log($"[AbilityDataImporter] {message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AbilityDataImporter] Import failed: {ex.Message}");
                EditorUtility.DisplayDialog("Import Error", $"Failed to import: {ex.Message}", "OK");
            }
        }

        // 解析 JSON 为 AbilityDataJson 列表
        private static List<AbilityDataJson> ParseJSON(string jsonContent)
        {
            var wrapper = JsonUtility.FromJson<AbilityListWrapper>("{\"abilities\":" + jsonContent + "}");
            return wrapper.abilities;
        }

        // 导入单个 ability
        private static bool ImportAbility(AbilityDataJson abilityJson, out bool skipped)
        {
            skipped = false;
            string assetPath = $"{ASSET_BASE_PATH}/{abilityJson.id}.asset";

            // 检查文件是否已存在
            var existingAsset = AssetDatabase.LoadAssetAtPath<AbilityData>(assetPath);
            if (existingAsset != null)
            {
                bool overwrite = EditorUtility.DisplayDialog(
                    "Asset Already Exists",
                    $"AbilityData asset for ID {abilityJson.id} already exists.\nOverwrite?",
                    "Yes", "No");

                if (!overwrite)
                {
                    Debug.Log($"[AbilityDataImporter] Skipped ability ID {abilityJson.id}");
                    skipped = true;
                    return false;
                }
            }

            try
            {
                // 创建或更新 AbilityData
                AbilityData abilityData;
                if (existingAsset != null)
                {
                    abilityData = existingAsset;
                }
                else
                {
                    abilityData = ScriptableObject.CreateInstance<AbilityData>();
                }

                // 设置元数据
                abilityData.Id = abilityJson.id;
                abilityData.Name = abilityJson.name;
                abilityData.Desc = abilityJson.desc;
                abilityData.CostEffectID = abilityJson.costEffectID;
                abilityData.CoolDownEffectID = abilityJson.coolDownEffectID;
                abilityData.TimelineID = abilityJson.timelineID;

                // 解析 TargetType
                abilityData.TargetType = ParseTargetType(abilityJson.targetType);

                // 创建 Tracks
                var serializedTracks = CreateSerializedTracks(abilityJson.tracks);
                abilityData.SetTracks(serializedTracks);

                // 计算 TimelineDuration
                float duration = CalculateTimelineDuration(abilityJson.tracks);
                abilityData.TimelineDuration = duration;

                // 保存资产
                if (existingAsset == null)
                {
                    AssetDatabase.CreateAsset(abilityData, assetPath);
                }
                else
                {
                    EditorUtility.SetDirty(abilityData);
                }

                // 统计信息
                int clipCount = 0;
                foreach (var track in serializedTracks)
                {
                    clipCount += track.Clips.Count;
                }

                Debug.Log($"[AbilityDataImporter] Created asset: {assetPath} (ID={abilityJson.id}, Tracks={serializedTracks.Count}, Clips={clipCount})");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[AbilityDataImporter] Failed to import ability ID {abilityJson.id}: {ex.Message}");
                return false;
            }
        }

        // 解析 TargetType 枚举
        private static AbilityTargetType ParseTargetType(string targetTypeStr)
        {
            if (string.IsNullOrEmpty(targetTypeStr))
                return AbilityTargetType.Enemy;

            try
            {
                return (AbilityTargetType)Enum.Parse(typeof(AbilityTargetType), targetTypeStr);
            }
            catch
            {
                Debug.LogWarning($"[AbilityDataImporter] Failed to parse TargetType: {targetTypeStr}, using default Enemy");
                return AbilityTargetType.Enemy;
            }
        }

        // 创建 SerializedTrackData 列表
        private static List<SerializedTrackData> CreateSerializedTracks(List<TrackJson> tracksJson)
        {
            var tracks = new List<SerializedTrackData>();

            if (tracksJson == null || tracksJson.Count == 0)
                return tracks;

            foreach (var trackJson in tracksJson)
            {
                var track = new SerializedTrackData
                {
                    TrackName = trackJson.trackName,
                    TrackColor = Color.cyan,
                    IsEnabled = trackJson.isEnabled,
                    Clips = new List<TimelineClipData>()
                };

                // 创建 EffectClipData
                if (trackJson.clips != null)
                {
                    foreach (var clipJson in trackJson.clips)
                    {
                        var clip = new EffectClipData();
                        clip.EffectId = clipJson.effectId;
                        clip.TriggerTime = clipJson.triggerTime;
                        clip.ClipName = clipJson.clipName;
                        clip.IsEnabled = true;
                        clip.StackCount = 1;

                        track.Clips.Add(clip);
                    }
                }

                tracks.Add(track);
            }

            return tracks;
        }

        // 计算 TimelineDuration
        private static float CalculateTimelineDuration(List<TrackJson> tracksJson)
        {
            float maxTime = 0f;

            if (tracksJson != null)
            {
                foreach (var track in tracksJson)
                {
                    if (track.clips != null)
                    {
                        foreach (var clip in track.clips)
                        {
                            if (clip.triggerTime > maxTime)
                                maxTime = clip.triggerTime;
                        }
                    }
                }
            }

            // 添加 1 秒缓冲，最小 5 秒
            return Mathf.Max(maxTime + 1.0f, 5.0f);
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

        // JSON 数据结构
        [Serializable]
        private class AbilityListWrapper
        {
            public List<AbilityDataJson> abilities;
        }

        [Serializable]
        private class AbilityDataJson
        {
            public int id;
            public string name;
            public string desc;
            public int costEffectID;
            public int coolDownEffectID;
            public string targetType;
            public int timelineID;
            public List<TrackJson> tracks;
        }

        [Serializable]
        private class TrackJson
        {
            public string trackName;
            public bool isEnabled;
            public List<ClipJson> clips;
        }

        [Serializable]
        private class ClipJson
        {
            public int effectId;
            public float triggerTime;
            public string clipName;
        }
    }
}
