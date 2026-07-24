using System;
using System.Collections.Generic;
using Aquila.Extension;
using GameFramework;
using UnityEngine.Playables;

namespace Aquila.Fight
{
    public sealed class AbilityMontage : IReference
    {
        public static AbilityMontage Create(
            IReadOnlyList<MontageEventData> markers,
            int abilityId,
            long activationId,
            int sourceActorId,
            int[] targetActorIds)
        {
            var montage = ReferencePool.Acquire<AbilityMontage>();
            montage.Initialize(markers, abilityId, activationId, sourceActorId, targetActorIds);
            return montage;
        }

        public event Action<MontageGameplayEvent> GameplayEvent;

        public void Play(string assetPath, PlayableDirector director)
        {
            _director = director;
            _playRequest = GameEntry.Timeline.Play(assetPath, director);
            Start();
        }

        public void Start()
        {
            _isPlaying = true;
            DispatchRange(0f, 0f, true);
        }

        public void Advance(float previousTime, float currentTime)
        {
            if (!_isPlaying || currentTime < previousTime)
                return;

            DispatchRange(previousTime, currentTime, false);
        }

        public void Stop()
        {
            if (!_isPlaying)
                return;

            if (_playRequest.IsValid)
                GameEntry.Timeline.Stop(_playRequest, _director);
            _isPlaying = false;
        }

        public void Clear()
        {
            Stop();
            GameplayEvent = null;
            _markers.Clear();
            _targetActorIds = null;
            _nextMarkerIndex = 0;
            _abilityId = 0;
            _activationId = 0;
            _sourceActorId = 0;
            _director = null;
            _playRequest = default;
        }

        private void Initialize(
            IReadOnlyList<MontageEventData> markers,
            int abilityId,
            long activationId,
            int sourceActorId,
            int[] targetActorIds)
        {
            _markers.Clear();
            for (var i = 0; i < markers.Count; i++)
                _markers.Add(markers[i]);

            _markers.Sort(CompareMarker);
            _abilityId = abilityId;
            _activationId = activationId;
            _sourceActorId = sourceActorId;
            _targetActorIds = (int[])targetActorIds.Clone();
            _nextMarkerIndex = 0;
            _isPlaying = false;
        }

        private void DispatchRange(float previousTime, float currentTime, bool includeZeroTime)
        {
            while (_nextMarkerIndex < _markers.Count)
            {
                var marker = _markers[_nextMarkerIndex];
                if (marker.Time > currentTime)
                    break;

                if (!includeZeroTime && marker.Time <= previousTime)
                {
                    _nextMarkerIndex++;
                    continue;
                }

                _nextMarkerIndex++;
                GameplayEvent?.Invoke(new MontageGameplayEvent
                {
                    EventTag = marker.EventTag,
                    MarkerId = marker.MarkerId,
                    EventTime = marker.Time,
                    AbilityId = _abilityId,
                    ActivationId = _activationId,
                    SourceActorId = _sourceActorId,
                    TargetActorIds = _targetActorIds,
                    Sequence = marker.Sequence
                });
            }
        }

        private static int CompareMarker(MontageEventData left, MontageEventData right)
        {
            var timeCompare = left.Time.CompareTo(right.Time);
            return timeCompare != 0 ? timeCompare : left.Sequence.CompareTo(right.Sequence);
        }

        private readonly List<MontageEventData> _markers = new List<MontageEventData>();
        private int[] _targetActorIds;
        private int _nextMarkerIndex;
        private int _abilityId;
        private long _activationId;
        private int _sourceActorId;
        private bool _isPlaying;
        private PlayableDirector _director;
        private TimelinePlayRequestHandle _playRequest;
    }
}
