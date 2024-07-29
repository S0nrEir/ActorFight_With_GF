using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquila.Fight;
using Bright.Serialization;
using Cfg.Enum;
using GameFramework;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using MiniExcelLibs;

namespace Aquila.Editor
{
    public class AbilityEditorWindow : EditorWindow
    {
        //-----------mono-----------
        
        private void OnGUI()
        {
            Debug.Log("OnGUI calling...");
            EditorGUILayout.BeginVertical("box",new GUILayoutOption[]{GUILayout.Height(50)});
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal(GUILayout.Width(_windowMinSize.x),GUILayout.Height(_windowMinSize.y));
            { 
                //graph view area
                EditorGUILayout.BeginVertical("box",new GUILayoutOption[]{GUILayout.Width(_windowMinSize.x * 0.7f)});
                EditorGUILayout.LabelField("Impact Nodes");
                //draw graph view area
                DrawGraphViewArea();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(1);
                
                //draw ability & buttons area
                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField("Ability Base Info",EditorStyles.boldLabel);
                    DrawAbilityBaseArea();
                    EditorGUILayout.Space(2);
                    DrawTimelineInfo();
                    EditorGUILayout.Space(2);
                    DrawButtons();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();
            
            // Debug.Log("OnGUI calling...");
            // EditorGUILayout.BeginVertical("box",new GUILayoutOption[]{GUILayout.Height(50), GUILayout.Width(200)});
            // EditorGUILayout.EndVertical();
            // EditorGUILayout.BeginHorizontal(GUILayout.Width(_windowMinSize.x),GUILayout.Height(_windowMinSize.y));
            // {
            //     //graph view area
            //     EditorGUILayout.BeginVertical("box",new GUILayoutOption[]{GUILayout.Width(_windowMinSize.x * 0.7f)});
            //     EditorGUILayout.LabelField("Impact Nodes");
            //     //draw graph view area
            //     DrawGraphViewArea();
            //     EditorGUILayout.EndVertical();
            //     EditorGUILayout.Space(1);
            //
            //     //draw ability & buttons area
            //     EditorGUILayout.BeginVertical();
            //     {
            //         EditorGUILayout.LabelField("Ability Base Info",EditorStyles.boldLabel);
            //         DrawAbilityBaseArea();
            //         EditorGUILayout.Space(2);
            //         DrawTimelineInfo();
            //         EditorGUILayout.Space(2);
            //         DrawButtons();
            //         EditorGUILayout.IntField(99);
            //     }
            //     EditorGUILayout.EndVertical();
            //     EditorGUILayout.Space(10);
            // }
            // EditorGUILayout.EndHorizontal();
        }

        private void OnEnable()
        {
            _thisWindow = GetWindow<AbilityEditorWindow>();
            _abilityView = new AbilityView(this);
            _thisWindow.minSize = _windowMinSize;
            // _abilityView.style.width = _thisWindow.maxSize.x * 0.7f;
            // _abilityView.style.flexGrow = 0;
            // _abilityView.style.width = this.position.width * 0.7f;
            _abilityView.StretchToParentSize();
            _abilityView.style.marginRight = _windowMinSize.x * 0.3f;
            rootVisualElement.Add(_abilityView);

            _toolBar = new Toolbar();
            var button = new Button();
            button.text = "AddNode";
            button.clicked += OnClickToolBarAdd;
            _toolBar.Add(button);
            
            button = new Button();
            button.text = "RemoveNode";
            button.clicked += OnClickToolBarRemove;
            _toolBar.Add(button);

            // button = new Button();
            // button.text = "Save";
            // button.clicked += OnClickToolBarSave; 
            // _toolBar.Add(button);
            
            rootVisualElement.Add(_toolBar);
            
            if (_effectWindow is null)
                _effectWindow = GetEffectWindow();
            
            _effectWindow.Show();
            LoadTableData();
            _idPool = 0;
        }

        private void OnDisable()
        {
            _toolBar.Clear();
            _toolBar = null;
            
            _effectWindow?.Close();
            _effectWindow = null;
            
            EffectDataMgr.Clear();
            ClearTableData();
            _idPool = 0;
        }

        //-----------event-----------
        /// <summary>
        /// 添加节点
        /// </summary>
        private void OnClickToolBarAdd()
        {
            var nodeGroup = _abilityView.CreateOneInOneOut("TestNode",_idPool++,Port.Capacity.Single);
            EffectDataMgr.AddNodeGroup(nodeGroup);
        }

        /// <summary>
        /// 移除节点
        /// </summary>
        private void OnClickToolBarRemove()
        {
            var selection = _abilityView.selection;
            if (selection is null || selection.Count == 0)
                return;

            var first = selection[0];
            var node = first as AbilityEditorEffectGroupNode;
            if (node is null)
                return;
            
            _abilityView.RemoveElement(node);
            EffectDataMgr.RemoveNodeGroup(node);
            
        }

        private void OnClickToolBarSave()
        {
            var saveSucc = _abilityView.Save();
        }

        private void DrawGraphViewArea()
        {
            // EditorGUILayout.LabelField("test information");
            // EditorGUILayout.LabelField("test information");
            // EditorGUILayout.LabelField("test information");
            // EditorGUILayout.LabelField("test information");
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Export"))
            {
                AbilityEditorEffectGroupNode tempNode = null;
                var abilityGroupNodes = new List<AbilityEditorEffectGroupNode>();
                foreach (var node in _abilityView.nodes)
                {
                    tempNode = node as AbilityEditorEffectGroupNode;
                    (bool valid, string errMsg) effectValid = (false, string.Empty);
                    if (tempNode is null)
                    {
                        Debug.LogError($"<color=red>faild to cast node to AbilityEditorEffectGroupNode");
                        return;
                    }
                    
                    var effects = EffectDataMgr.GetEffects(tempNode);
                    if (effects is null || effects.Count == 0)
                    {
                        Debug.LogError($"faild to get effects from node:{tempNode.name},node id:{tempNode.ID}");
                        return;
                    }
                    
                    foreach (var effect in effects)
                    {
                        effectValid = effect.IsValid();
                        if (!effectValid.valid)
                        {
                            Debug.LogError($"<color=red>effect id:{effect.ID} is invalid,err msg:{effectValid.errMsg}</color>");
                            return;
                        }

                        if (_effectTableData.DataMap.ContainsKey(effect.ID))
                        {
                            Debug.LogError($"<color=ffefdb>effect id exist</color>");
                            return;
                        }

                    }//end foreach
                    abilityGroupNodes.Add(tempNode);
                }//end foreach

                if (_abilityBaseID < 0)
                {
                    Debug.LogError($"<color=ffefdb>ability base id < 0</color>");
                    return;
                }
                
                // if (_abilityTableData.Get(_abilityBaseID) != null)
                // {
                //     Debug.LogError($"<color=ffefdb>ability base id exist</color>");
                //     return;
                // }

                if (_abilityTableData.DataMap.ContainsKey(_abilityBaseID))
                {
                    Debug.LogError($"<color=ffefdb>ability base id exist</color>");
                    return;
                }

                if(_abilityTimelineTableData.DataMap.ContainsKey(_timelineID))
                {
                    Debug.LogError($"<color=ffefdb>timeline id exist</color>");
                    return;
                }
                
                if (string.IsNullOrEmpty(_abilityName))
                {
                    Debug.LogError($"<color=ffefdb>ability name is null</color>");
                    return;
                }
                
                if (string.IsNullOrEmpty(_abilityDesc))
                {
                    Debug.LogError($"<color=ffefdb>ability desc is null</color>");
                    return;
                }

                if (_costEffectID < 0)
                {
                    Debug.LogError($"<color=ffefdb>cost effect id <0</color>");
                    return;
                }

                if (_coolDownEffectID < 0)
                {
                    Debug.LogError($"<color=ffefdb>cool down effect id <0</color>");
                    return;
                }

                // if (_abilityTargetType == AbilityTargetType.Neutral)
                // {
                // }

                if (_timelineID < 0)
                {
                    Debug.LogError($"<color=ffefdb>time line id < 0</color>");
                    return;
                }

                // if (string.IsNullOrEmpty(_timelineDesc))
                // {
                //     Debug.LogError("<color=ffefdb>Timeline description is null</color>");
                //     return;
                // }

                if (string.IsNullOrEmpty(_timelineAssetPath))
                {
                    Debug.LogError("<color=ffefdb>Timeline asset path is null</color>");
                    return;
                }

                if (_timelineDuration < 0f)
                {
                    Debug.LogError("<color=ffefdb>Timeline duration < 0</color>");
                    return;
                }

                //调试标记用：
                _interpret += $"$#GBE";
                EditorUtility.DisplayProgressBar($"Write to Excel","Writing...",0.3f);
                var writeSucc = WriteToExcel(abilityGroupNodes);
                if (!writeSucc)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }

                EditorUtility.DisplayProgressBar($"Re generating...","generating...",0.6f);
                AssetDatabase.Refresh();
                EditorUtility.ClearProgressBar();
                
            }//end export button

            //写入测试数据
            if (GUILayout.Button("WriteTestData"))
            {
                _abilityBaseID    = 1007;
                _abilityDesc      = "测试Editor";
                _abilityName      = "测试Editor"; 
                _costEffectID     = 1000;
                _coolDownEffectID = 1001;
                _timelineID       = 1003;
                _interpret        = "测试Editor";

                _timelineID        = 1003;
                _timelineDesc      = "测试Editor";
                _timelineAssetPath = "Assets/Res/Timeline/Common/Common_Ability_1002.playable";
                _timelineDuration  = 2.5f;
                
                EffectDataMgr.ClearAllNodesData();
                var nodeGroup = _abilityView.CreateOneInOneOut("TestNode",_idPool++,Port.Capacity.Single);
                nodeGroup.WriteTestData();
                EffectDataMgr.AddNodeGroup(nodeGroup);

                var guid = new Guid().ToString();
                AbilityEffect effect = new AbilityEffect(guid);
                effect.ID = 1012;
                // effect.Name = "测试Editor";
                effect.Desc = "测试Editor";
                effect.Type = EffectType.Instant_PhyDamage;
                effect.ExtensionFloatParam_1 = -10;
                effect.ExtensionFloatParam_2 = -1;
                effect.ExtensionFloatParam_3 = -1;
                effect.ExtensionFloatParam_4 = -1;
                effect.ExtensionStringParm_1 = "-1";
                effect.ExtensionStringParm_2 = "-1";
                effect.ExtensionStringParm_3 = "-1";
                effect.ExtensionStringParm_4 = "-1";
                effect.ModifierType = NumricModifierType.Sum;
                effect.EffectOnAwake = true;
                effect.DurationPolicy = DurationPolicy.Instant;
                effect.Period = 0;
                effect.Duration = -1;
                effect.Target = 1;
                effect.EffectType = actor_attribute.Curr_HP;
                EffectDataMgr.AddEffect(nodeGroup, effect);
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 将数据写入到Excel
        /// </summary>
        private bool WriteToExcel(List<AbilityEditorEffectGroupNode> nodes)
        {
            var path = string.Empty;
            nodes.Sort(new NodeGroupComparer());
            
            //write to AbilityBaseConfig
            try
            {
                path = Path.Combine(Application.dataPath, "..,", @"/DataTable/designer_configs/Datas","AbilityBase.xlsx");
                MiniExcel.SaveAs 
                    (
                        path,
                        new []
                        {
                            new
                            {
                                Column1  = string.Empty,
                                Column2  = _abilityBaseID.ToString(),
                                Column3  = _abilityName,
                                Column4  = _abilityDesc,
                                Column5  = _costEffectID.ToString(),
                                Column6  = _coolDownEffectID.ToString(),
                                Column7  = string.Empty,//Effects
                                Column8  = string.Empty,//TargetType
                                Column9  = _timelineID.ToString(),
                                Column10 = AbilityEffectsToString(nodes),
                                Column11 = _interpret,
                            },
                        }
                    );//end SaveAs()
            }
            catch (Exception e)
            {
                throw new GameFrameworkException($"faild to write to AbilityBaseConfig,err{e.Message}");
            }

            //write to EffectConfig
            try
            {
                path = Path.Combine(Application.dataPath, "..,", @"/DataTable/designer_configs/Datas","Effect.xlsx");
                foreach (var node in nodes)
                {
                    var effects = EffectDataMgr.GetEffects(node);
                    foreach (var effect in effects)
                    {
                        MiniExcel.SaveAs
                        (
                            path,
                            new
                            {
                                Column1 = string.Empty,
                                Column2 = effect.ID.ToString(),
                                // Column3 = effect.Name,
                                Column3 = effect.Desc,
                                Column4 = effect.Tag.ToString(),
                                Column5 = effect.Type.ToString(),
                                Column6 = effect.ExtensionFloatParam_1.ToString(),
                                Column7 = effect.ExtensionFloatParam_2.ToString(),
                                Column8 = effect.ExtensionFloatParam_3.ToString(),
                                Column9 = effect.ExtensionFloatParam_4.ToString(),
                                Column10 = effect.ExtensionStringParm_1,
                                Column11 = effect.ExtensionStringParm_2,
                                Column12 = effect.ExtensionStringParm_3,
                                Column13 = effect.ExtensionStringParm_4,
                                Column14 = effect.ModifierType.ToString(),
                                Column15 = effect.EffectOnAwake.ToString().ToUpper(),
                                Column16 = effect.DurationPolicy.ToString(),
                                Column17 = effect.Duration.ToString(),
                                Column18 = effect.Target.ToString(),
                                Column19 = effect.EffectType.ToString(),
                                Column20 = effect.DeriveEffects != null && effect.DeriveEffects.Length != 0 ? string.Join(",",effect.DeriveEffects) : string.Empty,
                                Column21 = effect.AwakeEffects != null && effect.AwakeEffects.Length != 0 ? string.Join(",",effect.AwakeEffects) : string.Empty,
                            }
                        );//end SaveAs()
                    }
                }
            }
            catch (Exception e)
            {
                throw new GameFrameworkException($"faild to write to EffectConfig,err{e.Message}");
            }

            //write to AbilityTimelineConfig
            try
            {
                path = Path.Combine(Application.dataPath, "..,", @"/DataTable/designer_configs/Datas","AbilityTimeline.xlsx");
                MiniExcel.SaveAs
                    (
                        path,
                        new
                        {
                            Column1 = string.Empty,
                            Column2 = _timelineID.ToString(),
                            Column3 = _timelineDesc ,
                            Column4 = _timelineDuration.ToString(),
                            Column5 = 0f.ToString(),
                        }
                    );
            }
            catch (Exception e)
            {
                throw new GameFrameworkException($"faild to write to AbilityTimelineConfig,err{e.Message}");
            }
            return true;
        }

        /// <summary>
        /// 将AbilityEffects转换为要写入配置的格式
        /// </summary>
        private string AbilityEffectsToString(List<AbilityEditorEffectGroupNode> nodes)
        {
            StringBuilder builder = new StringBuilder();
            var nodesCount = nodes.Count;
            var effects = new List<AbilityEffect>();
            for (var i = 0; i < nodesCount; i++)
            {
                builder.Append($"{nodes[i].TriggerTime},");
                effects = EffectDataMgr.GetEffects(nodes[i]);
                var effectCount = effects.Count;
                for (int j = 0; j < effectCount; j++)
                {
                    builder.Append($"{effects[i].ID};");
                    if (j + 1 < effectCount)
                        builder.Append($";");
                }

                if (i + 1 < nodesCount)
                    builder.Append("|");
            }
            
            return builder.ToString();
        }

        private void DrawTimelineInfo()
        {
            EditorGUILayout.TextField("Timeline Info", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            {
                _timelineID        = EditorGUILayout.IntField("Timeline ID:"          , _timelineID);
                _timelineDesc      = EditorGUILayout.TextField("Timeline Description:", _timelineDesc);
                _timelineAssetPath = EditorGUILayout.TextField("Timeline Asset Path:" , _timelineAssetPath);
                _timelineDuration  = EditorGUILayout.FloatField("Timeline Duration:"  , _timelineDuration);
                
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawAbilityBaseArea()
        {
            EditorGUILayout.BeginVertical("box",GUILayout.Height(_windowMinSize.y * .2f));
            {
                _abilityBaseID     = EditorGUILayout.IntField("base ID:"            , _abilityBaseID);
                _abilityName       = EditorGUILayout.TextField("name:"              , _abilityName);
                _abilityDesc       = EditorGUILayout.TextField("description:"       , _abilityDesc);
                _costEffectID      = EditorGUILayout.IntField("cost effect ID:"     , _costEffectID);
                _coolDownEffectID  = EditorGUILayout.IntField("cool down effect ID:", _coolDownEffectID);
                // _effectsIDArray    = EditorGUILayout.TextField("effects ID array:"  , _effectsIDArray);mm
                // _abilityTargetType = (AbilityTargetType)EditorGUILayout.EnumPopup("target type:"        , _abilityTargetType);
                _interpret         = EditorGUILayout.TextField("interpret:"         , _interpret);
            }
            EditorGUILayout.EndVertical();
        }
        
        //-----------public-----------
        
        /// <summary>
        /// 刷新节点信息面板
        /// </summary>
        public void RefreshNodePanel(AbilityEditorEffectGroupNode node)
        {
            _effectWindow.RefreshByEffectNode(node);
        }
        
        //-----------private-----------

        private void ClearTableData()
        {
            _abilityTableData.DataList.Clear();
            _abilityTableData.DataMap.Clear();
            _abilityTableData = null;
            
            _abilityTimelineTableData.DataList.Clear();
            _abilityTimelineTableData.DataMap.Clear();
            _abilityTimelineTableData = null;
            
            _effectTableData.DataList.Clear();
            _effectTableData.DataMap.Clear();
            _effectTableData = null;
        }

        /// <summary>
        /// 加载表数据
        /// </summary>
        private void LoadTableData()
        {
            var loader = new System.Func<string, ByteBuf>( ( file ) =>
            {
                return new ByteBuf( File.ReadAllBytes( $"{Utility.Path.GetRegularPath($"{Application.dataPath}/Res/DataTables/")}{file}.bytes" ) );
            } );
            
            _abilityTableData = new Cfg.Fight.Ability(loader("fight_ability"));
            _abilityTimelineTableData = new Cfg.Fight.AbilityTimeline(loader("fight_abilitytimeline"));
            _effectTableData = new Cfg.Common.Effect(loader("common_effect"));
        }

        private AbilityEffectGroupEditorWidnow GetEffectWindow()
        {
            return GetWindow<AbilityEffectGroupEditorWidnow>();
        }

        //-----------FIELDS-----------
        private int _abilityBaseID     = -1;
        private string _abilityName    = string.Empty;
        private string _abilityDesc    = string.Empty;
        private int _costEffectID      = -1;
        private int _coolDownEffectID  = -1;
        // private string _effectsIDArray = string.Empty;
        private AbilityTargetType _abilityTargetType = AbilityTargetType.Neutral;
        private int _timelineID        = -1;
        private string _interpret      = string.Empty;
        private string _timelineDesc = string.Empty;
        private string _timelineAssetPath = string.Empty;
        private float _timelineDuration = 0f;
        
        private EditorWindow _thisWindow = null;
        private static Vector2 _windowMinSize = new Vector2(1200, 700);

        /// <summary>
        /// effect窗体的引用
        /// </summary>
        private AbilityEffectGroupEditorWidnow _effectWindow = null;

        /// <summary>
        /// 节点检视面板
        /// </summary>
        private AbilityView _abilityView = null;

        private Toolbar _toolBar = null;

        private Cfg.Fight.Ability _abilityTableData = null;
        private Cfg.Fight.AbilityTimeline _abilityTimelineTableData = null;
        private Cfg.Common.Effect _effectTableData = null;
        
        /// <summary>
        /// ID池
        /// </summary>
        private int _idPool = 0;

        /// <summary>
        /// 写入的列前缀
        /// </summary>
        private const string ColumnPrefix = "Column";

        /// <summary>
        /// NodeGroup排序器，按照TriggerTime排序
        /// </summary>
        private class NodeGroupComparer : IComparer<AbilityEditorEffectGroupNode>
        {
            public int Compare(AbilityEditorEffectGroupNode x, AbilityEditorEffectGroupNode y)
            {
                if (x.TriggerTime < y.TriggerTime)
                    return -1;

                if (x.TriggerTime > y.TriggerTime)
                    return 1;

                return 0;
            }
        }

        [MenuItem("Aquila/Ability/AbilityEditor")]
        public static void ShowExample()
        {
            AbilityEditorWindow wnd = GetWindow<AbilityEditorWindow>();
            wnd.titleContent = new GUIContent("AbilityEditorWindow");
        }
    }   
}
