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
            _targetInstanceIdArr = null;
            _abilityID = -1;
        }

        public static CastCmd CreateWithOneTarget(int castorInstanceId, int targetInstanceId, int abilityId)
        {
            var cmd = ReferencePool.Acquire<CastCmd>();
            cmd._castorInstanceId = castorInstanceId;
            cmd._targetInstanceIdArr = new int[]{targetInstanceId};
            cmd._abilityID = abilityId;
            return cmd;
        }
        
        public static CastCmd CreateWithMultiTarget(int castorInstanceId, int[] targetInstanceId, int abilityId)
        {
            var cmd = ReferencePool.Acquire<CastCmd>();
            cmd._castorInstanceId = castorInstanceId;
            cmd._targetInstanceIdArr = targetInstanceId;
            cmd._abilityID = abilityId;
            return cmd;
        }

        public int _castorInstanceId = -1;
        //#todo：数组缓存，现在每次都要创建targetInstanceID
        public int[] _targetInstanceIdArr = null;
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
        public int[] TargetInstanceId;
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
                TargetInstanceId = cmd._targetInstanceIdArr,
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
                TargetInstanceId = cmd._targetInstanceIdArr,
                AbilityId = cmd?._abilityID ?? -1
            };
        }
    }
}