namespace Aquila.Formula
{
    /// <summary>
    /// 公式引擎错误码常量 / Error code constants for formula engine.
    /// </summary>
    public static class FormulaErrorCodes
    {
        /// <summary>语法错误 / Syntax error.</summary>
        public const string SyntaxError = "FORMULA_SYNTAX_ERROR";

        /// <summary>未知变量 / Unknown variable.</summary>
        public const string UnknownVariable = "FORMULA_UNKNOWN_VARIABLE";

        /// <summary>未知函数 / Unknown function.</summary>
        public const string UnknownFunction = "FORMULA_UNKNOWN_FUNCTION";

        /// <summary>参数数量不匹配 / Argument count mismatch.</summary>
        public const string ArgCountMismatch = "FORMULA_ARG_COUNT_MISMATCH";

        /// <summary>除零错误 / Divide-by-zero error.</summary>
        public const string DivideByZero = "FORMULA_DIVIDE_BY_ZERO";

        /// <summary>运行时错误 / Runtime error.</summary>
        public const string RuntimeError = "FORMULA_RUNTIME_ERROR";
    }
}