using System;
using System.Collections.Generic;
using Aquila.Fight.Addon;

namespace Aquila.Combat.Resolve
{
    public interface IResolvePhaseHandler
    {
        ResolvePhaseType PhaseType { get; }
        void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result);
    }

    internal abstract class ResolvePhaseHandlerBase : IResolvePhaseHandler
    {
        public abstract ResolvePhaseType PhaseType { get; }

        public virtual void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            result.SetContinue();
        }
    }
}
