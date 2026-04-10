using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 防御修正：应用受击方的防御力减免、百分比减伤 / Defense modifiers: applies defender damage reduction from defense and % reduction.
    /// </summary>
    internal sealed class DefenseModsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.DefenseMods;
    }
}
