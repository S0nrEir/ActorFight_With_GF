using System;
using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 语义校验器（变量/函数/参数） / Semantic validator (variables/functions/arguments).
    /// </summary>
    public sealed class FormulaValidator
    {
        /// <summary>
        /// 构造校验器，可选指定允许变量集合 / Construct validator with optional allowed variable set.
        /// </summary>
        public FormulaValidator(ISet<string> allowedVariables = null)
        {
            _allowedVariables = allowedVariables;
        }

        /// <summary>
        /// 校验 AST 是否满足运行前约束 / Validate AST against pre-runtime constraints.
        /// </summary>
        public bool TryValidate(FormulaAst ast, int formulaId, out FormulaCompileError error)
        {
            error = null;
            if (ast == null || ast.Root == null)
            {
                error = new FormulaCompileError(formulaId, FormulaErrorCodes.SyntaxError, "AST is empty.", 0);
                return false;
            }

            return TryValidateNode(ast.Root, formulaId, out error);
        }

        /// <summary>
        /// 递归校验节点语义 / Recursively validate node semantics.
        /// </summary>
        private bool TryValidateNode(FormulaAstNode node, int formulaId, out FormulaCompileError error)
        {
            error = null;
            switch (node)
            {
                case FormulaNumberNode:
                    return true;

                case FormulaVariableNode variableNode:
                    // 若配置了白名单，变量必须在白名单中 / Variable must be in allow-list when allow-list is configured.
                    if (_allowedVariables != null && _allowedVariables.Count > 0 && !_allowedVariables.Contains(variableNode.Name))
                    {
                        error = new FormulaCompileError(
                            formulaId,
                            FormulaErrorCodes.UnknownVariable,
                            $"Unknown variable '{variableNode.Name}'.",
                            variableNode.Position);
                        return false;
                    }

                    return true;

                case FormulaUnaryNode unaryNode:
                    return TryValidateNode(unaryNode.Operand, formulaId, out error);

                case FormulaBinaryNode binaryNode:
                    if (!TryValidateNode(binaryNode.Left, formulaId, out error))
                    {
                        return false;
                    }

                    return TryValidateNode(binaryNode.Right, formulaId, out error);

                case FormulaFunctionCallNode functionNode:
                    if (!FormulaBuiltInFunctions.TryGet(functionNode.FunctionName, out var definition))
                    {
                        error = new FormulaCompileError(
                            formulaId,
                            FormulaErrorCodes.UnknownFunction,
                            $"Unknown function '{functionNode.FunctionName}'.",
                            functionNode.Position);
                        return false;
                    }

                    int argCount = functionNode.Arguments.Count;
                    if (argCount < definition.MinArgCount || argCount > definition.MaxArgCount)
                    {
                        string expected = definition.MaxArgCount == int.MaxValue
                            ? $">= {definition.MinArgCount}"
                            : $"{definition.MinArgCount}";
                        error = new FormulaCompileError(
                            formulaId,
                            FormulaErrorCodes.ArgCountMismatch,
                            $"Function '{functionNode.FunctionName}' argument count mismatch. Expected {expected}, actual {argCount}.",
                            functionNode.Position);
                        return false;
                    }

                    for (int i = 0; i < functionNode.Arguments.Count; i++)
                    {
                        if (!TryValidateNode(functionNode.Arguments[i], formulaId, out error))
                        {
                            return false;
                        }
                    }

                    return true;

                default:
                    error = new FormulaCompileError(
                        formulaId,
                        FormulaErrorCodes.SyntaxError,
                        $"Unsupported AST node type '{node.GetType().Name}'.",
                        node.Position);
                    return false;
            }
        }

        private readonly ISet<string> _allowedVariables;
    }
}