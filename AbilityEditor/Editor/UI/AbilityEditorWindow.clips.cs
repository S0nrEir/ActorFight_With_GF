using Aquila.AbilityEditor;
using UnityEditor;
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
        private AudioClipInspectorProxy _audioClipInspectorProxy;

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
        private void RegisterTrackItemToClipManager(TimelineTrackItem track, VisualElement timelineElement)
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

            Aquila.Toolkit.Tools.Logger.Info($"Clip selected: {clipUI.ClipData.GetDisplayInfo()}");

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
            if (clipUI.ClipData is EffectClipData effectClip)
            {
                if (_clipInspectorProxy == null)
                {
                    _clipInspectorProxy = CreateInstance<EffectClipInspectorProxy>();
                    _clipInspectorProxy.name = "Effect Clip Inspector";
                }

                _clipInspectorProxy.BindEffectClipData(effectClip,clipUI,_timelineDuration);
                Selection.activeObject = _clipInspectorProxy;
                Aquila.Toolkit.Tools.Logger.Info($"Showing Effect Clip in Unity Inspector - ID: {effectClip.EffectId}, Timeline Duration: {_timelineDuration:F2}s");
            }
            else if (clipUI.ClipData is AudioClipData audioClip)
            {
                if (_audioClipInspectorProxy == null)
                {
                    _audioClipInspectorProxy = CreateInstance<AudioClipInspectorProxy>();
                    _audioClipInspectorProxy.name = "Audio Clip Inspector";
                }

                _audioClipInspectorProxy.BindAudioClipData(audioClip, clipUI, _timelineDuration);
                Selection.activeObject = _audioClipInspectorProxy;
                Aquila.Toolkit.Tools.Logger.Info($"Showing Audio Clip in Unity Inspector - Path: {audioClip.AudioPath}, Timeline Duration: {_timelineDuration:F2}s");
            }
            else
            {
                // 不是EffectClip或AudioClip，清除选择
                Selection.activeObject = null;
            }
        }

        private void OnClipModified(TimelineClipUI clipUI)
        {
            if (clipUI == null)
                return;

            // 如果修改的是当前选中的clip，同步更新Inspector显示
            if (_selectedClipUI == clipUI)
            {
                if (clipUI.ClipData is EffectClipData && _clipInspectorProxy != null)
                {
                    _clipInspectorProxy.SyncFromClipData();
                    EditorUtility.SetDirty(_clipInspectorProxy);
                }
                else if (clipUI.ClipData is AudioClipData && _audioClipInspectorProxy != null)
                {
                    _audioClipInspectorProxy.SyncFromClipData();
                    EditorUtility.SetDirty(_audioClipInspectorProxy);
                }
            }

            if (_currentAbilityData != null)
                EditorUtility.SetDirty(_currentAbilityData);
        }

        private void OnClipDeleted(TimelineClipUI clipUI)
        {
            if (clipUI == null)
                return;

            Aquila.Toolkit.Tools.Logger.Info($"Clip deleted: {clipUI.ClipData.GetDisplayInfo()}");

            if (_currentAbilityData != null)
                EditorUtility.SetDirty(_currentAbilityData);
        }

        #endregion
    }
}
