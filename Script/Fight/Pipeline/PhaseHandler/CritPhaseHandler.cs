 using Aquila.Toolkit;
 using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 暴击判定：通过阶段公式计算是否触发暴击，并写入 ResolveContext 位标记。
    /// </summary>
    internal sealed class CritCheckPhaseHandler : ResolvePhaseHandlerBase
    {
        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context._CritIo.Input = context.FinalDelta;
            if (!TryEvaluatePhaseFormula(context, result, out var computed))
                return;

            context._CritIo.Output = computed + context.FinalDelta;
            context.FinalDelta = computed + context.FinalDelta;
            result.SetContinue();
        }
        
        public override ResolvePhaseType PhaseType => ResolvePhaseType.CritCheck;
    }

    /// <summary>
    /// 暴击：判定暴击触发情况并计算暴击倍率 / Critical hit: determines crit trigger and calculates crit multiplier.
    /// </summary>
    internal sealed class CritPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Crit;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.CritIo.Input  = context.FinalDelta;
            if (!context.HasPhaseFlag(ResolvePhaseFlags.CritTriggered))
            {
                context.CritIo.Output = context.CritIo.Input;
                context.CritIncrease = 0f;
                result.SetSkip();
                return;
            }

            if (!TryEvaluatePhaseFormula(context, result, out var computed))
                return;

            context.CritIo.Output = computed;
            context.FinalDelta = computed;
            context.CritIncrease  = context.CritIo.Output - context.CritIo.Input;
            result.SetContinue();
        }
    }
}
