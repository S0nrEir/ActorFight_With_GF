using System;
using System.Collections.Generic;
using System.Globalization;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式词法分析器 / Formula tokenizer.
    /// </summary>
    public static class FormulaTokenizer
    {
        /// <summary>
        /// 将表达式字符串切分为 token 序列 / Tokenize source expression into token sequence.
        /// </summary>
        internal static bool TryTokenize(string source, int formulaId, out List<FormulaToken> tokens, out FormulaCompileError error)
        {
            tokens = null;
            error = null;

            if (source == null)
            {
                error = new FormulaCompileError(formulaId, FormulaErrorCodes.SyntaxError, "Expression cannot be null.", 0);
                return false;
            }

            var result = new List<FormulaToken>();
            int index = 0;
            while (index < source.Length)
            {
                char ch = source[index];
                // 跳过空白字符 / Skip whitespace characters.
                if (char.IsWhiteSpace(ch))
                {
                    index++;
                    continue;
                }

                // 优先尝试数字字面量 / Try numeric literal first.
                // 找出字符串中的数字
                if (IsNumberStart(source, index))
                {
                    if (!TryReadNumber(source, ref index, formulaId, out var token, out error))
                        return false;

                    result.Add(token);
                    continue;
                }

                // 读取变量名或函数名标识符 / Read variable or function identifier.
                // 找出字符串中的标识符，比如actor.atk
                if (IsIdentifierStart(ch))
                {
                    int start = index;
                    index++;
                    while (index < source.Length && IsIdentifierPart(source[index]))
                    {
                        index++;
                    }

                    string identifier = source.Substring(start, index - start);
                    result.Add(FormulaToken.ForIdentifier(identifier, start));
                    continue;
                }

                // 处理 DSL 支持的单字符符号 / Handle single-character symbols in DSL.
                // 处理运算符号
                switch (ch)
                {
                    case '+':
                        result.Add(FormulaToken.ForSymbol(FormulaTokenType.Plus, "+", index));
                        index++;
                        break;

                    case '-':
                        result.Add(FormulaToken.ForSymbol(FormulaTokenType.Minus, "-", index));
                        index++;
                        break;

                    case '*':
                        result.Add(FormulaToken.ForSymbol(FormulaTokenType.Star, "*", index));
                        index++;
                        break;

                    case '/':
                        result.Add(FormulaToken.ForSymbol(FormulaTokenType.Slash, "/", index));
                        index++;
                        break;

                    case '(':
                        result.Add(FormulaToken.ForSymbol(FormulaTokenType.LeftParen, "(", index));
                        index++;
                        break;

                    case ')':
                        result.Add(FormulaToken.ForSymbol(FormulaTokenType.RightParen, ")", index));
                        index++;
                        break;

                    case ',':
                        result.Add(FormulaToken.ForSymbol(FormulaTokenType.Comma, ",", index));
                        index++;
                        break;

                    default:
                        error = new FormulaCompileError(
                            formulaId,
                            FormulaErrorCodes.SyntaxError,
                            $"Unsupported character '{ch}'.",
                            index);
                        return false;
                }
            }

            // 追加结束标记供语法分析器消费 / Append end token for parser.
            result.Add(FormulaToken.ForSymbol(FormulaTokenType.End, string.Empty, source.Length));
            tokens = result;
            return true;
        }

        /// <summary>
        /// 判断当前位置是否为数字起始 / Check whether current position starts a number literal.
        /// </summary>
        private static bool IsNumberStart(string source, int index)
        {
            char ch = source[index];
            if (char.IsDigit(ch))
                return true;

            if (ch == '.' && index + 1 < source.Length)
                return char.IsDigit(source[index + 1]);

            return false;
        }

        /// <summary>
        /// 读取一个浮点数字面量 / Read one floating-point literal.
        /// </summary>
        private static bool TryReadNumber(
            string source,
            ref int index,
            int formulaId,
            out FormulaToken token,
            out FormulaCompileError error)
        {
            token = default;
            error = null;
            int start = index;
            bool hasDot = false;

            while (index < source.Length)
            {
                char ch = source[index];
                if (char.IsDigit(ch))
                {
                    index++;
                    continue;
                }

                if (ch == '.')
                {
                    if (hasDot)
                    {
                        break;
                    }

                    hasDot = true;
                    index++;
                    continue;
                }

                break;
            }

            string text = source.Substring(start, index - start);
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                error = new FormulaCompileError(
                    formulaId,
                    FormulaErrorCodes.SyntaxError,
                    $"Invalid number literal '{text}'.",
                    start);
                return false;
            }

            token = FormulaToken.ForNumber(text, value, start);
            return true;
        }

        /// <summary>
        /// 判断是否为标识符首字符 / Check whether char can start an identifier.
        /// </summary>
        private static bool IsIdentifierStart(char ch)
        {
            return char.IsLetter(ch) || ch == '_';
        }

        /// <summary>
        /// 判断是否为标识符组成字符 / Check whether char can be part of an identifier.
        /// </summary>
        private static bool IsIdentifierPart(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '_' || ch == '.';
        }
    }
}