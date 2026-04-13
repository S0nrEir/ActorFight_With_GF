using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 鎶ょ浘鍚告敹锛氭寜浼樺厛绾у簭鍒楄绠楁姢鐩炬墸闄ら€昏緫 / Shield absorption: absorbs remaining damage by shield priority order.
    /// </summary>
    internal sealed class ShieldPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Shield;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("shield_invalid_context");
                return;
            }

            context.ShieldIo.Input = context.FinalDelta;
            context.ShieldIo.Output = context.FinalDelta;
            context.ShieldAbsorb = context.ShieldIo.Input - context.ShieldIo.Output;
            result.SetContinue();
        }
    }
}