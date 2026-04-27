using System;
using System.Collections.Generic;
using Cfg.Enum;
using GameFramework;
using UnityGameFramework.Runtime;

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

            _phaseByType.Clear();
            _defaultPhases.Clear();

            if (!BuildFromTables() || _defaultPhases.Count == 0)
                BuildFallbackDefaults();

            _initialized = true;
        }

        private bool BuildFromTables()
        {
            var luBan = GameEntry.LuBan;
            if (luBan == null || luBan.Tables == null)
                return false;

            var phaseProviderTable = luBan.Tables.CombatPhaseProvider;
            var phaseTable = luBan.Tables.CombatPhase;
            if (phaseProviderTable == null || phaseTable == null || phaseProviderTable.DataList == null || phaseProviderTable.DataList.Count == 0)
                return false;

            foreach (var providerRow in phaseProviderTable.DataList)
            {
                if (providerRow == null || providerRow.PhaseIds == null || providerRow.PhaseIds.Length == 0)
                    continue;

                var phases = new List<ResolvePhaseDefinition>(providerRow.PhaseIds.Length);
                for (var i = 0; i < providerRow.PhaseIds.Length; i++)
                {
                    var phaseRow = phaseTable.GetOrDefault(providerRow.PhaseIds[i]);
                    if (phaseRow == null)
                        continue;

                    phases.Add(new ResolvePhaseDefinition
                    {
                        ResolveTypeId = providerRow.id,
                        Phase = phaseRow.PhaseType,
                        PhaseOrder = i + 1,
                        Policy = ResolvePhasePolicy.None,
                    });
                }

                if (phases.Count == 0)
                    continue;

                phases.Sort(SortByPhaseOrder);
                _phaseByType[providerRow.id] = phases;
            }

            if (_phaseByType.TryGetValue(DefaultResolveTypeId, out var defaultPhases) && defaultPhases.Count > 0)
                _defaultPhases.AddRange(defaultPhases);

            return _phaseByType.Count > 0;
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

        private readonly Dictionary<int, List<ResolvePhaseDefinition>> _phaseByType =
            new Dictionary<int, List<ResolvePhaseDefinition>>(8);

        private readonly List<ResolvePhaseDefinition> _defaultPhases = new List<ResolvePhaseDefinition>(16);

        private static readonly ResolvePhaseType[] FallbackPhaseOrder =
        {
            ResolvePhaseType.Validity,
            ResolvePhaseType.HitCheck,
            ResolvePhaseType.BaseValue,
            ResolvePhaseType.OffenseMods,
            ResolvePhaseType.DefenseMods,
            ResolvePhaseType.CritCheck,
            ResolvePhaseType.Crit,
            ResolvePhaseType.Block,
            ResolvePhaseType.Shield,
            ResolvePhaseType.HpApply,
            ResolvePhaseType.MpApply,
            ResolvePhaseType.PostEffects,
            ResolvePhaseType.LifecycleCheck,
            ResolvePhaseType.ResolveEnd,
        };

        private bool _initialized;
    }
}
