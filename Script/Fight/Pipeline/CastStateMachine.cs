using System;
using GameFramework;

namespace Aquila.Combat
{
    public enum CastPhase
    {
        None = 0,
        PreCast = 1,
        Channel = 2,
        BackSwing = 3,
        Interrupted = 4,
        Completed = 5
    }

    public enum CastInterruptReason
    {
        None = 0,
        TargetLost = 1,
        Unknown = 99,
    }

    public sealed class CastStateMachine : IReference
    {
        /// <summary>
        /// 从对象池获取状态机并绑定当前运行时实例。
        /// </summary>
        public static CastStateMachine Create(CastRuntimeInstance runtime)
        {
            var stateMachine = ReferencePool.Acquire<CastStateMachine>();
            stateMachine._runtime = runtime;
            stateMachine.CurrentPhase = CastPhase.None;
            return stateMachine;
        }

        /// <summary>
        /// 回收到对象池前重置状态并解除运行时引用。
        /// </summary>
        public void Clear()
        {
            CurrentPhase = CastPhase.None;
            _runtime = null;
        }

        public CastPhase CurrentPhase { get; private set; }

        public void EnterPreCast()
        {
            if (IsTerminal())
                return;

            if (_runtime == null)
                return;

            CurrentPhase = CastPhase.PreCast;
            _runtime.DeductResourceOnce();
        }

        public void EnterChannel()
        {
            if (IsTerminal())
                return;

            CurrentPhase = CastPhase.Channel;
        }

        public void EnterBackSwing()
        {
            if (IsTerminal())
                return;

            CurrentPhase = CastPhase.BackSwing;
        }

        public void Interrupt(CastInterruptReason reason)
        {
            if (IsTerminal())
                return;

            CurrentPhase = CastPhase.Interrupted;
            _runtime.MarkInterrupted(reason);
        }

        /// <summary>
        /// 按时间推进施法阶段，命中终止态后不再迁移。
        /// </summary>
        public void FixedUpdate()
        {
            if (IsTerminal())
                return;

            if (_runtime == null)
                return;

            if (CurrentPhase == CastPhase.None)
                EnterPreCast();

            if (CurrentPhase == CastPhase.PreCast && _runtime.Elapsed >= _runtime.PreCastEndTime)
            {
                if (_runtime.ChannelEndTime > _runtime.PreCastEndTime + TimeEpsilon)
                    EnterChannel();
                else
                    EnterBackSwing();
            }

            if (CurrentPhase == CastPhase.Channel && _runtime.Elapsed >= _runtime.ChannelEndTime)
                EnterBackSwing();

            if (CurrentPhase == CastPhase.BackSwing && _runtime.Elapsed >= _runtime.BackSwingEndTime)
            {
                CurrentPhase = CastPhase.Completed;
                _runtime.MarkCompleted();
            }
        }

        private bool IsTerminal()
        {
            return CurrentPhase == CastPhase.Interrupted || CurrentPhase == CastPhase.Completed;
        }

        private const float TimeEpsilon = 0.0001f;
        private CastRuntimeInstance _runtime;
    }
}