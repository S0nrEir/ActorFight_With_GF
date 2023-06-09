using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Event
{
    /// <summary>
    /// 技能命中的参数
    /// </summary>
    public class AbilityResult_Hit : IReference
    {
        public void Clear()
        {
            
        }

        /// <summary>
        /// 状态描述
        /// </summary>
        public int _stateDescription = 0;

        /// <summary>
        /// 造成的伤害
        /// </summary>
        public float _dealedDamage = 0f;

        /// <summary>
        /// 目标ActorID
        /// </summary>
        public int _targetActorID = -1;

        /// <summary>
        /// 施法者ActorID
        /// </summary>
        public int _castorActorID = -1;
    }
}