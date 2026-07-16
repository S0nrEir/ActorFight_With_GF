using GameFramework;
using GameFramework.Event;

namespace Aquila.Event
{
    public class EventArg_OnHitAbility : GameEventArgs
    {
        public static readonly int EventID = typeof(EventArg_OnHitAbility).GetHashCode();

        public static EventArg_OnHitAbility Create(int castorID, int targetActorID, int abilityID, bool succ)
        {
            var arg = ReferencePool.Acquire<EventArg_OnHitAbility>();
            arg._castorID = castorID;
            arg._targetActorID = targetActorID;
            arg._abilityID = abilityID;
            arg._succ = succ;
            return arg;
        }

        public override void Clear()
        {
            _castorID = -1;
            _targetActorID = -1;
            _abilityID = -1;
            _succ = false;
        }

        public int _castorID = -1;
        public int _targetActorID = -1;
        public int _abilityID = -1;
        public bool _succ;

        public override int Id => EventID;
    }
}