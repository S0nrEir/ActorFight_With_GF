using UnityEngine;
using UnityEditor;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Audio Clip的Inspector代理类
    /// 用于在Unity Inspector中显示和编辑AudioClipData
    /// </summary>
    public class AudioClipInspectorProxy : ClipInspectorProxyBase<AudioClipData>
    {
        /// <summary>
        /// 将对应的AudioClip数据绑定到自己（为了保持向后兼容）
        /// </summary>
        public void BindAudioClipData(AudioClipData audioClipData, TimelineClipUI clipUI, float duration)
        {
            BindClipData(audioClipData, clipUI, duration);
        }

        /// <summary>
        /// 从AudioClipData同步数据到代理对象
        /// </summary>
        public override void SyncFromClipData()
        {
            if (!IsDataValid())
                return;

            _clipName = TargetClipData.ClipName;
            _audioPath = TargetClipData.AudioPath;
            _volume = TargetClipData.Volume;
            _loop = TargetClipData.Loop;
            _fadeInDuration = TargetClipData.FadeInDuration;
            _fadeOutDuration = TargetClipData.FadeOutDuration;
            // 限制StartTime和EndTime范围
            _startTime = Mathf.Clamp(TargetClipData.StartTime, 0f, TimelineDuration);
            _endTime = Mathf.Clamp(TargetClipData.EndTime, 0f, TimelineDuration);
        }

        /// <summary>
        /// 同步代理对象的数据到AudioClipData
        /// </summary>
        public override void SyncToClipData()
        {
            if (!IsDataValid())
                return;

            TargetClipData.ClipName = _clipName;
            TargetClipData.AudioPath = _audioPath;
            TargetClipData.Volume = _volume;
            TargetClipData.Loop = _loop;
            TargetClipData.FadeInDuration = _fadeInDuration;
            TargetClipData.FadeOutDuration = _fadeOutDuration;
            TargetClipData.StartTime = _startTime;
            TargetClipData.EndTime = _endTime;

            // 刷新UI显示
            RefreshUI();
        }

        [Header("Audio Clip Properties")]
        [SerializeField]
        private string _clipName;

        [SerializeField]
        private string _audioPath;

        [SerializeField]
        [Range(0f, 1f)]
        private float _volume;

        [SerializeField]
        private bool _loop;

        [SerializeField]
        private float _fadeInDuration;

        [SerializeField]
        private float _fadeOutDuration;

        [SerializeField]
        private float _startTime;

        [SerializeField]
        private float _endTime;

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

        public string AudioPath
        {
            get => _audioPath;
            set
            {
                if (_audioPath != value)
                {
                    _audioPath = value;
                    SyncToClipData();
                }
            }
        }

        public float Volume
        {
            get => _volume;
            set
            {
                float clampedValue = Mathf.Clamp01(value);
                if (Mathf.Abs(_volume - clampedValue) > 0.001f)
                {
                    _volume = clampedValue;
                    SyncToClipData();
                }
            }
        }

        public bool Loop
        {
            get => _loop;
            set
            {
                if (_loop != value)
                {
                    _loop = value;
                    SyncToClipData();
                }
            }
        }

        public float FadeInDuration
        {
            get => _fadeInDuration;
            set 
            {
                float clampedValue = Mathf.Max(0, value);
                if (Mathf.Abs(_fadeInDuration - clampedValue) > 0.001f)
                {
                    _fadeInDuration = clampedValue;
                    SyncToClipData();
                }
            }
        }

        public float FadeOutDuration
        {
            get => _fadeOutDuration;
            set
            {
                float clampedValue = Mathf.Max(0, value);
                if (Mathf.Abs(_fadeOutDuration - clampedValue) > 0.001f)
                {
                    _fadeOutDuration = clampedValue;
                    SyncToClipData();
                }
            }
        }

        public float StartTime
        {
            get => _startTime;
            set
            {
                // 限制范围：0 到 timeline时长
                float clampedValue = Mathf.Clamp(value, 0f, TimelineDuration);
                if (Mathf.Abs(_startTime - clampedValue) > 0.001f)
                {
                    _startTime = clampedValue;
                    SyncToClipData();
                }
            }
        }

        public float EndTime
        {
            get => _endTime;
            set
            {
                // 限制范围：StartTime 到 timeline时长
                float clampedValue = Mathf.Clamp(value, _startTime, TimelineDuration);
                if (Mathf.Abs(_endTime - clampedValue) > 0.001f)
                {
                    _endTime = clampedValue;
                    SyncToClipData();
                }
            }
        }
    }

    /// <summary>
    /// AudioClipInspectorProxy的自定义Editor
    /// </summary>
    [CustomEditor(typeof(AudioClipInspectorProxy))]
    public class AudioClipInspectorProxyEditor : UnityEditor.Editor
    {
        private SerializedProperty _clipNameProp;
        private SerializedProperty _audioPathProp;
        private SerializedProperty _volumeProp;
        private SerializedProperty _loopProp;
        private SerializedProperty _fadeInDurationProp;
        private SerializedProperty _fadeOutDurationProp;
        private SerializedProperty _startTimeProp;
        private SerializedProperty _endTimeProp;

        private void OnEnable()
        {
            _clipNameProp        = serializedObject.FindProperty("_clipName");
            _audioPathProp       = serializedObject.FindProperty("_audioPath");
            _volumeProp          = serializedObject.FindProperty("_volume");
            _loopProp            = serializedObject.FindProperty("_loop");
            _fadeInDurationProp  = serializedObject.FindProperty("_fadeInDuration");
            _fadeOutDurationProp = serializedObject.FindProperty("_fadeOutDuration");
            _startTimeProp       = serializedObject.FindProperty("_startTime");
            _endTimeProp         = serializedObject.FindProperty("_endTime");

            // 注册编辑器更新回调，用于实时刷新Inspector
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            // 取消注册更新回调
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            // 定期更新SerializedObject，确保Inspector显示最新数据
            if (target != null)
            {
                serializedObject.Update();
                Repaint();
            }
        }

        public override void OnInspectorGUI()
        {
            var proxy = target as AudioClipInspectorProxy;
            if (proxy == null || proxy.TargetClipData == null)
            {
                EditorGUILayout.HelpBox("No clip data available", MessageType.Info);
                return;
            }

            serializedObject.Update();

            EditorGUILayout.LabelField("Audio Clip Properties", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_clipNameProp, new GUIContent("Clip Name"));
            EditorGUILayout.PropertyField(_audioPathProp, new GUIContent("Audio Path"));
            EditorGUILayout.Space();
            EditorGUILayout.Slider(_volumeProp, 0f, 1f, new GUIContent("Volume"));
            EditorGUILayout.PropertyField(_loopProp, new GUIContent("Loop"));
            EditorGUILayout.Space();

            // Fade In Duration
            float currentFadeIn = _fadeInDurationProp.floatValue;
            float newFadeIn = EditorGUILayout.FloatField(
                new GUIContent("Fade In Duration (s)", "淡入时间（秒）"),
                currentFadeIn
            );
            if (newFadeIn < 0) newFadeIn = 0;
            if (Mathf.Abs(newFadeIn - currentFadeIn) > 0.001f)
            {
                _fadeInDurationProp.floatValue = newFadeIn;
            }

            // Fade Out Duration
            float currentFadeOut = _fadeOutDurationProp.floatValue;
            float newFadeOut = EditorGUILayout.FloatField(
                new GUIContent("Fade Out Duration (s)", "淡出时间（秒）"),
                currentFadeOut
            );
            
            if (newFadeOut < 0) 
                newFadeOut = 0;
            
            if (Mathf.Abs(newFadeOut - currentFadeOut) > 0.001f)
                _fadeOutDurationProp.floatValue = newFadeOut;

            EditorGUILayout.Space();

            // Start Time - 使用Slider，范围限制为0到timeline时长
            float currentStartTime = _startTimeProp.floatValue;
            float newStartTime = EditorGUILayout.Slider(
                new GUIContent("Start Time", $"范围: 0 ~ {proxy.TimelineDuration:F2}s"),
                currentStartTime,
                0f,
                proxy.TimelineDuration
            );

            if (Mathf.Abs(newStartTime - currentStartTime) > 0.001f)
            {
                _startTimeProp.floatValue = newStartTime;
            }

            // End Time - 使用Slider，范围限制为StartTime到timeline时长
            float currentEndTime = _endTimeProp.floatValue;
            float newEndTime = EditorGUILayout.Slider(
                new GUIContent("End Time", $"范围: {newStartTime:F2} ~ {proxy.TimelineDuration:F2}s"),
                currentEndTime,
                newStartTime,
                proxy.TimelineDuration
            );

            if (Mathf.Abs(newEndTime - currentEndTime) > 0.001f)
                _endTimeProp.floatValue = newEndTime;
            
            float duration = newEndTime - newStartTime;
            EditorGUILayout.LabelField("Duration", $"{duration:F2}s");

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                proxy.SyncToClipData();
                EditorUtility.SetDirty(proxy);
            }

            EditorGUILayout.Space();

            // 显示范围信息
            EditorGUILayout.HelpBox(
                $"Start Time范围: 0.00s ~ {proxy.TimelineDuration:F2}s\n" +
                $"End Time范围: {newStartTime:F2}s ~ {proxy.TimelineDuration:F2}s\n" +
                "修改属性后会自动同步到Audio Clip。",
                MessageType.Info
            );
        }
    }
}
