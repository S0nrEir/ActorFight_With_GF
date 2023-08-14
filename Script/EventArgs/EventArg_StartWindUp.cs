using GameFramework;
using GameFramework.Event;

namespace  Aquila.Event
{
    /// <summary>
    /// 开始读条
    /// </summary>
    public class EventArg_StartWindUp : GameEventArgs
    {
        public static readonly int EventID = typeof(EventArg_StartWindUp).GetHashCode();
        
        public override void Clear()
        {
        }

        public override int Id => EventID;

        public float totalTime = 0f;
        
        public static EventArg_StartWindUp Create(float totalTime)
        {
            var arg = ReferencePool.Acquire<EventArg_StartWindUp>();
            arg.totalTime = totalTime;
            return arg;
        }
    }
   
}
