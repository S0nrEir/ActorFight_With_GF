using Aquila.Combat.Resolve;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 即时伤害效果共用逻辑：将配置值转换为正向生命扣除量后进入统一结算管线。
    /// </summary>
    public abstract class EffectSpec_Instant_Damage : EffectSpec_Base
    {
        protected abstract ResolveSourceType DamageSourceType { get; }

        public override void Apply(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            if (target == null)
            {
                Tools.Logger.Warning($"[{GetType().Name}] target is null");
                return;
            }

            var attrAddon = target.GetAddon<Addon_BaseAttrNumric>();
            if (attrAddon == null)
            {
                Tools.Logger.Warning($"[{GetType().Name}] target attribute addon is null");
                return;
            }

            var inputDelta = Mathf.Abs(Meta.GetFloatParam1());
            var resolveResult = CombatResolveEntry.Resolve(castor, target, this, inputDelta, DamageSourceType);
            if (!resolveResult.Success)
            {
                Tools.Logger.Error($"[{GetType().Name}] Resolve failed. Interrupted={resolveResult.Interrupted}, Aborted={resolveResult.Aborted}");
            }
        }
    }

    /// <summary>
    /// 物理伤害，立即生效，目标敌方单体，伤害生命值，参与属性和装备计算。
    /// </summary>
    public class EffectSpec_Instant_PhyDamage : EffectSpec_Instant_Damage
    {
        protected override ResolveSourceType DamageSourceType => ResolveSourceType.EffectDirect;

        public EffectSpec_Instant_PhyDamage()
        {
        }

        public EffectSpec_Instant_PhyDamage(EffectData meta)
        {
        }
    }

    /// <summary>
    /// 魔法伤害，立即生效，按目标魔抗进行平减。
    /// </summary>
    public sealed class EffectSpec_Instant_MagicDamage : EffectSpec_Instant_Damage
    {
        protected override ResolveSourceType DamageSourceType => ResolveSourceType.MagicDamage;

        public EffectSpec_Instant_MagicDamage()
        {
        }

        public EffectSpec_Instant_MagicDamage(EffectData meta)
        {
        }
    }

    /// <summary>
    /// 纯粹伤害，立即生效，跳过物理与魔法防御修正。
    /// </summary>
    public sealed class EffectSpec_Instant_TrueDamage : EffectSpec_Instant_Damage
    {
        protected override ResolveSourceType DamageSourceType => ResolveSourceType.TrueDamage;

        public EffectSpec_Instant_TrueDamage()
        {
        }

        public EffectSpec_Instant_TrueDamage(EffectData meta)
        {
        }
    }
}