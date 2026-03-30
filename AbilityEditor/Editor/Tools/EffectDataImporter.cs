using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Aquila.AbilityEditor.Config;
using Cfg.Enum;
using UnityEditor;
using UnityEngine;

namespace Aquila.AbilityEditor.Tools
{
    /// <summary>
    /// Effect数据导入器
    /// 从Excel文件通过Python脚本生成JSON，然后创建ScriptableObject资源
    /// </summary>
    public static class EffectDataImporter
    {
        private const string PYTHON_SCRIPT = "Tools/generate_effect_json.py";
        private const string EXCEL_PATH = "DataTable/designer_configs/Datas/Effect.xlsx";
        private const string TEMP_JSON_PATH = "Temp/effect_data.json";

        [MenuItem("Aquila/AbilityEditor/Import Effects from Excel")]
        public static void ImportEffectsFromExcel()
        {
            try
            {
                Toolkit.Tools.Logger.Info("Step 1: Running Python script to generate JSON...");
                if (!RunPythonScript())
                {
                    Toolkit.Tools.Logger.Error("Failed to run Python script!");
                    return;
                }

                Toolkit.Tools.Logger.Info("Step 2: Reading JSON file...");
                if (!File.Exists(TEMP_JSON_PATH))
                {
                    Toolkit.Tools.Logger.Error($"JSON file not found: {TEMP_JSON_PATH}");
                    return;
                }

                string jsonContent = File.ReadAllText(TEMP_JSON_PATH);
                EffectDataList effectList = JsonUtility.FromJson<EffectDataList>("{\"effects\":" + jsonContent + "}");

                if (effectList == null || effectList.effects == null)
                {
                    Toolkit.Tools.Logger.Error("Failed to parse JSON!");
                    return;
                }

                Toolkit.Tools.Logger.Info($"Step 3: Creating ScriptableObject assets for {effectList.effects.Count} effects...");
                CreateEffectAssets(effectList.effects);

                Toolkit.Tools.Logger.Info("<color=green>Import completed successfully!</color>");
            }
            catch (Exception ex)
            {
                Toolkit.Tools.Logger.Error($"Import failed: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private static bool RunPythonScript()
        {
            try
            {
                Directory.CreateDirectory("Temp");
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = $"{PYTHON_SCRIPT} {EXCEL_PATH} {TEMP_JSON_PATH}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Application.dataPath.Replace("/Assets", "")
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        Toolkit.Tools.Logger.Error("Failed to start Python process!");
                        return false;
                    }

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(output))
                        Toolkit.Tools.Logger.Info($"Python output:\n{output}");

                    if (!string.IsNullOrEmpty(error))
                        Toolkit.Tools.Logger.Warning($"Python stderr:\n{error}");

                    if (process.ExitCode != 0)
                    {
                        Toolkit.Tools.Logger.Error($"Python script exited with code {process.ExitCode}");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Toolkit.Tools.Logger.Error($"Failed to run Python script: {ex.Message}");
                return false;
            }
        }

        private static void CreateEffectAssets(List<EffectDataJson> effectJsonList)
        {
            // if (!Directory.Exists(OUTPUT_DIR))
            // {
            //     Directory.CreateDirectory(OUTPUT_DIR);
            //     Aquila.Toolkit.Tools.Logger.Info($"Created directory: {OUTPUT_DIR}");
            // }

            int successCount = 0;
            int failCount = 0;

            foreach (var effectJson in effectJsonList)
            {
                try
                {
                    string assetPath = $"{Misc.NEW_EFFECT_DATA_PATH}/{effectJson.id}.asset";

                    var effectData = AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(assetPath);
                    bool isNewAsset = effectData == null;

                    if (isNewAsset)
                        effectData = ScriptableObject.CreateInstance<EffectEditorSOData>();

                    // 复制数据
                    effectData.id = effectJson.id;
                    effectData.Description = effectJson.Description ?? "";
                    effectData.Type = (EffectType)effectJson.Type;
                    effectData.ModifierType = (NumricModifierType)effectJson.ModifierType;
                    effectData.EffectOnAwake = effectJson.EffectOnAwake;
                    effectData.Policy = (DurationPolicy)effectJson.Policy;
                    effectData.Period = effectJson.Period;
                    effectData.Duration = effectJson.Duration;
                    effectData.Target = effectJson.Target;
                    effectData.AffectedAttribute = (actor_attribute)effectJson.EffectType;
                    effectData.DeriveEffects = effectJson.DeriveEffects ?? new int[0];
                    effectData.AwakeEffects = effectJson.AwakeEffects ?? new int[0];

                    // 复制扩展参数
                    if (effectJson.ExtensionParam != null)
                    {
                        effectData.ExtensionParam = new EffectExtensionParam
                        {
                            float_1 = effectJson.ExtensionParam.float_1,
                            float_2 = effectJson.ExtensionParam.float_2,
                            float_3 = effectJson.ExtensionParam.float_3,
                            float_4 = effectJson.ExtensionParam.float_4,
                            int_1 = effectJson.ExtensionParam.int_1,
                            int_2 = effectJson.ExtensionParam.int_2,
                            int_3 = effectJson.ExtensionParam.int_3,
                            int_4 = effectJson.ExtensionParam.int_4
                        };
                    }

                    // 保存资源
                    if (isNewAsset)
                    {
                        AssetDatabase.CreateAsset(effectData, assetPath);
                        Toolkit.Tools.Logger.Info($"Created: {assetPath}");
                    }
                    else
                    {
                        EditorUtility.SetDirty(effectData);
                        Toolkit.Tools.Logger.Info($"Updated: {assetPath}");
                    }

                    successCount++;
                }
                catch (Exception ex)
                {
                    Toolkit.Tools.Logger.Error($"Failed to create asset for Effect ID {effectJson.id}: {ex.Message}");
                    failCount++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Toolkit.Tools.Logger.Info($"<color=cyan>Asset Creation Summary:</color> Success: {successCount}, Failed: {failCount}");
            Toolkit.Tools.Logger.Info($"Assets saved to: {Misc.NEW_EFFECT_DATA_PATH}");
        }

        [Serializable]
        private class EffectDataList
        {
            public List<EffectDataJson> effects;
        }

        [Serializable]
        private class EffectDataJson
        {
            public int id;
            public string Description;
            public int Type;
            public ExtensionParamJson ExtensionParam;
            public int ModifierType;
            public bool EffectOnAwake;
            public int Policy;
            public float Period;
            public float Duration;
            public int Target;
            public int EffectType;
            public int[] DeriveEffects;
            public int[] AwakeEffects;
        }

        [Serializable]
        private class ExtensionParamJson
        {
            public float float_1;
            public float float_2;
            public float float_3;
            public float float_4;
            public int int_1;
            public int int_2;
            public int int_3;
            public int int_4;
        }
    }
}
