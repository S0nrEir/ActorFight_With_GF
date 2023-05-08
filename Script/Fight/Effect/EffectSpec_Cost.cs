using System.Collections;
using System.Collections.Generic;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Module;
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
        public override void Apply(Module_Proxy_Actor.ActorInstance instance,ref AbilityHitResult result)
        {
            var attr_addon = instance.GetAddon<Addon_BaseAttrNumric>();
            if(attr_addon is null)
                return;
            
            var curr_value = attr_addon.GetCorrectionFinalValue(Actor_Attr.Curr_MP);
            curr_value.value += Meta.ModifierNumric;
            var res = attr_addon.SetBaseValue(Actor_Attr.Curr_MP, curr_value.value);
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
        }

    }
   
}
