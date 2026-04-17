using Aquila.Combat.Resolve;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 物理伤害，立即生效，目标敌方单体，伤害生命值，参与属性和装备计算。
    /// </summary>
    public class EffectSpec_Instant_PhyDamage : EffectSpec_Base
    {
        public override void Apply(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            var attrAddon = target.GetAddon<Addon_BaseAttrNumric>();
            if (attrAddon is null)
            {
                Tools.Logger.Warning("<color=red>EffectSpec_Damage--->attr_addon is null</color>");
                return;
            }

            var resolveResult = CombatResolveEntry.Resolve(castor, target, this, 0f, ResolveSourceType.EffectDirect);
            if (!resolveResult.Success)
            {
                Tools.Logger.Error($"[EffectSpec_Instant_PhyDamage] Resolve failed. Interrupted={resolveResult.Interrupted}, Aborted={resolveResult.Aborted}");
            }
        }

        public EffectSpec_Instant_PhyDamage()
        {
        }

        public EffectSpec_Instant_PhyDamage(EffectData meta)
        {
        }
    }
}