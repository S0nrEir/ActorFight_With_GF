using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 命中裁定：基于命中率与闪避率进行随机判定，Miss 时跳过数值阶段 / Hit check: roll against hit rate vs evasion; skip value phases on miss.
    /// </summary>
    internal sealed class HitCheckPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.HitCheck;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.HitCheckIo.Input = context.FinalDelta;
            if (!TryEvaluatePhaseFormula(context, result, out var computed))
                return;

            //#todo:100改成常量值
            if(computed >= 100)
                result.SetAbort("miss");
            
            context.HitCheckIo.Output = computed;
            context.FinalDelta = computed;
            result.SetContinue();
        }
    }
}
