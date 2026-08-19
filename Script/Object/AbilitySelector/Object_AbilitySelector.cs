using System;
using System.Collections.Generic;
using Aquila.Combat;
using Aquila.Fight;
using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using UnityEngine;

namespace Aquila.ObjectPool
{
    public abstract class Object_AbilitySelectorBase : Object_Base
    {
        public static void Preload(Action<bool> onComplete)
        {
            if (onComplete != null)
                _preloadCallbacks.Add(onComplete);

            if (_isPreloading)
                return;

            var pool = GetSelectorPool();
            _pendingSelectorTypes.Clear();
            _hasPreloadFailed = false;

            foreach (AbilitySelectType selectType in Enum.GetValues(typeof(AbilitySelectType)))
            {
                var assetPath = GetSelectorAssetPath(selectType);
                if (!IsSupportedSelectorType(selectType))
                {
                    Tools.Logger.Error(
                        $"[AbilitySelector] Selector type mapping is missing, type: {selectType}, asset: {assetPath}.");
                    CompletePreload(false);
                    return;
                }

                var selectorName = selectType.ToString();
                if (_registeredSelectorNames.Contains(selectorName) || pool.CanSpawn(selectorName))
                {
                    _registeredSelectorNames.Add(selectorName);
                    continue;
                }

                _pendingSelectorTypes.Add(selectType);
            }

            if (_pendingSelectorTypes.Count == 0)
            {
                CompletePreload(true);
                return;
            }

            _isPreloading = true;
            var preloadGeneration = ++_preloadGeneration;
            if (_loadAssetCallbacks == null)
                _loadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailed);

            var selectorTypesToLoad = new List<AbilitySelectType>(_pendingSelectorTypes);
            foreach (var selectType in selectorTypesToLoad)
            {
                var assetPath = GetSelectorAssetPath(selectType);
                var request = new SelectorLoadRequest(selectType, preloadGeneration);
                GameEntry.Resource.LoadAsset(assetPath, _loadAssetCallbacks, request);
            }
        }

        public static bool StartSelection(int castorId, int abilityId)
        {
            if (!GameEntry.AbilityPool.TryGetAbility(abilityId, out var abilityData))
            {
                Tools.Logger.Warning($"<color=yellow>Ability selector start failed, ability not found:{abilityId}</color>");
                return false;
            }

            if (abilityData.GetTargetType() == AbilityTargetType.Self)
            {
                SubmitCast(CastCmd.CreateWithSingleTarget(castorId, castorId, abilityId));
                return true;
            }

            var selector = Spawn(abilityData.GetSelectType());
            if (selector == null)
                return false;

            selector.Begin(castorId, abilityId, abilityData);
            return true;
        }

        public void Begin(int castorId, int abilityId, AbilityData abilityData)
        {
            _castorId = castorId;
            _abilityId = abilityId;
            _abilityData = abilityData;
            OnBegin();
        }

        public void ConfirmSelection()
        {
            if (_isReleased)
                return;

            OnConfirm();
        }

        public void CancelSelection()
        {
            Cancel();
        }

        protected virtual void OnBegin()
        {
        }

        public override void Setup(GameObject go)
        {
            _driver = Tools.GetComponent<AbilitySelectorDriver>(go);
            if (_driver == null)
            {
                Tools.Logger.Warning($"<color=yellow>Ability selector setup failed, AbilitySelectorDriver component not found in {go.name}.</color>");
                return;
            }

            _driver.Setup(this);
        }
        
        protected abstract void OnConfirm();

        protected bool IsLegalTarget(Module_ProxyActor.ActorInstance target)
        {
            if (target?.Actor == null)
                return false;

            var actorId = target.Actor.ActorID;
            var targetType = _abilityData.GetTargetType();
            if ((targetType & AbilityTargetType.Self) != 0 && actorId == _castorId)
                return true;

            if ((targetType & AbilityTargetType.Enemy) != 0 && actorId != _castorId)
                return true;

            if ((targetType & AbilityTargetType.Ally) != 0 && actorId == _castorId)
                return true;

            return false;
        }

