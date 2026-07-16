using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Timeline Track 的序列化数据包装器
    /// 用于将 TimelineTrackItem（运行时对象）序列化到 AbilityData（ScriptableObject）
    /// </summary>
    [Serializable]
    public class SerializedTrackData : ISerializationCallbackReceiver
    {
        /// <summary>
        /// 轨道名称
        /// </summary>
        public string TrackName;

        /// <summary>
        /// 轨道颜色
        /// </summary>
        public Color TrackColor;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled;

        /// <summary>
        /// Clip 列表（使用 SerializeReference 支持多态序列化）
        /// </summary>
        [SerializeReference]
        public List<TimelineClipData> Clips;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SerializedTrackData()
        {
            TrackName = string.Empty;
            TrackColor = Color.white;
            IsEnabled = true;
            Clips = new List<TimelineClipData>();
        }

        /// <summary>
        /// Unity 序列化前调用
        /// </summary>
        public void OnBeforeSerialize()
        {
            // 确保 Clips 列表不为 null
            if (Clips == null)
            {
                Clips = new List<TimelineClipData>();
            }
        }

        /// <summary>
        /// Unity 反序列化后调用，确保 Clips 列表被初始化
        /// </summary>
        public void OnAfterDeserialize()
        {
            // 确保 Clips 列表不为 null
            if (Clips == null)
            {
                Clips = new List<TimelineClipData>();
            }
        }

        /// <summary>
        /// 从 TimelineTrackItem 创建 SerializedTrackData（深拷贝）
        /// </summary>
        public SerializedTrackData(TimelineTrackItem trackItem)
        {
            if (trackItem == null)
            {
                TrackName = string.Empty;
                TrackColor = Color.white;
                IsEnabled = true;
                Clips = new List<TimelineClipData>();
                return;
            }

            TrackName = trackItem.Name;
            TrackColor = trackItem.TrackColor;
            IsEnabled = trackItem.IsEnabled;
            Clips = new List<TimelineClipData>();

            // 深拷贝所有 Clips
            foreach (var clip in trackItem.Clips)
            {
                if (clip != null)
                    Clips.Add(clip.Clone());
            }
        }

        /// <summary>
        /// 转换为 TimelineTrackItem
        /// </summary>
        public TimelineTrackItem ToTrackItem()
        {
            var trackItem = new TimelineTrackItem(TrackName, TrackColor, IsEnabled);

            // 添加所有 Clips（不需要再次 Clone，因为这些数据已经是独立的）
            if (Clips != null)
            {
                foreach (var clip in Clips)
                {
                    if (clip != null)
                    {
                        trackItem.AddClip(clip);
                    }
                }
            }

            return trackItem;
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        public bool Validate(out string errorMessage)
        {
            if (string.IsNullOrEmpty(TrackName))
            {
                errorMessage = "Track name cannot be empty";
                return false;
            }

            if (Clips == null)
            {
                errorMessage = "Clips list cannot be null";
                return false;
            }

            for (int i = 0; i < Clips.Count; i++)
            {
                var clip = Clips[i];
                if (clip == null)
                {
                    errorMessage = $"Clip at index {i} is null";
                    return false;
                }

                if (!clip.Validate(out string clipError))
                {
                    errorMessage = $"Clip at index {i} is invalid: {clipError}";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        public override string ToString()
        {
            return $"SerializedTrackData[Name={TrackName}, Enabled={IsEnabled}, Clips={Clips?.Count ?? 0}]";
        }
    }
}
