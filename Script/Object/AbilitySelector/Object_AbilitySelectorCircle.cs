using System.Collections;
using System.Collections.Generic;
using Aquila.Combat;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using UnityEngine;

namespace Aquila.ObjectPool
{
    public class Object_AbilitySelectorCircle : Object_AbilitySelectorBase
    {
        public override void Setup(GameObject go)
        {
            base.Setup(go);
            _lineRenderer = go.GetComponent<LineRenderer>();
            if (_lineRenderer == null)
                _lineRenderer = go.AddComponent<LineRenderer>();

            _lineRenderer.useWorldSpace = false;
            _lineRenderer.loop = true;
            _lineRenderer.positionCount = CircleSegmentCount;
            _lineRenderer.startWidth = 0.04f;
            _lineRenderer.endWidth = 0.04f;
            if (_lineRenderer.sharedMaterial == null)
                _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = new Color(0.2f, 0.75f, 1f, 0.9f);
            _lineRenderer.endColor = _lineRenderer.startColor;
        }

        protected override void OnBegin()
        {
            DrawCircle(_abilityData.GetSelectRadius());
        }

        protected override void OnConfirm()
        {
            if (!TryGetMouseGroundPoint(out var groundPoint))
            {
                Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
                ReleaseSelf();
                return;
            }

            _targetGameObject.transform.position = groundPoint;
            var radius = _abilityData.GetSelectRadius();
            CollectLegalTargetsInCircle(groundPoint, radius, _targetIds);
            if (_targetIds.Count <= 0)
            {
                Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
                ReleaseSelf();
                return;
            }

            SubmitAndRelease(CastCmd.CreateWithMultiTarget(_castorId, _targetIds.ToArray(), _abilityId));
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

        private void DrawCircle(float radius)
        {
            if (_lineRenderer == null)
                return;

            for (var i = 0; i < CircleSegmentCount; i++)
            {
                var angle = i / (float)CircleSegmentCount * Mathf.PI * 2f;
                var pos = new Vector3(Mathf.Cos(angle) * radius, 0.03f, Mathf.Sin(angle) * radius);
                _lineRenderer.SetPosition(i, pos);
            }
        }

        public static Object_AbilitySelectorCircle Gen(string name, GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_AbilitySelectorCircle>();
            obj.Initialize(name, go);
            return obj;
        }

        private const int CircleSegmentCount = 64;
        private readonly List<int> _targetIds = new List<int>(16);
        private LineRenderer _lineRenderer;
    }
}
