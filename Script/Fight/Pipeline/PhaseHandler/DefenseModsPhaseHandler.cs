using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 防御修正：应用受击方的防御力减免、百分比减伤 / Defense modifiers: applies defender damage reduction from defense and % reduction.
    /// </summary>
    internal sealed class DefenseModsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.DefenseMods;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("defense_mods_invalid_context");
                return;
            }

            context.DefenseModsIo.Input = context.FinalDelta;
            context.DefenseModsIo.Output = context.FinalDelta;
            context.DefenseReduction = context.DefenseModsIo.Input - context.DefenseModsIo.Output;
            result.SetContinue();
        }
    }
}