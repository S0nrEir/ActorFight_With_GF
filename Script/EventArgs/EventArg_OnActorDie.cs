using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Event
{
    public class EventArg_OnActorDie : GameEventArgs
    {
        public static EventArg_OnActorDie Create( int actorID )
        {
            var arg = ReferencePool.Acquire<EventArg_OnActorDie>();
            arg._actorID = actorID;
            return arg;
        }

        public static readonly int EventID = typeof( EventArg_OnActorDie ).GetHashCode();

        public override int Id => EventID;

        public override void Clear()
        {
            _actorID = -1;
        }

        public int _actorID = -1;
    }

}
