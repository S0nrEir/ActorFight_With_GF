using GameFramework;
using GameFramework.Event;

namespace Aquila.Event
{
    public class EventArg_OnHitAbility : GameEventArgs
    {
        public static readonly int EventID = typeof(EventArg_OnHitAbility).GetHashCode();

        public static EventArg_OnHitAbility Create(AbilityResult_Hit result)
        {
            var arg = ReferencePool.Acquire<EventArg_OnHitAbility>();
            arg._resultParam = result;
            return arg;
        }

        public override void Clear()
        {
            _resultParam = null;
        }

        public AbilityResult_Hit _resultParam = null;
        
        public override int Id => EventID;
    }
}
