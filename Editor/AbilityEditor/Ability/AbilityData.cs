using UnityEngine;
using System;
using System.Collections.Generic;
using Cfg.Enum;

namespace Aquila.AbilityEditor
{
    [CreateAssetMenu(fileName = "New Skill", menuName = "Ability Editor/New Ability")]
    public class AbilityData : ScriptableObject
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
        /// 触发器节点集合，原表为数组/复杂字符串，这里先保存为字符串数组（每项为原单元格文本）,,由timeline编辑器生成
        /// </summary>
        // public string[] Triggers;
    }
}