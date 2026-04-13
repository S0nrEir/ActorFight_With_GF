namespace Aquila.Formula
{
    /// <summary>
    /// 公式编译错误模型 / Formula compile error model.
    /// </summary>
    public sealed class FormulaCompileError
    {
        /// <summary>
        /// 构造编译错误对象 / Construct a compile error instance.
        /// </summary>
        public FormulaCompileError(int formulaId, string errorCode, string message, int? position)
        {
            FormulaId = formulaId;
            ErrorCode = errorCode;
            Message = message;
            Position = position;
        }

        /// <summary>
        /// 发生错误的公式 ID / Formula ID where error occurs.
        /// </summary>
        public int FormulaId { get; }

        /// <summary>
        /// 结构化错误码 / Structured error code.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// 错误描述信息 / Error description message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 源表达式位置（可选） / Source expression position (optional).
        /// </summary>
        public int? Position { get; }
    }
}