namespace Aquila.Formula
{
    /// <summary>
    /// 公式配置定义（最小字段集） / Formula configuration definition (minimal schema).
    /// </summary>
    public sealed class FormulaDefinition
    {
        /// <summary>
        /// 构造公式定义对象 / Construct a formula definition instance.
        /// </summary>
        public FormulaDefinition(int id, string expression, bool enabled)
        {
            Id = id;
            Expression = expression ?? string.Empty;
            Enabled = enabled;
        }

        /// <summary>
        /// 公式唯一 ID / Unique formula ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 公式表达式文本 / Formula expression text.
        /// </summary>
        public string Expression { get; }

        /// <summary>
        /// 是否启用该公式 / Whether this formula is enabled.
        /// </summary>
        public bool Enabled { get; }
    }
}