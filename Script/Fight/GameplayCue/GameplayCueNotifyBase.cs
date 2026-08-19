using UnityEngine;

namespace Aquila.Fight
{
    public abstract class GameplayCueNotifyBase : ScriptableObject
    {
        public string CueTag => _cueTag;

        public abstract void Execute(in GameplayCueParameters parameters);

        [SerializeField]
        private string _cueTag;
    }
}
