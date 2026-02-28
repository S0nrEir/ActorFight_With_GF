using System.IO;
using Aquila.AbilityEditor;
using Aquila.AbilityEditor.Config;
using Editor.AbilityEditor.Tools;
using UnityEditor;
using UnityEngine;

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
            effectData.EffectType = effectClip.AffectedAttribute;
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
    }
}
