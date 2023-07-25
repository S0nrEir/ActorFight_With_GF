using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Common;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 物理伤害，立即生效，目标敌方单体，伤害生命值，参与属性和装备计算。
    /// </summary>
    public class EffectSpec_Instant_PhyDamage : EffectSpec_Base
    {
        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            var attr_addon = target.GetAddon<Addon_BaseAttrNumric>();
            if (attr_addon is null)
            {
                Log.Warning("<color=red>EffectSpec_Damage--->attr_addon is null</color>");
                return;
            }

            var cur_hp = attr_addon.GetCurrHPCorrection();
            var final = cur_hp + Meta.ExtensionParam.FloatParam_1;
            attr_addon.SetCurrHP(final);
            result._dealedDamage = Tools.Fight.AddDealedDamage( result._dealedDamage, (int)Meta.ExtensionParam.FloatParam_1 );
        }

        public EffectSpec_Instant_PhyDamage()
        {

        }
        public EffectSpec_Instant_PhyDamage( Table_Effect meta ) : base( meta )
        {
        }
    }
}
