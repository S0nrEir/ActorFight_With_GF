using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 进攻修正：应用攻击方的百分比增伤、属性转化等加成 / Offense modifiers: applies attacker bonuses such as damage % buffs and attribute conversions.
    /// </summary>
    internal sealed class OffenseModsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.OffenseMods;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.OffenseModsIo.Input = context.FinalDelta;
            if (!TryEvaluatePhaseFormula(context, result, out var computed))
                return;

            context.OffenseModsIo.Output = computed;
            context.FinalDelta = computed;
            context.OffenseIncrease = context.OffenseModsIo.Output - context.OffenseModsIo.Input;
            result.SetContinue();
        }
    }
}
