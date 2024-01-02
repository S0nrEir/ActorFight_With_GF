using System;
using System.Drawing;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Vector2 = UnityEngine.Vector2;


public class AbilityEditorWindow : EditorWindow
{
    private void OnEnable()
    {
        _thisWindow = GetWindow<AbilityEditorWindow>();
        _thisWindow.minSize = _windowMinSize;
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(_windowMinSize.x),GUILayout.Height(_windowMinSize.y));
        {
            //graph view area
            var rect = EditorGUILayout.BeginVertical("box",new GUILayoutOption[]{GUILayout.Width(_windowMinSize.x * 0.7f)});
            EditorGUILayout.LabelField("Impact Nodes");
            DrawGraphViewArea();
            EditorGUILayout.EndVertical();
            
            //ability&buttons
            EditorGUILayout.BeginVertical(GUILayout.Width(_windowMinSize.x * 0.3f));
            {
                DrawAbilityBaseArea();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawGraphViewArea()
    {
        EditorGUILayout.LabelField("test information");
        EditorGUILayout.LabelField("test information");
        EditorGUILayout.LabelField("test information");
        EditorGUILayout.LabelField("test information");
    }

    private void DrawAbilityBaseArea()
    {
        EditorGUILayout.TextField(new GUIContent("AbilityBaseID:"),"");
        EditorGUILayout.TextField(new GUIContent("AbilityBaseID:"),"");
        EditorGUILayout.TextField(new GUIContent("AbilityBaseID:"),"");
        EditorGUILayout.TextField(new GUIContent("AbilityBaseID:"),"");
    }
    
    private EditorWindow _thisWindow = null;
    private static Vector2 _windowMinSize = new Vector2(860, 700);

    [MenuItem("Aquila/Ability/AbilityEditor")]
    public static void ShowExample()
    {
        AbilityEditorWindow wnd = GetWindow<AbilityEditorWindow>();
        wnd.titleContent = new GUIContent("AbilityEditorWindow");
        //wnd.Show();
    }
}