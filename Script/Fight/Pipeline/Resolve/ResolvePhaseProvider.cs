using System;
using System.Collections.Generic;
using Cfg.Enum;
using Cfg.Fight;
using GameFramework;

namespace Aquila.Combat.Resolve
{
    public sealed class ResolvePhaseProvider
    {

        public bool TryGetPhases(int resolveTypeId, List<ResolvePhaseDefinition> output)
        {
            if (output == null)
                return false;

            output.Clear();
            EnsureInitialized();

            if (resolveTypeId < 0)
                throw new GameFrameworkException($"ResolvePhase type {resolveTypeId} not found.");
            
            if (_phaseByType.TryGetValue(resolveTypeId, out var list) && list.Count > 0)
            {
                output.AddRange(list);
                return true;
            }

            output.AddRange(_defaultPhases);
            return false;
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            _initialized = true;
            _phaseByType.Clear();
            _defaultPhases.Clear();

            // BuildFromTables();
            // if (_defaultPhases.Count == 0)
            //     BuildFallbackDefaults();
            
            //#todo:_phaseByType初始化缓存所有结算阶段
        }
        
        private void BuildFallbackDefaults()
        {
            var sortOrder = 1;
            for (var i = 0; i < FallbackPhaseOrder.Length; i++)
            {
                _defaultPhases.Add(new ResolvePhaseDefinition
                {
                    ResolveTypeId = DefaultResolveTypeId,
                    Phase = FallbackPhaseOrder[i],
                    PhaseOrder = sortOrder++,
                    Policy = ResolvePhasePolicy.None,
                });
            }
        }

        private static int SortByPhaseOrder(ResolvePhaseDefinition a, ResolvePhaseDefinition b)
        {
            var orderCompare = a.PhaseOrder.CompareTo(b.PhaseOrder);
            if (orderCompare != 0)
                return orderCompare;

            return a.Phase.CompareTo(b.Phase);
        }
        
        public const int DefaultResolveTypeId = 1;
        private readonly Dictionary<int, List<ResolvePhaseDefinition>> _phaseByType = new Dictionary<int, List<ResolvePhaseDefinition>>(8);
        private readonly List<ResolvePhaseDefinition> _defaultPhases = new List<ResolvePhaseDefinition>(16);
        
        private static readonly ResolvePhaseType[] FallbackPhaseOrder =
        {
            ResolvePhaseType.Validity,
            ResolvePhaseType.HitCheck,
            ResolvePhaseType.BaseValue,
            ResolvePhaseType.OffenseMods,
            ResolvePhaseType.DefenseMods,
            ResolvePhaseType.Crit,
            ResolvePhaseType.Block,
            ResolvePhaseType.Shield,
            ResolvePhaseType.HpApply,
            ResolvePhaseType.PostEffects,
            ResolvePhaseType.LifecycleCheck,
        };
        
        private bool _initialized;
    }
}