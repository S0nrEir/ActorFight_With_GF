using System;
using System.Collections.Generic;
using Cfg.Fight;

namespace Aquila.Combat.Resolve
{
    public sealed class ResolvePhaseProvider
    {
        public const int DefaultResolveTypeId = 1;

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

        private readonly Dictionary<int, List<ResolvePhaseDefinition>> _phaseByType = new Dictionary<int, List<ResolvePhaseDefinition>>(8);
        private readonly List<ResolvePhaseDefinition> _defaultPhases = new List<ResolvePhaseDefinition>(16);
        private bool _initialized;

        public bool TryGetPhases(int resolveTypeId, List<ResolvePhaseDefinition> output)
        {
            if (output == null)
                return false;

            output.Clear();
            EnsureInitialized();

            var typeId = resolveTypeId > 0 ? resolveTypeId : DefaultResolveTypeId;
            if (_phaseByType.TryGetValue(typeId, out var list) && list.Count > 0)
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
            if (_defaultPhases.Count == 0)
                BuildFallbackDefaults();
        }

        // private void BuildFromTables()
        // {
        //     var tables = GameEntry.LuBan != null ? GameEntry.LuBan.Tables : null;
        //     if (tables == null || tables.CombatPhase == null)
        //         return;
        //
        //     var formulaMap = tables.FormulaSlot != null ? tables.FormulaSlot.DataMap : null;
        //
        //     var rows = tables.CombatPhase.DataList;
        //     for (var i = 0; i < rows.Count; i++)
        //     {
        //         var row = rows[i];
        //         if (row == null)
        //             continue;
        //
        //         if (!Enum.IsDefined(typeof(ResolvePhaseType), row.phase))
        //             continue;
        //
        //         var phaseType = (ResolvePhaseType)row.phase;
        //         var typeId = row.profile_id > 0 ? row.profile_id : DefaultResolveTypeId;
        //         var definition = new ResolvePhaseDefinition
        //         {
        //             ResolveTypeId = typeId,
        //             Phase = phaseType,
        //             PhaseOrder = row.phase_order,
        //             Policy = (ResolvePhasePolicy)row.policy,
        //             FormulaSlot = BuildFormulaSlot(formulaMap, row.phase),
        //         };
        //
        //         if (!_phaseByType.TryGetValue(typeId, out var list))
        //         {
        //             list = new List<ResolvePhaseDefinition>(16);
        //             _phaseByType.Add(typeId, list);
        //         }
        //
        //         list.Add(definition);
        //     }
        //
        //     foreach (var kv in _phaseByType)
        //         kv.Value.Sort(SortByPhaseOrder);
        //
        //     if (_phaseByType.TryGetValue(DefaultResolveTypeId, out var defaults) && defaults.Count > 0)
        //     {
        //         _defaultPhases.AddRange(defaults);
        //         return;
        //     }
        //
        //     foreach (var kv in _phaseByType)
        //     {
        //         if (kv.Value.Count <= 0)
        //             continue;
        //
        //         _defaultPhases.AddRange(kv.Value);
        //         break;
        //     }
        // }

        // private static ResolveFormulaSlotRef BuildFormulaSlot(Dictionary<int, Table_Formula> formulaMap, int phase)
        // {
        //     if (formulaMap == null)
        //         return default;
        //
        //     if (!formulaMap.TryGetValue(phase, out var slot) || slot == null)
        //         return default;
        //
        //     return new ResolveFormulaSlotRef
        //     {
        //         
        //     };
        // }

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
                    // FormulaSlot = default,
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
    }
}