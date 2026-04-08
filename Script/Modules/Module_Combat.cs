using System;
using System.Collections.Generic;
using Aquila.Combat;
using Aquila.Extension;
using Aquila.Fight.Addon;
using Cfg.Enum;
using GameFramework;
using UnityEngine;

namespace Aquila.Module
{
    public class Module_Combat : GameFrameworkModuleBase, IFixedUpdate
    {
        /// <summary>
        /// 施法请求受理检查：仅做参数、目标、冷却和消耗校验，不执行施法。
        /// </summary>
        public CastAcceptResult RequestCast(CastCmd cmd)
        {
            if (cmd == null)
                return RejectAndRelease(null, CastRejectCode.InvalidCmd, CastRejectFlags.InvalidCmd);

            var argFlags = CastRejectFlags.None;
            if (cmd._castorInstanceId <= 0)
                argFlags |= CastRejectFlags.InvalidCastorId;

            if (cmd._abilityID <= 0)
                argFlags |= CastRejectFlags.InvalidAbilityId;

            if (cmd._targetInstanceId <= 0)
                argFlags |= CastRejectFlags.TargetNotFound;

            if (argFlags != CastRejectFlags.None)
                return RejectAndRelease(cmd, ResolvePrimaryCode(argFlags), argFlags);

            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            if (actorMgr == null)
                return RejectAndRelease(cmd, CastRejectCode.Unknown, CastRejectFlags.None);

            var castor = actorMgr.Get(cmd._castorInstanceId);
            var target = actorMgr.Get(cmd._targetInstanceId);
            var actorFlags = CastRejectFlags.None;
            if (castor == null)
                actorFlags |= CastRejectFlags.CastorNotFound;
            if (target == null)
                actorFlags |= CastRejectFlags.TargetNotFound;

            if (actorFlags != CastRejectFlags.None)
                return RejectAndRelease(cmd, ResolvePrimaryCode(actorFlags), actorFlags);

            var abilityAddon = castor.GetAddon<Addon_Ability>();
            if (abilityAddon == null)
                return RejectAndRelease(cmd, CastRejectCode.MissingAbilityAddon, CastRejectFlags.MissingAbilityAddon);

            var canUse = NormalizeCanUseCode(abilityAddon.CanUseAbility(cmd._abilityID));
            if (canUse != CastRejectCode.None)
                return RejectAndRelease(cmd, canUse, MapCodeToFlag(canUse));

            EnqueueCast(cmd);
            return CastAcceptResult.Accept(cmd);
        }

        private static CastAcceptResult Reject(CastCmd cmd, CastRejectCode code, CastRejectFlags flags)
        {
            return CastAcceptResult.Reject(cmd, code, flags, BuildLegacyState(code, flags));
        }

        private static CastAcceptResult RejectAndRelease(CastCmd cmd, CastRejectCode code, CastRejectFlags flags)
        {
            var result = Reject(cmd, code, flags);
            if (cmd != null)
                ReferencePool.Release(cmd);
            return result;
        }

        private static CastRejectFlags MapCodeToFlag(CastRejectCode code)
        {
            switch (code)
            {
                case CastRejectCode.InvalidCmd:
                    return CastRejectFlags.InvalidCmd;

                case CastRejectCode.InvalidCastorId:
                    return CastRejectFlags.InvalidCastorId;

                case CastRejectCode.InvalidAbilityId:
                    return CastRejectFlags.InvalidAbilityId;

                case CastRejectCode.CastorNotFound:
                    return CastRejectFlags.CastorNotFound;

                case CastRejectCode.TargetNotFound:
                    return CastRejectFlags.TargetNotFound;

                case CastRejectCode.MissingAbilityAddon:
                    return CastRejectFlags.MissingAbilityAddon;

                case CastRejectCode.AbilitySpecMissing:
                    return CastRejectFlags.AbilitySpecMissing;

                case CastRejectCode.CooldownNotReady:
                    return CastRejectFlags.CooldownNotReady;

                case CastRejectCode.CostNotEnough:
                    return CastRejectFlags.CostNotEnough;

                case CastRejectCode.UnsupportedTargetType:
                    return CastRejectFlags.UnsupportedTargetType;

                default:
                    return CastRejectFlags.None;
            }
        }

