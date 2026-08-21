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
            return StartSelection(castorId, abilityId, null);
        }

        public static bool StartSelection(
            int castorId,
            int abilityId,
            IEnumerable<int> secondaryCandidateActorIds)
        {
            if (!GameEntry.AbilityPool.TryGetAbility(abilityId, out var abilityData))
            {
                Tools.Logger.Warning($"<color=yellow>Ability selector start failed, ability not found:{abilityId}</color>");
                return false;
            }

            if (abilityData.GetTargetType() == AbilityTargetType.Self)
            {
                var castor = GameEntry.Module.GetModule<Module_ActorMgr>()?.Get(castorId);
                var castOrigin = castor?.Actor?.CachedTransform != null
                    ? castor.Actor.CachedTransform.position
                    : Vector3.zero;
                SubmitCast(CastCmd.CreateWithSingleTarget(castorId, castorId, abilityId, castOrigin, castOrigin));
                return true;
            }

            HashSet<int> secondaryCandidates = null;
            if (abilityData.GetSelectType() == AbilitySelectType.SecondaryActor)
            {
                secondaryCandidates = secondaryCandidateActorIds == null
                    ? null
                    : new HashSet<int>(secondaryCandidateActorIds);
                if (secondaryCandidates == null || secondaryCandidates.Count == 0)
                {
                    Tools.Logger.Error(
                        "[AbilitySelector] SecondaryActor selection requires an explicit non-empty candidate actor ID set.");
                    return false;
                }
            }

            var selector = Spawn(abilityData.GetSelectType());
            if (selector == null)
                return false;

            selector.Begin(castorId, abilityId, abilityData, secondaryCandidates);
            return true;
        }

        public void Begin(int castorId, int abilityId, AbilityData abilityData)
        {
            Begin(castorId, abilityId, abilityData, null);
        }

        public void Begin(
            int castorId,
            int abilityId,
            AbilityData abilityData,
            IEnumerable<int> secondaryCandidateActorIds)
        {
            _castorId = castorId;
            _abilityId = abilityId;
            _abilityData = abilityData;
            _secondaryCandidateActorIds.Clear();
            if (secondaryCandidateActorIds != null)
            {
                foreach (var actorId in secondaryCandidateActorIds)
                    _secondaryCandidateActorIds.Add(actorId);
            }
            OnBegin();
        }

        public void UpdateSelection()
        {
            if (!_isReleased)
                OnUpdateSelection();
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

        protected virtual void OnUpdateSelection()
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
            if (actorBase == null)
                return false;

            actor = GameEntry.Module.GetModule<Module_ActorMgr>().Get(actorBase.ActorID);
            return actor != null;
        }

        protected bool TryGetCastOrigin(out Vector3 castOrigin)
        {
            castOrigin = Vector3.zero;
            var castor = GameEntry.Module.GetModule<Module_ActorMgr>()?.Get(_castorId);
            if (castor?.Actor?.CachedTransform == null)
                return false;

            castOrigin = castor.Actor.CachedTransform.position;
            return true;
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
            CombatTargetQueryService.QueryCircle(
                GameEntry.Module.GetModule<Module_ActorMgr>(),
                center,
                radius,
                IsLegalTarget,
                results);
        }

        protected void CollectLegalTargetsInLine(
            Vector3 castOrigin,
            Vector3 targetPoint,
            float halfWidth,
            List<int> results)
        {
            CombatTargetQueryService.QueryLine(
                GameEntry.Module.GetModule<Module_ActorMgr>(),
                castOrigin,
                targetPoint,
                halfWidth,
                IsLegalTarget,
                results);
        }

        protected void CollectLegalTargetsGlobal(List<int> results)
        {
            CombatTargetQueryService.QueryGlobal(
                GameEntry.Module.GetModule<Module_ActorMgr>(),
                IsLegalTarget,
                results);
        }

        protected bool IsSecondaryCandidate(int actorId)
        {
            return _secondaryCandidateActorIds.Contains(actorId);
        }

        protected static LineRenderer SetupLineRenderer(GameObject go, bool loop)
        {
            var lineRenderer = go.GetComponent<LineRenderer>();
            if (lineRenderer == null)
                lineRenderer = go.AddComponent<LineRenderer>();

            lineRenderer.enabled = true;
            lineRenderer.useWorldSpace = false;
            lineRenderer.loop = loop;
            lineRenderer.startWidth = 0.04f;
            lineRenderer.endWidth = 0.04f;
            if (lineRenderer.sharedMaterial == null)
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = new Color(0.2f, 0.75f, 1f, 0.9f);
            lineRenderer.endColor = lineRenderer.startColor;
            return lineRenderer;
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
            _secondaryCandidateActorIds.Clear();
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

            if (selectType == AbilitySelectType.Point)
                return Object_AbilitySelectorPoint.Gen(selectorName, go);

            if (selectType == AbilitySelectType.Direction)
                return Object_AbilitySelectorDirection.Gen(selectorName, go);

            if (selectType == AbilitySelectType.Line)
                return Object_AbilitySelectorLine.Gen(selectorName, go);

            if (selectType == AbilitySelectType.GlobalActor)
                return Object_AbilitySelectorGlobalActor.Gen(selectorName, go);

            if (selectType == AbilitySelectType.SecondaryActor)
                return Object_AbilitySelectorSecondaryActor.Gen(selectorName, go);

            return null;
        }

        private static bool IsSupportedSelectorType(AbilitySelectType selectType)
        {
            return selectType == AbilitySelectType.Single ||
                   selectType == AbilitySelectType.Circle ||
                   selectType == AbilitySelectType.Point ||
                   selectType == AbilitySelectType.Direction ||
                   selectType == AbilitySelectType.Line ||
                   selectType == AbilitySelectType.GlobalActor ||
                   selectType == AbilitySelectType.SecondaryActor;
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
        private readonly HashSet<int> _secondaryCandidateActorIds = new HashSet<int>();

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

    public sealed class Object_AbilitySelectorPoint : Object_AbilitySelectorBase
    {
        public override void Setup(GameObject go)
        {
            base.Setup(go);
            _lineRenderer = SetupLineRenderer(go, true);
            _lineRenderer.positionCount = MarkerSegmentCount;
            for (var i = 0; i < MarkerSegmentCount; i++)
            {
                var angle = i / (float)MarkerSegmentCount * Mathf.PI * 2f;
                _lineRenderer.SetPosition(
                    i,
                    new Vector3(Mathf.Cos(angle) * MarkerRadius, 0.03f, Mathf.Sin(angle) * MarkerRadius));
            }
        }

        protected override void OnUpdateSelection()
        {
            if (TryGetMouseGroundPoint(out var targetPoint))
                _targetGameObject.transform.position = targetPoint;
        }

        protected override void OnConfirm()
        {
            if (!TryGetCastOrigin(out var castOrigin) || !TryGetMouseGroundPoint(out var targetPoint))
            {
                RejectSelection();
                return;
            }

            SubmitAndRelease(CastCmd.CreateWithPointTarget(_castorId, _abilityId, castOrigin, targetPoint));
        }

        public static Object_AbilitySelectorPoint Gen(string name, GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_AbilitySelectorPoint>();
            obj.Initialize(name, go);
            return obj;
        }

        protected override void Release(bool isShutdown)
        {
            _lineRenderer = null;
            base.Release(isShutdown);
        }

        private void RejectSelection()
        {
            Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
            ReleaseSelf();
        }

        private const int MarkerSegmentCount = 32;
        private const float MarkerRadius = 0.15f;
        private LineRenderer _lineRenderer;
    }

    public sealed class Object_AbilitySelectorDirection : Object_AbilitySelectorBase
    {
        public override void Setup(GameObject go)
        {
            base.Setup(go);
            _lineRenderer = SetupLineRenderer(go, false);
            _lineRenderer.positionCount = 2;
        }

        protected override void OnUpdateSelection()
        {
            if (!TryGetCastOrigin(out var castOrigin) || !TryGetMouseGroundPoint(out var targetPoint))
                return;

            DrawDirection(castOrigin, targetPoint);
        }

        protected override void OnConfirm()
        {
            if (!TryGetCastOrigin(out var castOrigin) || !TryGetMouseGroundPoint(out var targetPoint))
            {
                RejectSelection();
                return;
            }

            var direction = targetPoint - castOrigin;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0f)
            {
                RejectSelection();
                return;
            }

            SubmitAndRelease(CastCmd.CreateWithDirectionTarget(_castorId, _abilityId, castOrigin, targetPoint));
        }

        protected override void Release(bool isShutdown)
        {
            _lineRenderer = null;
            base.Release(isShutdown);
        }

        private void DrawDirection(Vector3 castOrigin, Vector3 targetPoint)
        {
            _targetGameObject.transform.position = castOrigin;
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, targetPoint - castOrigin);
        }

        private void RejectSelection()
        {
            Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
            ReleaseSelf();
        }

        public static Object_AbilitySelectorDirection Gen(string name, GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_AbilitySelectorDirection>();
            obj.Initialize(name, go);
            return obj;
        }

        private LineRenderer _lineRenderer;
    }

    public sealed class Object_AbilitySelectorLine : Object_AbilitySelectorBase
    {
        public override void Setup(GameObject go)
        {
            base.Setup(go);
            _lineRenderer = SetupLineRenderer(go, true);
            _lineRenderer.positionCount = 4;
        }

        protected override void OnUpdateSelection()
        {
            if (!TryGetCastOrigin(out var castOrigin) || !TryGetMouseGroundPoint(out var targetPoint))
                return;

            DrawLine(castOrigin, targetPoint, _abilityData.GetSelectRadius());
        }

        protected override void OnConfirm()
        {
            if (!TryGetCastOrigin(out var castOrigin) || !TryGetMouseGroundPoint(out var targetPoint))
            {
                RejectSelection();
                return;
            }

            var direction = targetPoint - castOrigin;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0f)
            {
                RejectSelection();
                return;
            }

            direction.Normalize();
            CollectLegalTargetsInLine(
                castOrigin,
                targetPoint,
                _abilityData.GetSelectRadius(),
                _targetIds);
            SubmitAndRelease(CastCmd.CreateWithMultiTarget(
                _castorId,
                _targetIds.ToArray(),
                _abilityId,
                castOrigin,
                targetPoint,
                direction));
        }

        protected override void OnUnspawn()
        {
            _targetIds.Clear();
            base.OnUnspawn();
        }

        protected override void Release(bool isShutdown)
        {
            _lineRenderer = null;
            _targetIds.Clear();
            base.Release(isShutdown);
        }

        private void DrawLine(Vector3 castOrigin, Vector3 targetPoint, float halfWidth)
        {
            var offset = targetPoint - castOrigin;
            offset.y = 0f;
            if (offset.sqrMagnitude <= 0f)
                return;

            var right = Vector3.Cross(Vector3.up, offset.normalized) * Mathf.Max(0f, halfWidth);
            _targetGameObject.transform.position = castOrigin;
            _lineRenderer.SetPosition(0, right);
            _lineRenderer.SetPosition(1, offset + right);
            _lineRenderer.SetPosition(2, offset - right);
            _lineRenderer.SetPosition(3, -right);
        }

        private void RejectSelection()
        {
            Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
            ReleaseSelf();
        }

        public static Object_AbilitySelectorLine Gen(string name, GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_AbilitySelectorLine>();
            obj.Initialize(name, go);
            return obj;
        }

        private readonly List<int> _targetIds = new List<int>(16);
        private LineRenderer _lineRenderer;
    }

    public sealed class Object_AbilitySelectorGlobalActor : Object_AbilitySelectorBase
    {
        protected override void OnConfirm()
        {
            if (!TryGetCastOrigin(out var castOrigin))
            {
                RejectSelection();
                return;
            }

            CollectLegalTargetsGlobal(_targetIds);
            if (_targetIds.Count == 0)
            {
                RejectSelection();
                return;
            }

            SubmitAndRelease(CastCmd.CreateWithMultiTarget(
                _castorId,
                _targetIds.ToArray(),
                _abilityId,
                castOrigin));
        }

        protected override void OnUnspawn()
        {
            _targetIds.Clear();
            base.OnUnspawn();
        }

        protected override void Release(bool isShutdown)
        {
            _targetIds.Clear();
            base.Release(isShutdown);
        }

        private void RejectSelection()
        {
            Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
            ReleaseSelf();
        }

        public static Object_AbilitySelectorGlobalActor Gen(string name, GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_AbilitySelectorGlobalActor>();
            obj.Initialize(name, go);
            return obj;
        }

        private readonly List<int> _targetIds = new List<int>(32);
    }

    public sealed class Object_AbilitySelectorSecondaryActor : Object_AbilitySelectorBase
    {
        protected override void OnConfirm()
        {
            if (!TryPickActor(out var actor) ||
                !IsSecondaryCandidate(actor.Actor.ActorID) ||
                !IsLegalTarget(actor) ||
                !TryGetCastOrigin(out var castOrigin))
            {
                Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
                ReleaseSelf();
                return;
            }

            var targetPoint = actor.Actor.CachedTransform.position;
            SubmitAndRelease(CastCmd.CreateWithSingleTarget(
                _castorId,
                actor.Actor.ActorID,
                _abilityId,
                castOrigin,
                targetPoint));
        }

        public static Object_AbilitySelectorSecondaryActor Gen(string name, GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_AbilitySelectorSecondaryActor>();
            obj.Initialize(name, go);
            return obj;
        }
    }
}
