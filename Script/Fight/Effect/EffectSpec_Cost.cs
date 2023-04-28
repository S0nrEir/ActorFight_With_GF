using System.Collections;
using System.Collections.Generic;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Numric;
using Aquila.Toolkit;
using Cfg.common;
using Cfg.Enum;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 消耗类effect
    /// </summary>
    public class EffectSpec_Cost : EffectSpec_Base
    {
        public override void Apply(TActorBase actor, Addon_Base[] addon)
        {
            var attr_addon = Tools.Actor.FilterAddon<Addon_BaseAttrNumric>(addon);
            if (attr_addon != null)
            {
                var curr_value = attr_addon.GetCorrectionFinalValue(Actor_Attr.Curr_MP);
                curr_value.value += Meta.ModifierNumric;
                var res = attr_addon.SetBaseValue(Actor_Attr.Curr_MP, curr_value.value);
            }
        }

        /// <summary>
        /// 计算消耗后的值
        /// </summary>
        public float Calc(float val_to_modify)
        {
            return _modifier.Calc(val_to_modify);
        }

        public EffectSpec_Cost(Effect meta) : base(meta)
        {
            _modifier = ReferencePool.Acquire<Numric_Modifier>();
            _modifier.Setup(meta.ModifierType,meta.ModifierNumric);
        }
        
        public override void Clear()
        {
            ReferencePool.Release(_modifier);
            _modifier = null;
        }
        
        /// <summary>
        /// 对应的数值修改器
        /// </summary>
        private Numric_Modifier _modifier = null;

    }
   
}
