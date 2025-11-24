using Aquila.AbilityEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// AbilityEditorWindow 的 Clips 管理部分
    /// 负责处理timeline clips的相关功能
    /// </summary>
    public partial class AbilityEditorWindow
    {
        private TimelineClipManager _clipManager;
        private TimelineClipUI _selectedClipUI;
        private EffectClipInspectorProxy _clipInspectorProxy;

        /// <summary>
        /// 初始化Clip管理器
        /// </summary>
        private void InitializeClipManager()
        {
            _clipManager = new TimelineClipManager();

            // 注册clip事件
            _clipManager.OnClipSelected += OnClipSelected;
            _clipManager.OnClipModified += OnClipModified;
            _clipManager.OnClipDeleted += OnClipDeleted;
        }

        /// <summary>
        /// 注册轨道到clip管理器
        /// </summary>
        private void RegisterTrackItemToClipManager(TimelineTrackItem track, UnityEngine.UIElements.VisualElement timelineElement)
        {
            if (_clipManager == null)
                InitializeClipManager();

            _clipManager.RegisterTrackItem(track, timelineElement);
        }

        /// <summary>
        /// 更新clip管理器的timeline参数
        /// </summary>
        private void UpdateClipManagerTimelineParams()
        {
            if (_clipManager == null)
                return;

            _clipManager.UpdateTimelineParams(_pixelsPerSecond, _currentZoom, 0f, _timelineDuration);
        }

        /// <summary>
        /// 刷新所有clips UI
        /// </summary>
        private void RefreshAllClipsUI()
        {
            if (_clipManager == null)
                return;

            _clipManager.RefreshAllClips();
        }

        /// <summary>
        /// 清空所有轨道的clips
        /// </summary>
        private void ClearAllClips()
        {
            if (_clipManager == null)
                return;

            _clipManager.ClearAllTracks();
        }

        #region Clip Event Handlers

        private void OnClipSelected(TimelineClipUI clipUI)
        {
            if (clipUI == null)
                return;

            Debug.Log($"Clip selected: {clipUI.ClipData.GetDisplayInfo()}");

            // 保存当前选中的clip
            _selectedClipUI = clipUI;

            // 在Unity Inspector中显示clip的属性
            ShowClipInUnityInspector(clipUI);
        }

        /// <summary>
        /// 在Unity Inspector中显示Clip的属性
        /// </summary>
        private void ShowClipInUnityInspector(TimelineClipUI clipUI)
        {
            // 检查是否是EffectClipData
            if (clipUI.ClipData is EffectClipData effectClip)
            {
                // 创建或重用Inspector代理对象
                if (_clipInspectorProxy == null)
                {
                    _clipInspectorProxy = ScriptableObject.CreateInstance<EffectClipInspectorProxy>();
                    _clipInspectorProxy.name = "Effect Clip Inspector";
                }

                // 设置代理对象的目标
                _clipInspectorProxy.TargetClipData = effectClip;
                _clipInspectorProxy.TargetClipUI = clipUI;

                // 设置Timeline时长（用于限制Trigger Time范围）
                _clipInspectorProxy.TimelineDuration = _timelineDuration;

                // 从ClipData同步数据到代理对象
                _clipInspectorProxy.SyncFromClipData();

                // 在Unity Inspector中显示代理对象
                Selection.activeObject = _clipInspectorProxy;

                Debug.Log($"Showing Effect Clip in Unity Inspector - ID: {effectClip.EffectId}, Timeline Duration: {_timelineDuration:F2}s");
            }
            else
            {
                // 不是EffectClip，清除选择
                Selection.activeObject = null;
            }
        }

        private void OnClipModified(TimelineClipUI clipUI)
        {
            if (clipUI == null)
                return;

            // 标记数据为dirty，需要保存
            if (_currentAbilityData != null)
                EditorUtility.SetDirty(_currentAbilityData);
        }

        private void OnClipDeleted(TimelineClipUI clipUI)
        {
            if (clipUI == null)
                return;

            Debug.Log($"Clip deleted: {clipUI.ClipData.GetDisplayInfo()}");

            if (_currentAbilityData != null)
                EditorUtility.SetDirty(_currentAbilityData);
        }

        #endregion
    }
}
