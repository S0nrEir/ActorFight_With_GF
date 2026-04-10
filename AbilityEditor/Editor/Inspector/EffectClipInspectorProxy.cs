using Cfg.Enum;
using UnityEditor;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Effect Clip的Inspector代理类
    /// 用于在Unity Inspector中显示和编辑EffectClipData
    /// </summary>
    public class EffectClipInspectorProxy : ClipInspectorProxyBase<EffectClipData>
    {
        /// <summary>
        /// 将对应的EffectClip数据绑定到自己（为了保持向后兼容）
        /// </summary>
        public void BindEffectClipData(EffectClipData effectClipData, TimelineClipUI clipUI, float duration)
        {
            BindClipData(effectClipData, clipUI, duration);
        }

        /// <summary>
        /// 从EffectClipData同步数据到代理对象
        /// </summary>
        public override void SyncFromClipData()
        {
            if (!IsDataValid())
                return;

            _effectId = TargetClipData.EffectId;
            _clipName = TargetClipData.ClipName;
            // 限制TriggerTime范围
            _triggerTime = Mathf.Clamp(TargetClipData.TriggerTime, 0f, TimelineDuration);
            _canStack = TargetClipData.CanStack;
            _stackCount = TargetClipData.StackCount;

            // 同步Effect配置字段
            _effectType = TargetClipData.EffectType;
            _modifierType = TargetClipData.ModifierType;
            _affectedAttribute = TargetClipData.AffectedAttribute;
            _target = TargetClipData.Target;
            _resolveTypeID = TargetClipData.ResolveTypeID;
            _duration = TargetClipData.Duration;
            _period = TargetClipData.Period;
            _policy = TargetClipData.Policy;
            _effectOnAwake = TargetClipData.EffectOnAwake;
            _extensionParam = TargetClipData.ExtensionParam?.Clone() ?? new EffectClipData.EffectExtensionParam();
            _deriveEffects = TargetClipData.DeriveEffects != null ? (int[])TargetClipData.DeriveEffects.Clone() : new int[0];
            _awakeEffects = TargetClipData.AwakeEffects != null ? (int[])TargetClipData.AwakeEffects.Clone() : new int[0];
        }

        /// <summary>
        /// 同步代理对象的数据到EffectClipData
        /// </summary>
        public override void SyncToClipData()
        {
            if (!IsDataValid())
                return;

            TargetClipData.EffectId = _effectId;
            TargetClipData.ClipName = _clipName;
            TargetClipData.TriggerTime = _triggerTime;
            TargetClipData.CanStack = _canStack;
            TargetClipData.StackCount = _stackCount;

            // 同步Effect配置字段到ClipData
            TargetClipData.EffectType = _effectType;
            TargetClipData.ModifierType = _modifierType;
            TargetClipData.AffectedAttribute = _affectedAttribute;
            TargetClipData.Target = _target;
            TargetClipData.ResolveTypeID = _resolveTypeID;
            TargetClipData.Duration = _duration;
            TargetClipData.Period = _period;
            TargetClipData.Policy = _policy;
            TargetClipData.EffectOnAwake = _effectOnAwake;
            TargetClipData.ExtensionParam = _extensionParam?.Clone() ?? new EffectClipData.EffectExtensionParam();
            TargetClipData.DeriveEffects = _deriveEffects != null ? (int[])_deriveEffects.Clone() : new int[0];
            TargetClipData.AwakeEffects = _awakeEffects != null ? (int[])_awakeEffects.Clone() : new int[0];

            // 刷新UI显示
            RefreshUI();
        }

        [Header("Effect Clip Properties")]
        [SerializeField]
        private int _effectId;

        [SerializeField]
        private string _clipName;

        [SerializeField]
        private float _triggerTime;

        [SerializeField]
        private bool _canStack;

        [SerializeField]
        private int _stackCount = 1;

        [Header("Effect配置")]
        [SerializeField]
        private EffectType _effectType;

        [SerializeField]
        private NumricModifierType _modifierType;

        [SerializeField]
        private actor_attribute _affectedAttribute;

        [SerializeField]
        private int _target;

        [SerializeField]
        private int _resolveTypeID = -1;

        [SerializeField]
        private float _duration;

        [SerializeField]
        private float _period;

        [SerializeField]
        private DurationPolicy _policy;

        [SerializeField]
        private bool _effectOnAwake;

        [SerializeField]
        private EffectClipData.EffectExtensionParam _extensionParam;

        [SerializeField]
        private int[] _deriveEffects;

        [SerializeField]
        private int[] _awakeEffects;

        public int EffectId
        {
            get => _effectId;
            set
            {
                if (_effectId != value)
                {
                    _effectId = value;
                    SyncToClipData();
                }
            }
        }

        public string ClipName
        {
            get => _clipName;
            set
            {
                if (_clipName != value)
                {
                    _clipName = value;
                    SyncToClipData();
                }
            }
        }

        public float TriggerTime
        {
            get => _triggerTime;
            set
            {
                // 限制范围：0 到 timeline时长
                float clampedValue = Mathf.Clamp(value, 0f, TimelineDuration);
                if (_triggerTime != clampedValue)
                {
                    _triggerTime = clampedValue;
                    SyncToClipData();
                }
            }
        }
    }

    /// <summary>
    /// EffectClipInspectorProxy的自定义Editor
    /// </summary>
    [CustomEditor(typeof(EffectClipInspectorProxy))]
    public class EffectClipInspectorProxyEditor : UnityEditor.Editor
    {
        private SerializedProperty _effectIdProp;
        private SerializedProperty _clipNameProp;
        private SerializedProperty _triggerTimeProp;
        private SerializedProperty _canStackProp;
        private SerializedProperty _stackCountProp;

        // Effect配置属性
        private SerializedProperty _effectTypeProp;
        private SerializedProperty _modifierTypeProp;
        private SerializedProperty _affectedAttributeProp;
        private SerializedProperty _targetProp;
        private SerializedProperty _resolveTypeIDProp;
        private SerializedProperty _durationProp;
        private SerializedProperty _periodProp;
        private SerializedProperty _policyProp;
        private SerializedProperty _effectOnAwakeProp;
        private SerializedProperty _extensionParamProp;
        private SerializedProperty _deriveEffectsProp;
        private SerializedProperty _awakeEffectsProp;

        // Foldout状态
        private bool _basicConfigFoldout = true;
        private bool _timeParamsFoldout;
        private bool _extensionParamsFoldout;
        private bool _deriveEffectsFoldout;

        private void OnEnable()
        {
            _effectIdProp = serializedObject.FindProperty("_effectId");
            _clipNameProp = serializedObject.FindProperty("_clipName");
            _triggerTimeProp = serializedObject.FindProperty("_triggerTime");
            _canStackProp = serializedObject.FindProperty("_canStack");
            _stackCountProp = serializedObject.FindProperty("_stackCount");

            // 查找Effect配置属性
            _effectTypeProp = serializedObject.FindProperty("_effectType");
            _modifierTypeProp = serializedObject.FindProperty("_modifierType");
            _affectedAttributeProp = serializedObject.FindProperty("_affectedAttribute");
            _targetProp = serializedObject.FindProperty("_target");
            _resolveTypeIDProp = serializedObject.FindProperty("_resolveTypeID");
            _durationProp = serializedObject.FindProperty("_duration");
            _periodProp = serializedObject.FindProperty("_period");
            _policyProp = serializedObject.FindProperty("_policy");
            _effectOnAwakeProp = serializedObject.FindProperty("_effectOnAwake");
            _extensionParamProp = serializedObject.FindProperty("_extensionParam");
            _deriveEffectsProp = serializedObject.FindProperty("_deriveEffects");
            _awakeEffectsProp = serializedObject.FindProperty("_awakeEffects");

            // 从EditorPrefs加载Foldout状态
            _basicConfigFoldout = EditorPrefs.GetBool("EffectClipInspector_BasicConfig_Foldout", true);
            _timeParamsFoldout = EditorPrefs.GetBool("EffectClipInspector_TimeParams_Foldout", false);
            _extensionParamsFoldout = EditorPrefs.GetBool("EffectClipInspector_ExtensionParams_Foldout", false);
            _deriveEffectsFoldout = EditorPrefs.GetBool("EffectClipInspector_DeriveEffects_Foldout", false);

            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            if (target != null)
            {
                serializedObject.Update();
                Repaint();
            }
        }

        public override void OnInspectorGUI()
        {
            var proxy = target as EffectClipInspectorProxy;
            if (proxy == null || proxy.TargetClipData == null)
            {
                EditorGUILayout.HelpBox("No clip data available", MessageType.Info);
                return;
            }

            serializedObject.Update();

            EditorGUILayout.LabelField("Effect Clip属性", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            // Effect ID
            EditorGUILayout.PropertyField(_effectIdProp, new GUIContent("Effect ID"));

            // Clip Name
            EditorGUILayout.PropertyField(_clipNameProp, new GUIContent("Clip名称"));

            // Can Stack
            EditorGUILayout.PropertyField(_canStackProp, new GUIContent("可堆叠"));

            // Stack Count - 只在可堆叠时启用
            EditorGUI.BeginDisabledGroup(!_canStackProp.boolValue);
            EditorGUILayout.PropertyField(_stackCountProp, new GUIContent("堆叠层数"));
            EditorGUI.EndDisabledGroup();

            // Trigger Time - 使用Slider，范围限制为0到timeline时长
            float currentTriggerTime = _triggerTimeProp.floatValue;
            float newTriggerTime = EditorGUILayout.Slider(
                new GUIContent("触发时间", $"范围: 0 ~ {proxy.TimelineDuration:F2}s"),
                currentTriggerTime,
                0f,
                proxy.TimelineDuration
            );

            if (Mathf.Abs(newTriggerTime - currentTriggerTime) > 0.001f)
            {
                _triggerTimeProp.floatValue = newTriggerTime;
            }

            EditorGUILayout.Space();

            // 基本配置Foldout
            _basicConfigFoldout = EditorGUILayout.Foldout(_basicConfigFoldout, "基本配置", true, EditorStyles.foldoutHeader);
            EditorPrefs.SetBool("EffectClipInspector_BasicConfig_Foldout", _basicConfigFoldout);
            if (_basicConfigFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_effectTypeProp, new GUIContent("效果类型"));
                EditorGUILayout.PropertyField(_modifierTypeProp, new GUIContent("修改器类型"));
                EditorGUILayout.PropertyField(_affectedAttributeProp, new GUIContent("影响属性"));

                // 目标类型使用自定义显示
                int target = _targetProp.intValue;
                target = EditorGUILayout.IntPopup("目标类型 / Target", target, new[] { "Friendly", "Enemy" }, new[] { 0, 1 });
                _targetProp.intValue = target;
                EditorGUILayout.PropertyField(_resolveTypeIDProp, new GUIContent("Resolve Type ID", "<=0 uses default resolve type"));

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // 时间参数Foldout
            _timeParamsFoldout = EditorGUILayout.Foldout(_timeParamsFoldout, "时间参数", true, EditorStyles.foldoutHeader);
            EditorPrefs.SetBool("EffectClipInspector_TimeParams_Foldout", _timeParamsFoldout);
            if (_timeParamsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_durationProp, new GUIContent("持续时间(秒)", "持续时间，-1表示瞬时效果"));
                EditorGUILayout.PropertyField(_periodProp, new GUIContent("生效周期(秒)", "周期性效果的生效间隔"));
                EditorGUILayout.PropertyField(_policyProp, new GUIContent("生效策略"));
                EditorGUILayout.PropertyField(_effectOnAwakeProp, new GUIContent("立即生效"));
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // 扩展参数Foldout
            _extensionParamsFoldout = EditorGUILayout.Foldout(_extensionParamsFoldout, "扩展参数", true, EditorStyles.foldoutHeader);
            EditorPrefs.SetBool("EffectClipInspector_ExtensionParams_Foldout", _extensionParamsFoldout);
            if (_extensionParamsFoldout)
            {
                EditorGUI.indentLevel++;
                if (_extensionParamProp != null)
                {
                    SerializedProperty float1 = _extensionParamProp.FindPropertyRelative("FloatParam_1");
                    SerializedProperty float2 = _extensionParamProp.FindPropertyRelative("FloatParam_2");
                    SerializedProperty float3 = _extensionParamProp.FindPropertyRelative("FloatParam_3");
                    SerializedProperty float4 = _extensionParamProp.FindPropertyRelative("FloatParam_4");
                    SerializedProperty int1 = _extensionParamProp.FindPropertyRelative("IntParam_1");
                    SerializedProperty int2 = _extensionParamProp.FindPropertyRelative("IntParam_2");
                    SerializedProperty int3 = _extensionParamProp.FindPropertyRelative("IntParam_3");
                    SerializedProperty int4 = _extensionParamProp.FindPropertyRelative("IntParam_4");

                    EditorGUILayout.PropertyField(float1, new GUIContent("参数Float 1"));
                    EditorGUILayout.PropertyField(float2, new GUIContent("参数Float 2"));
                    EditorGUILayout.PropertyField(float3, new GUIContent("参数Float 3"));
                    EditorGUILayout.PropertyField(float4, new GUIContent("参数Float 4"));
                    EditorGUILayout.PropertyField(int1, new GUIContent("参数Int 1"));
                    EditorGUILayout.PropertyField(int2, new GUIContent("参数Int 2"));
                    EditorGUILayout.PropertyField(int3, new GUIContent("参数Int 3"));
                    EditorGUILayout.PropertyField(int4, new GUIContent("参数Int 4"));

                    EditorGUILayout.HelpBox("值为-1表示该参数未使用", MessageType.Info);
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            // 派生效果Foldout
            _deriveEffectsFoldout = EditorGUILayout.Foldout(_deriveEffectsFoldout, "派生Effect", true, EditorStyles.foldoutHeader);
            EditorPrefs.SetBool("EffectClipInspector_DeriveEffects_Foldout", _deriveEffectsFoldout);
            if (_deriveEffectsFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_deriveEffectsProp, new GUIContent("派生EffectID列表"), true);
                EditorGUILayout.PropertyField(_awakeEffectsProp, new GUIContent("唤起EffectID列表"), true);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                proxy.SyncToClipData();

                // 标记编辑器窗口需要刷新
                EditorUtility.SetDirty(proxy);
            }

            EditorGUILayout.Space();

            // 显示提示信息
            EditorGUILayout.HelpBox(
                "修改属性后会自动同步到Effect Clip。\n" +
                "所有Effect配置字段都会被序列化保存。",
                MessageType.Info
            );
        }
    }
}
