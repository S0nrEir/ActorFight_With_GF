using Aquila.Fight;
using Aquila.Module;

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
            var typeId = GetResolveTypeId(effectSpec.Meta);
            // var sourceMeta = BuildSourceMeta(castor, target, effectSpec, sourceType);
            var request = ResolveRequest.Create(castor, target, effectSpec, typeId);
            return Resolver.Resolve(request);
        }

        private static readonly CombatResolver Resolver = new CombatResolver(new ResolvePhaseProvider(), new PhaseRegistry());
        
        // private static ResolveSourceMeta BuildSourceMeta(
        //     Module_ProxyActor.ActorInstance castor,
        //     Module_ProxyActor.ActorInstance target,
        //     EffectSpec_Base effectSpec,
        //     ResolveSourceType sourceType)
        // {
        //     var castorId = castor?.Actor?.ActorID ?? 0;
        //     var targetId = target?.Actor?.ActorID ?? 0;
        //
        //     ResolveSourceMeta effectMeta = default;
        //     var hasEffectMeta = effectSpec != null && effectSpec.TryGetResolveSourceMeta(out effectMeta);
        //     var hasScopeMeta = ResolveSourceScope.TryGetCurrent(out var scopeMeta);
        //
        //     if (sourceType != ResolveSourceType.Unknown)
        //     {
        //         var explicitMeta = hasEffectMeta ? effectMeta : (hasScopeMeta ? scopeMeta : default);
        //         explicitMeta.SourceType = sourceType;
        //         if (sourceType == ResolveSourceType.EffectDirect && explicitMeta.AbilityId <= 0 && explicitMeta.TriggerIndex == 0)
        //             explicitMeta.TriggerIndex = -1;
        //         
        //         return explicitMeta;
        //     }
        //
        //     return ResolveSourceScope.CreateDirect(castorId, targetId);
        // }

    }
}
