using System.Collections.Generic;
using Aquila.Event;
using Aquila.Fight;
using Aquila.Fight.Addon;
using Aquila.Module;
using GameFramework;
using UnityEngine;

namespace Aquila.Combat
{
    public sealed class CastRuntimeInstance : IReference
    {
        /// <summary>
        /// 从对象池获取施法运行时实例并完成初始化。
        /// </summary>
        public static CastRuntimeInstance Create(
            CastCmd castCmd,
            AbilityData abilityData,
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target,
            Addon_Ability abilityAddon)
        {
            var runtime = ReferencePool.Acquire<CastRuntimeInstance>();
            runtime.Initialize(castCmd, abilityData, castor, target, abilityAddon);
            return runtime;
        }

        /// <summary>
        /// 回收前清理运行时状态，并释放内部池对象，避免外层重复释放。
        /// </summary>
        public void Clear()
        {
            if (StateMachine != null)
            {
                ReferencePool.Release(StateMachine);
                StateMachine = null;
            }

            if (TriggerScheduler != null)
            {
                ReferencePool.Release(TriggerScheduler);
                TriggerScheduler = null;
            }

            if (CastCmd != null)
            {
                ReferencePool.Release(CastCmd);
                CastCmd = null;
            }

            AbilityData = default;
            Castor = null;
            Target = null;
            AbilityAddon = null;

            Elapsed = 0f;
            PreCastEndTime = 0f;
            ChannelEndTime = 0f;
            BackSwingEndTime = 0f;

            ResourceDeducted = false;
            IsCompleted = false;
            IsInterrupted = false;
            InterruptReason = CastInterruptReason.None;
        }

        public CastCmd CastCmd { get; private set; }
        public AbilityData AbilityData { get; private set; }
        public Module_ProxyActor.ActorInstance Castor { get; private set; }
        public Module_ProxyActor.ActorInstance Target { get; private set; }
        public Addon_Ability AbilityAddon { get; private set; }
        public TriggerScheduler TriggerScheduler { get; private set; }
        public CastStateMachine StateMachine { get; private set; }

        public float Elapsed { get; private set; }
        public float PreCastEndTime { get; private set; }
        public float ChannelEndTime { get; private set; }
        public float BackSwingEndTime { get; private set; }

        public bool ResourceDeducted { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsInterrupted { get; private set; }
        public CastInterruptReason InterruptReason { get; private set; }

        public void RefreshTarget(Module_ProxyActor.ActorInstance target)
        {
            Target = target;
        }

        public void Tick(float elapsed)
        {
            if (IsCompleted || IsInterrupted)
                return;

            if (elapsed > 0f)
                Elapsed += elapsed;

            StateMachine.FixedUpdate();
        }

        public void DeductResourceOnce()
        {
            if (ResourceDeducted)
                return;

            Castor.Actor.Notify(AddonEventTypeEnum.USE_ABILITY, new AddonParam_OnUseAbility { _abilityID = CastCmd._abilityID });
            ResourceDeducted = true;
        }

        public void CollectReadyTriggerIndices(List<int> output)
        {
            TriggerScheduler.CollectReadyIndices(Elapsed, output);
        }

        public void ExecuteTrigger(int triggerIndex)
        {
            var targetActorId = Target?.Actor?.ActorID ?? -1;
            var hitResult = AbilityResult_Hit.Create(CastCmd._castorInstanceId, targetActorId, CastCmd._abilityID);
            hitResult._targetActorID = targetActorId;
            hitResult._targetPosition = Target?.Actor?.CachedTransform != null
                ? Target.Actor.CachedTransform.position
                : Vector3.zero;

            AbilityAddon.UseAbility(CastCmd._abilityID, triggerIndex, Target, hitResult);
            GameEntry.Event.Fire(this, EventArg_OnHitAbility.Create(hitResult));
        }

        public void MarkInterrupted(CastInterruptReason reason)
        {
            IsInterrupted = true;
            InterruptReason = reason;
        }

        public void MarkCompleted()
        {
            IsCompleted = true;
        }

        /// <summary>
        /// 初始化一次施法运行时快照，包括阶段边界与子调度组件。
        /// </summary>
        private void Initialize(
            CastCmd castCmd,
            AbilityData abilityData,
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target,
            Addon_Ability abilityAddon)
        {
            CastCmd = castCmd;
            AbilityData = abilityData;
            Castor = castor;
            Target = target;
            AbilityAddon = abilityAddon;
            Elapsed = 0f;
            IsCompleted = false;
            IsInterrupted = false;
            InterruptReason = CastInterruptReason.None;
            ResourceDeducted = false;

            BackSwingEndTime = Mathf.Max(abilityData.GetTimelineDuration(), 0f);
            var effects = abilityData.GetEffects();
            if (effects.Count <= 0)
            {
                PreCastEndTime = 0f;
                ChannelEndTime = 0f;
            }
            else
            {
                var minStart = BackSwingEndTime;
                var maxEnd = 0f;
                for (var i = 0; i < effects.Count; i++)
                {
                    var startTime = Mathf.Clamp(effects[i].GetStartTime(), 0f, BackSwingEndTime);
                    var endTime = Mathf.Clamp(effects[i].GetEndTime(), 0f, BackSwingEndTime);
                    if (startTime < minStart)
                        minStart = startTime;
                    if (endTime > maxEnd)
                        maxEnd = endTime;
                }

                if (minStart > maxEnd)
                    minStart = maxEnd;

                PreCastEndTime = minStart;
                ChannelEndTime = Mathf.Max(maxEnd, PreCastEndTime);
            }

            TriggerScheduler = TriggerScheduler.Create(abilityData);
            StateMachine = CastStateMachine.Create(this);
        }
    }
}