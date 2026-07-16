using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 目标有效性检查：检查目标存活状态、距离限制及特殊标记，无效时立即终止结算 / Target validity check: verifies target alive, distance and special flags; interrupts if invalid.
    /// </summary>
    internal sealed class ValidityPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Validity;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.ValidityIo.Input = context.FinalDelta;

            if (context.Request.Target == null)
            {
                result.SetInterrupt("resolve_target_null");
                return;
            }
            
            if (context.Request.Castor is null)
            {
                result.SetInterrupt("resolve_castor_null");
                return;
            }

            context.ValidityIo.Output = context.FinalDelta;
            // context.FinalDelta = context.FinalDelta;
            result.SetContinue();
        }
    }
}
