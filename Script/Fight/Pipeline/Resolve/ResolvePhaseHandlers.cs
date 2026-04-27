using Aquila.Fight.Addon;
using Cfg.Enum;

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
}
