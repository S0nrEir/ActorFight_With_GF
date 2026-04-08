using GameFramework;
using UnityEngine;

namespace Aquila.Fight
{
    public class AbilityResult_Hit : IReference
    {
        public static AbilityResult_Hit Create(int castorID, int targetActorID, int abilityID)
        {
            var result = ReferencePool.Acquire<AbilityResult_Hit>();
            result._castorID = castorID;
            result._targetActorID = targetActorID;
            result._abilityID = abilityID;
            return result;
        }

        public void Clear()
        {
            _stateDescription = 0;
            _dealedDamage = 0;
            _abilityID = -1;
            _castorID = -1;
            _targetActorID = -1;
            _targetPosition = Vector3.zero;
        }

        public int _stateDescription;
        public int _dealedDamage;
        public int _abilityID = -1;
        public int _castorID = -1;
        public int _targetActorID = -1;
        public Vector3 _targetPosition = Vector3.zero;
    }
}