using System.Collections.Generic;
using Cfg.Enum;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    [CreateAssetMenu(fileName = "NewAbility", menuName = "Ability Editor/New Ability")]
    public class AbilityEditorSOData : ScriptableObject
    {
        /// <summary>
        /// 技能id
        /// </summary>
        public int Id;

        /// <summary>
        /// 技能名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 技能描述
        /// </summary>
        [TextArea]
        public string Desc;

        /// <summary>
        /// 技能消耗（Effect ID）
        /// </summary>
        public int CostEffectID;

        /// <summary>
        /// 技能冷却（Effect ID）
        /// </summary>
        public int CoolDownEffectID;

        /// <summary>
        /// 目标类型，使用已存在的枚举 Cfg.Enum.AbilityTargetType
        /// </summary>
        public AbilityTargetType TargetType;
        
        /// <summary>
        /// 携带的effect集合（逗号分隔的ID列表）,由timeline编辑器生成
        /// </summary>
        // public int[] Effects;

        /// <summary>
        /// 技能表现 timelineID
        /// </summary>
        public int TimelineID;

        /// <summary>
        /// Timeline 资产路径
        /// </summary>
        public string TimelineAssetPath = "";

        /// <summary>
        /// 触发器节点集合，原表为数组/复杂字符串，这里先保存为字符串数组（每项为原单元格文本）,,由timeline编辑器生成
        /// </summary>
        // public string[] Triggers;

        /// <summary>
        /// Timeline 总时长（秒）
        /// </summary>
        public float TimelineDuration = 5f;

        /// <summary>
        /// 序列化的 Track 数据列表
        /// </summary>
        [SerializeField]
        private List<SerializedTrackData> _tracks = new List<SerializedTrackData>();

        /// <summary>
        /// 只读访问 Track 列表
        /// </summary>
        public IReadOnlyList<SerializedTrackData> Tracks => _tracks.AsReadOnly();

        /// <summary>
        /// 设置 Track 列表
        /// </summary>
        public void SetTracks(List<SerializedTrackData> tracks)
        {
            if (tracks == null)
            {
                _tracks = new List<SerializedTrackData>();
                return;
            }

            _tracks = new List<SerializedTrackData>(tracks);
        }

        /// <summary>
        /// 清空 Track 列表
        /// </summary>
        public void ClearTracks()
        {
            _tracks.Clear();
        }

        /// <summary>
        /// 按索引获取 Track
        /// </summary>
        public SerializedTrackData GetTrack(int index)
        {
            if (index < 0 || index >= _tracks.Count)
                return null;

            return _tracks[index];
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        public bool Validate(out string error)
        {
            error = string.Empty;

            if (Id <= 0)
            {
                error = "Ability ID must be greater than 0";
                return false;
            }

            if (TimelineDuration <= 0)
            {
                error = "Timeline duration must be greater than 0";
                return false;
            }

            // 验证 Timeline 资产路径格式（可选字段，仅警告）
            if (!string.IsNullOrEmpty(TimelineAssetPath) && !TimelineAssetPath.StartsWith("Assets/"))
            {
                Toolkit.Tools.Logger.Warning($"Timeline asset path should start with 'Assets/': {TimelineAssetPath}");
            }

            // 验证所有 Tracks
            if (_tracks != null)
            {
                if (_tracks.Count == 0)
                {
                    error = "tracks list is empty";
                    return false;
                }

                for (int i = 0; i < _tracks.Count; i++)
                {
                    var track = _tracks[i];
                    if (track == null)
                    {
                        error = $"Track at index {i} is null";
                        return false;
                    }

                    if (!track.Validate(out string trackError))
                    {
                        error = $"Track '{track.TrackName}' at index {i} is invalid: {trackError}";
                        return false;
                    }
                }
            }

            return true;
        }
    }
}