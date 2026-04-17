using Aquila.Combat.Resolve;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 周期性固定伤害
    /// </summary>
    public class EffectSpec_Period_FixedDamage : EffectSpec_Base
    {
        public override void Init(EffectData data, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null)
        {
            base.Init(data, castor, target);
            _modifier.Setup(Meta.GetModifierType(), _effectData.GetFloatParam1());
        }

        public override void Apply(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            var addon = target.GetAddon<Addon_BaseAttrNumric>();
            if (addon is null)
            {
                Tools.Logger.Warning("EffectSpec_PeriodFixedDamage.Apply()--->addon is null");
                return;
            }

            var resolveResult = CombatResolveEntry.Resolve(castor, target, this, 0f, ResolveSourceType.PoisonDamage);
            if (!resolveResult.Success)
            {
                Tools.Logger.Error($"[EffectSpec_Period_FixedDamage] Resolve failed. Interrupted={resolveResult.Interrupted}, Aborted={resolveResult.Aborted}");
            }
        }
    }
}