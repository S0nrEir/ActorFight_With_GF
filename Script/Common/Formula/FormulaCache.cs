using System;
using System.Collections.Generic;

namespace Aquila.Formula
{
    /// <summary>
    /// 公式缓存键（formulaId + sourceHash + version） / Formula cache key (formulaId + sourceHash + version).
    /// </summary>
    internal readonly struct FormulaCacheKey : IEquatable<FormulaCacheKey>
    {
        public FormulaCacheKey(int formulaId, string sourceHash, int version)
        {
            FormulaId = formulaId;
            SourceHash = sourceHash ?? string.Empty;
            Version = version;
        }

        public int FormulaId { get; }

        public string SourceHash { get; }

        public int Version { get; }

        public bool Equals(FormulaCacheKey other)
        {
            return FormulaId == other.FormulaId
                   && Version == other.Version
                   && string.Equals(SourceHash, other.SourceHash, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is FormulaCacheKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = FormulaId;
                hash = (hash * 397) ^ Version;
                hash = (hash * 397) ^ SourceHash.GetHashCode();
                return hash;
            }
        }
    }

    /// <summary>
    /// 编译结果缓存 / Compiled formula cache.
    /// </summary>
    public sealed class FormulaCache
    {
        private readonly Dictionary<FormulaCacheKey, CompiledFormula> _cacheByKey = new Dictionary<FormulaCacheKey, CompiledFormula>();
        private readonly Dictionary<int, FormulaCacheKey> _latestKeyById = new Dictionary<int, FormulaCacheKey>();

        /// <summary>
        /// 写入或覆盖某公式的最新编译结果 / Set or replace latest compiled formula by ID.
        /// </summary>
        internal void Set(CompiledFormula compiled)
        {
            if (compiled == null)
            {
                return;
            }

            var newKey = new FormulaCacheKey(compiled.FormulaId, compiled.SourceHash, compiled.Version);
            if (_latestKeyById.TryGetValue(compiled.FormulaId, out var oldKey))
            {
                _cacheByKey.Remove(oldKey);
            }

            _cacheByKey[newKey] = compiled;
            _latestKeyById[compiled.FormulaId] = newKey;
        }

        /// <summary>
        /// 按公式 ID 读取最新编译结果 / Try get latest compiled formula by formula ID.
        /// </summary>
        internal bool TryGetById(int formulaId, out CompiledFormula compiled)
        {
            compiled = null;
            if (!_latestKeyById.TryGetValue(formulaId, out var key))
            {
                return false;
            }

            return _cacheByKey.TryGetValue(key, out compiled);
        }

        /// <summary>
        /// 按公式 ID 删除缓存 / Remove cache by formula ID.
        /// </summary>
        internal void RemoveById(int formulaId)
        {
            if (!_latestKeyById.TryGetValue(formulaId, out var key))
            {
                return;
            }

            _cacheByKey.Remove(key);
            _latestKeyById.Remove(formulaId);
        }
    }
}