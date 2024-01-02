using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class AbilityEditorWindow : EditorWindow
{
    private void InitWindow()
    {
        _thisWindow = GetWindow<AbilityEditorWindow>();
        _thisWindow.minSize = new Vector2(860, 700);
        _thisWindow.maxSize = new Vector2(860, 700);
    }
    
    private void InitObjects()
    {
        var tempGroup = rootVisualElement.Q<GroupBox>("buttonArean");
        _saveButton = tempGroup.Q<Button>("saveButton");
        _exportButton = tempGroup.Q<Button>("exportButton");

    }

    private void InitRoot()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(@"Assets/Script/Editor/AbilityEditor/AbilityEditorWindow_UXML.uxml");
        var uxml = visualTree.Instantiate();
        rootVisualElement.Add(uxml);
    }
    
    public void CreateGUI()
    {
        InitWindow();
        InitRoot();
        InitObjects();
    }

    private Button _saveButton = null;
    private Button _exportButton = null;
    private GroupBox _graphViewArea = null;
    private GroupBox _abilityInfoArea = null;
    private GroupBox _buttonArea = null;
    private EditorWindow _thisWindow = null;

    [MenuItem("Aquila/Ability/AbilityEditor")]
    public static void ShowExample()
    {
        AbilityEditorWindow wnd = GetWindow<AbilityEditorWindow>();
        wnd.titleContent = new GUIContent("AbilityEditorWindow");
        //wnd.Show();
    }
}