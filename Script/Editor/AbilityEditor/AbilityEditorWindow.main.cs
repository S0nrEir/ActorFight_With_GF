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
            button.text = "Add Node";
            button.clicked += OnClickToolBarAdd;
            _toolBar.Add(button);
            
            button = new Button();
            button.text = "Remove Node";
            button.clicked += OnClickToolBarRemove;

            button = new Button();
            button.text = "Save";
            button.clicked += OnClickToolBarSave; 
            
            _toolBar.Add(button);
            rootVisualElement.Add(_toolBar);
            
            if (_effectWindow is null)
                _effectWindow = GetEffectWindow();
            
            _effectWindow.Show();
        }

        private void OnDisable()
        {
            _toolBar.Clear();
            _toolBar = null;
            
            _effectWindow?.Close();
            _effectWindow = null;
            
            EffectDataMgr.Clear();
        }

        //-----------event-----------
        /// <summary>
        /// 添加节点
        /// </summary>
        private void OnClickToolBarAdd()
        {
            var nodeGroup = _abilityView.CreateOneInOneOut("TestNode",Port.Capacity.Single);
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
            if (GUILayout.Button("save"))
            {
                
            }

            if (GUILayout.Button("export"))
            {
                    
            }
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
                _effectsIDArray    = EditorGUILayout.TextField("effects ID array:"  , _effectsIDArray);
                _abilityTargetType = EditorGUILayout.IntField("target type:"        , _abilityTargetType);
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
        private string _effectsIDArray = string.Empty;
        private int _abilityTargetType = -1;
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

        [MenuItem("Aquila/Ability/AbilityEditor")]
        public static void ShowExample()
        {
            AbilityEditorWindow wnd = GetWindow<AbilityEditorWindow>();
            wnd.titleContent = new GUIContent("AbilityEditorWindow");
        }
    }   
}
