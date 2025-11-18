using Aquila.AbilityEditor;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor
{
    /// <summary>
    /// AbilityEditorWindow 的 Clips 管理部分
    /// 负责处理timeline clips的相关功能
    /// </summary>
    public partial class AbilityEditorWindow
    {
        private TimelineClipManager _clipManager;

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

            // 这里可以显示clip的属性面板
            // 例如：显示SkillClipData的SkillId、EffectIds等信息
        }

        private void OnClipModified(TimelineClipUI clipUI)
        {
            if (clipUI == null)
                return;

            Debug.Log($"Clip modified: {clipUI.ClipData.GetDisplayInfo()}");

            // 标记数据为dirty，需要保存
            if (_currentAbilityData != null)
            {
                EditorUtility.SetDirty(_currentAbilityData);
            }
        }

        private void OnClipDeleted(TimelineClipUI clipUI)
        {
            if (clipUI == null)
                return;

            Debug.Log($"Clip deleted: {clipUI.ClipData.GetDisplayInfo()}");

            // 标记数据为dirty，需要保存
            if (_currentAbilityData != null)
            {
                EditorUtility.SetDirty(_currentAbilityData);
            }
        }

        #endregion
    }
}
