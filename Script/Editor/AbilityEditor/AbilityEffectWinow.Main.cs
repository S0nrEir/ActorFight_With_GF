using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Cfg.Enum;
using Codice.Client.BaseCommands.TubeClient;
using UnityEditor;
using UnityEditor.Rendering;
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
            _triggerTime = selectedNode.TriggerTime;
            _effects = EffectDataMgr.GetEffects(node);
            _foldOuts = new bool[_effects.Count];
            Repaint();
            // selectedPorts.AddRange(node.GetAllPorts());
        }
        
        //-----------priv-----------

        /// <summary>
        /// 添加effect
        /// </summary>
        private void OnClickToolBarAdd()
        {
            if (selectedNode is null)
            {
                Debug.LogError($"AbilityEffectWindow.Main.cs: selectedNode is null.");
                return;
            }

            EffectDataMgr.AddEffect(selectedNode, new AbilityEffect(new Guid().ToString()));
            RefreshByEffectNode(selectedNode);
        }

        /// <summary>
        /// 保存nodeGroup
        /// </summary>
        private void OnClickSave()
        {
            if (_effects is null || _effects.Count == 0)
            {
                Debug.LogError($"AbilityEffectWindow.Main.cs: effects count is 0.");
                return;
            }

            (bool effectIsValid,string errMsg) result = (true,string.Empty);
            var index = 0;
            foreach (var effect in _effects)
            {
                result = effect.IsValid();
                if (!result.effectIsValid)
                {
                    Debug.LogError($"effect_{index} is invalid,errMsg:{result.errMsg}");
                    break;
                }
            }

            if (_triggerTime < 0)
            {
                Debug.LogError($"trigger time < 0,trigger time:{_triggerTime}");
                return;
            }
            
            EffectDataMgr.SetEffects(selectedNode,_effects);
            
            //更新对应node
            selectedNode.Repaint();
            Debug.Log($"<color=green>save effects success.</color>");
        }

        //-----------mono-----------

        private void OnGUI()
        {
            if (selectedNode is null /*|| selectedPorts.Count == 0*/)
                return;

            EditorGUILayout.Space(20);
            //trigger time
            EditorGUILayout.BeginVertical();
            {
                _triggerTime = EditorGUILayout.FloatField("Trigger Time:", _triggerTime);
            }
            EditorGUILayout.EndVertical();

            if (_effects.Count == 0)
                return;
            
            //effect data:
            EditorGUILayout.BeginVertical();
            {
                if (_isRepaint)
                    _isRepaint = !_isRepaint;
                
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                var cnt = _effects.Count;
                AbilityEffect effect = null;
                
                GUIStyle HeaderLabelStyle = new GUIStyle(GUI.skin.label);
                HeaderLabelStyle.fontStyle = FontStyle.Bold;
                HeaderLabelStyle.fontSize = 15;
                HeaderLabelStyle.normal.textColor = Color.green;

                var tempArrayString = string.Empty;
                string[] tempArray = null;
                for (int i = 0; i < cnt; i++)
                {
                    effect = _effects[i];
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"Effect-{i+1}",HeaderLabelStyle);
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Remove", GUILayout.Width(100)))
                        {
                            _isRepaint = true;
                            Debug.Log("clicked remove button.");
                            EffectDataMgr.RemoveEffect(selectedNode, effect);
                            // RefreshByEffectNode(selectedNode);
                            // // break;
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    _foldOuts[i] = EditorGUILayout.Foldout(_foldOuts[i],"foldOut");
                    if (_foldOuts[i])
                    {
                        effect.ID = EditorGUILayout.IntField($"ID", effect.ID);
                        // effect.Name = EditorGUILayout.TextField("Name", effect.Name);
                        effect.Desc = EditorGUILayout.TextField($"Desc", effect.Desc);
                        effect.Type = (EffectType)EditorGUILayout.EnumPopup($"Type", effect.Type);
                        effect.ExtensionFloatParam_1 = EditorGUILayout.FloatField($"ExtensionFloatParam_1", effect.ExtensionFloatParam_1);
                        effect.ExtensionFloatParam_2 = EditorGUILayout.FloatField($"ExtensionFloatParam_2", effect.ExtensionFloatParam_2);
                        effect.ExtensionFloatParam_3 = EditorGUILayout.FloatField($"ExtensionFloatParam_3", effect.ExtensionFloatParam_3);
                        effect.ExtensionFloatParam_4 = EditorGUILayout.FloatField($"ExtensionFloatParam_4", effect.ExtensionFloatParam_4);
                        effect.ExtensionStringParm_1 = EditorGUILayout.TextField($"ExtensionStringParm_1", effect.ExtensionStringParm_1);
                        effect.ExtensionStringParm_2 = EditorGUILayout.TextField($"ExtensionStringParm_2", effect.ExtensionStringParm_2);
                        effect.ExtensionStringParm_3 = EditorGUILayout.TextField($"ExtensionStringParm_3", effect.ExtensionStringParm_3);
                        effect.ExtensionStringParm_4 = EditorGUILayout.TextField($"ExtensionStringParm_4", effect.ExtensionStringParm_4);
                        effect.EffectOnAwake = EditorGUILayout.Toggle($"EffectOnAwake", effect.EffectOnAwake);
                        effect.ModifierType = (NumricModifierType)EditorGUILayout.EnumPopup("ModifierType", effect.ModifierType);
                        effect.DurationPolicy = (DurationPolicy)EditorGUILayout.EnumPopup($"DurationPolicy", effect.DurationPolicy);
                        effect.Period = EditorGUILayout.FloatField($"Period", effect.Period);
                        effect.Duration = EditorGUILayout.FloatField($"Duration", effect.Duration);
                        effect.Target = EditorGUILayout.IntField($"Target", effect.Target);
                        effect.EffectType = (actor_attribute)EditorGUILayout.EnumPopup($"EffectType", effect.EffectType);

                        try
                        {
                            tempArrayString = EditorGUILayout.TextField($"DeriveEffects", effect.DeriveEffects is null ? string.Empty : ArrayToString(effect.DeriveEffects));
                            effect.DeriveEffects = tempArrayString.Split(',').Select(int.Parse).ToArray();
                        }
                        catch
                        {
                            effect.DeriveEffects = null;
                        }

                        try
                        {
                            tempArrayString = EditorGUILayout.TextField($"AwakeEffects", effect.AwakeEffects is null ? string.Empty : ArrayToString(effect.AwakeEffects));
                            effect.AwakeEffects = tempArrayString.Split(',').Select(int.Parse).ToArray();
                        }
                        catch
                        {
                            effect.AwakeEffects = null;
                        }
                        
                        EditorGUILayout.Space(10);
                    }
                }//end for
                
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
            
            EffectDataMgr.SetEffects(selectedNode,_effects);
            
            if (_isRepaint)
            {
                RefreshByEffectNode(selectedNode);
                // _effects = EffectDataMgr.GetEffects(selectedNode).AsReadOnly();
                // _foldOuts = new bool[_effects.Count];
            }
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
            var button = new Button();
            button.text = "Add Effect";
            button.clicked += OnClickToolBarAdd;
            _toolBar.Add(button);

            button = new Button();
            button.text = "Save";
            button.clicked += OnClickSave;
            _toolBar.Add(button);
            
            rootVisualElement.Add(_toolBar);
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

        private Vector2 _scrollPosition;
        private bool[] _foldOuts = null;
        private bool _isRepaint = false;
    }
}
