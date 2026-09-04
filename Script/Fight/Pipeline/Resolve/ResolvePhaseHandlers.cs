using Aquila.Fight.Addon;
using Cfg.Enum;
using UnityEngine;

namespace Aquila.Combat.Resolve
{
    internal static class ResolvePhaseHandlers
    {
    }

    internal sealed class ResolveEndPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.ResolveEnd;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.ResolveEndIo.Input = context.FinalDelta;
            context.ResolveEndIo.Output = context.FinalDelta;
            var target = context.Request.Target;
            target.GetAddon<Addon_HP>().Refresh();
            GameEntry.InfoBoard.ShowDamageNumber( context.FinalDelta.ToString(), target.Actor.CachedTransform.position );
            result.SetContinue();
        }
    }

    internal sealed class MpResolveEndPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.MpResolveEnd;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.MpResolveEndIo.Input = context.FinalDelta;
            context.MpResolveEndIo.Output = context.FinalDelta;

            var castor = context.Request.Castor;
            var worldPos = castor?.Actor?.CachedTransform != null
                ? castor.Actor.CachedTransform.position
                : Vector3.zero;

            // if (castor?.Actor?.CachedTransform != null)
            //     GameEntry.InfoBoard.ShowMpNumber($"-{Mathf.Abs(context.FinalDelta)}", worldPos);

            result.SetContinue();
        }
    }

    /// <summary>
    /// 伤害结算中的无状态数值规则。
    /// </summary>
    public static class CombatDamageCalculator
    {
        public static float ApplyDefense(
            float inputDelta,
            float physicalDefense,
            float magicDefense,
            ResolveSourceType sourceType)
        {
            var damage = Mathf.Max(0f, inputDelta);
            if (sourceType == ResolveSourceType.TrueDamage)
                return damage;

            var defense = sourceType == ResolveSourceType.MagicDamage
                ? magicDefense
                : physicalDefense;
            return Mathf.Max(0f, damage - defense);
        }

        public static float NormalizeDamage(float inputDelta)
        {
            return Mathf.Floor(Mathf.Max(0f, inputDelta));
        }
    }
}
