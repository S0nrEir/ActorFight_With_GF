using UnityEditor;
using UnityEngine;
using Aquila.AbilityEditor.Config;

namespace Aquila.AbilityEditor.Tools
{
    /// <summary>
    /// Effect数据浏览器 - 直接在Inspector中显示
    /// </summary>
    public class EffectDataBrowser
    {
        private static EffectEditorSOData _tempEffectData;

        [MenuItem("Aquila/AbilityEditor/EffectDataInspector")]
        public static void ShowInInspector()
        {
            // 如果已经有选中的EffectData，就用选中的
            if (Selection.activeObject is EffectEditorSOData selectedEffect)
            {
                _tempEffectData = selectedEffect;
                EditorGUIUtility.PingObject(_tempEffectData);
            }
            // 否则创建一个新的临时对象
            else
            {
                if (_tempEffectData == null)
                {
                    _tempEffectData = ScriptableObject.CreateInstance<EffectEditorSOData>();
                    _tempEffectData.name = "New Effect Data";
                }
                Selection.activeObject = _tempEffectData;
            }
        }
    }

    /// <summary>
    /// EffectData的自定义Inspector
    /// </summary>
    [CustomEditor(typeof(EffectEditorSOData))]
    public class EffectDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var effectData = (EffectEditorSOData)target;
            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.Space(5);
            GUI.backgroundColor = new Color(0.5f, 0.8f, 0.5f);
            if (GUILayout.Button("Create New EffectData Asset", GUILayout.Height(30)))
                CreateNewAsset(effectData);

            GUI.backgroundColor = Color.white;
        }

        private void CreateNewAsset(EffectEditorSOData sourceData)
        {
            // if (!AssetDatabase.IsValidFolder(folderPath))
            // {
            //     EditorUtility.DisplayDialog("Error", $"Folder not found: {folderPath}", "OK");
            //     return;
            // }

            string fileName = $"{sourceData.id}.asset";
            string fullPath = $"{Misc.NEW_EFFECT_DATA_PATH}/{fileName}";

            if (AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(fullPath) != null)
            {
                EditorUtility.DisplayDialog("File Exists",
                    $"EffectData with ID {sourceData.id} already exists at:\n{fullPath}\n\nPlease use a different ID.",
                    "OK");
                return;
            }

            var newData = ScriptableObject.CreateInstance<EffectEditorSOData>();

            newData.id = sourceData.id;
            newData.Description = sourceData.Description;
            newData.Type = sourceData.Type;

            if (sourceData.ExtensionParam != null)
            {
                newData.ExtensionParam = new EffectExtensionParam
                {
                    float_1 = sourceData.ExtensionParam.float_1,
                    float_2 = sourceData.ExtensionParam.float_2,
                    float_3 = sourceData.ExtensionParam.float_3,
                    float_4 = sourceData.ExtensionParam.float_4,
                    int_1 = sourceData.ExtensionParam.int_1,
                    int_2 = sourceData.ExtensionParam.int_2,
                    int_3 = sourceData.ExtensionParam.int_3,
                    int_4 = sourceData.ExtensionParam.int_4
                };
            }

            newData.ModifierType = sourceData.ModifierType;
            newData.EffectOnAwake = sourceData.EffectOnAwake;
            newData.Policy = sourceData.Policy;
            newData.Period = sourceData.Period;
            newData.Duration = sourceData.Duration;
            newData.Target = sourceData.Target;
            newData.AffectedAttribute = sourceData.AffectedAttribute;

            if (sourceData.DeriveEffects != null)
                newData.DeriveEffects = (int[])sourceData.DeriveEffects.Clone();

            if (sourceData.AwakeEffects != null)
                newData.AwakeEffects = (int[])sourceData.AwakeEffects.Clone();

            AssetDatabase.CreateAsset(newData, fullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = newData;
            EditorGUIUtility.PingObject(newData);

            Debug.Log($"<color=green>Created new EffectData at: {fullPath}</color>");
            EditorUtility.DisplayDialog("Success", $"Created new EffectData:\n{fullPath}", "OK");
        }
    }
}
