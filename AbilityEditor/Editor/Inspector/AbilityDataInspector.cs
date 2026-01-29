using System.IO;
using Aquila.AbilityEditor;
using Editor.AbilityEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Inspector
{
    /// <summary>
    /// AbilityData的自定义Inspector
    /// </summary>
    [CustomEditor(typeof(AbilityData))]
    public class AbilityDataInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            var abilityData = (AbilityData)target;

            if (GUILayout.Button("Export to Binary (.ablt)", GUILayout.Height(30)))
                ExportToBinary(abilityData);
        }

        private void ExportToBinary(AbilityData data)
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
