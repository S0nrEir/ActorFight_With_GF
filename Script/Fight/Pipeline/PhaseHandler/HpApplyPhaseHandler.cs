using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 鐢熷懡鍐欏叆锛氬皢鏈€缁堝噣鍊煎悓姝ヨ嚦灞炴€х粍浠讹紝閫氳繃 Addon_BaseAttrNumric 鎵ц HP 鍔犲噺 / HP apply: writes final delta to attribute addon and triggers HP change signal.
    /// </summary>
    internal sealed class HpApplyPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.HpApply;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null || context.Request == null || context.Request.Target == null)
            {
                result.SetInterrupt("hp_apply_invalid_request");
                return;
            }

            context.HpApplyIo.Input = context.FinalDelta;

            var addon = context.Request.Target.GetAddon<Addon_BaseAttrNumric>();
            if (addon == null)
            {
                result.SetInterrupt("hp_apply_missing_addon");
                return;
            }

            var currHp = addon.GetCurrHPCorrection();
            var nextHp = currHp + context.FinalDelta;
            addon.SetCurrHP(nextHp);
            context.AppliedHpDelta = context.FinalDelta;
            context.HpApplyIo.Output = context.FinalDelta;
            context.HasApplied = true;
            result.SetContinue();
        }
    }
}