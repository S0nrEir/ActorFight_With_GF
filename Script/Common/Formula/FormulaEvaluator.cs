using System;
using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式运行时求值器 / Runtime formula evaluator.
    /// </summary>
    public sealed class FormulaEvaluator
    {
        private const double ZeroEpsilon = 1e-12;

        /// <summary>
        /// 执行已编译公式 / Evaluate compiled formula.
        /// </summary>
        internal FormulaResult Evaluate(CompiledFormula formula, IReadOnlyDictionary<string, double> variables)
        {
            if (formula == null)
            {
                return FormulaResult.Fail(FormulaErrorCodes.RuntimeError, "Compiled formula is null.");
            }

            var variableMap = variables ?? EmptyVariables.Instance;
            if (!TryEvaluateNode(formula.Ast.Root, variableMap, out var value, out var errorCode, out var errorMessage))
            {
                return FormulaResult.Fail(errorCode, errorMessage);
            }

            return FormulaResult.Ok(value);
        }

        /// <summary>
        /// 递归执行 AST 节点 / Recursively evaluate AST node.
        /// </summary>
        private static bool TryEvaluateNode(
            FormulaAstNode node,
            IReadOnlyDictionary<string, double> variables,
            out double value,
            out string errorCode,
            out string errorMessage)
        {
            value = 0d;
            errorCode = null;
            errorMessage = null;

            switch (node)
            {
                case FormulaNumberNode numberNode:
                    value = numberNode.Value;
                    return true;

                case FormulaVariableNode variableNode:
                    if (!variables.TryGetValue(variableNode.Name, out value))
                    {
                        errorCode = FormulaErrorCodes.UnknownVariable;
                        errorMessage = $"Variable '{variableNode.Name}' is not provided.";
                        return false;
                    }

                    return true;

                case FormulaUnaryNode unaryNode:
                    if (!TryEvaluateNode(unaryNode.Operand, variables, out var unaryValue, out errorCode, out errorMessage))
                    {
                        return false;
                    }

                    value = unaryNode.Operator == FormulaUnaryOperator.Minus ? -unaryValue : unaryValue;
                    return true;

                case FormulaBinaryNode binaryNode:
                    if (!TryEvaluateNode(binaryNode.Left, variables, out var leftValue, out errorCode, out errorMessage))
                    {
                        return false;
                    }

                    if (!TryEvaluateNode(binaryNode.Right, variables, out var rightValue, out errorCode, out errorMessage))
                    {
                        return false;
                    }

                    switch (binaryNode.Operator)
                    {
                        case FormulaBinaryOperator.Add:
                            value = leftValue + rightValue;
                            return true;
                        case FormulaBinaryOperator.Subtract:
                            value = leftValue - rightValue;
                            return true;
                        case FormulaBinaryOperator.Multiply:
                            value = leftValue * rightValue;
                            return true;
                        case FormulaBinaryOperator.Divide:
                            if (Math.Abs(rightValue) <= ZeroEpsilon)
                            {
                                errorCode = FormulaErrorCodes.DivideByZero;
                                errorMessage = "Division by zero.";
                                return false;
                            }

                            value = leftValue / rightValue;
                            return true;
                        default:
                            errorCode = FormulaErrorCodes.RuntimeError;
                            errorMessage = $"Unsupported binary operator '{binaryNode.Operator}'.";
                            return false;
                    }

                case FormulaFunctionCallNode functionNode:
                    if (!FormulaBuiltInFunctions.TryGet(functionNode.FunctionName, out var definition))
                    {
                        errorCode = FormulaErrorCodes.UnknownFunction;
                        errorMessage = $"Unknown function '{functionNode.FunctionName}'.";
                        return false;
                    }

                    int argCount = functionNode.Arguments.Count;
                    if (argCount < definition.MinArgCount || argCount > definition.MaxArgCount)
                    {
                        errorCode = FormulaErrorCodes.ArgCountMismatch;
                        errorMessage = $"Function '{functionNode.FunctionName}' argument count mismatch.";
                        return false;
                    }

                    // 先递归求参数，再调用白名单函数 / Evaluate args first, then call allow-listed function.
                    var args = new double[argCount];
                    for (int i = 0; i < argCount; i++)
                    {
                        if (!TryEvaluateNode(functionNode.Arguments[i], variables, out args[i], out errorCode, out errorMessage))
                        {
                            return false;
                        }
                    }

                    value = definition.Evaluate(args);
                    return true;

                default:
                    errorCode = FormulaErrorCodes.RuntimeError;
                    errorMessage = $"Unsupported AST node '{node.GetType().Name}'.";
                    return false;
            }
        }

        /// <summary>
        /// 空变量字典单例 / Empty variable dictionary singleton.
        /// </summary>
        private static class EmptyVariables
        {
            public static readonly IReadOnlyDictionary<string, double> Instance = new Dictionary<string, double>();
        }
    }
}