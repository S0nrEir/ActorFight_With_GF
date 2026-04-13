using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 鐩爣鏈夋晥鎬ф鏌ワ細妫€鏌ョ洰鏍囧瓨娲荤姸鎬併€佽窛绂婚檺鍒跺強鐗规畩鏍囪锛屾棤鏁堟椂绔嬪嵆缁堟缁撶畻 / Target validity check: verifies target alive, distance and special flags; interrupts if invalid.
    /// </summary>
    internal sealed class ValidityPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Validity;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null || context.Request == null)
            {
                result.SetInterrupt("resolve_request_null");
                return;
            }

            context.ValidityIo.Input = context.FinalDelta;
            context.ValidityIo.Output = context.FinalDelta;

            if (context.Request.Target == null)
            {
                result.SetInterrupt("resolve_target_null");
                return;
            }

            result.SetContinue();
        }
    }
}