using Aquila.Formula;
using Aquila.Toolkit;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 基础值：从配置或效果数据中提取原始数值，作为后续修正的基数 / Base value: extracts raw value from config or effect data as the base for modifiers.
    /// </summary>
    internal sealed class BaseValuePhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.BaseValue;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            var formulaId = context.Request.EffectData.GetFormulaID();
            if (formulaId <= 0)
            {
                Tools.Logger.Error($"[Resolve] FormulaID invalid: {formulaId}, EffectID={context.Request.EffectData.GetEffectId()}");
                result.SetInterrupt("formula_id_invalid");
                return;
            }

            if (!FormulaEngine.Instance.TryEvaluate(formulaId, context, out var computed, out var reason))
            {
                Tools.Logger.Error($"[Resolve] Formula evaluate failed. formulaID={formulaId}, reason={reason}");
                result.SetInterrupt("formula_evaluate_failed");
                return;
            }

            context.BaseValueIo.Input = context.FinalDelta;
            context.BaseValueAmount = computed;
            context.FinalDelta = computed;
            context.BaseValueIo.Output = computed;
            result.SetContinue();
        }
    }
}
