using System.Collections.Generic;
using Cfg.Enum;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// 全局 Actor Tag 状态组件。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TagComponent : GameFrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();
            _actorStates = new Dictionary<int, ActorTagState>();
        }

        public bool RegisterActor(int actorId)
        {
            if (actorId <= 0 || _actorStates == null || _actorStates.ContainsKey(actorId))
                return false;

            _actorStates.Add(actorId, new ActorTagState());
            return true;
        }

        public bool UnregisterActor(int actorId)
        {
            if (actorId <= 0 || _actorStates == null)
                return false;

            return _actorStates.Remove(actorId);
        }

        public bool AcquireTag(int actorId, ActorTagType mainType, ushort subType, int sourceHashCode)
        {
            if (!IsValidTag(actorId, mainType, subType, sourceHashCode) || !_actorStates.TryGetValue(actorId, out var actorState))
                return false;

            var tagKey = CreateTagKey(mainType, subType);
            if (!actorState.Sources.TryGetValue(tagKey, out var sources))
            {
                sources = new HashSet<int>();
                actorState.Sources.Add(tagKey, sources);
            }

            if (!sources.Add(sourceHashCode))
                return false;

            actorState.TagBits[(int)mainType] |= 1u << subType;
            return true;
        }

        public bool ReleaseTag(int actorId, ActorTagType mainType, ushort subType, int sourceHashCode)
        {
            if (!IsValidTag(actorId, mainType, subType, sourceHashCode) || !_actorStates.TryGetValue(actorId, out var actorState))
                return false;

            var tagKey = CreateTagKey(mainType, subType);
            if (!actorState.Sources.TryGetValue(tagKey, out var sources) || !sources.Remove(sourceHashCode))
                return false;

            if (sources.Count == 0)
            {
                actorState.Sources.Remove(tagKey);
                actorState.TagBits[(int)mainType] &= ~(1u << subType);
            }

            return true;
        }

        public bool HasTag(int actorId, ActorTagType mainType, ushort subType)
        {
            if (!IsValidTag(actorId, mainType, subType, 1) || !_actorStates.TryGetValue(actorId, out var actorState))
                return false;

            return (actorState.TagBits[(int)mainType] & (1u << subType)) != 0;
        }

        private static bool IsValidTag(int actorId, ActorTagType mainType, ushort subType, int sourceHashCode)
        {
            return actorId > 0 &&
                   (int)mainType >= 0 &&
                   (int)mainType < (int)ActorTagType.Max &&
                   subType < 32 &&
                   sourceHashCode != 0;
        }

        private static int CreateTagKey(ActorTagType mainType, ushort subType)
        {
            return ((int)mainType << 5) | subType;
        }

        private sealed class ActorTagState
        {
            public readonly uint[] TagBits = new uint[(int)ActorTagType.Max];
            public readonly Dictionary<int, HashSet<int>> Sources = new Dictionary<int, HashSet<int>>();
        }

        private Dictionary<int, ActorTagState> _actorStates;
    }
}
