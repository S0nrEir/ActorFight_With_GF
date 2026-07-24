using System;
using System.Collections.Generic;

namespace Aquila.Fight
{
    public sealed class GameplayCueTagIndex
    {
        public void Build(IReadOnlyList<GameplayCueNotifyBase> notifies)
        {
            _notifyByTag.Clear();
            _parentChainCache.Clear();

            for (var i = 0; i < notifies.Count; i++)
            {
                var notify = notifies[i];
                if (notify == null)
                    throw new ArgumentException($"GameplayCue notify at index {i} is null.", nameof(notifies));

                var cueTag = Validate(notify.CueTag);
                if (!_notifyByTag.TryGetValue(cueTag, out var list))
                {
                    list = new List<GameplayCueNotifyBase>();
                    _notifyByTag.Add(cueTag, list);
                }

                list.Add(notify);
            }
        }

        public void Resolve(string cueTag, List<GameplayCueNotifyBase> output)
        {
            output.Clear();
            _resolvedNotifySet.Clear();

            var normalizedTag = Validate(cueTag);
            var parentChain = GetParentChain(normalizedTag);
            for (var i = 0; i < parentChain.Length; i++)
            {
                if (!_notifyByTag.TryGetValue(parentChain[i], out var notifies))
                    continue;

                for (var notifyIndex = 0; notifyIndex < notifies.Count; notifyIndex++)
                {
                    var notify = notifies[notifyIndex];
                    if (_resolvedNotifySet.Add(notify))
                        output.Add(notify);
                }
            }
        }

        public static string Validate(string cueTag)
        {
            if (!IsValid(cueTag))
                throw new ArgumentException($"Invalid GameplayCue tag: '{cueTag}'", nameof(cueTag));

            return cueTag;
        }

        public static bool IsValid(string cueTag)
        {
            if (string.IsNullOrWhiteSpace(cueTag) ||
                cueTag[0] == '.' ||
                cueTag[cueTag.Length - 1] == '.' ||
                cueTag.Contains(".."))
                return false;

            var segments = cueTag.Split('.');
            for (var i = 0; i < segments.Length; i++)
            {
                if (segments[i].Length == 0 || segments[i] != segments[i].Trim())
                    return false;
            }

            return true;
        }

        private string[] GetParentChain(string cueTag)
        {
            if (_parentChainCache.TryGetValue(cueTag, out var cached))
                return cached;

            var chain = new List<string>();
            var current = cueTag;
            while (true)
            {
                chain.Add(current);
                var separator = current.LastIndexOf('.');
                if (separator < 0)
                    break;

                current = current.Substring(0, separator);
            }

            cached = chain.ToArray();
            _parentChainCache.Add(cueTag, cached);
            return cached;
        }

        private readonly Dictionary<string, List<GameplayCueNotifyBase>> _notifyByTag =
            new Dictionary<string, List<GameplayCueNotifyBase>>(StringComparer.Ordinal);

        private readonly Dictionary<string, string[]> _parentChainCache =
            new Dictionary<string, string[]>(StringComparer.Ordinal);

        private readonly HashSet<GameplayCueNotifyBase> _resolvedNotifySet =
            new HashSet<GameplayCueNotifyBase>();
    }
}
