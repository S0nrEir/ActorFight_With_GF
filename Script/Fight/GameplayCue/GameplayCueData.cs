using System;
using UnityEngine;

namespace Aquila.Fight
{
    public enum GameplayCueEventType : byte
    {
        Execute = 0,
        Add = 1,
        Remove = 2
    }

    public enum GameplayCueTargetPolicy : byte
    {
        Caster = 0,
        PrimaryTarget = 1,
        EachTarget = 2
    }

    public enum GameplayCueLocationPolicy : byte
    {
        Source = 0,
        Target = 1
    }

    [Serializable]
    public sealed class MontageEventData
    {
        public float Time;
        public int Sequence;
        public string MarkerId;
        public string EventTag;

        public MontageEventData()
        {
        }

        public MontageEventData(float time, int sequence, string markerId, string eventTag)
        {
            Time = time;
            Sequence = sequence;
            MarkerId = markerId;
            EventTag = eventTag;
        }
    }

    [Serializable]
    public sealed class AbilityCueBindingData
    {
        public string EventTag;
        public string CueTag;
        public GameplayCueEventType EventType;
        public GameplayCueTargetPolicy TargetPolicy;
        public GameplayCueLocationPolicy LocationPolicy;
        public float Magnitude = 1f;
        public Vector3 LocationOffset;

        public AbilityCueBindingData()
        {
        }

        public AbilityCueBindingData(
            string eventTag,
            string cueTag,
            GameplayCueTargetPolicy targetPolicy,
            GameplayCueLocationPolicy locationPolicy,
            float magnitude,
            Vector3 locationOffset,
            GameplayCueEventType eventType = GameplayCueEventType.Execute)
        {
            EventTag = eventTag;
            CueTag = cueTag;
            EventType = eventType;
            TargetPolicy = targetPolicy;
            LocationPolicy = locationPolicy;
            Magnitude = magnitude;
            LocationOffset = locationOffset;
        }
    }

    [Serializable]
    public struct MontageGameplayEvent
    {
        public string EventTag;
        public string MarkerId;
        public float EventTime;
        public int AbilityId;
        public long ActivationId;
        public int SourceActorId;
        public int[] TargetActorIds;
        public int Sequence;
    }

    [Serializable]
    public struct GameplayCueParameters
    {
        public int AbilityId;
        public long ActivationId;
        public int SourceActorId;
        public int TargetActorId;
        public Vector3 Location;
        public Vector3 Direction;
        public float Magnitude;
        public float EventTime;
        public int Sequence;
    }

    public readonly struct GameplayCueHandle
    {
        public GameplayCueHandle(long value, long activationId, string cueTag)
        {
            Value = value;
            ActivationId = activationId;
            CueTag = cueTag;
        }

        public long Value { get; }
        public long ActivationId { get; }
        public string CueTag { get; }
        public bool IsValid => Value > 0;
    }

    [Serializable]
    public struct GameplayCueCommand
    {
        public GameplayCueEventType EventType;
        public string CueTag;
        public GameplayCueParameters Parameters;
    }
}
