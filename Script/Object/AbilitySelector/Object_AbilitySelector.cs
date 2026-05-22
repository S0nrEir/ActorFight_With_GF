using System.Collections.Generic;
using Aquila.Combat;
using Aquila.Fight;
using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Aquila.ObjectPool
{
    public abstract class Object_AbilitySelectorBase : Object_Base
    {
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
            // var selectorName = SelectorName(selectType);
            var selectorName = selectType.ToString();
            var pool = GetSelectorPool();
            if (!pool.CanSpawn(selectorName))
                RegisterSelector(pool, selectType, selectorName);

            var selector = pool.Spawn(selectorName);
            selector.Setup(selector.Target as GameObject);
            return selector;
        }

        private static IObjectPool<Object_AbilitySelectorBase> GetSelectorPool()
        {
            var pool = GameEntry.ObjectPool.GetObjectPool<Object_AbilitySelectorBase>(nameof(Object_AbilitySelectorBase));
            if (pool != null)
                return pool;

            pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_AbilitySelectorBase>(nameof(Object_AbilitySelectorBase));
            pool.ExpireTime = 60f;
            return pool;
        }

        private static void RegisterSelector(
            IObjectPool<Object_AbilitySelectorBase> pool,
            AbilitySelectType selectType,
            string selectorName)
        {
            var go = new GameObject($"{nameof(Object_AbilitySelectorBase)}_{selectorName}");
            var selector = CreateSelector(selectType, selectorName, go);
            pool.Register(selector, false);
        }

        private static Object_AbilitySelectorBase CreateSelector(AbilitySelectType selectType, string selectorName, GameObject go)
        {
            if (selectType == AbilitySelectType.Circle)
                return Object_AbilitySelectorCircle.Gen(selectorName, go);

            return Object_AbilitySelectorSingle.Gen(selectorName, go);
        }

        // private static string SelectorName(AbilitySelectType selectType)
        // {
        //     if (selectType == AbilitySelectType.Circle)
        //         return CircleSelectorName;
        //
        //     return SingleSelectorName;
        // }

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

        // protected const string SingleSelectorName = "Single";
        // protected const string CircleSelectorName = "Circle";
        // private const string SelectorPoolName = nameof(Object_AbilitySelectorBase);

        private AbilitySelectorDriver _driver;
        private bool _isReleased;
    }
}