        private static CastRejectCode ResolvePrimaryCode(CastRejectFlags flags)
        {
            if ((flags & CastRejectFlags.InvalidCmd) != 0)
                return CastRejectCode.InvalidCmd;

            if ((flags & CastRejectFlags.InvalidCastorId) != 0)
                return CastRejectCode.InvalidCastorId;

            if ((flags & CastRejectFlags.InvalidAbilityId) != 0)
                return CastRejectCode.InvalidAbilityId;

            if ((flags & CastRejectFlags.CastorNotFound) != 0)
                return CastRejectCode.CastorNotFound;

            if ((flags & CastRejectFlags.TargetNotFound) != 0)
                return CastRejectCode.TargetNotFound;

            if ((flags & CastRejectFlags.MissingAbilityAddon) != 0)
                return CastRejectCode.MissingAbilityAddon;

            if ((flags & CastRejectFlags.AbilitySpecMissing) != 0)
                return CastRejectCode.AbilitySpecMissing;

            if ((flags & CastRejectFlags.CooldownNotReady) != 0)
                return CastRejectCode.CooldownNotReady;

            if ((flags & CastRejectFlags.CostNotEnough) != 0)
                return CastRejectCode.CostNotEnough;

            if ((flags & CastRejectFlags.UnsupportedTargetType) != 0)
                return CastRejectCode.UnsupportedTargetType;

            return CastRejectCode.Unknown;
        }

         private static CastRejectCode NormalizeCanUseCode(int rawCanUseCode)
        {
            if (rawCanUseCode == 0)
                return CastRejectCode.None;

            // 兼容历史返回码：1=消耗不足，2=CD未就绪，3=无效(按无技能实例处理)
            switch (rawCanUseCode)
            {
                case 1:
                    return CastRejectCode.CostNotEnough;
                case 2:
                    return CastRejectCode.CooldownNotReady;
                case 3:
                    return CastRejectCode.AbilitySpecMissing;
            }

            if (Enum.IsDefined(typeof(CastRejectCode), rawCanUseCode))
                return (CastRejectCode)rawCanUseCode;

            return CastRejectCode.Unknown;
        }

        private static int BuildLegacyState(CastRejectCode code, CastRejectFlags flags)
        {
            if (flags != CastRejectFlags.None)
                return (int)flags;

            return (int)code;
        }

        private void EnqueueCast(CastCmd cmd)
        {
            if (!_pendingCastByCaster.TryGetValue(cmd._castorInstanceId, out var queue))
            {
                queue = new Queue<CastCmd>();
                _pendingCastByCaster.Add(cmd._castorInstanceId, queue);
            }

            queue.Enqueue(cmd);
        }

        private void DrivePendingQueue()
        {
            if (_pendingCastByCaster.Count <= 0)
                return;

            _tempCasterIds.Clear();
            foreach (var kv in _pendingCastByCaster)
                _tempCasterIds.Add(kv.Key);

            for (var i = 0; i < _tempCasterIds.Count; i++)
            {
                var castorActorId = _tempCasterIds[i];
                if (_abilityRuntimeService.IsCasting(castorActorId))
                    continue;

                if (!_pendingCastByCaster.TryGetValue(castorActorId, out var queue) || queue.Count <= 0)
                    continue;

                while (queue.Count > 0 && !_abilityRuntimeService.IsCasting(castorActorId))
                {
                    var cmd = queue.Peek();
                    if (_abilityRuntimeService.TryStartCast(cmd, out _, out _))
                    {
                        queue.Dequeue();
                        break;
                    }

                    queue.Dequeue();
                    if (cmd != null)
                        ReferencePool.Release(cmd);
                }

                if (queue.Count <= 0)
                    _pendingCastByCaster.Remove(castorActorId);
            }

            _tempCasterIds.Clear();
        }

        private void ClearPendingQueue()
        {
            foreach (var kv in _pendingCastByCaster)
            {
                var queue = kv.Value;
                while (queue.Count > 0)
                {
                    var cmd = queue.Dequeue();
                    if (cmd != null)
                        ReferencePool.Release(cmd);
                }
            }

            _pendingCastByCaster.Clear();
            _tempCasterIds.Clear();
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            _abilityRuntimeService = new AbilityRuntimeService();
        }

        public override void Open(object param)
        {
            base.Open(param);
        }

        public override void Close()
        {
            base.Close();
            ClearPendingQueue();
            _abilityRuntimeService.Clear();
            _abilityRuntimeService = null;
        }

        public void OnFixedUpdate()
        {
            _abilityRuntimeService.FixedUpdate(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
            DrivePendingQueue();
        }

        private AbilityRuntimeService _abilityRuntimeService;
        private readonly Dictionary<int, Queue<CastCmd>> _pendingCastByCaster = new Dictionary<int, Queue<CastCmd>>(16);
        private readonly List<int> _tempCasterIds = new List<int>(16);
    }
}