using Cfg.Enum;
using GameFramework;

namespace Aquila.Combat.Resolve
{
    public struct ResolvePhaseDefinition
    {
        public int ResolveTypeId;
        public ResolvePhaseType Phase;
        public int PhaseOrder;
        public ResolvePhasePolicy Policy;
        // public ResolveFormulaSlotRef FormulaSlot;
    }

    public struct ResolvePhaseIoState
    {
        public float Input;
        public float Output;
    }

    public struct ResolveResultData
    {
        public bool Success;
        public bool Interrupted;
        public bool Aborted;
        public int ResolveTypeId;
        public ResolveSourceType SourceType;
        public float InputDelta;
        public float FinalDelta;
        public float TotalIncrease;
        public float TotalReduction;
        public float TotalAbsorb;
        public float AppliedDelta;
        public ResolvePhaseType LastPhase;
        public string Reason;
    }

    public sealed class PhaseExecutionResult : IReference
    {
        public ResolveFlowSignalType SignalType { get; private set; }
        public ResolvePhaseType JumpToPhase { get; private set; }
        public string Reason { get; private set; }

        public void SetContinue()
        {
            SignalType = ResolveFlowSignalType.Continue;
            JumpToPhase = ResolvePhaseType.Validity;
            Reason = null;
        }

        public void SetSkip()
        {
            SignalType = ResolveFlowSignalType.Skip;
            JumpToPhase = ResolvePhaseType.Validity;
            Reason = null;
        }

        public void SetInterrupt(string reason)
        {
            SignalType = ResolveFlowSignalType.Interrupt;
            JumpToPhase = ResolvePhaseType.Validity;
            Reason = reason;
        }

        public void SetJumpTo(ResolvePhaseType targetPhase)
        {
            SignalType = ResolveFlowSignalType.JumpTo;
            JumpToPhase = targetPhase;
            Reason = null;
        }

        public void SetAbort(string reason)
        {
            SignalType = ResolveFlowSignalType.Abort;
            JumpToPhase = ResolvePhaseType.Validity;
            Reason = reason;
        }

        public void Clear()
        {
            SignalType = ResolveFlowSignalType.Continue;
            JumpToPhase = ResolvePhaseType.Validity;
            Reason = null;
        }
    }
}