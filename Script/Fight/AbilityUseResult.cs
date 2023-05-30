using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 技能使用结果
    /// </summary>
    public struct AbilityUseResult
    {
        public void Init()
        {
            _succ = false;
            _abilityID = -1;
            _castorID  = -1;
            _targetID  = -1;
            _stateDescription = 0b_0000_0000;
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
    }

    /// <summary>
    /// 使用技能结果状态描述
    /// </summary>
    public enum AbilityUseResultTypeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCC = 0,
        
        /// <summary>
        /// 无目标
        /// </summary>
        NO_TARGET = 1,
        
        /// <summary>
        /// 无施法者
        /// </summary>
        NO_CASTOR = 2,
        
        /// <summary>
        /// 没有技能表数据
        /// </summary>
        NO_META = 3,
    }
}

