using Aquila.Fight;
using Cfg.Common;
using Cfg.Enum;

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
            /// 根据配表类型生成对应的effect逻辑实例
            /// </summary>
            public static EffectSpec_Base CreateEffectSpec(Table_Effect meta)
            {
                switch (meta.Type)
                {
                    case EffectType.PhyDamage:
                        return new EffectSpec_PhyDamage(meta);
                    
                    default:
                        return null;
                }

                return null;
            }

        }//end class Ability
    }//end class Tools
}
