using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityGameFramework.Runtime;

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
            EditorSceneManager.OpenScene( "Assets/AbilityEditor/AbilityEditorEntry.unity" );
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        private static void OnSceneOpened( UnityEngine.SceneManagement.Scene scene, OpenSceneMode mode )
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            switch ( scene.name )
            {
                case "Start":
                {

                }
                break;

                case "AbilityEditorEntry":
                    //ConfigureAbilityEditorProcedure();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 配置技能编辑其场景流程
        /// </summary>
        private static void ConfigureAbilityEditorProcedure()
        {
            ProcedureComponent procedureComponent = FindObjectOfType<ProcedureComponent>(true);
            if ( procedureComponent != null )
            {
                Debug.Log( $"Found ProcedureComponent on GameObject: {procedureComponent.gameObject.name}" );
                SerializedObject serializedObject = new SerializedObject( procedureComponent );

                // 获取 m_AvailableProcedureTypeNames 属性
                SerializedProperty availableProcedures = serializedObject.FindProperty( "m_AvailableProcedureTypeNames" );

                Debug.Log( $"Current available procedures count: {availableProcedures.arraySize}" );

                // 清空现有的 procedures
                availableProcedures.ClearArray();
                availableProcedures.InsertArrayElementAtIndex( 0 );
                availableProcedures.GetArrayElementAtIndex( 0 ).stringValue = "Aquila.Procedure.Procedure_EnterAbilityEditorSandBox";

                availableProcedures.InsertArrayElementAtIndex( 1 );
                availableProcedures.GetArrayElementAtIndex( 1 ).stringValue = "Aquila.Procedure.Procedure_RunningAbilityEditorSandBox";

                SerializedProperty entranceProcedure = serializedObject.FindProperty( "m_EntranceProcedureTypeName" );
                entranceProcedure.stringValue = "Aquila.Procedure.Procedure_RunningAbilityEditorSandBox";
                serializedObject.ApplyModifiedProperties();

                Debug.Log( $"Configured procedures. New count: {availableProcedures.arraySize}, Entrance: {entranceProcedure.stringValue}" );

                UnityEngine.SceneManagement.Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                EditorSceneManager.MarkSceneDirty( activeScene );

                Debug.Log( "ProcedureComponent configured for AbilityEditorEntry scene." );
            }
            else
            {
                Debug.LogError( "ProcedureComponent not found in AbilityEditorEntry scene!" );
            }
        }
    }
}
