using System;

namespace Aquila.Combat.Resolve
{
    [Flags]
    public enum ResolvePhasePolicy
    {
        None = 0,
        Skip = 1,
        InterruptBeforeExecute = 1 << 1,
    }

    public enum ResolvePhaseType
    {
        Validity = 0,
        HitCheck = 1,
        BaseValue = 2,
        OffenseMods = 3,
        DefenseMods = 4,
        Crit = 5,
        Block = 6,
        Shield = 7,
        HpApply = 8,
        PostEffects = 9,
        LifecycleCheck = 10,
    }

    public enum ResolveFlowSignalType
    {
        Continue = 0,
        Skip = 1,
        Interrupt = 2,
        JumpTo = 3,
        Abort = 4,
    }

    public enum ResolveSourceType
    {
        Unknown = 0,
        PipelineTrigger = 1,
        EffectDirect = 2,
    }
}