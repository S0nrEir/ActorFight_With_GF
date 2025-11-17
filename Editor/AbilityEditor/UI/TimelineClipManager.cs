using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Timeline Clip管理器
    /// 负责管理所有轨道上的clips的UI创建、更新和交互
    /// </summary>
    public class TimelineClipManager
    {
        // 轨道与其对应的clip UI映射
        private Dictionary<TimelineTrackItem, List<TimelineClipUI>> _trackClipUIs;

        // 轨道与其timeline UI元素的映射
        private Dictionary<TimelineTrackItem, VisualElement> _trackTimelineElements;

        // 当前选中的clip
        private TimelineClipUI _selectedClip;

        // Timeline参数
        private float _pixelsPerSecond = 100f;
        private float _zoom = 1f;
        private float _timelineStartTime = 0f;
        private float _timelineEndTime = 5f;

        // 事件
        public event Action<TimelineClipUI> OnClipSelected;
        public event Action<TimelineClipUI> OnClipModified;
        public event Action<TimelineClipUI> OnClipDeleted;

        public TimelineClipUI SelectedClip => _selectedClip;

        public TimelineClipManager()
        {
            _trackClipUIs = new Dictionary<TimelineTrackItem, List<TimelineClipUI>>();
            _trackTimelineElements = new Dictionary<TimelineTrackItem, VisualElement>();
        }

        #region Track Management

        /// <summary>
        /// 注册轨道和其timeline UI元素
        /// </summary>
        public void RegisterTrackItem(TimelineTrackItem trackData, VisualElement timelineElement)
        {
            if ( trackData == null || timelineElement == null)
                return;

            if (!_trackClipUIs.ContainsKey( trackData ) )
                _trackClipUIs[trackData] = new List<TimelineClipUI>();

            _trackTimelineElements[trackData] = timelineElement;
            RegisterTrackItemContextMenu( trackData, timelineElement);
        }

        /// <summary>
        /// 注销轨道
        /// </summary>
        public void UnregisterTrackItem(TimelineTrackItem track)
        {
            if (track == null)
                return;

            if (_trackClipUIs.TryGetValue(track, out var clipUIs))
            {
                foreach (var clipUI in clipUIs)
                    clipUI.Destroy();

                clipUIs.Clear();
            }

            _trackClipUIs.Remove(track);
            _trackTimelineElements.Remove(track);
        }

        /// <summary>
        /// 清空所有轨道
        /// </summary>
        public void ClearAllTracks()
        {
            foreach (var kvp in _trackClipUIs)
            {
                foreach (var clipUI in kvp.Value)
                    clipUI.Destroy();
            }

            _trackClipUIs.Clear();
            _trackTimelineElements.Clear();
            _selectedClip = null;
        }

        #endregion

        #region Clip Management

        /// <summary>
        /// 添加clip到指定轨道
        /// </summary>
        public TimelineClipUI AddClip(TimelineTrackItem track, TimelineClipData clipData)
        {
            if (track == null || clipData == null)
                return null;

            // 添加数据
            if (!track.AddClip(clipData))
                return null;

            // 创建UI
            return CreateClipUI(track, clipData);
        }

        /// <summary>
        /// 移除clip
        /// </summary>
        public bool RemoveClip(TimelineTrackItem track, TimelineClipUI clipUI)
        {
            if (track == null || clipUI == null)
                return false;

            track.RemoveClip(clipUI.ClipData);
            if (_trackClipUIs.TryGetValue(track, out var clipUIs))
            {
                clipUIs.Remove(clipUI);
            }

            clipUI.Destroy();

            if (_selectedClip == clipUI)
                _selectedClip = null;

            return true;
        }

        /// <summary>
        /// 刷新指定轨道的所有clips UI
        /// </summary>
        public void RefreshTrackItemClips(TimelineTrackItem track)
        {
            if (track == null)
                return;

            if (!_trackClipUIs.TryGetValue(track, out var clipUIs))
                return;

            // 清理现有UI
            foreach (var clipUI in clipUIs)
                clipUI.Destroy();

            clipUIs.Clear();

            // 重新创建所有clip UI
            foreach (var clipData in track.Clips)
                CreateClipUI(track, clipData);
        }

        /// <summary>
        /// 刷新所有clips UI
        /// </summary>
        public void RefreshAllClips()
        {
            foreach (var track in _trackClipUIs.Keys.ToList())
            {
                RefreshTrackItemClips(track);
            }
        }

        /// <summary>
        /// 更新timeline参数（当zoom等变化时调用）
        /// </summary>
        public void UpdateTimelineParams(float pixelsPerSecond, float zoom, float timelineStartTime, float timelineEndTime)
        {
            _pixelsPerSecond = pixelsPerSecond;
            _zoom = zoom;
            _timelineStartTime = timelineStartTime;
            _timelineEndTime = timelineEndTime;

            // 更新所有clip UI的参数
            foreach (var clipUIs in _trackClipUIs.Values)
            {
                foreach (var clipUI in clipUIs)
                {
                    clipUI.UpdateTimelineParams(_pixelsPerSecond, _zoom, _timelineStartTime, _timelineEndTime);
                }
            }
        }

        #endregion

        #region Private Methods

        private TimelineClipUI CreateClipUI(TimelineTrackItem track, TimelineClipData clipData)
        {
            if (!_trackTimelineElements.TryGetValue(track, out var timelineElement))
            {
                Debug.LogError($"TimelineClipManager: No timeline element found for track '{track.Name}'");
                return null;
            }

            if (!_trackClipUIs.TryGetValue(track, out var clipUIs))
            {
                clipUIs = new List<TimelineClipUI>();
                _trackClipUIs[track] = clipUIs;
            }

            // 创建clip UI
            var clipUI = new TimelineClipUI(clipData);

            // 注册事件
            clipUI.OnClipSelected += HandleClipSelected;
            clipUI.OnClipModified += HandleClipModified;
            clipUI.OnClipDeleted += HandleClipDeleted;

            // 创建可视化元素
            var clipElement = clipUI.CreateVisualElement(_pixelsPerSecond, _zoom, _timelineStartTime, _timelineEndTime);
            timelineElement.Add(clipElement);

            clipUIs.Add(clipUI);

            return clipUI;
        }

        private void HandleClipSelected(TimelineClipUI clipUI)
        {
            // 取消之前选中的clip
            if (_selectedClip != null && _selectedClip != clipUI)
            {
                _selectedClip.SetSelected(false);
            }

            _selectedClip = clipUI;
            _selectedClip.SetSelected(true);

            OnClipSelected?.Invoke(clipUI);
        }

        private void HandleClipModified(TimelineClipUI clipUI)
        {
            OnClipModified?.Invoke(clipUI);
        }

        private void HandleClipDeleted(TimelineClipUI clipUI)
        {
            // 找到clip所属的轨道
            TimelineTrackItem ownerTrack = null;
            foreach (var kvp in _trackClipUIs)
            {
                if (kvp.Value.Contains(clipUI))
                {
                    ownerTrack = kvp.Key;
                    break;
                }
            }

            if (ownerTrack != null)
            {
                RemoveClip(ownerTrack, clipUI);
                OnClipDeleted?.Invoke(clipUI);
            }
        }

        private void RegisterTrackItemContextMenu(TimelineTrackItem track, VisualElement timelineElement)
        {
            timelineElement.AddManipulator(new ContextualMenuManipulator(evt =>
            {
                // 计算点击位置对应的时间
                float localX = evt.mousePosition.x - timelineElement.worldBound.x;
                float clickTime = localX / (_pixelsPerSecond * _zoom);
                clickTime = Mathf.Clamp(clickTime, _timelineStartTime, _timelineEndTime);

                evt.menu.AppendAction("Add Skill Clip", action => AddSkillClip(track, clickTime));
                evt.menu.AppendAction("Add Buff Clip", action => AddBuffClip(track, clickTime));
                evt.menu.AppendAction("Add Audio Clip", action => AddAudioClip(track, clickTime));
                evt.menu.AppendAction("Add VFX Clip", action => AddVFXClip(track, clickTime));
            }));
        }

        private void AddSkillClip(TimelineTrackItem track, float startTime)
        {
            var clipData = new SkillClipData($"Skill", startTime, startTime + 0.5f, 1);
            AddClip(track, clipData);
        }

        private void AddBuffClip(TimelineTrackItem track, float startTime)
        {
            var clipData = new BuffClipData($"Buff", startTime, startTime + 1f, 1);
            AddClip(track, clipData);
        }

        private void AddAudioClip(TimelineTrackItem track, float startTime)
        {
            var clipData = new AudioClipData($"Audio", startTime, startTime + 1f, "audio/default");
            AddClip(track, clipData);
        }

        private void AddVFXClip(TimelineTrackItem track, float startTime)
        {
            var clipData = new VFXClipData($"VFX", startTime, startTime + 1f, "vfx/default");
            AddClip(track, clipData);
        }

        #endregion

        #region Public Utility Methods

        /// <summary>
        /// 获取指定轨道的所有clip UI
        /// </summary>
        public List<TimelineClipUI> GetTrackItemClips(TimelineTrackItem track)
        {
            if (track == null || !_trackClipUIs.TryGetValue(track, out var clipUIs))
                return new List<TimelineClipUI>();

            return new List<TimelineClipUI>(clipUIs);
        }

        /// <summary>
        /// 取消选中当前clip
        /// </summary>
        public void ClearSelection()
        {
            if (_selectedClip != null)
            {
                _selectedClip.SetSelected(false);
                _selectedClip = null;
            }
        }

        #endregion
    }
}
