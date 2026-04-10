using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 格挡：判定格挡触发情况，计算格挡减免值 / Block: determines block trigger and calculates blocked damage reduction.
    /// </summary>
    internal sealed class BlockPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Block;
    }
}
