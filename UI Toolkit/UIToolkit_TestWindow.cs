using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class UIToolkit_TestWindow : EditorWindow
{
    [MenuItem("Window/UI Toolkit/UIToolkit_TestWindow")]
    public static void ShowExample()
    {
        UIToolkit_TestWindow wnd = GetWindow<UIToolkit_TestWindow>();
        wnd.titleContent = new GUIContent("UIToolkit_TestWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI Toolkit/UIToolkit_TestWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI Toolkit/UIToolkit_TestWindow.uss");
        VisualElement labelWithStyle = new Label("Hello World! With Style");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);

        var this_window = GetWindow<EditorWindow>();
        this_window.maxSize = new Vector2( 1000f, 1000f );
        this_window.minSize = new Vector2( 1000f, 1000f );

        _test_btn = root.Q<Button>( "test_button" );
        _test_btn.clicked += () =>
        {
            Debug.Log( "clicked!" );
        };
    }

    private Button _test_btn;
}