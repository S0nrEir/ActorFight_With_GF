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

        /// <summary>
        /// 读条时长
        /// </summary>
        public float _totalTime = 0f;

        /// <summary>
        /// 施加的目标ActorID
        /// </summary>
        public int _targetActorID = 0;
        
        public static EventArg_StartWindUp Create(float totalTime,int targetActorID)
        {
            var arg            = ReferencePool.Acquire<EventArg_StartWindUp>();
            arg._totalTime     = totalTime;
            arg._targetActorID = targetActorID;
            return arg;
        }
    }
}
