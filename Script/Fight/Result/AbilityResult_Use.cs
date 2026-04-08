using GameFramework;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 技能使用结果（受理与前置校验结果）。
    /// </summary>
    public class AbilityResult_Use : IReference
    {
        public bool _succ;
        public int _stateDescription;

        public int _abilityID = -1;
        public int _castorID = -1;
        public int[] _targetIDArr;
        public Vector3 _targetPosition = Vector3.zero;

        public void Clear()
        {
            _succ = false;
            _stateDescription = 0;
            _abilityID = -1;
            _castorID = -1;
            _targetIDArr = null;
            _targetPosition = Vector3.zero;
        }

        public bool StateFlagIsClean()
        {
            return _stateDescription == 0;
        }
    }
}