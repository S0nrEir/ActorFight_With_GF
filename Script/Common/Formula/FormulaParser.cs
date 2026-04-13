using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式语法分析器（递归下降） / Formula parser (recursive descent).
    /// </summary>
    public sealed class FormulaParser
    {
        private readonly IReadOnlyList<FormulaToken> _tokens;
        private readonly int _formulaId;
        private int _position;

        private FormulaParser(IReadOnlyList<FormulaToken> tokens, int formulaId)
        {
            _tokens = tokens;
            _formulaId = formulaId;
            _position = 0;
        }

        /// <summary>
        /// 从 token 序列构建 AST / Build AST from token sequence.
        /// </summary>
        internal static bool TryParse(IReadOnlyList<FormulaToken> tokens, int formulaId, out FormulaAst ast, out FormulaCompileError error)
        {
            ast = null;
            error = null;
            if (tokens == null || tokens.Count == 0)
            {
                error = new FormulaCompileError(formulaId, FormulaErrorCodes.SyntaxError, "Token stream is empty.", 0);
                return false;
            }

            var parser = new FormulaParser(tokens, formulaId);
            if (!parser.TryParseExpression(out var root, out error))
            {
                return false;
            }

            // 要求消费完所有 token / Require all tokens to be consumed.
            if (parser.Current.Type != FormulaTokenType.End)
            {
                error = parser.SyntaxError($"Unexpected token '{parser.Current.Text}'.", parser.Current.Position);
                return false;
            }

            ast = new FormulaAst(root);
            return true;
        }

        /// <summary>
        /// expression -> term ((+|-) term)* / Parse additive expression.
        /// </summary>
        private bool TryParseExpression(out FormulaAstNode node, out FormulaCompileError error)
        {
            if (!TryParseTerm(out node, out error))
            {
                return false;
            }

            while (Match(FormulaTokenType.Plus) || Match(FormulaTokenType.Minus))
            {
                FormulaToken op = Previous;
                if (!TryParseTerm(out var right, out error))
                {
                    return false;
                }

                node = new FormulaBinaryNode(
                    op.Type == FormulaTokenType.Plus ? FormulaBinaryOperator.Add : FormulaBinaryOperator.Subtract,
                    node,
                    right,
                    op.Position);
            }

            return true;
        }

        /// <summary>
        /// term -> factor ((*|/) factor)* / Parse multiplicative expression.
        /// </summary>
        private bool TryParseTerm(out FormulaAstNode node, out FormulaCompileError error)
        {
            if (!TryParseFactor(out node, out error))
            {
                return false;
            }

            while (Match(FormulaTokenType.Star) || Match(FormulaTokenType.Slash))
            {
                FormulaToken op = Previous;
                if (!TryParseFactor(out var right, out error))
                {
                    return false;
                }

                node = new FormulaBinaryNode(
                    op.Type == FormulaTokenType.Star ? FormulaBinaryOperator.Multiply : FormulaBinaryOperator.Divide,
                    node,
                    right,
                    op.Position);
            }

            return true;
        }

        /// <summary>
        /// factor -> (+|-) factor | primary / Parse unary expression.
        /// </summary>
        private bool TryParseFactor(out FormulaAstNode node, out FormulaCompileError error)
        {
            if (Match(FormulaTokenType.Plus))
            {
                var op = Previous;
                if (!TryParseFactor(out var operand, out error))
                {
                    node = null;
                    return false;
                }

                node = new FormulaUnaryNode(FormulaUnaryOperator.Plus, operand, op.Position);
                return true;
            }

            if (Match(FormulaTokenType.Minus))
            {
                var op = Previous;
                if (!TryParseFactor(out var operand, out error))
                {
                    node = null;
                    return false;
                }

                node = new FormulaUnaryNode(FormulaUnaryOperator.Minus, operand, op.Position);
                return true;
            }

            return TryParsePrimary(out node, out error);
        }

        /// <summary>
        /// primary -> number | identifier | call | '(' expression ')' / Parse primary expression.
        /// </summary>
        private bool TryParsePrimary(out FormulaAstNode node, out FormulaCompileError error)
        {
            if (Match(FormulaTokenType.Number))
            {
                var token = Previous;
                node = new FormulaNumberNode(token.NumberValue, token.Position);
                error = null;
                return true;
            }

            if (Match(FormulaTokenType.Identifier))
            {
                var identifier = Previous;
                // 标识符后接 '(' 视为函数调用 / Identifier followed by '(' is a function call.
                if (Match(FormulaTokenType.LeftParen))
                {
                    var args = new List<FormulaAstNode>();
                    if (!Match(FormulaTokenType.RightParen))
                    {
                        while (true)
                        {
                            if (!TryParseExpression(out var arg, out error))
                            {
                                node = null;
                                return false;
                            }

                            args.Add(arg);
                            if (Match(FormulaTokenType.Comma))
                            {
                                continue;
                            }

                            break;
                        }

                        if (!Consume(FormulaTokenType.RightParen, "Missing ')' after function arguments.", out error))
                        {
                            node = null;
                            return false;
                        }
                    }

                    node = new FormulaFunctionCallNode(identifier.Text, args, identifier.Position);
                    return true;
                }

                node = new FormulaVariableNode(identifier.Text, identifier.Position);
                error = null;
                return true;
            }

            if (Match(FormulaTokenType.LeftParen))
            {
                if (!TryParseExpression(out node, out error))
                {
                    return false;
                }

                if (!Consume(FormulaTokenType.RightParen, "Missing ')' after expression.", out error))
                {
                    return false;
                }

                return true;
            }

            error = SyntaxError($"Unexpected token '{Current.Text}'.", Current.Position);
            node = null;
            return false;
        }

        /// <summary>
        /// 消费指定 token，否则返回语法错误 / Consume expected token or return syntax error.
        /// </summary>
        private bool Consume(FormulaTokenType expected, string message, out FormulaCompileError error)
        {
            if (Current.Type == expected)
            {
                Advance();
                error = null;
                return true;
            }

            error = SyntaxError(message, Current.Position);
            return false;
        }

        /// <summary>
        /// 匹配并前进一个 token / Match and advance one token.
        /// </summary>
        private bool Match(FormulaTokenType tokenType)
        {
            if (Current.Type != tokenType)
            {
                return false;
            }

            Advance();
            return true;
        }

        /// <summary>
        /// 前进读取指针 / Advance token cursor.
        /// </summary>
        private void Advance()
        {
            if (_position < _tokens.Count - 1)
            {
                _position++;
            }
        }

        private FormulaToken Current => _tokens[_position];

        private FormulaToken Previous => _tokens[_position - 1];

        /// <summary>
        /// 构造语法错误对象 / Build syntax error object.
        /// </summary>
        private FormulaCompileError SyntaxError(string message, int position)
        {
            return new FormulaCompileError(_formulaId, FormulaErrorCodes.SyntaxError, message, position);
        }
    }
}