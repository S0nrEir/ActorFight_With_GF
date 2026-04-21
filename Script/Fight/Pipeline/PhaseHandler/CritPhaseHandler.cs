using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 暴击：判定暴击触发情况并计算暴击倍率 / Critical hit: determines crit trigger and calculates crit multiplier.
    /// </summary>
    internal sealed class CritPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Crit;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.CritIo.Input  = context.FinalDelta;
            context.CritIo.Output = context.FinalDelta;
            context.CritIncrease  = context.CritIo.Output - context.CritIo.Input;
            result.SetContinue();
        }
    }
}