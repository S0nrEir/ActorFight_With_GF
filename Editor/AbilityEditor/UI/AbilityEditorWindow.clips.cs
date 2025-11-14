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
        private void RegisterTrackToClipManager(TimelineTrack track, UnityEngine.UIElements.VisualElement timelineElement)
        {
            if (_clipManager == null)
                InitializeClipManager();

            _clipManager.RegisterTrack(track, timelineElement);
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

        #region Public API for Testing

        /// <summary>
        /// 添加测试clip（用于测试功能）
        /// </summary>
        [MenuItem("Aquila/AbilityEditor/Add Test Clips")]
        public static void AddTestClips()
        {
            var window = GetWindow<AbilityEditorWindow>();
            if (window == null || window._clipManager == null || window._timelineTracks == null)
            {
                Debug.LogWarning("AbilityEditorWindow is not initialized or has no tracks");
                return;
            }

            // 确保至少有一个轨道
            if (window._timelineTracks.Count == 0)
            {
                Debug.LogWarning("No tracks available. Please create a track first.");
                return;
            }

            var firstTrack = window._timelineTracks[0];

            // 添加测试clips
            var skillClip = new SkillClipData("Test Skill", 0.5f, 1.5f, 1001);
            window._clipManager.AddClip(firstTrack, skillClip);

            var buffClip = new BuffClipData("Test Buff", 2f, 4f, 2001);
            window._clipManager.AddClip(firstTrack, buffClip);

            var audioClip = new AudioClipData("Test Audio", 1f, 2f, "audio/test");
            window._clipManager.AddClip(firstTrack, audioClip);

            var vfxClip = new VFXClipData("Test VFX", 2.5f, 3.5f, "vfx/test");
            window._clipManager.AddClip(firstTrack, vfxClip);

            Debug.Log("Test clips added successfully!");
        }

        #endregion
    }
}
