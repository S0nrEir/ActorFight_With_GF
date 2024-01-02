using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace  Aquila.Editor
{
    
    /// <summary>
    /// 技能编辑器界面
    /// </summary>
    public class AbilityEditorWindow : EditorWindow
    {
        //---------------------周期回调
        public void CreateGUI()
        {
            _root = rootVisualElement;
            InitObjects();
        }

        private void InitObjects()
        {
            _exportButton = _root.Q<Button>("export_button");
            _saveButton = _root.Q<Button>("save_button");
            _thisWindow = GetWindow<EditorWindow>();
        }

        //---------------------fields

        /// <summary>
        /// 窗体节点
        /// </summary>
        private VisualElement _root = null;
        
        /// <summary>
        /// 导出
        /// </summary>
        private Button _exportButton = null;
        
        /// <summary>
        /// 保存
        /// </summary>
        private Button _saveButton = null;

        /// <summary>
        /// 窗体对象
        /// </summary>
        private EditorWindow _thisWindow = null;
        
        /// <summary>
        /// 打开技能编辑器窗口
        /// </summary>
        [MenuItem("Aquila/Ability/Ability Editor")]
        public static void OpenAbilityEditorWindow()
        {
            AbilityEditorWindow window = GetWindow<AbilityEditorWindow>();
        }
    }
}
