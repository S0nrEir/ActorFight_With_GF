using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Event
{
    public class EventArg_OnUseAblity : GameEventArgs
    {
        public static readonly int EventID = typeof( EventArg_OnUseAblity ).GetHashCode();

        public override int Id => EventID;

        public override void Clear()
        {
        }

        public static EventArg_OnUseAblity Create(AbilityResult_Use resultParam)
        {
            var arg = ReferencePool.Acquire<EventArg_OnUseAblity>();
            arg._resultParam = resultParam;
            return arg;
        }

        /// <summary>
        /// 参数
        /// </summary>
        public AbilityResult_Use _resultParam = null;
    }
}
