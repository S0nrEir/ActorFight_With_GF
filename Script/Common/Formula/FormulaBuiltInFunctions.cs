using System;
using System.Collections.Generic;
using System.Linq;

namespace Aquila.Formula
{
    /// <summary>
    /// 内置函数元数据定义 / Built-in function metadata definition.
    /// </summary>
    internal sealed class FormulaFunctionDefinition
    {
        public FormulaFunctionDefinition(string name, int minArgCount, int maxArgCount, Func<IReadOnlyList<double>, double> evaluate)
        {
            Name = name;
            MinArgCount = minArgCount;
            MaxArgCount = maxArgCount;
            Evaluate = evaluate;
        }

        /// <summary>
        /// 函数名 / Function name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 最小参数数量 / Minimum argument count.
        /// </summary>
        public int MinArgCount { get; }

        /// <summary>
        /// 最大参数数量 / Maximum argument count.
        /// </summary>
        public int MaxArgCount { get; }

        /// <summary>
        /// 函数执行委托 / Function execution delegate.
        /// </summary>
        public Func<IReadOnlyList<double>, double> Evaluate { get; }
    }

    /// <summary>
    /// 内置函数注册表（白名单） / Built-in function registry (allow-list).
    /// </summary>
    internal static class FormulaBuiltInFunctions
    {
        // 仅注册 MVP 允许函数 / Register MVP allowed functions only.
        private static readonly IReadOnlyDictionary<string, FormulaFunctionDefinition> Definitions =
            new Dictionary<string, FormulaFunctionDefinition>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "abs",
                    new FormulaFunctionDefinition("abs", 1, 1, args => Math.Abs(args[0]))
                },
                {
                    "clamp",
                    new FormulaFunctionDefinition(
                        "clamp",
                        3,
                        3,
                        args =>
                        {
                            double min = Math.Min(args[1], args[2]);
                            double max = Math.Max(args[1], args[2]);
                            return Math.Min(Math.Max(args[0], min), max);
                        })
                },
                {
                    "min",
                    new FormulaFunctionDefinition("min", 2, int.MaxValue, args => args.Min())
                },
                {
                    "max",
                    new FormulaFunctionDefinition("max", 2, int.MaxValue, args => args.Max())
                }
            };

        /// <summary>
        /// 按名称查找函数定义 / Try get function definition by name.
        /// </summary>
        public static bool TryGet(string name, out FormulaFunctionDefinition definition)
        {
            return Definitions.TryGetValue(name, out definition);
        }

        /// <summary>
        /// 获取全部注册函数 / Get all registered functions.
        /// </summary>
        public static IReadOnlyDictionary<string, FormulaFunctionDefinition> GetAll()
        {
            return Definitions;
        }
    }
}