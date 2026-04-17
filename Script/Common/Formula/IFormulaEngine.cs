using System;
using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 标识符重定向取值函数：传入 context，返回标识符对应的值。
    /// </summary>
    public delegate bool FormulaIdentifierRedirector(object context, out double value);

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
        // FormulaResult Evaluate(int formulaId, IReadOnlyDictionary<string, double> variables);
        //
        // /// <summary>
        // /// 按公式 ID 执行求值，可传入上下文供标识符重定向使用。
        // /// </summary>
        // FormulaResult Evaluate(int formulaId, IReadOnlyDictionary<string, double> variables, object context);

        /// <summary>
        /// TryEvaluate：包含初始化、求值、错误原因输出。
        /// </summary>
        // bool TryEvaluate(int formulaId, IReadOnlyDictionary<string, double> variables, object context, out float value, out string reason);

        /// <summary>
        /// 批量设置标识符重定向器。
        /// </summary>
        // void SetIdentifierRedirectors(IReadOnlyDictionary<string, FormulaIdentifierRedirector> redirectors);
        //
        // /// <summary>
        // /// 注册单个标识符重定向器。
        // /// </summary>
        // bool RegisterIdentifierRedirector(string identifier, FormulaIdentifierRedirector redirector);
        //
        // /// <summary>
        // /// 清空标识符重定向器。
        // /// </summary>
        // void ClearIdentifierRedirectors();
    }
}
