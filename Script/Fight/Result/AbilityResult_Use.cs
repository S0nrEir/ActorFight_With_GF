using Aquila.Fight;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;

namespace Aquila.Event
{
    /// <summary>
    /// 技能使用参数`
    /// </summary>
    public class AbilityResult_Use : IReference
    {
        /// <summary>
        /// 错误状态标识是否全都为0，表示没有任何错误
        /// </summary>
        /// <returns></returns>
        public bool StateFlagIsClean()
        {
            return !Tools.GetBitValue( _stateDescription, ( int ) AbilityUseResultTypeEnum.CD_NOT_OK ) &&
                   !Tools.GetBitValue( _stateDescription, ( int ) AbilityUseResultTypeEnum.COST_NOT_ENOUGH ) &&
                   !Tools.GetBitValue( _stateDescription, ( int ) AbilityUseResultTypeEnum.NONE_ABILITY_META ) &&
                   !Tools.GetBitValue( _stateDescription, ( int ) AbilityUseResultTypeEnum.NONE_TIMELINE_META ) &&
                   !Tools.GetBitValue( _stateDescription, ( int ) AbilityUseResultTypeEnum.NO_CASTOR ) &&
                   !Tools.GetBitValue( _stateDescription, ( int ) AbilityUseResultTypeEnum.NO_TARGET );
        }

        public void Clear()
        {
            _stateDescription = 0;
            _castorID         = -1;
            _targetIDArr      = null;
            _abilityID        = -1;
            _targetPosition   = GameEntry.GlobalVar.InvalidPosition;
            _succ             = false;
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
        /// 目标ID集合
        /// </summary>
        public int[] _targetIDArr;

        /// <summary>
        /// 技能ID
        /// </summary>
        public int _abilityID;

        /// <summary>
        /// 技能目标位置
        /// </summary>
        public Vector3 _targetPosition;

        /// <summary>
        /// 使用结果，是否成功
        /// </summary>
        public bool _succ;
    }

}
