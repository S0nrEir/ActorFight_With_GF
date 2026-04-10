namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 暴击：判定暴击触发情况并计算暴击倍率 / Critical hit: determines crit trigger and calculates crit multiplier.
    /// </summary>
    internal sealed class CritPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.Crit;
    }
}
