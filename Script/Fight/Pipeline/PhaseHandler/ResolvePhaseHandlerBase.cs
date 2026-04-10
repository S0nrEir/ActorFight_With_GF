using System;
using System.Collections.Generic;
using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 结算阶段处理器接口 / Resolve phase handler interface.
    /// </summary>
    public interface IResolvePhaseHandler
    {
        ResolvePhaseType PhaseType { get; }
        void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result);
    }

    /// <summary>
    /// 结算阶段处理器基类，提供默认的继续执行行为 / Base class for resolve phase handlers with default continue behavior.
    /// </summary>
    internal abstract class ResolvePhaseHandlerBase : IResolvePhaseHandler
    {
        public abstract ResolvePhaseType PhaseType { get; }

        public virtual void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            result.SetContinue();
        }
    }
}
