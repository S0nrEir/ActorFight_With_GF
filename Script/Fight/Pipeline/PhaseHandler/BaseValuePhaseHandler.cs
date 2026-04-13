using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 基础值：从配置或效果数据中提取原始数值，作为后续修正的基数/ Base value: extracts raw value from config or effect data as the base for modifiers.
    /// </summary>
    internal sealed class BaseValuePhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.BaseValue;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("base_value_invalid_context");
                return;
            }

            context.BaseValueIo.Input  = context.FinalDelta;
            context.BaseValueAmount    = context.FinalDelta;
            context.BaseValueIo.Output = context.FinalDelta;
            result.SetContinue();
        }
    }
}