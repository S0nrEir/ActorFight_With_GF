using System.Collections.Generic;
using Aquila;
using Aquila.Formula;
using Aquila.Toolkit;
using Cfg.Enum;
using Cfg.Fight;

namespace Aquila.Combat.Resolve
{
    /// <summary>
    /// 结算阶段处理器接口 / Resolve phase handler interface.
    /// </summary>
    public interface IResolvePhaseHandler
    {
        ResolvePhaseType PhaseType { get; }
        void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result);
    }

    /// <summary>
    /// 结算阶段处理器基类，提供默认的继续执行行为 / Base class for resolve phase handlers with default continue behavior.
    /// </summary>
    internal abstract class ResolvePhaseHandlerBase : IResolvePhaseHandler
    {
        public abstract ResolvePhaseType PhaseType { get; }

        public void Initialize()
        {
            TryLoadPhaseFormulaId();
        }

        public virtual void Execute(ResolveContext context, ResolvePhaseDefinition definition, PhaseExecutionResult result)
        {
            result.SetContinue();
        }

        protected bool TryEvaluatePhaseFormula(ResolveContext context, PhaseExecutionResult result, out float computed)
        {
            computed = 0f;
            if (!EnsurePhaseFormulaReady(out var phaseFormulaId))
            {
                Tools.Logger.Error($"[Resolve] Phase formula missing. phase={PhaseType}");
                result.SetInterrupt("phase_formula_missing");
                return false;
            }

            if (phaseFormulaId <= 0)
                return true;
            
            if (!FormulaEngine.Instance.TryEvaluate(phaseFormulaId, context, out computed, out var reason))
            {
                Tools.Logger.Error($"[Resolve] Phase formula evaluate failed. phase={PhaseType}, formulaID={phaseFormulaId}, reason={reason}");
                result.SetInterrupt("phase_formula_evaluate_failed");
                return false;
            }

            return true;
        }

        private bool EnsurePhaseFormulaReady(out int phaseFormulaId)
        {
            if (_isFormulaReady)
            {
                phaseFormulaId = _phaseFormulaId;
                return true;
            }

            TryLoadPhaseFormulaId();
            phaseFormulaId = _phaseFormulaId;
            return _isFormulaReady;
        }

        private void TryLoadPhaseFormulaId()
        {
            _isFormulaReady = false;
            _phaseFormulaId = -1;

            _phaseFormulaId = GameEntry.LuBan.Tables.CombatPhase.Get((int)PhaseType).FormulaId;
            _isFormulaReady = true;
            // _phaseFormulaId = GameEntry.LuBan.Tables.Formula.Get(phaseID.FormulaId);
            //
            // for (var i = 0; i < combatPhaseRows.Count; i++)
            // {
            //     Table_CombatPhase row = combatPhaseRows[i];
            //     if (row == null || row.PhaseType != PhaseType)
            //         continue;
            //
            //     if (row.FormulaId <= 0)
            //         continue;
            //
            //     _phaseFormulaId = row.FormulaId;
            //     _isFormulaReady = true;
            //     return;
            // }
        }

        private int _phaseFormulaId = -1;
        private bool _isFormulaReady;
    }
}
