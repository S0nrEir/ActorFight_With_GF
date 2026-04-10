using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 基础值：从配置或效果数据中提取原始数值，作为后续修正的基数 / Base value: extracts raw value from config or effect data as the base for modifiers.
    /// </summary>
    internal sealed class BaseValuePhaseHandler : ResolvePhaseHandlerBase
    {
        public override ResolvePhaseType PhaseType => ResolvePhaseType.BaseValue;
    }
}
