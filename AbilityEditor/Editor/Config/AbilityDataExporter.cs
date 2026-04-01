using System.Collections.Generic;
using System.IO;
using Aquila.AbilityEditor;
using Aquila.AbilityEditor.Config;
using Aquila.Procedure;
using Editor.AbilityEditor.Tools;
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
            Aquila.Toolkit.Tools.Logger.Info($"[AbilityDataExporter] {action}配置资产: {assetPath}");
        }

        /// <summary>
        /// 导出配置为沙盒测试用的 .ablt 和 .efct 二进制文件（固定路径和文件名）
        /// </summary>
        public static void ExportToSandBox(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            EnsureDirectoryExists(Procedure_EnterAbilityEditorSandBox.SANDBOX_ABILITY_PATH);
            ClearDirectory(Procedure_EnterAbilityEditorSandBox.SANDBOX_ABILITY_PATH);
            
            // 创建临时 AbilityEditorSOData 用于导出
            AbilityEditorSOData abilityData = CreateAbilityData(config, tracks);
            string abilityFileName = Path.GetFileNameWithoutExtension("sand_box");
            string abltPath = Path.Combine(Procedure_EnterAbilityEditorSandBox.SANDBOX_ABILITY_PATH, $"{abilityFileName}.ablt");
            AbilityBinaryExporter.ExportAbility(abilityData, abltPath);
            ExportEffectClipsToSandBox(abilityData);
            AssetDatabase.Refresh();
            Aquila.Toolkit.Tools.Logger.Info($"[AbilityDataExporter] 已导出沙盒测试配置: {abltPath}");
        }

        /// <summary>
        /// 导出技能中的所有 Effect Clips 为 .efct 文件到沙盒目录
        /// </summary>
        private static void ExportEffectClipsToSandBox(AbilityEditorSOData abilityData)
        {
            if (abilityData.Tracks == null)
                return;

            HashSet<int> exportedEffectIds = new HashSet<int>();

            foreach (var track in abilityData.Tracks)
            {
                if (track.Clips == null)
                    continue;

                foreach (var clip in track.Clips)
                {
                    if (clip is EffectClipData effectClip && effectClip.EffectId > 0)
                    {
                        if (exportedEffectIds.Contains(effectClip.EffectId))
                            continue;

                        exportedEffectIds.Add(effectClip.EffectId);
                        string efctPath = Path.Combine(Procedure_EnterAbilityEditorSandBox.SANDBOX_ABILITY_PATH, $"{effectClip.EffectId}.efct");
                        EffectBinaryExporter.ExportEffect(effectClip, efctPath);
                    }
                }
            }

            // 导出 Cost Effect 和 CoolDown Effect
            ExportEffectSODataById(abilityData.CostEffectID, exportedEffectIds);
            ExportEffectSODataById(abilityData.CoolDownEffectID, exportedEffectIds);

            if (exportedEffectIds.Count > 0)
            {
                Aquila.Toolkit.Tools.Logger.Info($"[AbilityDataExporter] 已导出 {exportedEffectIds.Count} 个 Effect 到沙盒目录");
            }
        }

        /// <summary>
        /// 按 ID 从 EFFECT_ASSET_BASE_PATH 查找并导出对应的 EffectEditorSOData 到沙盒目录
        /// </summary>
        private static void ExportEffectSODataById(int effectId, HashSet<int> exportedEffectIds)
        {
            if (effectId <= 0 || exportedEffectIds.Contains(effectId))
                return;

            string[] guids = AssetDatabase.FindAssets("t:EffectEditorSOData", new[] { Misc.EFFECT_ASSET_BASE_PATH });
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var effectData = AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(assetPath);
                if (effectData == null || effectData.id != effectId)
                    continue;

                exportedEffectIds.Add(effectId);
                string efctPath = Path.Combine(Procedure_EnterAbilityEditorSandBox.SANDBOX_ABILITY_PATH, $"{effectId}.efct");
                EffectBinaryExporter.ExportEffect(effectData, efctPath);
                return;
            }

            Aquila.Toolkit.Tools.Logger.Warning($"[AbilityDataExporter] 未找到 Effect ID={effectId} 的资产文件");
        }

        /// <summary>
        /// 从 EffectClipData 创建 EffectEditorSOData
        /// </summary>
        private static void CreateEffectDataFromClip(EffectClipData clip,EffectEditorSOData effectData)
        {
            //var effectData = ScriptableObject.CreateInstance<EffectEditorSOData>();
            effectData.id = clip.EffectId;
            effectData.Type = clip.EffectType;
            effectData.ModifierType = clip.ModifierType;
            effectData.AffectedAttribute = clip.AffectedAttribute;
            effectData.Target = clip.Target;
            effectData.Duration = clip.Duration;
            effectData.Period = clip.Period;
            effectData.Policy = clip.Policy;
            effectData.EffectOnAwake = clip.EffectOnAwake;
            effectData.DeriveEffects = clip.DeriveEffects;
            effectData.AwakeEffects = clip.AwakeEffects;

            // 复制扩展参数
            var extParam = clip.ExtensionParam;
            effectData.ExtensionParam = new EffectExtensionParam
            {
                float_1 = extParam.FloatParam_1,
                float_2 = extParam.FloatParam_2,
                float_3 = extParam.FloatParam_3,
                float_4 = extParam.FloatParam_4,
                int_1 = extParam.IntParam_1,
                int_2 = extParam.IntParam_2,
                int_3 = extParam.IntParam_3,
                int_4 = extParam.IntParam_4
            };

            //return effectData;
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

        // 清空目录下所有文件（保留目录本身）
        private static void ClearDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path))
                File.Delete(file);
        }
    }
}
