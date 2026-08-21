using System;
using System.Collections.Generic;
using Cfg.Enum;
using Aquila.Module;
using GameFramework;
using UnityEngine;

namespace Aquila.Combat
{
    public class CastCmd : IReference
    {
        public void Clear()
        {
            _castorInstanceId = -1;
            _targetInstanceIdArr = Array.Empty<int>();
            _abilityID = -1;
            _castOrigin = Vector3.zero;
            _targetPoint = Vector3.zero;
            _direction = Vector3.zero;
            _hasCastOrigin = false;
            _hasTargetPoint = false;
            _hasDirection = false;
        }

        public static CastCmd CreateWithSingleTarget(int castorInstanceId, int targetInstanceId, int abilityId)
        {
            return CreateInternal(castorInstanceId, new[] { targetInstanceId }, abilityId);
        }

        public static CastCmd CreateWithSingleTarget(
            int castorInstanceId,
            int targetInstanceId,
            int abilityId,
            Vector3 castOrigin,
            Vector3 targetPoint)
        {
            return CreateInternal(
                castorInstanceId,
                new[] { targetInstanceId },
                abilityId,
                castOrigin,
                targetPoint,
                NormalizeHorizontal(targetPoint - castOrigin),
                true,
                true,
                true);
        }
        
        public static CastCmd CreateWithMultiTarget(int castorInstanceId, int[] targetInstanceId, int abilityId)
        {
            return CreateInternal(castorInstanceId, targetInstanceId, abilityId);
        }

        public static CastCmd CreateWithMultiTarget(
            int castorInstanceId,
            int[] targetInstanceId,
            int abilityId,
            Vector3 castOrigin)
        {
            return CreateInternal(
                castorInstanceId,
                targetInstanceId,
                abilityId,
                castOrigin,
                Vector3.zero,
                Vector3.zero,
                true,
                false,
                false);
        }

        public static CastCmd CreateWithMultiTarget(
            int castorInstanceId,
            int[] targetInstanceId,
            int abilityId,
            Vector3 castOrigin,
            Vector3 targetPoint)
        {
            return CreateInternal(
                castorInstanceId,
                targetInstanceId,
                abilityId,
                castOrigin,
                targetPoint,
                Vector3.zero,
                true,
                true,
                false);
        }

        public static CastCmd CreateWithMultiTarget(
            int castorInstanceId,
            int[] targetInstanceId,
            int abilityId,
            Vector3 castOrigin,
            Vector3 targetPoint,
            Vector3 direction)
        {
            return CreateInternal(
                castorInstanceId,
                targetInstanceId,
                abilityId,
                castOrigin,
                targetPoint,
                NormalizeHorizontal(direction),
                true,
                true,
                true);
        }

        public static CastCmd CreateWithPointTarget(
            int castorInstanceId,
            int abilityId,
            Vector3 castOrigin,
            Vector3 targetPoint)
        {
            return CreateInternal(
                castorInstanceId,
                Array.Empty<int>(),
                abilityId,
                castOrigin,
                targetPoint,
                Vector3.zero,
                true,
                true,
                false);
        }

        public static CastCmd CreateWithDirectionTarget(
            int castorInstanceId,
            int abilityId,
            Vector3 castOrigin,
            Vector3 targetPoint)
        {
            return CreateInternal(
                castorInstanceId,
                Array.Empty<int>(),
                abilityId,
                castOrigin,
                targetPoint,
                NormalizeHorizontal(targetPoint - castOrigin),
                true,
                true,
                true);
        }

        public static bool RequiresActorTargets(AbilitySelectType selectType)
        {
            return selectType == AbilitySelectType.Single ||
                   selectType == AbilitySelectType.Circle ||
                   selectType == AbilitySelectType.SecondaryActor;
        }

