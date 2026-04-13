using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 娆＄敓鏁堟灉锛氬鐞嗗惛琛€銆佸弽浼ゃ€佽Е鍙戠被绛夊熀浜庢渶缁堢粨绠楁暟鍊间骇鐢熺殑娆＄敓鏁堟灉 / Post effects: processes lifesteal, reflect, and trigger-based secondary effects.
    /// </summary>
    internal sealed class PostEffectsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.PostEffects;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            if (context == null)
            {
                result.SetInterrupt("post_effects_invalid_context");
                return;
            }

            context.PostEffectsIo.Input = context.FinalDelta;
            context.PostEffectsIo.Output = context.FinalDelta;
            result.SetContinue();
        }
    }
}