using Aquila.Fight;
using Aquila.Module;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Combat.Resolve
{
    public sealed class ResolveRequest : IReference
    {
        public static ResolveRequest Create(
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target,
            EffectSpec_Base effectSpec,
            int resolveTypeId)
        {
            var request = ReferencePool.Acquire<ResolveRequest>();
            request.Castor = castor;
            request.Target = target;
            request.EffectSpec = effectSpec;
            request.EffectData = effectSpec.Meta;
            request.InputDelta = 0f;
            request.ResolveTypeId = resolveTypeId;
            request.SourceType = ResolveSourceType.Unknown;
            return request;
        }

        public static ResolveRequest Create(
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target,
            EffectSpec_Base effectSpec,
            float inputDelta,
            int resolveTypeId,
            ResolveSourceType sourceType)
        {
            var request = Create(
                castor,
                target,
                effectSpec,
                resolveTypeId);

            request.InputDelta = inputDelta;
            request.SourceType = sourceType;
            return request;
        }

        public Module_ProxyActor.ActorInstance Castor { get; private set; }
        public Module_ProxyActor.ActorInstance Target { get; private set; }
        public EffectSpec_Base EffectSpec { get; private set; }
        public EffectData EffectData { get; private set; }
        public float InputDelta { get; private set; }
        public int ResolveTypeId { get; private set; }
        public ResolveSourceType SourceType { get; private set; }

        public void Clear()
        {
            Castor = null;
            Target = null;
            EffectSpec = null;
            EffectData = default;
            InputDelta = 0f;
            ResolveTypeId = 0;
            SourceType = ResolveSourceType.Unknown;
        }
    }
}