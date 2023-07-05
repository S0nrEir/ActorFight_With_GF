using Aquila.Fight;
using Cfg.Common;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// 技能工具类
        /// </summary>
        public static class Ability
        {
            /// <summary>
            /// 根据配表类型生成对应的effect逻辑实例，拿不到返回null
            /// </summary>
            public static EffectSpec_Base CreateEffectSpec(Table_Effect meta)
            {
                EffectSpec_Base effect = null;
                switch (meta.Type)
                {
                    case EffectType.Instant_PhyDamage:
                        //return new EffectSpec_PhyDamage(meta);
                        effect = ReferencePool.Acquire<EffectSpec_PhyDamage>();
                        break;

                    case EffectType.Period_FixedDamage:
                        effect = ReferencePool.Acquire<EffectSpec_PeriodFixedDamage>();
                        break;

                    default:
                        return null;
                }

                effect.Init( meta );
                return effect;
            }

        }//end class Ability
    }//end class Tools
}
