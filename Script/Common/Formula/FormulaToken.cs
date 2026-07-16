namespace Aquila.Formula
{
    /// <summary>
    /// 词法单元类型定义 / Token type definitions.
    /// </summary>
    internal enum FormulaTokenType
    {
        Number,
        Identifier,
        Plus,
        Minus,
        Star,
        Slash,
        LeftParen,
        RightParen,
        Comma,
        End
    }

    /// <summary>
    /// 词法单元结构 / Token structure.
    /// </summary>
    internal readonly struct FormulaToken
    {
        public FormulaToken(FormulaTokenType type, string text, double numberValue, int position)
        {
            Type = type;
            Text = text;
            NumberValue = numberValue;
            Position = position;
        }

        /// <summary>
        /// 词法类型 / Token type.
        /// </summary>
        public FormulaTokenType Type { get; }

        /// <summary>
        /// 原始文本 / Raw token text.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 数值字面量值（非 Number 时忽略） / Numeric literal value (ignored for non-number tokens).
        /// </summary>
        public double NumberValue { get; }

        /// <summary>
        /// 在源表达式中的起始位置 / Start position in source expression.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// 创建符号 token / Create a symbol token.
        /// </summary>
        public static FormulaToken ForSymbol(FormulaTokenType type, string text, int position)
        {
            return new FormulaToken(type, text, 0d, position);
        }

        /// <summary>
        /// 创建数字 token / Create a number token.
        /// </summary>
        public static FormulaToken ForNumber(string text, double value, int position)
        {
            return new FormulaToken(FormulaTokenType.Number, text, value, position);
        }

        /// <summary>
        /// 创建标识符 token / Create an identifier token.
        /// </summary>
        public static FormulaToken ForIdentifier(string text, int position)
        {
            return new FormulaToken(FormulaTokenType.Identifier, text, 0d, position);
        }
    }
}