using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Aquila.Formula
{
    /// <summary>
    /// 编译产物载体 / Compiled formula payload.
    /// </summary>
    internal sealed class CompiledFormula
    {
        public CompiledFormula(int formulaId, string expression, string sourceHash, int version, FormulaAst ast)
        {
            FormulaId = formulaId;
            Expression = expression;
            SourceHash = sourceHash;
            Version = version;
            Ast = ast;
        }

        public int FormulaId { get; }

        public string Expression { get; }

        public string SourceHash { get; }

        public int Version { get; }

        public FormulaAst Ast { get; }
    }

    /// <summary>
    /// 公式编译器（词法->语法->校验->常量折叠） / Formula compiler (tokenize->parse->validate->constant-fold).
    /// </summary>
    public sealed class FormulaCompiler
    {
        /// <summary>
        /// 构造编译器 / Construct compiler.
        /// </summary>
        public FormulaCompiler(FormulaValidator validator)
        {
            _validator = validator ?? new FormulaValidator();
        }

        /// <summary>
        /// 编译单条公式定义 / Compile a single formula definition.
        /// </summary>
        internal bool TryCompile(FormulaDefinition definition, int version, out CompiledFormula compiled, out FormulaCompileError error)
        {
            compiled = null;
            error = null;

            if (definition == null)
            {
                error = new FormulaCompileError(-1, FormulaErrorCodes.SyntaxError, "Formula definition is null.", 0);
                return false;
            }

            string expression = definition.Expression ?? string.Empty;
            if (!FormulaTokenizer.TryTokenize(expression, definition.Id, out var tokens, out error))
                return false;

            if (!FormulaParser.TryParse(tokens, definition.Id, out var ast, out error))
                return false;

            if (!_validator.TryValidate(ast, definition.Id, out error))
                return false;

            // 编译阶段执行常量折叠，减少运行时计算 / Do constant folding at compile time to reduce runtime cost.
            if (!TryFoldAst(ast, definition.Id, out var foldedAst, out error))
                return false;

            string sourceHash = ComputeSourceHash(expression);
            compiled = new CompiledFormula(definition.Id, expression, sourceHash, version, foldedAst);
            return true;
        }

        /// <summary>
        /// 折叠整棵语法树常量节点 / Fold constant nodes in the whole AST.
        /// </summary>
        private static bool TryFoldAst(FormulaAst ast, int formulaId, out FormulaAst foldedAst, out FormulaCompileError error)
        {
            foldedAst = null;
            if (!TryFoldNode(ast.Root, formulaId, out var root, out error))
                return false;

            foldedAst = new FormulaAst(root);
            return true;
        }

        /// <summary>
        /// 递归折叠节点 / Recursively fold node.
        /// </summary>
        private static bool TryFoldNode(FormulaAstNode node, int formulaId, out FormulaAstNode folded, out FormulaCompileError error)
        {
            error = null;
            switch (node)
            {
                case FormulaNumberNode:
                case FormulaVariableNode:
                {
                    folded = node;
                    return true;
                }

                //一元节点，拿一元节点符号后面的操作数或表达式，然后根据符号对其进行运算，缓存起来
                case FormulaUnaryNode unaryNode:
                {
                    if (!TryFoldNode(unaryNode.Operand, formulaId, out var unaryOperand, out error))
                    {
                        folded = null;
                        return false;
                    }
                    
                    //检查一元节点的符号，取值
                    if (unaryOperand is FormulaNumberNode unaryNumber)
                    {
                        double value = unaryNode.Operator == FormulaUnaryOperator.Minus ? -unaryNumber.Value : unaryNumber.Value;
                        folded = new FormulaNumberNode(value, unaryNode.Position);
                        return true;
                    }

                    folded = new FormulaUnaryNode(unaryNode.Operator, unaryOperand, unaryNode.Position);
                    return true;
                }//uynary node

                //二元表达式节点，先拿左右的节点，如果左右都是数值字面量节点（常量数值节点），根据符号算一下然后缓存，然后用一个简单的一元表达式节点代替二元（因为已经算过了）
                case FormulaBinaryNode binaryNode:
                {
                    if (!TryFoldNode(binaryNode.Left, formulaId, out var left, out error))
                    {
                        folded = null;
                        return false;
                    }

                    if (!TryFoldNode(binaryNode.Right, formulaId, out var right, out error))
                    {
                        folded = null;
                        return false;
                    }

                    if (left is FormulaNumberNode leftNumber && right is FormulaNumberNode rightNumber)
                    {
                        if (!TryEvaluateBinary(binaryNode.Operator, leftNumber.Value, rightNumber.Value, formulaId, binaryNode.Position, out var binaryValue, out error))
                        {
                            folded = null;
                            return false;
                        }

                        folded = new FormulaNumberNode(binaryValue, binaryNode.Position);
                        return true;
                    }

                    folded = new FormulaBinaryNode(binaryNode.Operator, left, right, binaryNode.Position);
                    return true;
                }//binary node

                //函数调用表达式节点，就把括号里的参数都扫一遍。
                case FormulaFunctionCallNode functionNode:
                {
                    var args = new List<FormulaAstNode>(functionNode.Arguments.Count);
                    bool allConstant = true;
                    for (int i = 0; i < functionNode.Arguments.Count; i++)
                    {
                        if (!TryFoldNode(functionNode.Arguments[i], formulaId, out var argNode, out error))
                        {
                            folded = null;
                            return false;
                        }

                        if (!(argNode is FormulaNumberNode))
                            allConstant = false;

                        args.Add(argNode);
                    }
                    
                    //如果参数全部都是常量，就去函数表里找这个函数，计算结果然后作为一个数字节点缓存，否则就视为不能折叠
                    if (allConstant)
                    {
                        if (!FormulaBuiltInFunctions.TryGet(functionNode.FunctionName, out var functionDef))
                        {
                            folded = null;
                            error = new FormulaCompileError(
                                formulaId,
                                FormulaErrorCodes.UnknownFunction,
                                $"Unknown function '{functionNode.FunctionName}'.",
                                functionNode.Position);
                            return false;
                        }

                        if (functionDef.AllowConstantFolding)
                        {
                            var values = new List<double>(args.Count);
                            for (int i = 0; i < args.Count; i++)
                                values.Add(((FormulaNumberNode)args[i]).Value);

                            double value = functionDef.Evaluate(values);
                            folded = new FormulaNumberNode(value, functionNode.Position);
                            return true;
                        }
                    }

                    folded = new FormulaFunctionCallNode(functionNode.FunctionName, args, functionNode.Position);
                    return true;
                }//case formula function call node

                default:
                {
                    folded = null;
                    error = new FormulaCompileError(
                        formulaId,
                        FormulaErrorCodes.SyntaxError,
                        $"Unsupported AST node type '{node.GetType().Name}'.",
                        node.Position);
                    return false;
                }
            }
        }

        /// <summary>
        /// 计算常量二元表达式 / Evaluate constant binary expression.
        /// </summary>
        private static bool TryEvaluateBinary(
            FormulaBinaryOperator op,
            double left,
            double right,
            int formulaId,
            int position,
            out double value,
            out FormulaCompileError error)
        {
            error = null;
            value = 0d;
            switch (op)
            {
                case FormulaBinaryOperator.Add:
                    value = left + right;
                    return true;
                
                case FormulaBinaryOperator.Subtract:
                    value = left - right;
                    return true;
                
                case FormulaBinaryOperator.Multiply:
                    value = left * right;
                    return true;
                
                case FormulaBinaryOperator.Divide:
                    if (Math.Abs(right) <= ZeroEpsilon)
                    {
                        error = new FormulaCompileError(
                            formulaId,
                            FormulaErrorCodes.DivideByZero,
                            "Division by zero in constant expression.",
                            position);
                        return false;
                    }
                    value = left / right;
                    return true;
                
                default:
                    error = new FormulaCompileError(
                        formulaId,
                        FormulaErrorCodes.SyntaxError,
                        $"Unsupported operator '{op}'.",
                        position);
                    return false;
            }
        }

        /// <summary>
        /// 计算源表达式哈希（缓存键组成） / Compute source hash (part of cache key).
        /// </summary>
        private static string ComputeSourceHash(string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source ?? string.Empty);
            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
            }
        }

        private const double ZeroEpsilon = 1e-12;
        private readonly FormulaValidator _validator;
    }
}
