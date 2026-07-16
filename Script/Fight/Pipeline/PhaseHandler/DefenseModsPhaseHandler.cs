using Aquila.Fight.Addon;
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
            context.DefenseModsIo.Input = context.FinalDelta;
            // if (!TryEvaluatePhaseFormula(context, result, out var computed))
            //     return;

            var attrAddon = context.Request.Target.GetAddon<Addon_BaseAttrNumric>();
            var def = attrAddon.GetCorrectionValue(actor_attribute.DEF, 0);
            context.DefenseModsIo.Output = context.FinalDelta - def;
            context.FinalDelta = context.DefenseModsIo.Output;
            context.DefenseReduction = context.DefenseModsIo.Input - context.DefenseModsIo.Output;
            result.SetContinue();
        }
    }
}
