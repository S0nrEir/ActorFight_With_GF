using System.Collections.Generic;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Combat.Resolve
{
    [System.Flags]
    public enum ResolvePhaseFlags : uint
    {
        None = 0,
        CritTriggered = 1 << 0,
    }

    public sealed class ResolveContext : IReference
    {
        /// <summary>
        /// 关联resolve request
        /// </summary>
        public void SetupResolveReq(ResolveRequest request)
        {
            Request = request;
            FinalDelta = request != null ? request.InputDelta : 0f;
            HasApplied = false;
            IsInterrupted = false;
            IsAborted = false;
            Reason = null;
            LastPhase = ResolvePhaseType.Validity;
            _phaseFlags = ResolvePhaseFlags.None;

            _floatValues.Clear();
            _skippedPhases.Clear();
        }

        public void ResetPhaseStates(List<ResolvePhaseDefinition> phases)
        {
            if (phases == null)
                return;

            for (var i = 0; i < phases.Count; i++)
                ResetPhaseState(phases[i].Phase);
        }

        public void ResetPhaseState(ResolvePhaseType phase)
        {
            switch (phase)
            {
                case ResolvePhaseType.Validity:
                    ValidityIo = default;
                    break;

                case ResolvePhaseType.HitCheck:
                    HitCheckIo = default;
                    break;

                case ResolvePhaseType.BaseValue:
                    BaseValueIo = default;
                    BaseValueAmount = 0f;
                    break;

                case ResolvePhaseType.OffenseMods:
                    OffenseModsIo = default;
                    OffenseIncrease = 0f;
                    break;

                case ResolvePhaseType.DefenseMods:
                    DefenseModsIo = default;
                    DefenseReduction = 0f;
                    break;

                case ResolvePhaseType.CritCheck:
                    CritCheckIo = default;
                    ClearPhaseFlag(ResolvePhaseFlags.CritTriggered);
                    break;

                case ResolvePhaseType.Crit:
                    CritIo = default;
                    CritIncrease = 0f;
                    break;

                case ResolvePhaseType.Block:
                    BlockIo = default;
                    BlockReduction = 0f;
                    break;

                case ResolvePhaseType.Shield:
                    ShieldIo = default;
                    ShieldAbsorb = 0f;
                    break;

                case ResolvePhaseType.HpApply:
                    HpApplyIo = default;
                    AppliedHpDelta = 0f;
                    break;

                case ResolvePhaseType.PostEffects:
                    PostEffectsIo = default;
                    break;

                case ResolvePhaseType.LifecycleCheck:
                    LifecycleCheckIo = default;
                    break;
            }
        }

        public void SetFloat(string key, float value)
        {
            if (string.IsNullOrEmpty(key))
                return;

            _floatValues[key] = value;
        }

        public bool TryGetFloat(string key, out float value)
        {
            if (string.IsNullOrEmpty(key))
            {
                value = 0f;
                return false;
            }

            return _floatValues.TryGetValue(key, out value);
        }

        public void MarkSkipped(ResolvePhaseType phase)
        {
            _skippedPhases.Add(phase);
        }

        public bool IsPhaseSkipped(ResolvePhaseType phase)
        {
            return _skippedPhases.Contains(phase);
        }

        public void SetPhaseFlag(ResolvePhaseFlags flag)
        {
            Tools.SetBitValue_i64((int)_phaseFlags, (ushort)flag, true);
        }

        public void ClearPhaseFlag(ResolvePhaseFlags flag)
        {
            _phaseFlags &= ~flag;
        }

        public bool HasPhaseFlag(ResolvePhaseFlags flag)
        {
            return Tools.GetBitValue_i64((int)_phaseFlags, (ushort)flag);
        }

        public void MarkInterrupted(string reason)
        {
            IsInterrupted = true;
            Reason = reason;
        }

        public void MarkAborted(string reason)
        {
            IsAborted = true;
            Reason = reason;
        }

        public void Clear()
        {
            Request = null;
            FinalDelta = 0f;
            HasApplied = false;
            IsInterrupted = false;
            IsAborted = false;
            Reason = null;
            LastPhase = ResolvePhaseType.Validity;
            _phaseFlags = ResolvePhaseFlags.None;

            ValidityIo = default;
            HitCheckIo = default;
            BaseValueIo = default;
            BaseValueAmount = 0f;
            OffenseModsIo = default;
            OffenseIncrease = 0f;
            DefenseModsIo = default;
            DefenseReduction = 0f;
            _CritIo = default;
            CritIo = default;
            CritIncrease = 0f;
            BlockIo = default;
            BlockReduction = 0f;
            ShieldIo = default;
            ShieldAbsorb = 0f;
            HpApplyIo = default;
            AppliedHpDelta = 0f;
            PostEffectsIo = default;
            LifecycleCheckIo = default;

            _floatValues.Clear();
            _skippedPhases.Clear();
        }

        private readonly Dictionary<string, float> _floatValues = new Dictionary<string, float>(8);
        private readonly HashSet<ResolvePhaseType> _skippedPhases = new HashSet<ResolvePhaseType>();
        private ResolvePhaseFlags _phaseFlags;

        public ResolveRequest Request { get; private set; }
        public float FinalDelta { get; set; }
        public bool HasApplied { get; set; }
        public bool IsInterrupted { get; private set; }
        public bool IsAborted { get; private set; }
        public string Reason { get; private set; }
        public ResolvePhaseType LastPhase { get; set; }

        public ResolvePhaseIoState ValidityIo;
        public ResolvePhaseIoState HitCheckIo;

        public ResolvePhaseIoState BaseValueIo;
        public float BaseValueAmount;

        public ResolvePhaseIoState OffenseModsIo;
        public float OffenseIncrease;

        public ResolvePhaseIoState DefenseModsIo;
        public float DefenseReduction;

        public ResolvePhaseIoState _CritIo;

        public ResolvePhaseIoState CritIo;
        public float CritIncrease;

        public ResolvePhaseIoState BlockIo;
        public float BlockReduction;

        public ResolvePhaseIoState ShieldIo;
        public float ShieldAbsorb;

        public ResolvePhaseIoState HpApplyIo;
        public float AppliedHpDelta;

        public ResolvePhaseIoState PostEffectsIo;
        public ResolvePhaseIoState LifecycleCheckIo;
    }
}
