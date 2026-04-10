using Aquila.Fight.Addon;

namespace Aquila.Combat.Resolve
{
    internal sealed class HpApplyPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.HpApply;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null || context.Request == null || context.Request.Target == null)
            {
                result.SetInterrupt("hp_apply_invalid_request");
                return;
            }

            var addon = context.Request.Target.GetAddon<Addon_BaseAttrNumric>();
            if (addon == null)
            {
                result.SetInterrupt("hp_apply_missing_addon");
                return;
            }

            var currHp = addon.GetCurrHPCorrection();
            var nextHp = currHp + context.FinalDelta;
            addon.SetCurrHP(nextHp);
            context.HasApplied = true;
            result.SetContinue();
        }
    }
}
