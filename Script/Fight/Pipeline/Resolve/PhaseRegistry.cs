using System.Collections.Generic;
using Cfg.Enum;

namespace Aquila.Combat.Resolve
{
    public sealed class PhaseRegistry
    {
        private readonly Dictionary<ResolvePhaseType, IResolvePhaseHandler> _handlers = new Dictionary<ResolvePhaseType, IResolvePhaseHandler>(16);

        public PhaseRegistry()
        {
            Register(new ValidityPhaseHandler());
            Register(new HitCheckPhaseHandler());
            Register(new BaseValuePhaseHandler());
            Register(new OffenseModsPhaseHandler());
            Register(new DefenseModsPhaseHandler());
            Register(new CritPhaseHandler());
            Register(new BlockPhaseHandler());
            Register(new ShieldPhaseHandler());
            Register(new HpApplyPhaseHandler());
            Register(new PostEffectsPhaseHandler());
            Register(new LifecycleCheckPhaseHandler());
        }

        public bool TryGetHandler(ResolvePhaseType phase, out IResolvePhaseHandler handler)
        {
            return _handlers.TryGetValue(phase, out handler);
        }

        private void Register(IResolvePhaseHandler handler)
        {
            if (handler == null)
                return;

            _handlers[handler.PhaseType] = handler;
        }
    }
}
