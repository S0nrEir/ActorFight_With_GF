using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Aquila.Editor
{
    /// <summary>
    /// 打开启动场景
    /// </summary>
    public class OpenSceneStart : EditorWindow
    {
        [MenuItem("File/OpenScene_Start")]
        public static void OpenSceneStart_()
        {
            EditorSceneManager.OpenScene(@"Assets/Res/Scene/Start.unity");
        }
    }
}
