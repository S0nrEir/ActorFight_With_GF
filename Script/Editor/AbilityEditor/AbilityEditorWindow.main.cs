using System.IO;
using System.Security.AccessControl;
using Bright.Serialization;
using Cfg;
using Cfg.Enum;
using GameFramework;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;

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
                    EditorGUILayout.LabelField("Ability Base Info");
                    DrawAbilityBaseArea();
                    EditorGUILayout.Space(2);
                    DrawButtons();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnEnable()
        {
            _thisWindow = GetWindow<AbilityEditorWindow>();
            _abilityView = new AbilityView(this);
            _thisWindow.minSize = _windowMinSize;
            rootVisualElement.Add(_abilityView);
            _abilityView.StretchToParentSize();

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
            EditorGUILayout.LabelField("functions");
            EditorGUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Export"))
            {
                AbilityEditorEffectGroupNode tempNode = null;
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
                    }//end foreach
                }//end foreach

                if (_abilityBaseID < 0)
                {
                    Debug.LogError($"<color=ffefdb>ability base id < 0</color>");
                    return;
                }

                if (_abilityTableData.Get(_abilityBaseID) != null)
                {
                    Debug.LogError($"<color=ffefdb>ability base id exist</color>");
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
                
                

            }//end gui if
            EditorGUILayout.EndHorizontal();
        }
        
        private void DrawAbilityBaseArea()
        {
            EditorGUILayout.BeginVertical("box",GUILayout.Height(_windowMinSize.y * .85f));
            {
                _abilityBaseID     = EditorGUILayout.IntField("base ID:"            , _abilityBaseID);
                _abilityName       = EditorGUILayout.TextField("name:"              , _abilityName);
                _abilityDesc       = EditorGUILayout.TextField("description:"       , _abilityDesc);
                _costEffectID      = EditorGUILayout.IntField("cost effect ID:"     , _costEffectID);
                _coolDownEffectID  = EditorGUILayout.IntField("cool down effect ID:", _coolDownEffectID);
                // _effectsIDArray    = EditorGUILayout.TextField("effects ID array:"  , _effectsIDArray);
                // _abilityTargetType = (AbilityTargetType)EditorGUILayout.EnumPopup("target type:"        , _abilityTargetType);
                _timelineID        = EditorGUILayout.IntField("timeline ID:"        , _timelineID);
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
        
        [MenuItem("Aquila/Ability/AbilityEditor")]
        public static void ShowExample()
        {
            AbilityEditorWindow wnd = GetWindow<AbilityEditorWindow>();
            wnd.titleContent = new GUIContent("AbilityEditorWindow");
        }
    }   
}
