using Aquila.Fight.Addon;
using Aquila.Module;
using Cfg.Common;
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

            var cur_hp = attr_addon.GetCurrHPCorrection();
            var final = cur_hp + Meta.ExtensionParam.FloatParam_1;
            attr_addon.SetCurrHP(final);
            result.AddDealedDamage(Meta.ExtensionParam.FloatParam_1);
        }

        public EffectSpec_PhyDamage(Table_Effect meta) : base(meta)
        {
        }
    }
}