        protected bool TryPickActor(out Module_ProxyActor.ActorInstance actor)
        {
            actor = null;
            var camera = ResolveWorldCamera();
            if (camera == null)
                return false;

            var ray = camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 500f))
                return false;

            var actorBase = hit.collider.GetComponentInParent<Actor_Base>();
            actor = GameEntry.Module.GetModule<Module_ActorMgr>().Get(actorBase.ActorID);
            return actor != null;
        }

        protected bool TryGetMouseGroundPoint(out Vector3 point)
        {
            point = Vector3.zero;
            var camera = ResolveWorldCamera();
            if (camera == null)
                return false;

            var ray = camera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 500f))
            {
                point = hit.point;
                return true;
            }

            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (!groundPlane.Raycast(ray, out var enter))
                return false;

            point = ray.GetPoint(enter);
            return true;
        }

        protected void CollectLegalTargetsInCircle(Vector3 center, float radius, List<int> results)
        {
            results.Clear();

            var radiusSqr = radius * radius;
            foreach (var actor in GameEntry.Module.GetModule<Module_ActorMgr>().AllActorInstances())
            {
                if (!IsLegalTarget(actor))
                    continue;

                var delta = actor.Actor.CachedTransform.position - center;
                delta.y = 0f;
                if (delta.sqrMagnitude <= radiusSqr)
                    results.Add(actor.Actor.ActorID);
            }
        }

        protected void SubmitAndRelease(CastCmd cmd)
        {
            SubmitCast(cmd);
            ReleaseSelf();
        }

        protected void Cancel()
        {
            ReleaseSelf();
        }

        protected void ReleaseSelf()
        {
            if (_isReleased)
                return;

            _isReleased = true;
            var pool = GameEntry.ObjectPool.GetObjectPool<Object_AbilitySelectorBase>(nameof(Object_AbilitySelectorBase));
            pool?.Unspawn(Target);
        }

        protected static void SubmitCast(CastCmd cmd)
        {
            var requestResult = GameEntry.Module.GetModule<Module_Combat>().RequestCast(cmd);
            if (!requestResult.Accepted)
            {
                var errorMsg = Tools.Fight.UsingAbilityFaildDescription_l10n((int)requestResult.ReasonFlags);
                Tools.Logger.Info(errorMsg);
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            _isReleased = false;
        }

        protected override void OnUnspawn()
        {
            if (_driver != null)
                _driver.Setup(null);

            _castorId = -1;
            _abilityId = -1;
            _abilityData = default;
            _isReleased = true;
            base.OnUnspawn();
        }

        protected override void Release(bool isShutdown)
        {
            _driver = null;
            base.Release(isShutdown);
        }

        private static Object_AbilitySelectorBase Spawn(AbilitySelectType selectType)
        {
            var selectorName = selectType.ToString();
            var pool = GetSelectorPool();
            if (!pool.CanSpawn(selectorName))
            {
                Tools.Logger.Error(
                    $"[AbilitySelector] Selector is not preloaded or is already in use, type: {selectType}, name: {selectorName}.");
                return null;
            }

            var selector = pool.Spawn(selectorName);
            if (selector == null)
            {
                Tools.Logger.Error(
                    $"[AbilitySelector] Failed to spawn preloaded selector, type: {selectType}, name: {selectorName}.");
                return null;
            }

            selector.Setup(selector.Target as GameObject);
            return selector;
        }

        private static IObjectPool<Object_AbilitySelectorBase> GetSelectorPool()
        {
            var pool = GameEntry.ObjectPool.GetObjectPool<Object_AbilitySelectorBase>(nameof(Object_AbilitySelectorBase));
            if (pool == null)
                pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_AbilitySelectorBase>(nameof(Object_AbilitySelectorBase));

            pool.ExpireTime = float.MaxValue;
            if (!ReferenceEquals(_selectorPool, pool))
            {
                _selectorPool = pool;
                _registeredSelectorNames.Clear();
            }

            return pool;
        }

        private static void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            if (!(userData is SelectorLoadRequest request))
            {
                if (asset != null)
                    GameEntry.Resource.UnloadAsset(asset);

                Tools.Logger.Error(
                    $"[AbilitySelector] Load callback context is invalid, asset: {assetName}.");
                if (_isPreloading)
                    CompletePreload(false);

                return;
            }

            if (!_isPreloading || request.PreloadGeneration != _preloadGeneration)
            {
                if (asset != null)
                    GameEntry.Resource.UnloadAsset(asset);

                return;
            }

            var selectType = request.SelectType;
            if (!_pendingSelectorTypes.Contains(selectType))
            {
                if (asset != null)
                    GameEntry.Resource.UnloadAsset(asset);

                Tools.Logger.Warning(
                    $"[AbilitySelector] Duplicate load callback ignored, type: {selectType}, asset: {assetName}.");
                return;
            }

            if (!(asset is GameObject prefab))
            {
                if (asset != null)
                    GameEntry.Resource.UnloadAsset(asset);

                Tools.Logger.Error(
                    $"[AbilitySelector] Loaded asset is not a GameObject, type: {selectType}, asset: {assetName}.");
                CompleteSelectorLoad(selectType, false);
                return;
            }

            var selectorName = selectType.ToString();
            var pool = GetSelectorPool();
            if (_registeredSelectorNames.Contains(selectorName) || pool.CanSpawn(selectorName))
            {
                GameEntry.Resource.UnloadAsset(asset);
                _registeredSelectorNames.Add(selectorName);
                CompleteSelectorLoad(selectType, true);
                return;
            }

            var instance = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            instance.SetActive(false);
            GameEntry.Resource.UnloadAsset(asset);
            var selector = CreateSelector(selectType, selectorName, instance);
            if (selector == null)
            {
                UnityEngine.Object.Destroy(instance);
                Tools.Logger.Error(
                    $"[AbilitySelector] Selector type mapping is missing, type: {selectType}, asset: {assetName}.");
                CompleteSelectorLoad(selectType, false);
                return;
            }

            pool.Register(selector, false);
            pool.SetLocked(selector, true);
            _registeredSelectorNames.Add(selectorName);
            CompleteSelectorLoad(selectType, true);
        }

        private static void OnLoadAssetFailed(
            string assetName,
            LoadResourceStatus status,
            string errorMessage,
            object userData)
        {
            if (!_isPreloading)
                return;

            if (!(userData is SelectorLoadRequest request))
            {
                Tools.Logger.Error(
                    $"[AbilitySelector] Load failure callback context is invalid, asset: {assetName}, status: {status}, error: {errorMessage}.");
                if (_isPreloading)
                    CompletePreload(false);

                return;
            }

            if (!_isPreloading || request.PreloadGeneration != _preloadGeneration)
                return;

            var selectType = request.SelectType;
            Tools.Logger.Error(
                $"[AbilitySelector] Failed to load selector prefab, type: {selectType}, asset: {assetName}, status: {status}, error: {errorMessage}.");
            CompleteSelectorLoad(selectType, false);
        }

        private static Object_AbilitySelectorBase CreateSelector(AbilitySelectType selectType, string selectorName, GameObject go)
        {
            if (selectType == AbilitySelectType.Single)
                return Object_AbilitySelectorSingle.Gen(selectorName, go);

            if (selectType == AbilitySelectType.Circle)
                return Object_AbilitySelectorCircle.Gen(selectorName, go);

            return null;
        }

        private static bool IsSupportedSelectorType(AbilitySelectType selectType)
        {
            return selectType == AbilitySelectType.Single || selectType == AbilitySelectType.Circle;
        }

        private static string GetSelectorAssetPath(AbilitySelectType selectType)
        {
            return $"{SelectorPrefabDirectory}/{SelectorPrefabPrefix}{selectType}.prefab";
        }

        private static void CompleteSelectorLoad(AbilitySelectType selectType, bool succeeded)
        {
            if (!_pendingSelectorTypes.Remove(selectType))
                return;

            if (!succeeded)
                _hasPreloadFailed = true;

            if (_pendingSelectorTypes.Count == 0)
                CompletePreload(!_hasPreloadFailed);
        }

        private static void CompletePreload(bool succeeded)
        {
            _isPreloading = false;
            _hasPreloadFailed = false;
            _pendingSelectorTypes.Clear();

            var callbacks = _preloadCallbacks.ToArray();
            _preloadCallbacks.Clear();
            foreach (var callback in callbacks)
                callback(succeeded);
        }

        private sealed class SelectorLoadRequest
        {
            public SelectorLoadRequest(AbilitySelectType selectType, int preloadGeneration)
            {
                SelectType = selectType;
                PreloadGeneration = preloadGeneration;
            }

            public AbilitySelectType SelectType { get; }

            public int PreloadGeneration { get; }
        }

        private static Camera ResolveWorldCamera()
        {
            var camera = GameEntry.CameraHub != null ? GameEntry.CameraHub.GetWorldCamera() : null;
            if (camera != null)
                return camera;

            return Camera.main;
        }

        protected int _castorId = -1;
        protected int _abilityId = -1;
        protected AbilityData _abilityData;

        private const string SelectorPrefabDirectory = "Assets/Res/Prefab/AbilitySelector";
        private const string SelectorPrefabPrefix = "Object_AbilitySelector";

        private static readonly HashSet<string> _registeredSelectorNames = new HashSet<string>();
        private static readonly HashSet<AbilitySelectType> _pendingSelectorTypes = new HashSet<AbilitySelectType>();
        private static readonly List<Action<bool>> _preloadCallbacks = new List<Action<bool>>();
        private static IObjectPool<Object_AbilitySelectorBase> _selectorPool;
        private static LoadAssetCallbacks _loadAssetCallbacks;
        private static int _preloadGeneration;
        private static bool _isPreloading;
        private static bool _hasPreloadFailed;

        private AbilitySelectorDriver _driver;
        private bool _isReleased;
    }
}
