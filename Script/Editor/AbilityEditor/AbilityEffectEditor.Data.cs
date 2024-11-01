using System.Collections;
using System.Collections.Generic;
using System.Text;
using Cfg.Enum;
using UnityEngine;

namespace Aquila.Editor
{
    /// <summary>
    /// Effect节点
    /// </summary>
    public class AbilityEffect
    {
        public (bool effectIsValid,string errMsg) IsValid()
        {
            var effectIsValid = true;
            StringBuilder builder = new StringBuilder();
            if (ID <= 0)
            {
                effectIsValid = false;
                builder.Append($"id <= 0\n");
            }

            if (string.IsNullOrEmpty(Desc))
            {
                effectIsValid = false;
                builder.Append($" desc is null\n");
            }

            // if (string.IsNullOrEmpty(Name))
            // {
            //     effectIsValid = false;
            //     builder.Append(" name is null\n");
            // }

            // if (effect.Tag == ActorTagType.None)
            // {
            //     effectIsValid = false;
            //     builder.Append($"effect-{index} tag is none\n");
            // }

            //运行数值修改器为none
            // if (ModifierType == NumricModifierType.None)
            // {
            //     effectIsValid = false;
            //     builder.Append($" modifier type is none\n");
            // }

            if (EffectType == actor_attribute.Invalid)
            {
                effectIsValid = false;
                builder.Append($" effect type is none\n");
            }

            return (effectIsValid,builder.ToString());
        }

        public AbilityEffect(string guid)
        {
            GUID                  = guid;
            ID                    = -1;
            Desc                  = string.Empty;
            // Name                  = string.Empty;
            // Tag                   = ActorTagType.None;
            Type                  = Cfg.Enum.EffectType.Period_CoolDown;
            ExtensionFloatParam_1 = -1f;
            ExtensionFloatParam_2 = -1f;
            ExtensionFloatParam_3 = -1f;
            ExtensionFloatParam_4 = -1f;
            ExtensionStringParm_1 = string.Empty;
            ExtensionStringParm_2 = string.Empty;
            ExtensionStringParm_3 = string.Empty;
            ExtensionStringParm_4 = string.Empty;
            ModifierType          = NumricModifierType.None;
            EffectOnAwake         = false;
            DurationPolicy        = DurationPolicy.Infinite;
            Period                = -1f;
            Duration              = -1f;
            Target                = 0;
            EffectType            = actor_attribute.Invalid;
            DeriveEffects         = null;
            AwakeEffects          = null;
        }

        public string GUID;
        
        /// <summary>
        /// id
        /// </summary>
        public int ID;

        /// <summary>
        /// 名称
        /// </summary>
        // public string Name;
        
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc;

        /// <summary>
        /// effect tag，暂时没用
        /// </summary>
        // public ActorTagType Tag;

        /// <summary>
        /// effect的类型
        /// </summary>
        public EffectType Type;

        /// <summary>
        /// float类型参数1
        /// </summary>
        public float ExtensionFloatParam_1;
        
        /// <summary>
        /// float类型参数2
        /// </summary>
        public float ExtensionFloatParam_2;
        
        /// <summary>
        /// float类型参数3
        /// </summary>
        public float ExtensionFloatParam_3;
        
        /// <summary>
        /// float类型参数4
        /// </summary>
        public float ExtensionFloatParam_4;

        /// <summary>
        /// string类型参数1
        /// </summary>
        public string ExtensionStringParm_1;
        
        /// <summary>
        /// string类型参数2
        /// </summary>
        public string ExtensionStringParm_2;
        
        /// <summary>
        /// string类型参数3
        /// </summary>
        public string ExtensionStringParm_3;
        
        /// <summary>
        /// string类型参数4
        /// </summary>
        public string ExtensionStringParm_4;

        /// <summary>
        /// 数值修改器类型
        /// </summary>
        public NumricModifierType ModifierType;

        /// <summary>
        /// 施加后立刻生效
        /// </summary>
        public bool EffectOnAwake;

        /// <summary>
        /// 生效策略
        /// </summary>
        public DurationPolicy DurationPolicy;

        /// <summary>
        /// 生效周期，秒
        /// </summary>
        public float Period;

        /// <summary>
        /// 持续时间，秒
        /// </summary>
        public float Duration;

        /// <summary>
        /// 目标类型，0=我方，1=敌方
        /// </summary>
        public int Target;

        /// <summary>
        /// 影响的数值类型
        /// </summary>
        public actor_attribute EffectType;

        /// <summary>
        /// 派生effect，可空
        /// </summary>
        public int[] DeriveEffects;

        /// <summary>
        /// 被唤起时发生一次的effect
        /// </summary>
        public int[] AwakeEffects;
    }
}
