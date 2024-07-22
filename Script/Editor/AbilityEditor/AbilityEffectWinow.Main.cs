using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.Editor
{
    public class AbilityEffectGroupEditorWidnow : EditorWindow
    {
        //-----------pub-----------
        public void RefreshByEffectNode(AbilityEditorEffectGroupNode node)
        {
            // selectedPorts.Clear();
            if (node is null)
            {
                Debug.Log($"AbilityEditorWindow.Main.cs: node is null.");
                return;
            }

            if (node is AbilityEditorNode_StartNode)
            {
                Debug.Log($"AbilityEditorWindow.Main.cs: node is AbilityEditorNode_StartNode.");
                return;
            }

            selectedNode = node;
            if(_effects is null)
                _effects = new List<AbilityEffect>();

            _effects = EffectDataMgr.GetEffects(node);
            // selectedPorts.AddRange(node.GetAllPorts());
        }
        
        //-----------priv-----------

        /// <summary>
        /// 按下toolBar的add button
        /// </summary>
        private void OnClickToolBarAdd()
        {
            if (selectedNode is null)
            {
                Debug.LogError($"AbilityEffectWindow.Main.cs: selectedNode is null.");
                return;
            }
            
            
        }

        //-----------mono-----------

        private void OnGUI()
        {
            if (selectedNode is null /*|| selectedPorts.Count == 0*/)
                return;
            
            //trigger time
            EditorGUILayout.BeginVertical();
            {
                _triggerTime = EditorGUILayout.FloatField("Trigger Time:", selectedNode.TriggerTime);
            }
            EditorGUILayout.EndVertical();

            if (_effects.Count == 0)
                return;
            
            //effect data:
            EditorGUILayout.BeginVertical();
            {
                var index = 0;
                foreach (var effect in _effects)
                {
                    EditorGUILayout.LabelField($"Effect-{index}");
                    EditorGUILayout.IntField($"ID", effect.ID);
                    EditorGUILayout.TextField($"Desc", effect.Desc);
                    EditorGUILayout.EnumPopup($"Type", effect.Type);
                    EditorGUILayout.FloatField($"ExtensionFloatParam_1", effect.ExtensionFloatParam_1);
                    EditorGUILayout.FloatField($"ExtensionFloatParam_2", effect.ExtensionFloatParam_2);
                    EditorGUILayout.FloatField($"ExtensionFloatParam_3", effect.ExtensionFloatParam_3);
                    EditorGUILayout.FloatField($"ExtensionFloatParam_4", effect.ExtensionFloatParam_4);
                    EditorGUILayout.TextField($"ExtensionStringParm_1", effect.ExtensionStringParm_1);
                    EditorGUILayout.TextField($"ExtensionStringParm_2", effect.ExtensionStringParm_2);
                    EditorGUILayout.TextField($"ExtensionStringParm_3", effect.ExtensionStringParm_3);
                    EditorGUILayout.TextField($"ExtensionStringParm_4", effect.ExtensionStringParm_4);
                    EditorGUILayout.Toggle($"EffectOnAwake", effect.EffectOnAwake);
                    EditorGUILayout.EnumPopup($"DurationPolicy", effect.DurationPolicy);
                    EditorGUILayout.FloatField($"Period", effect.Period);
                    EditorGUILayout.FloatField($"Duration", effect.Duration);
                    EditorGUILayout.IntField($"Target", effect.Target);
                    EditorGUILayout.EnumPopup($"EffectType", effect.EffectType);
                    EditorGUILayout.TextField($"DeriveEffects", effect.DeriveEffects is null ? string.Empty : ArrayToString(effect.DeriveEffects));
                    EditorGUILayout.TextField($"DeriveEffects", effect.DeriveEffects is null ? string.Empty : ArrayToString(effect.AwakeEffects));
                    EditorGUILayout.Space(10);
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// effect array转字符串表示，用逗号表示
        /// </summary>
        private string ArrayToString(int[] array)
        {
            try
            {
                return string.Join(",", array);
            }
            catch (Exception e)
            {
                throw new Exception($"AbilityEffectWindow.Main.cs: ArrayToString error. {e.Message}");
            }
        }

        private void OnEnable()
        {
            // selectedPorts = null;
            // selectedPorts = new List<AbilityViewPort>();

            _toolBar = new Toolbar();
            var addEffectButton = new Button();
            addEffectButton.text = "Add Effect";
            addEffectButton.clicked += OnClickToolBarAdd;
            _toolBar.Add(addEffectButton);
        }

        private void OnDisable()
        {
            // selectedPorts.Clear();
            // selectedPorts = null;
            selectedNode = null;
            
            _toolBar.Clear();;
            _toolBar = null;
        }

        //-----------fields-----------
        // /// <summary>
        // /// 当前选中节点的端口
        // /// </summary>
        // private List<AbilityViewPort> selectedPorts = null;

        /// <summary>
        /// 当前选中的节点
        /// </summary>
        private AbilityEditorEffectGroupNode selectedNode = null;

        /// <summary>
        /// 选中节点携带的effect
        /// </summary>
        private List<AbilityEffect> _effects = null;

        /// <summary>
        /// 触发时间
        /// </summary>
        private float _triggerTime = 0f;

        /// <summary>
        /// toolBar
        /// </summary>
        private Toolbar _toolBar = null;
    }
}
