namespace Aquila.Combat.Resolve
{
    internal sealed class ValidityPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Validity;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null || context.Request == null)
            {
                result.SetInterrupt("resolve_request_null");
                return;
            }

            if (context.Request.Target == null)
            {
                result.SetInterrupt("resolve_target_null");
                return;
            }

            result.SetContinue();
        }
    }
}
