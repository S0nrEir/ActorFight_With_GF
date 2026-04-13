using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 鐢熷懡鍛ㄦ湡妫€鏌ワ細妫€鏌ョ洰鏍?HP 鏄惁褰掗浂锛岃Е鍙戞浜℃祦绋嬫垨娑堣€楀娲绘満浼?/ Lifecycle check: checks if target HP reached zero to trigger death or revive.
    /// </summary>
    internal sealed class LifecycleCheckPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.LifecycleCheck;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("lifecycle_check_invalid_context");
                return;
            }

            context.LifecycleCheckIo.Input = context.FinalDelta;
            context.LifecycleCheckIo.Output = context.FinalDelta;
            result.SetContinue();
        }
    }
}