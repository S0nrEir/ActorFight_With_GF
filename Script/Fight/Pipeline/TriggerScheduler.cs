using System;
using System.Collections.Generic;
using Aquila.Fight;
using GameFramework;
using UnityEngine;

namespace Aquila.Combat
{
    public sealed class TriggerScheduler : IReference
    {
        /// <summary>
        /// 从对象池获取触发调度器，并按技能配置重建触发点。
        /// </summary>
        public static TriggerScheduler Create(AbilityData abilityData)
        {
            var scheduler = ReferencePool.Acquire<TriggerScheduler>();
            scheduler.Initialize(abilityData);
            return scheduler;
        }

        /// <summary>
        /// 回收前清空触发点与游标位置。
        /// </summary>
        public void Clear()
        {
            _triggerPoints = Array.Empty<TriggerPoint>();
            _cursor = 0;
        }

        public void CollectReadyIndices(float elapsed, List<int> output)
        {
            while (_cursor < _triggerPoints.Length && _triggerPoints[_cursor].TriggerTime <= elapsed + TimeEpsilon)
            {
                output.Add(_triggerPoints[_cursor].TriggerIndex);
                _cursor++;
            }
        }

        /// <summary>
        /// 根据 Effect 起始时间生成并排序触发点数组。
        /// </summary>
        private void Initialize(AbilityData abilityData)
        {
            var effects = abilityData.GetEffects();
            _triggerPoints = new TriggerPoint[effects.Count];
            var timelineDuration = Mathf.Max(abilityData.GetTimelineDuration(), 0f);

            for (var i = 0; i < effects.Count; i++)
            {
                var triggerTime = Mathf.Clamp(effects[i].GetStartTime(), 0f, timelineDuration);
                _triggerPoints[i] = new TriggerPoint(i, triggerTime);
            }

            Array.Sort(_triggerPoints, SortTriggerPoints);
            _cursor = 0;
        }

        private static int SortTriggerPoints(TriggerPoint a, TriggerPoint b)
        {
            var timeCompare = a.TriggerTime.CompareTo(b.TriggerTime);
            if (timeCompare != 0)
                return timeCompare;

            return a.TriggerIndex.CompareTo(b.TriggerIndex);
        }

        private TriggerPoint[] _triggerPoints = Array.Empty<TriggerPoint>();
        private int _cursor;
        private const float TimeEpsilon = 0.0001f;

        private readonly struct TriggerPoint
        {
            public TriggerPoint(int triggerIndex, float triggerTime)
            {
                TriggerIndex = triggerIndex;
                TriggerTime = triggerTime;
            }

            public int TriggerIndex { get; }
            public float TriggerTime { get; }
        }
    }
}