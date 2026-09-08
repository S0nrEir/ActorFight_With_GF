using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 为actor添加tag
    /// </summary>
    public class EffectSpec_Period_ActorTag : EffectSpec_Base
    {
        public override void OnEffectAwake(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            if (_awakeProcessed)
                return;

            _awakeProcessed = true;
            if (_effectData.GetEffectId() <= 0 || target == null || target.Actor == null || GameEntry.Tag == null)
            {
                Tools.Logger.Warning($"[EffectSpec_Period_ActorTag] Invalid effect/target/tag component, effectId:{_effectData.GetEffectId()}");
                return;
            }

            var mainTypeValue = _effectData.GetIntParam1();
            var subTypeValue = _effectData.GetIntParam2();
            var sourceHashCode = GetHashCode();
            if (mainTypeValue < 0 || mainTypeValue >= (int)ActorTagType.Max || subTypeValue < 0 || subTypeValue >= 32 ||
                target.Actor.ActorID <= 0 || sourceHashCode == 0)
            {
                Tools.Logger.Warning($"[EffectSpec_Period_ActorTag] Invalid tag parameters, effectId:{_effectData.GetEffectId()}, actorId:{target.Actor.ActorID}, mainType:{mainTypeValue}, subType:{subTypeValue}");
                return;
            }

            _targetActorId = target.Actor.ActorID;
            _mainType = (ActorTagType)mainTypeValue;
            _subType = (ushort)subTypeValue;
            _sourceHashCode = sourceHashCode;

            var wasPresent = GameEntry.Tag.HasTag(_targetActorId, _mainType, _subType);
            _tagAcquired = GameEntry.Tag.AcquireTag(_targetActorId, _mainType, _subType, _sourceHashCode);
            if (!_tagAcquired)
                return;

            base.OnEffectAwake(castor, target);
            if (!wasPresent)
                Tools.Logger.Info($"[EffectSpec_Period_ActorTag] Add tag, actorId:{_targetActorId}, mainType:{_mainType}, subType:{_subType}");
        }

        public override void OnEffectEnd(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            base.OnEffectEnd(castor, target);
            if (!_tagAcquired || GameEntry.Tag == null)
                return;

            var released = GameEntry.Tag.ReleaseTag(_targetActorId, _mainType, _subType, _sourceHashCode);
            _tagAcquired = false;
            if (released && !GameEntry.Tag.HasTag(_targetActorId, _mainType, _subType))
                Tools.Logger.Info($"[EffectSpec_Period_ActorTag] Remove tag, actorId:{_targetActorId}, mainType:{_mainType}, subType:{_subType}");
        }

        public override void Clear()
        {
            _targetActorId = 0;
            _mainType = default;
            _subType = 0;
            _sourceHashCode = 0;
            _tagAcquired = false;
            _awakeProcessed = false;
            base.Clear();
        }

        private int _targetActorId;
        private ActorTagType _mainType;
        private ushort _subType;
        private int _sourceHashCode;
        private bool _tagAcquired;
        private bool _awakeProcessed;
    }
}
