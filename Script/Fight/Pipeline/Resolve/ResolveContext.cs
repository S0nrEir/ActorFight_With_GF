using System.Collections.Generic;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Combat.Resolve
{
    public sealed class ResolveContext : IReference
    {

        public void Setup(ResolveRequest request)
        {
            Request = request;
            FinalDelta = request != null ? request.InputDelta : 0f;
            HasApplied = false;
            IsInterrupted = false;
            IsAborted = false;
            Reason = null;
            LastPhase = ResolvePhaseType.Validity;
            _floatValues.Clear();
            _skippedPhases.Clear();
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
            // SourceMeta = default;
            FinalDelta = 0f;
            HasApplied = false;
            IsInterrupted = false;
            IsAborted = false;
            Reason = null;
            LastPhase = ResolvePhaseType.Validity;
            _floatValues.Clear();
            _skippedPhases.Clear();
        }
        
        private readonly Dictionary<string, float> _floatValues = new Dictionary<string, float>(8);
        private readonly HashSet<ResolvePhaseType> _skippedPhases = new HashSet<ResolvePhaseType>();

        public ResolveRequest Request { get; private set; }
        // public ResolveSourceMeta SourceMeta { get; private set; }
        public float FinalDelta { get; set; }
        public bool HasApplied { get; set; }
        public bool IsInterrupted { get; private set; }
        public bool IsAborted { get; private set; }
        public string Reason { get; private set; }
        public ResolvePhaseType LastPhase { get; set; }
    }
}
