using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 格挡：判定格挡触发情况，计算格挡减免值 / Block: determines block trigger and calculates blocked damage reduction.
    /// </summary>
    internal sealed class BlockPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Block;

        public override void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            context.BlockIo.Input  = context.FinalDelta;
            context.BlockIo.Output = context.FinalDelta;
            context.BlockReduction = context.BlockIo.Input - context.BlockIo.Output;
            result.SetContinue();
        }
    }
}