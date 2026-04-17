using System;
using System.Collections.Generic;
using Aquila;
using Aquila.Combat.Resolve;
using Aquila.Fight.Addon;
using Aquila.Module;
using Cfg.Enum;
using Cfg.Fight;
using GameFramework;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式引擎门面（编译+缓存+执行）/ Formula engine facade (compile + cache + evaluate).
    /// </summary>
    public sealed class FormulaEngine : IFormulaEngine,IFormulaIdentifierResolver
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

            RegisterBuiltinIdentifierRedirectors(_instance);

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
        // public FormulaResult Evaluate(int formulaId, IReadOnlyDictionary<string, double> variables)
        // {
        //     return Evaluate(formulaId, variables, null);
        // }

        /// <summary>
        /// 执行指定公式 ID（携带 context 用于标识符重定向）.
        /// </summary>
        public FormulaResult Evaluate(int formulaId,Dictionary<string, FormulaIdentifierRedirector> identifierRedirectors ,object context)
        {
            if (!_cache.TryGetById(formulaId, out var compiled))
            {
                return FormulaResult.Fail(FormulaErrorCodes.RuntimeGenericError);
            }

            return _evaluator.Evaluate(compiled,identifierRedirectors, context);
        }

        /// <summary>
        /// TryEvaluate：包含初始化与失败原因。
        /// </summary>
        public bool TryEvaluate(int formulaId, object context, out float value, out string reason)
        {
            value = 0f;

            if (formulaId <= 0)
            {
                reason = $"formula_id_invalid:{formulaId}";
                return false;
            }
            var result = Evaluate(formulaId,_identifierRedirectors, context);
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

            if (!_identifierRedirectors.TryGetValue(identifier, out var redirector))
                return false;

            return redirector != null && redirector(context, out value);
        }

        /// <summary>
        /// 注册内置标识符重定向器：将 castor/target 与各属性的访问函数绑定。
        /// </summary>
        private static void RegisterBuiltinIdentifierRedirectors(FormulaEngine engine)
        {
            var redirectors = BuildBuiltinRedirectors();
            foreach (var kvp in redirectors)
                engine.RegisterIdentifierRedirector(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// 构建内置标识符重定向器映射表。
        /// 每条 castor.xxx / target.xxx 显式写死，不拼字符串。
        /// </summary>
        private static Dictionary<string, FormulaIdentifierRedirector> BuildBuiltinRedirectors()
        {
            var redirectors = new Dictionary<string, FormulaIdentifierRedirector>(StringComparer.OrdinalIgnoreCase);

            // ---- castor 属性 ----
            redirectors["castor.hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Curr_HP, 0f);
                return true;
            };

            redirectors["castor.max_hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Max_HP, 0f);
                return true;
            };

            redirectors["castor.mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Curr_MP, 0f);
                return true;
            };

            redirectors["castor.max_mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Max_MP, 0f);
                return true;
            };

            redirectors["castor.atk"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.ATK, 0f);
                return true;
            };

            redirectors["castor.def"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.DEF, 0f);
                return true;
            };

            redirectors["castor.spd"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.SPD, 0f);
                return true;
            };

            redirectors["castor.mvt"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.MVT, 0f);
                return true;
            };

            redirectors["castor.str"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.STR, 0f);
                return true;
            };

            redirectors["castor.agi"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.AGI, 0f);
                return true;
            };

            redirectors["castor.spw"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Castor == null)
                    return false;

                var attr = resolveCtx.Request.Castor.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.SPW, 0f);
                return true;
            };

            // ---- target 属性 ----
            redirectors["target.hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Curr_HP, 0f);
                return true;
            };

            redirectors["target.max_hp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Max_HP, 0f);
                return true;
            };

            redirectors["target.mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Curr_MP, 0f);
                return true;
            };

            redirectors["target.max_mp"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.Max_MP, 0f);
                return true;
            };

            redirectors["target.atk"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.ATK, 0f);
                return true;
            };

            redirectors["target.def"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.DEF, 0f);
                return true;
            };

            redirectors["target.spd"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.SPD, 0f);
                return true;
            };

            redirectors["target.mvt"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.MVT, 0f);
                return true;
            };

            redirectors["target.str"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.STR, 0f);
                return true;
            };

            redirectors["target.agi"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.AGI, 0f);
                return true;
            };

            redirectors["target.spw"] = (object context, out double value) =>
            {
                value = 0d;
                var resolveCtx = context as ResolveContext;
                if (resolveCtx?.Request?.Target == null)
                    return false;

                var attr = resolveCtx.Request.Target.GetAddon<Addon_BaseAttrNumric>();
                if (attr == null)
                    return false;


                value = attr.GetCorrectionValue(actor_attribute.SPW, 0f);
                return true;
            };

            return redirectors;
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

            _identifierRedirectors = new Dictionary<string, FormulaIdentifierRedirector>(StringComparer.OrdinalIgnoreCase);
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
        
        private bool _isInitialized;
        private string _initializeFailureReason;

        private Dictionary<string, FormulaIdentifierRedirector> _identifierRedirectors;
        
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
