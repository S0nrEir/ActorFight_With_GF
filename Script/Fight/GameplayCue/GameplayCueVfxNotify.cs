using UnityEngine;

namespace Aquila.Fight
{
    [CreateAssetMenu(fileName = "GameplayCueVfxNotify", menuName = "Aquila/GameplayCue/VFX Notify")]
    public sealed class GameplayCueVfxNotify : GameplayCueNotifyBase
    {
        public override void Execute(in GameplayCueParameters parameters)
        {
            var rotation = Quaternion.Euler(_rotationOffset);
            if (parameters.Direction.sqrMagnitude > 0f)
                rotation = Quaternion.LookRotation(parameters.Direction) * rotation;

            var instance = Instantiate(_prefab, parameters.Location + _positionOffset, rotation);
            instance.transform.localScale = Vector3.Scale(instance.transform.localScale, _scale);
            if (_lifeTime > 0f)
                Destroy(instance, _lifeTime);
        }

        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _lifeTime = 1f;
        [SerializeField] private Vector3 _positionOffset;
        [SerializeField] private Vector3 _rotationOffset;
        [SerializeField] private Vector3 _scale = Vector3.one;
    }
}
