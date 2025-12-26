// using System;
// using UnityEngine;
//
// namespace Aquila.AbilityEditor
// {
//     /// <summary>
//     /// 技能Clip数据类
//     /// 用于在timeline中表示技能触发
//     /// </summary>
//     [Serializable]
//     public class SkillClipData : TimelineClipData
//     {
//         /// <summary>
//         /// 技能ID（关联到配置表中的技能）
//         /// </summary>
//         [SerializeField]
//         private int _skillId;
//
//         /// <summary>
//         /// Effect ID列表（技能触发的效果）
//         /// </summary>
//         [SerializeField]
//         private int[] _effectIds;
//
//         /// <summary>
//         /// 技能目标类型
//         /// </summary>
//         [SerializeField]
//         private string _targetType;
//
//         public override TimelineClipType ClipType => TimelineClipType.Ability;
//
//         public SkillClipData() : base()
//         {
//             _skillId = 0;
//             _effectIds = new int[0];
//             _targetType = string.Empty;
//             ClipColor = new Color(0.3f, 0.6f, 1f); // 蓝色
//         }
//
//         public SkillClipData(string clipName, float startTime, float endTime, int skillId)
//             : base(clipName, startTime, endTime, new Color(0.3f, 0.6f, 1f))
//         {
//             _skillId = skillId;
//             _effectIds = new int[0];
//             _targetType = string.Empty;
//         }
//
//         #region Properties
//
//         public int SkillId
//         {
//             get => _skillId;
//             set => _skillId = value;
//         }
//
//         public int[] EffectIds
//         {
//             get => _effectIds;
//             set => _effectIds = value ?? new int[0];
//         }
//
//         public string TargetType
//         {
//             get => _targetType;
//             set => _targetType = value;
//         }
//
//         #endregion
//
//         #region Override Methods
//
//         public override TimelineClipData Clone()
//         {
//             var clone = new SkillClipData();
//             CopyBaseTo(clone);
//             clone._skillId = _skillId;
//             clone._effectIds = _effectIds != null ? (int[])_effectIds.Clone() : new int[0];
//             clone._targetType = _targetType;
//             return clone;
//         }
//
//         public override bool Validate(out string errorMessage)
//         {
//             if (!base.Validate(out errorMessage))
//                 return false;
//
//             if (_skillId <= 0)
//             {
//                 errorMessage = "Skill ID must be greater than 0";
//                 return false;
//             }
//
//             errorMessage = string.Empty;
//             return true;
//         }
//
//         public override string GetDisplayInfo()
//         {
//             return $"Skill {_skillId}: {ClipName} [{StartTime:F2}s - {EndTime:F2}s]";
//         }
//
//         #endregion
//     }
// }
