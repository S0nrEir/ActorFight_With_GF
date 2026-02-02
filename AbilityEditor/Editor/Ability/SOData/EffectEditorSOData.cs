using System;
using UnityEngine;
using Cfg.Enum;

namespace Aquila.AbilityEditor.Config
{
    /// <summary>
    /// Effect配置数据的ScriptableObject
    /// 从Excel直接导入，不依赖LuBan运行时
    /// </summary>
    [CreateAssetMenu(fileName = "EffectData", menuName = "Ability Editor/Effect Data")]
    public class EffectEditorSOData : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("Effect ID")]
        public int id;

        [Tooltip("描述/注释")]
        [TextArea(2, 4)]
        public string Description;

        [Tooltip("Effect的类型")]
        public EffectType Type;

        [Header("Extension Parameters")]
        [Tooltip("额外参数")]
        public EffectExtensionParam ExtensionParam;

        [Header("Modifier Settings")]
        [Tooltip("数值修改器类型")]
        public NumricModifierType ModifierType;

        [Tooltip("施加后立刻生效")]
        public bool EffectOnAwake;

        [Header("Duration Settings")]
        [Tooltip("生效策略")]
        public DurationPolicy Policy;

        [Tooltip("生效周期，单位秒")]
        public float Period;

        [Tooltip("持续时间,单位秒")]
        public float Duration;

        [Header("Target Settings")]
        [Tooltip("目标类型，0=我方，1=敌方")]
        public int Target;

        [Tooltip("影响的数值类型")]
        public actor_attribute EffectType;

        [Header("Derive Effects")]
        [Tooltip("派生effect")]
        public int[] DeriveEffects = new int[0];

        [Tooltip("effect被唤起时派生一次的effect")]
        public int[] AwakeEffects = new int[0];

        /// <summary>
        /// 获取显示信息
        /// </summary>
        public string GetDisplayInfo()
        {
            return $"Effect [{id}]: {Description} - {Type}, Duration={Duration}s, Target={Target}";
        }
    }

    /// <summary>
    /// Effect扩展参数
    /// </summary>
    [Serializable]
    public class EffectExtensionParam
    {
        public float float_1;
        public float float_2;
        public float float_3;
        public float float_4;
        public int int_1;
        public int int_2;
        public int int_3;
        public int int_4;
    }
}
