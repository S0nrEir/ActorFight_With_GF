using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 进攻修正：应用攻击方的百分比增伤、属性转化等加成 / Offense modifiers: applies attacker bonuses such as damage % buffs and attribute conversions.
    /// </summary>
    internal sealed class OffenseModsPhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.OffenseMods;
    }
}
