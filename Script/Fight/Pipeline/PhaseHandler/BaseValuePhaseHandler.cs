using Aquila.Formula;
using Aquila.Toolkit;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 鍩虹鍊硷細浠庨厤缃垨鏁堟灉鏁版嵁涓彁鍙栧師濮嬫暟鍊硷紝浣滀负鍚庣画淇鐨勫熀鏁? Base value: extracts raw value from config or effect data as the base for modifiers.
    /// </summary>
    internal sealed class BaseValuePhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.BaseValue;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null || context.Request == null)
            {
                result.SetInterrupt("base_value_invalid_context");
                return;
            }

            var formulaId = context.Request.EffectData.GetFormulaID();
            if (formulaId <= 0)
            {
                Tools.Logger.Error($"[Resolve] FormulaID invalid: {formulaId}, EffectID={context.Request.EffectData.GetEffectId()}");
                result.SetInterrupt("formula_id_invalid");
                return;
            }

            if (!FormulaEngine.Instance.TryEvaluate(formulaId, context.FormulaContext, context, out var computed, out var reason))
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
