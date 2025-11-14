using UnityEditor;
using UnityEditor.SceneManagement;

namespace Aquila.Editor
{
    /// <summary>
    /// 打开场景工具
    /// </summary>
    public class OpenScene : EditorWindow
    {
        [MenuItem("File/OpenScene/Launcer")]
        public static void OpenScene_Start()
        {
            EditorSceneManager.OpenScene(@"Assets/Res/Scene/Start.unity");
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        [MenuItem( "File/OpenScene/AbilityEditor" )]
        public static void OpenScene_AbilityEditor()
        {
            EditorSceneManager.OpenScene( @"Assets/Editor/AbilityEditor/AbilityEditorEntry.unity" );
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneOpened( UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode )
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            switch ( scene.name )
            {
                case "Start":
                    break;

                case "AbilityEditorEntry":
                    break;

                default:
                    break;
            }
        }
    }
}
