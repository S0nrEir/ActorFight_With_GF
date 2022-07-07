using Aquila.Fight.Actor;
using GameFramework.Event;

namespace Aquila.Event
{
    /// <summary>
    /// actor死亡事件广播
    /// </summary>
    public class ActorDieEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof( ActorDieEventArgs ).GetHashCode();

        public override int Id => EventId;

        public TActorBase Actor { get; private set; } = null;

        public override void Clear()
        {

        }


        public ActorDieEventArgs Fill( TActorBase actor )
        {
            Actor = actor;
            return this;
        }
    }
}