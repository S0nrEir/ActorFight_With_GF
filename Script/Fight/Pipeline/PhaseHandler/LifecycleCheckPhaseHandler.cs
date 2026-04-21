using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 生命周期检查：检查目标 HP 是否归零，触发死亡流程或消耗复活机会 / Lifecycle check: checks if target HP reached zero to trigger death or revive.
    /// </summary>
    internal sealed class LifecycleCheckPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.LifecycleCheck;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.LifecycleCheckIo.Input = context.FinalDelta;
            if (!TryEvaluatePhaseFormula(context, result, out var computed))
                return;

            context.LifecycleCheckIo.Output = computed;
            context.FinalDelta = computed;
            result.SetContinue();
        }
    }
}
