using UnityEngine;
using UnityEditor;

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

        private void OnEnable()
        {
            _effectIdProp = serializedObject.FindProperty("_effectId");
            _clipNameProp = serializedObject.FindProperty("_clipName");
            _triggerTimeProp = serializedObject.FindProperty("_triggerTime");

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

            EditorGUILayout.LabelField("Effect Clip Properties", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();

            // Effect ID
            EditorGUILayout.PropertyField(_effectIdProp, new GUIContent("Effect ID"));

            // Clip Name
            EditorGUILayout.PropertyField(_clipNameProp, new GUIContent("Clip Name"));

            // Trigger Time - 使用Slider，范围限制为0到timeline时长
            float currentTriggerTime = _triggerTimeProp.floatValue;
            float newTriggerTime = EditorGUILayout.Slider(
                new GUIContent("Trigger Time", $"范围: 0 ~ {proxy.TimelineDuration:F2}s"),
                currentTriggerTime,
                0f,
                proxy.TimelineDuration
            );

            if (Mathf.Abs(newTriggerTime - currentTriggerTime) > 0.001f)
            {
                _triggerTimeProp.floatValue = newTriggerTime;
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                proxy.SyncToClipData();

                // 标记编辑器窗口需要刷新
                EditorUtility.SetDirty(proxy);
            }

            EditorGUILayout.Space();

            // 显示范围信息
            EditorGUILayout.HelpBox(
                $"Trigger Time范围: 0.00s ~ {proxy.TimelineDuration:F2}s\n" +
                "修改属性后会自动同步到Effect Clip。",
                MessageType.Info
            );
        }
    }
}
