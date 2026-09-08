using Aquila.Event;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 吟唱effect
    /// </summary>
    public class EffectSpec_Period_WindUp : EffectSpec_Base
    {
        public override void OnEffectAwake(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            if (_awakeProcessed)
                return;

            _awakeProcessed = true;
            if (target == null || target.Actor == null || GameEntry.Tag == null)
            {
                Tools.Logger.Warning($"[EffectSpec_Period_WindUp] Invalid target/tag component, effectId:{_effectData.GetEffectId()}");
                return;
            }

            var sourceHashCode = GetHashCode();
            var actorId = target.Actor.ActorID;
            if (actorId <= 0 || sourceHashCode == 0)
            {
                Tools.Logger.Warning($"[EffectSpec_Period_WindUp] Invalid target/source, effectId:{_effectData.GetEffectId()}, actorId:{actorId}");
                return;
            }

            _targetActorId = actorId;
            _sourceHashCode = sourceHashCode;
            _mainType = ActorTagType.Ability;
            _subType = (ushort)ActorTagSubType_Ability.WindUp;

            var wasPresent = GameEntry.Tag.HasTag(_targetActorId, _mainType, _subType);
            _tagAcquired = GameEntry.Tag.AcquireTag(_targetActorId, _mainType, _subType, _sourceHashCode);
            if (_tagAcquired && !wasPresent)
                GameEntry.Event.Fire(this, EventArg_WindUp.CreateStartEventArg(_effectData.GetDuration(), _targetActorId));
        }

        public override void Apply(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            OnEffectAwake(castor, target);
        }

        public override void OnEffectEnd(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            if (!_tagAcquired || GameEntry.Tag == null)
                return;

            var released = GameEntry.Tag.ReleaseTag(_targetActorId, _mainType, _subType, _sourceHashCode);
            _tagAcquired = false;
            if (released && !GameEntry.Tag.HasTag(_targetActorId, _mainType, _subType))
                GameEntry.Event.Fire(this, EventArg_WindUp.CreateStopEventArg());
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
