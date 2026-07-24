using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight
{
    public static class AbilityCueRouter
    {
        public static void Route(
            IReadOnlyList<AbilityCueBindingData> bindings,
            in MontageGameplayEvent gameplayEvent,
            Func<int, Vector3> positionResolver,
            Action<string, GameplayCueParameters> execute)
        {
            for (var i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];
                if (!string.Equals(binding.EventTag, gameplayEvent.EventTag, StringComparison.Ordinal))
                    continue;

                if (binding.EventType != GameplayCueEventType.Execute)
                    continue;

                switch (binding.TargetPolicy)
                {
                    case GameplayCueTargetPolicy.Caster:
                        ExecuteForTarget(binding, gameplayEvent, gameplayEvent.SourceActorId, positionResolver, execute);
                        break;

                    case GameplayCueTargetPolicy.PrimaryTarget:
                        if (gameplayEvent.TargetActorIds.Length > 0)
                            ExecuteForTarget(binding, gameplayEvent, gameplayEvent.TargetActorIds[0], positionResolver, execute);
                        break;

                    case GameplayCueTargetPolicy.EachTarget:
                        for (var targetIndex = 0; targetIndex < gameplayEvent.TargetActorIds.Length; targetIndex++)
                        {
                            ExecuteForTarget(
                                binding,
                                gameplayEvent,
                                gameplayEvent.TargetActorIds[targetIndex],
                                positionResolver,
                                execute);
                        }
                        break;
                }
            }
        }

        private static void ExecuteForTarget(
            AbilityCueBindingData binding,
            in MontageGameplayEvent gameplayEvent,
            int targetActorId,
            Func<int, Vector3> positionResolver,
            Action<string, GameplayCueParameters> execute)
        {
            var sourcePosition = positionResolver(gameplayEvent.SourceActorId);
            var targetPosition = positionResolver(targetActorId);
            var direction = targetPosition - sourcePosition;
            if (direction.sqrMagnitude > 0f)
                direction.Normalize();

            var location = binding.LocationPolicy == GameplayCueLocationPolicy.Source
                ? sourcePosition
                : targetPosition;

            execute(binding.CueTag, new GameplayCueParameters
            {
                AbilityId = gameplayEvent.AbilityId,
                ActivationId = gameplayEvent.ActivationId,
                SourceActorId = gameplayEvent.SourceActorId,
                TargetActorId = targetActorId,
                Location = location + binding.LocationOffset,
                Direction = direction,
                Magnitude = binding.Magnitude,
                EventTime = gameplayEvent.EventTime,
                Sequence = gameplayEvent.Sequence
            });
        }
    }
}