        private static CastCmd CreateInternal(
            int castorInstanceId,
            int[] targetInstanceIds,
            int abilityId,
            Vector3 castOrigin = default,
            Vector3 targetPoint = default,
            Vector3 direction = default,
            bool hasCastOrigin = false,
            bool hasTargetPoint = false,
            bool hasDirection = false)
        {
            var cmd = ReferencePool.Acquire<CastCmd>();
            cmd._castorInstanceId = castorInstanceId;
            cmd._targetInstanceIdArr = targetInstanceIds == null
                ? Array.Empty<int>()
                : (int[])targetInstanceIds.Clone();
            cmd._abilityID = abilityId;
            cmd._castOrigin = castOrigin;
            cmd._targetPoint = targetPoint;
            cmd._direction = direction;
            cmd._hasCastOrigin = hasCastOrigin;
            cmd._hasTargetPoint = hasTargetPoint;
            cmd._hasDirection = hasDirection;
            return cmd;
        }

        private static Vector3 NormalizeHorizontal(Vector3 direction)
        {
            direction.y = 0f;
            return direction.sqrMagnitude > 0f ? direction.normalized : Vector3.zero;
        }

        public int _castorInstanceId = -1;
        //#todo：数组缓存，现在每次都要创建targetInstanceID
        public int[] _targetInstanceIdArr = Array.Empty<int>();
        public int _abilityID = -1;
        public Vector3 _castOrigin;
        public Vector3 _targetPoint;
        public Vector3 _direction;
        public bool _hasCastOrigin;
        public bool _hasTargetPoint;
        public bool _hasDirection;
        // public float _requestTick = -1;
    }

    public static class CombatTargetQueryService
    {
        public static void QueryCircle(
            Module_ActorMgr actorMgr,
            Vector3 center,
            float radius,
            Func<Module_ProxyActor.ActorInstance, bool> isLegalTarget,
            List<int> results)
        {
            results.Clear();
            if (actorMgr == null || isLegalTarget == null)
                return;

            var radiusSqr = Mathf.Max(0f, radius) * Mathf.Max(0f, radius);
            foreach (var actor in actorMgr.AllActorInstances())
            {
                if (!IsQueryable(actor) || !isLegalTarget(actor))
                    continue;

                var delta = actor.Actor.CachedTransform.position - center;
                delta.y = 0f;
                if (delta.sqrMagnitude <= radiusSqr)
                    results.Add(actor.Actor.ActorID);
            }

            results.Sort();
        }

        public static void QueryLine(
            Module_ActorMgr actorMgr,
            Vector3 origin,
            Vector3 targetPoint,
            float halfWidth,
            Func<Module_ProxyActor.ActorInstance, bool> isLegalTarget,
            List<int> results)
        {
            results.Clear();
            if (actorMgr == null || isLegalTarget == null)
                return;

            var segment = targetPoint - origin;
            segment.y = 0f;
            var segmentLengthSqr = segment.sqrMagnitude;
            var width = Mathf.Max(0f, halfWidth);
            var widthSqr = width * width;

            foreach (var actor in actorMgr.AllActorInstances())
            {
                if (!IsQueryable(actor) || !isLegalTarget(actor))
                    continue;

                var point = actor.Actor.CachedTransform.position;
                var fromOrigin = point - origin;
                fromOrigin.y = 0f;
                var projection = segmentLengthSqr > 0f
                    ? Vector3.Dot(fromOrigin, segment) / segmentLengthSqr
                    : 0f;
                if (projection < 0f || projection > 1f)
                    continue;

                var closest = origin + segment * projection;
                var offset = point - closest;
                offset.y = 0f;
                if (offset.sqrMagnitude <= widthSqr)
                    results.Add(actor.Actor.ActorID);
            }

            results.Sort();
        }

        public static void QueryGlobal(
            Module_ActorMgr actorMgr,
            Func<Module_ProxyActor.ActorInstance, bool> isLegalTarget,
            List<int> results)
        {
            results.Clear();
            if (actorMgr == null || isLegalTarget == null)
                return;

            foreach (var actor in actorMgr.AllActorInstances())
            {
                if (IsQueryable(actor) && isLegalTarget(actor))
                    results.Add(actor.Actor.ActorID);
            }

            results.Sort();
        }

        private static bool IsQueryable(Module_ProxyActor.ActorInstance actor)
        {
            return actor?.Actor != null && actor.Actor.CachedTransform != null;
        }
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
                TargetInstanceId = cmd?._targetInstanceIdArr ?? Array.Empty<int>(),
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
                TargetInstanceId = cmd?._targetInstanceIdArr ?? Array.Empty<int>(),
                AbilityId = cmd?._abilityID ?? -1
            };
        }
    }
}
