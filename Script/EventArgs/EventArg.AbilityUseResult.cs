using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Event
{
    public class EventArg_AbilityUseResult : GameEventArgs
    {
        public static readonly int EventID = typeof( EventArg_AbilityUseResult ).GetHashCode();

        public EventArg_AbilityUseResult()
        {
            _succ = false;
            _abilityID = -1;
            _castorID = -1;
            _targetID = -1;
            _stateDescription = 0b_0000_0000;
        }

        public override void Clear()
        {
        }

        /// <summary>
        /// 状态描述
        /// </summary>
        public int _stateDescription;

        /// <summary>
        /// 施法者ID
        /// </summary>
        public int _castorID;

        /// <summary>
        /// 目标ID
        /// </summary>
        public int _targetID;

        /// <summary>
        /// 技能ID
        /// </summary>
        public int _abilityID;

        /// <summary>
        /// 使用结果，是否成功
        /// </summary>
        public bool _succ;

        public override int Id => EventID;
    }

}
