using System.IO;
using Aquila.AbilityEditor;
using Aquila.AbilityEditor.Config;
using Editor.AbilityEditor.Tools;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Editor.AbilityEditor.Inspector
{
    /// <summary>
    /// AbilityData的自定义Inspector
    /// </summary>
    [CustomEditor(typeof(AbilityEditorSOData))]
    public class AbilityDataInspector : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            var abilityData = (AbilityEditorSOData)target;

            // Quick Add Effect Clip Section
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Quick Add Effect Clip", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Effect ID:", GUILayout.Width(70));
            _effectIdToAdd = EditorGUILayout.IntField(_effectIdToAdd, GUILayout.Width(80));
            
            EditorGUILayout.LabelField("Trigger Time:", GUILayout.Width(80));
            _triggerTime = EditorGUILayout.FloatField(_triggerTime, GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Add Effect Clip", GUILayout.Height(25)))
                AddEffectClipById(abilityData, _effectIdToAdd, _triggerTime);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Export Buttons
            if (GUILayout.Button("Export to Binary (.ablt)", GUILayout.Height(30)))
                ExportToBinary(abilityData);

            if (GUILayout.Button("Export All Effects to EffectData", GUILayout.Height(30)))
                ExportEffectsToAssets(abilityData);
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            DrawDefaultInspector();
        }

        private void ExportToBinary(AbilityEditorSOData data)
        {
            if (!data.Validate(out string error))
            {
                EditorUtility.DisplayDialog("Export Failed", $"Validation failed: {error}", "OK");
                return;
            }

            EnsureDirectoryExists(Misc.ABILITY_BIN_ASSET_PATH);
            string outputPath = Path.Combine(Misc.ABILITY_BIN_ASSET_PATH, $"{data.Id}.ablt");

            AbilityBinaryExporter.ExportAbility(data, outputPath);
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Export Success", $"Exported to:\n{outputPath}", "OK");
        }

        private void ExportEffectsToAssets(AbilityEditorSOData data)
        {
            if (data.Tracks == null || data.Tracks.Count == 0)
            {
                EditorUtility.DisplayDialog("Export Failed", "No tracks found in AbilityData", "OK");
                return;
            }

            EnsureDirectoryExists(Misc.NEW_EFFECT_DATA_PATH);

            int successCount = 0;
            int skipCount = 0;

            foreach (var track in data.Tracks)
            {
                if (track.Clips == null)
                    continue;

                foreach (var clip in track.Clips)
                {
                    if (clip is EffectClipData effectClip)
                    {
                        if (effectClip.EffectId <= 0)
                        {
                            skipCount++;
                            continue;
                        }

                        ExportEffectClipToAsset(effectClip);
                        successCount++;
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Export Complete", $"Exported: {successCount} effects\nSkipped: {skipCount} (invalid ID)\nPath: {Misc.NEW_EFFECT_DATA_PATH}", "OK");
        }

        private void ExportEffectClipToAsset(EffectClipData effectClip)
        {
            string assetPath = $"{Misc.NEW_EFFECT_DATA_PATH}/{effectClip.EffectId}.asset";

            var effectData = AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(assetPath);
            bool isNewAsset = effectData == null;

            if (isNewAsset)
                effectData = ScriptableObject.CreateInstance<EffectEditorSOData>();

            // 从EffectClipData复制数据到EffectData
            effectData.id = effectClip.EffectId;
            effectData.Description = effectClip.ClipName ?? "";
            effectData.Type = effectClip.EffectType;
            effectData.ModifierType = effectClip.ModifierType;
            effectData.EffectOnAwake = effectClip.EffectOnAwake;
            effectData.Policy = effectClip.Policy;
            effectData.Period = effectClip.Period;
            effectData.Duration = effectClip.Duration;
            effectData.Target = effectClip.Target;
            effectData.AffectedAttribute = effectClip.AffectedAttribute;
            effectData.DeriveEffects = effectClip.DeriveEffects ?? new int[0];
            effectData.AwakeEffects = effectClip.AwakeEffects ?? new int[0];

            // 复制扩展参数
            if (effectClip.ExtensionParam != null)
            {
                effectData.ExtensionParam = new EffectExtensionParam
                {
                    float_1 = effectClip.ExtensionParam.FloatParam_1,
                    float_2 = effectClip.ExtensionParam.FloatParam_2,
                    float_3 = effectClip.ExtensionParam.FloatParam_3,
                    float_4 = effectClip.ExtensionParam.FloatParam_4,
                    int_1 = effectClip.ExtensionParam.IntParam_1,
                    int_2 = effectClip.ExtensionParam.IntParam_2,
                    int_3 = effectClip.ExtensionParam.IntParam_3,
                    int_4 = effectClip.ExtensionParam.IntParam_4
                };
            }

            if (isNewAsset)
            {
                AssetDatabase.CreateAsset(effectData, assetPath);
                Debug.Log($"[AbilityDataInspector] Created: {assetPath}");
            }
            else
            {
                EditorUtility.SetDirty(effectData);
                Debug.Log($"[AbilityDataInspector] Updated: {assetPath}");
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }
        }

        #region Quick Add Effect Clip

        /// <summary>
        /// 根据 Effect ID 添加 Effect Clip 到 Ability
        /// </summary>
        private void AddEffectClipById(AbilityEditorSOData abilityData, int effectId, float triggerTime)
        {
            if (effectId <= 0)
            {
                EditorUtility.DisplayDialog("Invalid Input", "Effect ID must be greater than 0", "OK");
                return;
            }

            // 加载 Effect 数据
            var effectData = LoadEffectData(effectId);
            if (effectData == null)
            {
                EditorUtility.DisplayDialog("Effect Not Found", 
                    $"Could not find Effect asset: Assets/AbilityEditor/Editor/Config/Effects/{effectId}.asset", "OK");
                return;
            }

            // 创建 EffectClipData
            var effectClip = CreateEffectClipFromData(effectData, triggerTime);

            // 确保至少有一个 Track
            var tracks = new System.Collections.Generic.List<SerializedTrackData>();
            if (abilityData.Tracks != null && abilityData.Tracks.Count > 0)
            {
                // 复制现有 tracks
                foreach (var track in abilityData.Tracks)
                {
                    tracks.Add(track);
                }
            }
            else
            {
                // 创建新的 Track
                var newTrack = new SerializedTrackData
                {
                    TrackName = "Effect Track",
                    TrackColor = TrackColors[Random.Range(0, TrackColors.Length)],
                    IsEnabled = true,
                    Clips = new System.Collections.Generic.List<TimelineClipData>()
                };
                tracks.Add(newTrack);
            }

            // 添加 Clip 到第一个 Track
            if (tracks[0].Clips == null)
                tracks[0].Clips = new System.Collections.Generic.List<TimelineClipData>();
            
            tracks[0].Clips.Add(effectClip);

            // 更新 AbilityData
            abilityData.SetTracks(tracks);

            // 标记为脏并保存
            EditorUtility.SetDirty(abilityData);
            AssetDatabase.SaveAssets();

            Debug.Log($"[AbilityDataInspector] Added Effect Clip {effectId} to {abilityData.name} at time {triggerTime}s");
            EditorUtility.DisplayDialog("Success", 
                $"Added Effect Clip {effectId} to track '{tracks[0].TrackName}' at {triggerTime}s", "OK");
        }

        /// <summary>
        /// 加载 Effect 资产数据
        /// </summary>
        private EffectEditorSOData LoadEffectData(int effectId)
        {
            string assetPath = $"Assets/AbilityEditor/Editor/Config/Effects/{effectId}.asset";
            return AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(assetPath);
        }

        /// <summary>
        /// 从 EffectEditorSOData 创建 EffectClipData
        /// </summary>
        private EffectClipData CreateEffectClipFromData(EffectEditorSOData effectData, float triggerTime)
        {
            var clip = new EffectClipData
            {
                ClipName = $"Effect_{effectData.id}",
                TriggerTime = triggerTime,
                ClipColor = new Color(0.8f, 0.4f, 0.8f, 1f), // 紫色
                IsEnabled = true,

                // 映射 Effect 数据
                EffectId = effectData.id,
                EffectType = effectData.Type,
                ModifierType = effectData.ModifierType,
                AffectedAttribute = effectData.AffectedAttribute,
                Target = effectData.Target,
                Duration = effectData.Duration,
                Period = effectData.Period,
                Policy = effectData.Policy,
                EffectOnAwake = effectData.EffectOnAwake,
                StackCount = 1,
                CanStack = false
            };

            // 复制扩展参数
            if (effectData.ExtensionParam != null)
            {
                clip.ExtensionParam = new EffectClipData.EffectExtensionParam
                {
                    FloatParam_1 = effectData.ExtensionParam.float_1,
                    FloatParam_2 = effectData.ExtensionParam.float_2,
                    FloatParam_3 = effectData.ExtensionParam.float_3,
                    FloatParam_4 = effectData.ExtensionParam.float_4,
                    IntParam_1 = effectData.ExtensionParam.int_1,
                    IntParam_2 = effectData.ExtensionParam.int_2,
                    IntParam_3 = effectData.ExtensionParam.int_3,
                    IntParam_4 = effectData.ExtensionParam.int_4
                };
            }

            // 复制数组字段
            clip.DeriveEffects = effectData.DeriveEffects != null ? 
                (int[])effectData.DeriveEffects.Clone() : new int[0];
            clip.AwakeEffects = effectData.AwakeEffects != null ? 
                (int[])effectData.AwakeEffects.Clone() : new int[0];

            return clip;
        }

        #endregion
        
        #region fields
        private int _effectIdToAdd = 0;
        private float _triggerTime = 0.0f;
        // 预定义的轨道颜色
        private static readonly Color[] TrackColors = new Color[]
        {
            new Color(0.8867924f, 0.4475792f, 0.4475792f, 1f), // 红色
            new Color(0.8f, 0.8f, 0f, 1f),                      // 黄色
            new Color(0.4475792f, 0.8867924f, 0.4475792f, 1f), // 绿色
            new Color(0.4475792f, 0.4475792f, 0.8867924f, 1f), // 蓝色
            new Color(0.8867924f, 0.4475792f, 0.8867924f, 1f), // 紫色
        };
        #endregion
    }
}
