using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 护盾吸收：按优先级序列计算护盾扣除逻辑 / Shield absorption: absorbs remaining damage by shield priority order.
    /// </summary>
    internal sealed class ShieldPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Shield;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.ShieldIo.Input = context.FinalDelta;
            if (!TryEvaluatePhaseFormula(context, result, out var computed))
                return;

            context.ShieldIo.Output = computed;
            context.FinalDelta = computed;
            context.ShieldAbsorb = context.ShieldIo.Input - context.ShieldIo.Output;
            result.SetContinue();
        }
    }
}
