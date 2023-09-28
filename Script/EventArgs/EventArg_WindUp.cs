using GameFramework;
using GameFramework.Event;

namespace  Aquila.Event
{
    /// <summary>
    /// 读条/吟唱event
    /// </summary>
    public class EventArg_WindUp : GameEventArgs
    {
        public static readonly int EventID = typeof(EventArg_WindUp).GetHashCode();
        
        public override void Clear()
        {
            _totalTime = 0f;
            _targetActorID = 0;
            _isStart = false;
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

        /// <summary>
        /// 开始标记
        /// </summary>
        public bool _isStart = false;
        
        /// <summary>
        /// 创建开始标记的事件参数实例
        /// </summary>
        public static EventArg_WindUp CreateStartEventArg(float totalTime,int targetActorID)
        {
            var arg            = ReferencePool.Acquire<EventArg_WindUp>();
            arg._totalTime     = totalTime;
            arg._targetActorID = targetActorID;
            arg._isStart       = true;
            return arg;
        }

        /// <summary>
        /// 创建停止标记的事件参数实例
        /// </summary>
        public static EventArg_WindUp CreateStopEventArg()
        {
            var arg = ReferencePool.Acquire<EventArg_WindUp>();
            arg.Clear();
            return arg;
        }
    }
}
