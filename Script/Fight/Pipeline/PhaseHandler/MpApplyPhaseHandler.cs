using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 蓝量写入：HP结算后将MP消耗净值同步至施法者属性组件 / Mp apply: writes mp cost delta to castor attribute addon after hp apply.
    /// </summary>
    internal sealed class MpApplyPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.MpApply;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.MpApplyIo.Input = context.FinalDelta;

            var mpCost = context.FinalDelta;
            if (mpCost <= 0f)
            {
                context.MpApplyIo.Output = 0f;
                result.SetContinue();
                return;
            }

            var addon = context.Request.Castor?.GetAddon<Addon_BaseAttrNumric>();

            var currMp = addon.GetCurrMPCorrection();
            var mpToSet = currMp - mpCost;
            var succAndVal = addon.SetCurrMP(mpToSet);
            if (!succAndVal.setSucc)
            {
                result.SetInterrupt("mp_apply_failed_to_set_mp");
                return;
            }

            context.MpApplyIo.Output = context.FinalDelta;
            context.AppliedMpDelta = mpCost;
            result.SetContinue();
        }
    }
}
