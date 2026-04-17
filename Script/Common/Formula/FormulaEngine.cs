using System;
using System.Collections.Generic;
using Aquila;
using Cfg.Fight;
using GameFramework;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式引擎门面（编译+缓存+执行）/ Formula engine facade (compile + cache + evaluate).
    /// </summary>
    public sealed class FormulaEngine : IFormulaEngine, IFormulaIdentifierResolver
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
                variableSet = new HashSet<string>(allowedVariables, StringComparer.OrdinalIgnoreCase);

            _compiler = new FormulaCompiler(new FormulaValidator(variableSet));
            _identifierRedirectors = new Dictionary<string, FormulaIdentifierRedirector>(StringComparer.OrdinalIgnoreCase);
            

        }

        public static bool Init()
        {
            _instance = new FormulaEngine(1);
            if (!_instance.EnsureInitialized(GameEntry.LuBan.Tables.Formula.DataList,out var reason))
                throw new GameFrameworkException($"Failed to initialize FormulaEngine: {reason}");

            return true;
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

                if (!definition.Enabled)
                {
                    _cache.RemoveById(definition.Id);
                    continue;
                }

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
            return Evaluate(formulaId, variables, null);
        }

        /// <summary>
        /// 执行指定公式 ID（携带 context 用于标识符重定向）.
        /// </summary>
        public FormulaResult Evaluate(int formulaId, IReadOnlyDictionary<string, double> variables, object context)
        {
            if (!_cache.TryGetById(formulaId, out var compiled))
            {
                return FormulaResult.Fail(FormulaErrorCodes.RuntimeGenericError);
            }

            return _evaluator.Evaluate(compiled, variables, this, context);
        }

        /// <summary>
        /// TryEvaluate：包含初始化与失败原因。
        /// </summary>
        public bool TryEvaluate(int formulaId, IReadOnlyDictionary<string, double> variables, object context, out float value, out string reason)
        {
            value = 0f;

            if (formulaId <= 0)
            {
                reason = $"formula_id_invalid:{formulaId}";
                return false;
            }
            var result = Evaluate(formulaId, variables, context);
            if (!result.Success || !result.Value.HasValue)
            {
                reason = $"formula_eval_failed:{formulaId}:err={result.ErrorCode}";
                return false;
            }

            value = (float)result.Value.Value;
            reason = null;
            return true;
        }

        public void SetIdentifierRedirectors(IReadOnlyDictionary<string, FormulaIdentifierRedirector> redirectors)
        {
            _identifierRedirectors.Clear();
            if (redirectors == null)
                return;

            foreach (var pair in redirectors)
            {
                if (string.IsNullOrEmpty(pair.Key) || pair.Value == null)
                    continue;

                _identifierRedirectors[pair.Key] = pair.Value;
            }
        }

        public bool RegisterIdentifierRedirector(string identifier, FormulaIdentifierRedirector redirector)
        {
            if (string.IsNullOrEmpty(identifier) || redirector == null)
                return false;
            
            _identifierRedirectors[identifier] = redirector;

            return true;
        }

        public void ClearIdentifierRedirectors()
        {
            _identifierRedirectors.Clear();
        }

        bool IFormulaIdentifierResolver.TryResolveIdentifier(string identifier, object context, out double value)
        {
            value = 0d;

            if (string.IsNullOrEmpty(identifier))
                return false;

            FormulaIdentifierRedirector redirector;
            lock (_redirectSync)
            {
                if (!_identifierRedirectors.TryGetValue(identifier, out redirector))
                    return false;
            }

            return redirector != null && redirector(context, out value);
        }

        /// <summary>
        /// 初始化拆分函数：确保运行时公式完成编译缓存。
        /// </summary>
        private bool EnsureInitialized(List<Table_Formula> formulaList,out string reason)
        {
            if (_isInitialized)
            {
                reason = null;
                return true;
            }

            if (!string.IsNullOrEmpty(_initializeFailureReason))
            {
                reason = _initializeFailureReason;
                return false;
            }

            if (!TryBuildRuntimeDefinitions(formulaList , out var definitions, out reason))
            {
                _initializeFailureReason = reason;
                return false;
            }

            var compileErrors = CompileAll(definitions);
            if (compileErrors != null && compileErrors.Count > 0)
            {
                var first = compileErrors[0];
                reason = $"formula_compile_failed:{first.FormulaId}:{first.ErrorCode}";
                _initializeFailureReason = reason;
                return false;
            }

            _isInitialized = true;
            reason = null;
            return true;
            
        }

        private static bool TryBuildRuntimeDefinitions(List<Table_Formula> formulaList, out List<FormulaDefinition> definitions, out string reason)
        {
            definitions = null;
            reason = null;

            var formulaTable = GameEntry.LuBan?.Tables?.Formula;
            if (formulaTable == null)
            {
                reason = "formula_table_missing";
                return false;
            }
            
            
            definitions = new List<FormulaDefinition>(formulaList != null ? formulaList.Count : 0);
            if (formulaList == null)
                return true;

            for (var i = 0; i < formulaList.Count; i++)
            {
                var row = formulaList[i];
                if (row == null)
                    continue;

                definitions.Add(new FormulaDefinition(row.id, row.Expression, true));
            }

            return true;
        }

        private readonly FormulaCompiler _compiler;
        private readonly FormulaEvaluator _evaluator;
        private readonly FormulaCache _cache;
        private readonly int _version;

        private readonly object _initializeSync = new object();
        private bool _isInitialized;
        private string _initializeFailureReason;

        private readonly object _redirectSync = new object();
        private readonly Dictionary<string, FormulaIdentifierRedirector> _identifierRedirectors;
        
        public static FormulaEngine Instance
        {
            get
            {
                if(_instance is null)
                    _instance = new FormulaEngine();
                
                return _instance;
            }
        }

        private static FormulaEngine _instance = null;
    }
}
