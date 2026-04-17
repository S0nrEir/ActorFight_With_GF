using System;
using System.Collections.Generic;

namespace Aquila.Formula
{
    internal interface IFormulaIdentifierResolver
    {
        bool TryResolveIdentifier(string identifier, object context, out double value);
    }

    /// <summary>
    /// 公式运行时求值器 / Runtime formula evaluator.
    /// </summary>
    public sealed class FormulaEvaluator
    {
        /// <summary>
        /// 执行已编译公式 / Evaluate compiled formula.
        /// </summary>
        internal FormulaResult Evaluate(
            CompiledFormula formula,
            IReadOnlyDictionary<string, double> variables,
            IFormulaIdentifierResolver identifierResolver = null,
            object context = null)
        {
            if (formula == null)
            {
                return FormulaResult.Fail(FormulaErrorCodes.RuntimeGenericError);
            }

            var variableMap = variables ?? EmptyVariables.Instance;
            if (!TryEvaluateNode(formula.Ast.Root, variableMap, identifierResolver, context, out var value, out var errorCode))
            {
                return FormulaResult.Fail(errorCode);
            }

            return FormulaResult.Ok(value);
        }

        /// <summary>
        /// 递归执行 AST 节点 / Recursively evaluate AST node.
        /// </summary>
        private static bool TryEvaluateNode(
            FormulaAstNode node,
            IReadOnlyDictionary<string, double> variables,
            IFormulaIdentifierResolver identifierResolver,
            object context,
            out double value,
            out ushort errorCode)
        {
            value = 0d;
            errorCode = FormulaErrorCodes.RuntimeNone;

            switch (node)
            {
                case FormulaNumberNode numberNode:
                    value = numberNode.Value;
                    return true;

                case FormulaVariableNode variableNode:
                    if (variables.TryGetValue(variableNode.Name, out value))
                        return true;

                    if (identifierResolver != null && identifierResolver.TryResolveIdentifier(variableNode.Name, context, out value))
                        return true;

                    errorCode = FormulaErrorCodes.RuntimeUnknownVariable;
                    return false;

                case FormulaUnaryNode unaryNode:
                    if (!TryEvaluateNode(unaryNode.Operand, variables, identifierResolver, context, out var unaryValue, out errorCode))
                    {
                        return false;
                    }

                    value = unaryNode.Operator == FormulaUnaryOperator.Minus ? -unaryValue : unaryValue;
                    return true;

                case FormulaBinaryNode binaryNode:
                    if (!TryEvaluateNode(binaryNode.Left, variables, identifierResolver, context, out var leftValue, out errorCode))
                    {
                        return false;
                    }

                    if (!TryEvaluateNode(binaryNode.Right, variables, identifierResolver, context, out var rightValue, out errorCode))
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
                                errorCode = FormulaErrorCodes.RuntimeDivideByZero;
                                return false;
                            }

                            value = leftValue / rightValue;
                            return true;
                        default:
                            errorCode = FormulaErrorCodes.RuntimeGenericError;
                            return false;
                    }

                case FormulaFunctionCallNode functionNode:
                    if (!FormulaBuiltInFunctions.TryGet(functionNode.FunctionName, out var definition))
                    {
                        errorCode = FormulaErrorCodes.RuntimeUnknownFunction;
                        return false;
                    }

                    int argCount = functionNode.Arguments.Count;
                    if (argCount < definition.MinArgCount || argCount > definition.MaxArgCount)
                    {
                        errorCode = FormulaErrorCodes.RuntimeArgCountMismatch;
                        return false;
                    }

                    var args = new double[argCount];
                    for (int i = 0; i < argCount; i++)
                    {
                        if (!TryEvaluateNode(functionNode.Arguments[i], variables, identifierResolver, context, out args[i], out errorCode))
                        {
                            return false;
                        }
                    }

                    value = definition.Evaluate(args);
                    return true;

                default:
                    errorCode = FormulaErrorCodes.RuntimeGenericError;
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

        private const double ZeroEpsilon = 1e-12;
    }
}
