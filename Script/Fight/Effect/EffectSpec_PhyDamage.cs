using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.common;
using Cfg.Enum;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 物理伤害，立即生效，目标敌方单体，伤害生命值，参与属性和装备计算。
    /// </summary>
    public class EffectSpec_PhyDamage : EffectSpec_Base
    {
        public override void Apply(Module_Proxy_Actor.ActorInstance instance,ref AbilityHitResult result)
        {
            var attr_addon = instance.GetAddon<Addon_BaseAttrNumric>();
            if (attr_addon is null)
            {
                Log.Warning("<color=red>EffectSpec_Damage--->attr_addon is null</color>");
                return;
            }

            var res = attr_addon.GetCorrectionFinalValue(Actor_Attr.Curr_HP);
            if (!res.get_succ)
            {
                Log.Warning("<color=red>EffectSpec_Damage--->!res.get_succ</color>");
                return;
            }

            var def = attr_addon.GetCorrectionFinalValue(Actor_Attr.DEF);
            
            var final_value = Tools.Fight.CalcPhysicDamage( Meta.ModifierNumric, def.value );
            attr_addon.SetBaseValue(Meta.EffectType, final_value);
            result.AddDealedDamage((int)final_value);
        }

        public EffectSpec_PhyDamage(Effect meta) : base(meta)
        {
        }
        
    }
   
}
