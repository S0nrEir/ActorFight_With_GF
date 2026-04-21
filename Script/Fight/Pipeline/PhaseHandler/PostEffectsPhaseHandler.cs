using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 次生效果：处理吸血、反伤、触发类等基于最终结算数值产生的次生效果 / Post effects: processes lifesteal, reflect, and trigger-based secondary effects.
    /// </summary>
    internal sealed class PostEffectsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.PostEffects;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.PostEffectsIo.Input = context.FinalDelta;
            context.PostEffectsIo.Output = context.FinalDelta;
            result.SetContinue();
        }
    }
}