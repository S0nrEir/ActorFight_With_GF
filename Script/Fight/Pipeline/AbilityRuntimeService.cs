using System;
using System.Collections.Generic;
using Aquila.Fight.Addon;
using Aquila.Module;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Combat
{
    public class AbilityRuntimeService
    {
        public bool IsCasting(int castorActorId)
        {
            return _activeRuntimeByCaster.ContainsKey(castorActorId);
        }

        /// <summary>
        /// 校验施法请求并创建施法运行时实例；仅受理，不做帧推进。
        /// </summary>
        public bool TryStartCast(CastCmd cmd, out CastRejectCode rejectCode, out CastRejectFlags rejectFlags)
        {
            rejectCode = CastRejectCode.None;
            rejectFlags = CastRejectFlags.None;

            if (cmd == null)
            {
                rejectCode = CastRejectCode.InvalidCmd;
                rejectFlags = CastRejectFlags.InvalidCmd;
                return false;
            }

            if (_activeRuntimeByCaster.ContainsKey(cmd._castorInstanceId))
            {
                rejectCode = CastRejectCode.Unknown;
                return false;
            }

            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            if (actorMgr == null)
            {
                rejectCode = CastRejectCode.Unknown;
                return false;
            }

            var castor = actorMgr.Get(cmd._castorInstanceId);
            var target = actorMgr.Get(cmd._targetInstanceId);
            if (castor == null)
            {
                rejectCode = CastRejectCode.CastorNotFound;
                rejectFlags = CastRejectFlags.CastorNotFound;
                return false;
            }

            if (target == null)
            {
                rejectCode = CastRejectCode.TargetNotFound;
                rejectFlags = CastRejectFlags.TargetNotFound;
                return false;
            }

            var abilityAddon = castor.GetAddon<Addon_Ability>();
            if (abilityAddon == null)
            {
                rejectCode = CastRejectCode.MissingAbilityAddon;
                rejectFlags = CastRejectFlags.MissingAbilityAddon;
                return false;
            }

            var canUseCode = NormalizeCanUseCode(abilityAddon.CanUseAbility(cmd._abilityID));
            if (canUseCode != CastRejectCode.None)
            {
                rejectCode = canUseCode;
                rejectFlags = MapCodeToFlag(canUseCode);
                return false;
            }

            if (!GameEntry.AbilityPool.TryGetAbility(cmd._abilityID, out var abilityData))
            {
                rejectCode = CastRejectCode.AbilitySpecMissing;
                rejectFlags = CastRejectFlags.AbilitySpecMissing;
                return false;
            }

            if (!IsTargetTypeValid(castor, target, abilityData.GetTargetType()))
            {
                rejectCode = CastRejectCode.UnsupportedTargetType;
                rejectFlags = CastRejectFlags.UnsupportedTargetType;
                return false;
            }

            var castInstance = CastRuntimeInstance.Create(cmd, abilityData, castor, target, abilityAddon);
            castInstance.StateMachine.EnterPreCast();
            _activeRuntimeByCaster.Add(cmd._castorInstanceId, castInstance);
            return true;
        }

        /// <summary>
        /// 推进全部施法实例，并在完成/中断后统一回收运行时对象。
        /// </summary>
        public void FixedUpdate(float elapased, float realElapsed)
        {
            if (_activeRuntimeByCaster.Count <= 0)
                return;

            _toRemoveCasterIds.Clear();
            foreach (var kv in _activeRuntimeByCaster)
            {
                var castRuntimeInstance = kv.Value;
                if (castRuntimeInstance.IsInterrupted || castRuntimeInstance.IsCompleted)
                {
                    _toRemoveCasterIds.Add(kv.Key);
                    continue;
                }

                if (!TryRefreshTarget(castRuntimeInstance))
                {
                    castRuntimeInstance.StateMachine.Interrupt(CastInterruptReason.TargetLost);
                    _toRemoveCasterIds.Add(kv.Key);
                    continue;
                }

                castRuntimeInstance.Tick(elapased);

                _readyTriggerIndices.Clear();
                castRuntimeInstance.CollectReadyTriggerIndices(_readyTriggerIndices);
                for (var i = 0; i < _readyTriggerIndices.Count; i++)
                {
                    if (!TryRefreshTarget(castRuntimeInstance))
                    {
                        castRuntimeInstance.StateMachine.Interrupt(CastInterruptReason.TargetLost);
                        break;
                    }

                    castRuntimeInstance.ExecuteTrigger(_readyTriggerIndices[i]);
                }

                if (castRuntimeInstance.IsInterrupted || castRuntimeInstance.IsCompleted)
                    _toRemoveCasterIds.Add(kv.Key);
            }

            for (var i = 0; i < _toRemoveCasterIds.Count; i++)
            {
                var casterId = _toRemoveCasterIds[i];
                if (!_activeRuntimeByCaster.TryGetValue(casterId, out var runtime))
                    continue;

                _activeRuntimeByCaster.Remove(casterId);
                ReleaseRuntime(runtime);
            }
        }

        /// <summary>
        /// 清空服务内所有运行时与临时缓存。
        /// </summary>
        public void Clear()
        {
            foreach (var kv in _activeRuntimeByCaster)
                ReleaseRuntime(kv.Value);

            _activeRuntimeByCaster.Clear();
            _readyTriggerIndices.Clear();
            _toRemoveCasterIds.Clear();
        }

        private static CastRejectCode NormalizeCanUseCode(int rawCanUseCode)
        {
            if (rawCanUseCode == 0)
                return CastRejectCode.None;

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

        private static bool IsTargetTypeValid(
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target,
            AbilityTargetType targetType)
        {
            if (castor == null || target == null)
                return false;

            if (targetType == AbilityTargetType.Self)
                return castor.Actor.ActorID == target.Actor.ActorID;

            // 当前项目暂未暴露完整阵营关系，先保证“自身目标”严格校验；其他类型按存在性放行。
            return true;
        }

        private static bool TryRefreshTarget(CastRuntimeInstance instance)
        {
            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            // if (actorMgr == null)
            //     return false;

            var refreshedTarget = actorMgr.Get(instance.CastCmd._targetInstanceId);
            instance.RefreshTarget(refreshedTarget);
            return refreshedTarget != null;
        }

        /// <summary>
        /// 统一运行时回收入口：由运行时自身 Clear 负责释放内部对象。
        /// </summary>
        private static void ReleaseRuntime(CastRuntimeInstance runtime)
        {
            if (runtime == null)
                return;

            ReferencePool.Release(runtime);
        }

        private readonly Dictionary<int, CastRuntimeInstance> _activeRuntimeByCaster = new Dictionary<int, CastRuntimeInstance>(16);
        private readonly List<int> _readyTriggerIndices = new List<int>(8);
        private readonly List<int> _toRemoveCasterIds = new List<int>(8);
    }
}