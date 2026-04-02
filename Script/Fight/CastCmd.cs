using GameFramework;
using Cfg.Enum;
using UnityEngine;

namespace Aquila.Combat
{
    public class CastCmd : IReference
    {
        public void Clear()
        {
            _castorInstanceId = -1;
            _targetInstanceId = -1;
            _abilityID = -1;
        }

        public static CastCmd Create(int castorInstanceId, int targetInstanceId, int abilityId)
        {
            var cmd = ReferencePool.Acquire<CastCmd>();
            cmd._castorInstanceId = castorInstanceId;
            cmd._targetInstanceId = targetInstanceId;
            cmd._abilityID = abilityId;
            return cmd;
        }

        public int _castorInstanceId = -1;
        public int _targetInstanceId = -1;
        public int _abilityID = -1;
        // public float _requestTick = -1;
    }

    public struct CastAcceptResult
    {
        public bool Accepted;
        public CastRejectCode PrimaryCode;
        public CastRejectFlags ReasonFlags;
        public int LegacyStateDescription;
        public int CastorInstanceId;
        public int TargetInstanceId;
        public int AbilityId;

        public static CastAcceptResult Accept(CastCmd cmd)
        {
            return new CastAcceptResult
            {
                Accepted = true,
                PrimaryCode = CastRejectCode.None,
                ReasonFlags = CastRejectFlags.None,
                LegacyStateDescription = 0,
                CastorInstanceId = cmd?._castorInstanceId ?? -1,
                TargetInstanceId = cmd?._targetInstanceId ?? -1,
                AbilityId = cmd?._abilityID ?? -1
            };
        }

        public static CastAcceptResult Reject(CastCmd cmd, CastRejectCode code, CastRejectFlags flags, int legacyStateDescription = 0)
        {
            return new CastAcceptResult
            {
                Accepted = false,
                PrimaryCode = code,
                ReasonFlags = flags,
                LegacyStateDescription = legacyStateDescription,
                CastorInstanceId = cmd?._castorInstanceId ?? -1,
                TargetInstanceId = cmd?._targetInstanceId ?? -1,
                AbilityId = cmd?._abilityID ?? -1
            };
        }
    }
}

// namespace Aquila.Fight
// {
//     /// <summary>
//     /// 技能使用结果（受理与前置校验结果）。
//     /// </summary>
//     public class AbilityResult_Use : IReference
//     {
//         public bool _succ;
//         public int _stateDescription;
//
//         public int _abilityID = -1;
//         public int _castorID = -1;
//         public int[] _targetIDArr;
//         public Vector3 _targetPosition = Vector3.zero;
//
//         public void Clear()
//         {
//             _succ = false;
//             _stateDescription = 0;
//             _abilityID = -1;
//             _castorID = -1;
//             _targetIDArr = null;
//             _targetPosition = Vector3.zero;
//         }
//
//         public bool StateFlagIsClean()
//         {
//             return _stateDescription == 0;
//         }
//     }
// }