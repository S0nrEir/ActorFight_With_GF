using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 生命写入：将最终净值同步至属性组件，通过 Addon_BaseAttrNumric 执行 HP 加减 / HP apply: writes final delta to attribute addon and triggers HP change signal.
    /// </summary>
    internal sealed class HpApplyPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.HpApply;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.HpApplyIo.Input = context.FinalDelta;
            // if (!TryEvaluatePhaseFormula(context, result, out var computed))
            //     return;

            // context.HpApplyIo.Output = computed;
            // context.FinalDelta = computed;

            var addon = context.Request.Target.GetAddon<Addon_BaseAttrNumric>();
            if (addon == null)
            {
                result.SetInterrupt("hp_apply_missing_addon");
                return;
            }

            var currHp = addon.GetCurrHPCorrection();
            var hpToSet = currHp - context.FinalDelta;
            var succAndVal = addon.SetCurrHP(hpToSet);
            if (!succAndVal.setSucc)
            {
                result.SetInterrupt("hp_apply_failed_to_set_hp");
                return;
            }
            
            context.HpApplyIo.Output = context.FinalDelta;
            context.AppliedHpDelta = hpToSet;
            context.HasApplied = true;
            result.SetContinue();
        }
    }
}
