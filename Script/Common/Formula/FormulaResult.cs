namespace Aquila.Formula
{
    /// <summary>
    /// 公式执行结果 / Formula execution result.
    /// </summary>
    public sealed class FormulaResult
    {
        private FormulaResult(bool success, double? value, string errorCode, string errorMessage)
        {
            Success = success;
            Value = value;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 是否执行成功 / Whether evaluation succeeds.
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// 成功时的数值结果 / Numeric value when success.
        /// </summary>
        public double? Value { get; }

        /// <summary>
        /// 失败时错误码 / Error code when failed.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// 失败时错误信息 / Error message when failed.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// 创建成功结果 / Create success result.
        /// </summary>
        public static FormulaResult Ok(double value)
        {
            return new FormulaResult(true, value, null, null);
        }

        /// <summary>
        /// 创建失败结果 / Create failure result.
        /// </summary>
        public static FormulaResult Fail(string errorCode, string errorMessage)
        {
            return new FormulaResult(false, null, errorCode, errorMessage);
        }
    }
}