using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Timeline Track的Inspector代理类
    /// 用于在Unity Inspector中显示和编辑TimelineTrackItem
    /// </summary>
    public class TrackInspectorProxy : ScriptableObject
    {
        /// <summary>
        /// 绑定Track数据到代理
        /// </summary>
        public void BindTrackData(TimelineTrackItem trackItem, VisualElement trackElement, System.Action<TimelineTrackItem, string> onNameChanged)
        {
            _targetTrackItem = trackItem;
            _targetTrackElement = trackElement;
            _onNameChanged = onNameChanged;
            SyncFromTrackData();
        }

        /// <summary>
        /// 从TrackItem同步数据到代理对象
        /// </summary>
        public void SyncFromTrackData()
        {
            if (_targetTrackItem == null)
                return;

            _trackName = _targetTrackItem.Name;
            _isEnabled = _targetTrackItem.IsEnabled;
            _trackColor = _targetTrackItem.TrackColor;
            _clipCount = _targetTrackItem.GetClipCount();
        }

        /// <summary>
        /// 将代理对象的数据同步到TrackItem
        /// </summary>
        public void SyncToTrackData()
        {
            if (_targetTrackItem == null)
                return;

            if (_targetTrackItem.Name != _trackName)
            {
                _targetTrackItem.SetName(_trackName);
                _onNameChanged?.Invoke(_targetTrackItem, _trackName);
            }

            _targetTrackItem.SetEnabled(_isEnabled);
            _targetTrackItem.SetTrackColor(_trackColor);
        }

        [Header("Track Properties")]
        [SerializeField]
        private string _trackName;

        [SerializeField]
        private bool _isEnabled = true;

        [SerializeField]
        private Color _trackColor;

        [Header("Track Info (Read Only)")]
        [SerializeField]
        private int _clipCount;

        [HideInInspector]
        public TimelineTrackItem _targetTrackItem;

        [HideInInspector]
        public VisualElement _targetTrackElement;

        private System.Action<TimelineTrackItem, string> _onNameChanged;
    }

    /// <summary>
    /// TrackInspectorProxy的自定义Inspector编辑器
    /// </summary>
    [CustomEditor(typeof(TrackInspectorProxy))]
    public class TrackInspectorProxyEditor : UnityEditor.Editor
    {
        private TrackInspectorProxy _proxy;

        private void OnEnable()
        {
            _proxy = (TrackInspectorProxy)target;
        }

        public override void OnInspectorGUI()
        {
            if (_proxy._targetTrackItem == null)
            {
                EditorGUILayout.HelpBox("No track selected", MessageType.Info);
                return;
            }

            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck())
            {
                _proxy.SyncToTrackData();
                EditorUtility.SetDirty(_proxy);
            }
        }
    }
}
