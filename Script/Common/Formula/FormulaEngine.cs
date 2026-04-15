using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式引擎门面（编译+缓存+执行） / Formula engine facade (compile + cache + evaluate).
    /// </summary>
    public sealed class FormulaEngine : IFormulaEngine
    {
        /// <summary>
        /// 构造引擎并可选注入变量白名单 / Construct engine with optional variable allow-list.
        /// </summary>
        public FormulaEngine(int version = 1, IEnumerable<string> allowedVariables = null)
        {
            _version = version;
            _cache = new FormulaCache();
            _evaluator = new FormulaEvaluator();

            HashSet<string> variableSet = null;
            if (allowedVariables != null)
            {
                variableSet = new HashSet<string>(allowedVariables, System.StringComparer.OrdinalIgnoreCase);
            }

            _compiler = new FormulaCompiler(new FormulaValidator(variableSet));
        }

        /// <summary>
        /// 批量编译并刷新缓存 / Compile in batch and refresh cache.
        /// </summary>
        public IReadOnlyList<FormulaCompileError> CompileAll(IEnumerable<FormulaDefinition> definitions)
        {
            var errors = new List<FormulaCompileError>();
            if (definitions == null)
            {
                errors.Add(new FormulaCompileError(-1, FormulaErrorCodes.SyntaxError, "Formula definitions are null.", 0));
                return errors;
            }

            foreach (var definition in definitions)
            {
                if (definition == null)
                {
                    errors.Add(new FormulaCompileError(-1, FormulaErrorCodes.SyntaxError, "Formula definition item is null.", 0));
                    continue;
                }

                // disabled 公式不进入缓存 / Disabled formulas are removed from cache.
                if (!definition.Enabled)
                {
                    _cache.RemoveById(definition.Id);
                    continue;
                }

                //缓存所有的计算公式，ID就是表配置ID
                if (_compiler.TryCompile(definition, _version, out var compiled, out var compileError))
                {
                    _cache.Set(compiled);
                    continue;
                }

                errors.Add(compileError);
                _cache.RemoveById(definition.Id);
            }

            return errors;
        }

        /// <summary>
        /// 执行指定公式 ID / Evaluate specified formula ID.
        /// </summary>
        public FormulaResult Evaluate(int formulaId, IReadOnlyDictionary<string, double> variables)
        {
            if (!_cache.TryGetById(formulaId, out var compiled))
            {
                return FormulaResult.Fail(FormulaErrorCodes.RuntimeGenericError);
            }

            return _evaluator.Evaluate(compiled, variables);
        }
        
        private readonly FormulaCompiler _compiler;
        private readonly FormulaEvaluator _evaluator;
        private readonly FormulaCache _cache;
        private readonly int _version;
    }
}