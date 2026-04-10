using GameFramework;

namespace Aquila.Combat.Resolve
{
    // public struct ResolveSourceMeta
    // {
    //     public ResolveSourceType SourceType;
    //     public int AbilityId;
    //     public int TriggerIndex;
    //     public int CastorActorId;
    //     public int TargetActorId;
    // }

    public struct ResolveFormulaSlotRef
    {
        public int PreFormulaId;
        public int MainFormulaId;
        public int PostFormulaId;
    }

    public struct ResolvePhaseDefinition
    {
        public int ResolveTypeId;
        public ResolvePhaseType Phase;
        public int PhaseOrder;
        public ResolvePhasePolicy Policy;
        public ResolveFormulaSlotRef FormulaSlot;
    }

    public struct ResolveResultData
    {
        public bool Success;
        public bool Interrupted;
        public bool Aborted;
        public int ResolveTypeId;
        // public float FinalDelta;
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
