using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式语法树根节点包装 / Formula AST root wrapper.
    /// </summary>
    public sealed class FormulaAst
    {
        public FormulaAst(FormulaAstNode root)
        {
            Root = root;
        }

        /// <summary>
        /// 语法树根节点 / AST root node.
        /// </summary>
        public FormulaAstNode Root { get; }
    }

    /// <summary>
    /// 语法树节点抽象基类 / Abstract base class for AST nodes.
    /// </summary>
    public abstract class FormulaAstNode
    {
        protected FormulaAstNode(int position)
        {
            Position = position;
        }

        /// <summary>
        /// 节点在源表达式中的位置 / Node position in source expression.
        /// </summary>
        public int Position { get; }
    }

    /// <summary>
    /// 一元运算符定义 / Unary operator definitions.
    /// </summary>
    public enum FormulaUnaryOperator
    {
        Plus,
        Minus
    }

    /// <summary>
    /// 二元运算符定义 / Binary operator definitions.
    /// </summary>
    public enum FormulaBinaryOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    /// <summary>
    /// 数值常量节点 / Numeric literal node.
    /// </summary>
    public sealed class FormulaNumberNode : FormulaAstNode
    {
        public FormulaNumberNode(double value, int position)
            : base(position)
        {
            Value = value;
        }

        public double Value { get; }
    }

    /// <summary>
    /// 变量节点 / Variable node.
    /// </summary>
    public sealed class FormulaVariableNode : FormulaAstNode
    {
        public FormulaVariableNode(string name, int position)
            : base(position)
        {
            Name = name;
        }

        public string Name { get; }
    }

    /// <summary>
    /// 一元表达式节点 / Unary expression node.
    /// </summary>
    public sealed class FormulaUnaryNode : FormulaAstNode
    {
        public FormulaUnaryNode(FormulaUnaryOperator @operator, FormulaAstNode operand, int position)
            : base(position)
        {
            Operator = @operator;
            Operand = operand;
        }

        public FormulaUnaryOperator Operator { get; }

        /// <summary>
        /// 一元运算节点的操作数或其对应的表达式（如果是复合的）
        /// </summary>
        public FormulaAstNode Operand { get; }
    }

    /// <summary>
    /// 二元表达式节点 / Binary expression node.
    /// </summary>
    public sealed class FormulaBinaryNode : FormulaAstNode
    {
        public FormulaBinaryNode(FormulaBinaryOperator @operator, FormulaAstNode left, FormulaAstNode right, int position)
            : base(position)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public FormulaBinaryOperator Operator { get; }

        public FormulaAstNode Left { get; }

        public FormulaAstNode Right { get; }
    }

    /// <summary>
    /// 函数调用节点 / Function call node.
    /// </summary>
    public sealed class FormulaFunctionCallNode : FormulaAstNode
    {
        public FormulaFunctionCallNode(string functionName, IReadOnlyList<FormulaAstNode> arguments, int position)
            : base(position)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }

        public string FunctionName { get; }

        public IReadOnlyList<FormulaAstNode> Arguments { get; }
    }
}