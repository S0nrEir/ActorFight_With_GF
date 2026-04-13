using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式引擎统一入口接口 / Unified entry interface for formula engine.
    /// </summary>
    public interface IFormulaEngine
    {
        /// <summary>
        /// 批量编译公式定义并返回编译错误集合 / Compile formula definitions in batch and return compile errors.
        /// </summary>
        IReadOnlyList<FormulaCompileError> CompileAll(IEnumerable<FormulaDefinition> definitions);

        /// <summary>
        /// 按公式 ID 执行求值并返回结构化结果 / Evaluate by formula ID and return structured result.
        /// </summary>
        FormulaResult Evaluate(int formulaId, IReadOnlyDictionary<string, double> variables);
    }
}