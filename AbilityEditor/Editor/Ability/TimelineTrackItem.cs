using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    public class TimelineTrackItem
    {
        private List<TimelineClipData> _clips;

        public TimelineTrackItem()
        {
            Name = string.Empty;
            IsEnabled = true;
            TrackColor = Color.green;
            _clips = new List<TimelineClipData>();
        }

        public TimelineTrackItem( string name, Color color, bool isEnabled = true)
        {
            Name = name;
            IsEnabled = isEnabled;
            TrackColor = color;
            _clips = new List<TimelineClipData>();
        }

        public void SetName( string name ) => Name = name;
        public void SetEnabled( bool isEnabled ) => IsEnabled = isEnabled;
        public void SetTrackColor( Color color ) => TrackColor = color;
        public void SetTrackColor( float r, float g, float b, float a = 1f ) => TrackColor = new Color( r, g, b, a );


        public string Name { get; private set; }
        public bool IsEnabled { get; private set; }
        public Color TrackColor { get; private set; }

        /// <summary>
        /// 获取所有clip的只读列表
        /// </summary>
        public IReadOnlyList<TimelineClipData> Clips => _clips.AsReadOnly();

        #region Clip Management

        /// <summary>
        /// 添加clip到轨道
        /// </summary>
        public bool AddClip(TimelineClipData clip)
        {
            if (clip == null)
            {
                Toolkit.Tools.Logger.Warning("TimelineTrackItem.AddClip: clip is null");
                return false;
            }

            if (_clips.Contains(clip))
            {
                Toolkit.Tools.Logger.Warning($"TimelineTrackItem.AddClip: clip '{clip.ClipName}' already exists in track '{Name}'");
                return false;
            }

            _clips.Add(clip);
            Toolkit.Tools.Logger.Info($"TimelineTrackItem.AddClip: Added clip '{clip.ClipName}' to track '{Name}'");
            return true;
        }

        /// <summary>
        /// 移除clip
        /// </summary>
        public bool RemoveClip(TimelineClipData clip)
        {
            if (clip == null)
                return false;

            bool removed = _clips.Remove(clip);
            if (removed)
                Toolkit.Tools.Logger.Info($"TimelineTrackItem.RemoveClip: Removed clip '{clip.ClipName}' from track '{Name}'");

            return removed;
        }

        /// <summary>
        /// 根据ID移除clip
        /// </summary>
        public bool RemoveClipById(string clipId)
        {
            var clip = _clips.FirstOrDefault(c => c.ClipId == clipId);
            return RemoveClip(clip);
        }

        /// <summary>
        /// 清空所有clips
        /// </summary>
        public void ClearClips()
        {
            _clips.Clear();
            Toolkit.Tools.Logger.Info($"TimelineTrackItem.ClearClips: Cleared all clips from track '{Name}'");
        }

        /// <summary>
        /// 根据ID查找clip
        /// </summary>
        public TimelineClipData FindClipById(string clipId)
        {
            return _clips.FirstOrDefault(c => c.ClipId == clipId);
        }

        /// <summary>
        /// 获取指定时间点的所有clips
        /// </summary>
        public List<TimelineClipData> GetClipsAtTime(float time)
        {
            return _clips.Where(c => c.StartTime <= time && c.EndTime >= time).ToList();
        }

        /// <summary>
        /// 检查指定时间范围是否与现有clips重叠
        /// </summary>
        public bool HasOverlapInRange(float startTime, float endTime, TimelineClipData excludeClip = null)
        {
            var tempClip = new DummyClip(startTime, endTime);
            return _clips.Any(c => c != excludeClip && c.OverlapsWith(tempClip));
        }

        /// <summary>
        /// 获取clips数量
        /// </summary>
        public int GetClipCount()
        {
            return _clips.Count;
        }

        #endregion

        /// <summary>
        /// 临时clip类，用于重叠检测
        /// </summary>
        private class DummyClip : TimelineClipData
        {
            public override TimelineClipType ClipType => TimelineClipType.Custom;

            public DummyClip(float startTime, float endTime) : base("Dummy", startTime, endTime, Color.white)
            {
            }

            public override TimelineClipData Clone()
            {
                return new DummyClip(StartTime, EndTime);
            }
        }
    }
}
