using System;
using System.Collections.Generic;
using Aquila.Fight;
using Aquila.Toolkit;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    public sealed class Component_GameplayCue : GameFrameworkComponent
    {
        public void ExecuteGameplayCue(string cueTag, in GameplayCueParameters parameters)
        {
            if (!GameplayCueTagIndex.IsValid(cueTag))
            {
                Tools.Logger.Error(
                    $"[GameplayCue] Invalid cue tag, cueTag={cueTag}, abilityId={parameters.AbilityId}, activationId={parameters.ActivationId}");
                return;
            }

            _tagIndex.Resolve(cueTag, _resolvedNotifies);
            for (var i = 0; i < _resolvedNotifies.Count; i++)
            {
                try
                {
                    _resolvedNotifies[i].Execute(parameters);
                }
                catch (Exception exception)
                {
                    Tools.Logger.Error(
                        $"[GameplayCue] Execute failed, cueTag={cueTag}, abilityId={parameters.AbilityId}, activationId={parameters.ActivationId}, notify={_resolvedNotifies[i].name}, error={exception.Message}");
                }
            }
        }

        public void RebuildIndex()
        {
            _tagIndex.Build(_notifies);
        }

        protected override void Awake()
        {
            base.Awake();
            RebuildIndex();
        }

        [SerializeField]
        private List<GameplayCueNotifyBase> _notifies = new List<GameplayCueNotifyBase>();

        private readonly GameplayCueTagIndex _tagIndex = new GameplayCueTagIndex();
        private readonly List<GameplayCueNotifyBase> _resolvedNotifies = new List<GameplayCueNotifyBase>();
    }
}
