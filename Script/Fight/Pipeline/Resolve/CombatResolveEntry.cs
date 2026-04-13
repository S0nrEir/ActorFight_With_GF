using System.Collections.Generic;
using Aquila.Fight;
using Aquila.Module;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Combat.Resolve
{
    public static class CombatResolveEntry
    {
        public static int GetResolveTypeId(EffectData effectData)
        {
            var typeId = effectData.GetResolveTypeID();
            return typeId > 0 ? typeId : ResolvePhaseProvider.DefaultResolveTypeId;
        }

        public static ResolveResultData Resolve(
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target,
            EffectSpec_Base effectSpec,
            ResolveSourceType sourceType = ResolveSourceType.Unknown)
        {
            return Resolve(castor, target, effectSpec, 0f, sourceType);
        }

        public static ResolveResultData Resolve(
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target,
            EffectSpec_Base effectSpec,
            float inputDelta,
            ResolveSourceType sourceType = ResolveSourceType.Unknown)
        {
            var typeId = GetResolveTypeId(effectSpec.Meta);
            var request = ResolveRequest.Create(castor, target, effectSpec, inputDelta, typeId, sourceType);

            _phaseBuffer.Clear();
            PhaseProvider.TryGetPhases(typeId, _phaseBuffer);

            var context = ReferencePool.Acquire<ResolveContext>();
            context.Setup(request);
            context.ResetPhaseStates(_phaseBuffer);

            var result = Resolver.Resolve(request, context, _phaseBuffer);
            _phaseBuffer.Clear();
            return result;
        }

        private static readonly ResolvePhaseProvider PhaseProvider = new ResolvePhaseProvider();
        private static readonly CombatResolver Resolver = new CombatResolver(PhaseProvider, new PhaseRegistry());
        private static readonly List<ResolvePhaseDefinition> _phaseBuffer = new List<ResolvePhaseDefinition>(16);
    }
}