using System;
using System.Reflection;
using Aquila.Combat;
using Aquila.Fight;
using Aquila.ObjectPool;
using Cfg.Enum;
using GameFramework;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Aquila.Tests.Editor
{
    public sealed class AbilityTargetContextTests
    {
        [Test]
        public void CastCmd_CopiesActorIds_StoresSpatialContext_AndClearsAllFields()
        {
            var actorIds = new[] { 12, 7 };
            var origin = new Vector3(1f, 2f, 3f);
            var targetPoint = new Vector3(5f, 6f, 3f);
            var cmd = CastCmd.CreateWithMultiTarget(
                1,
                actorIds,
                99,
                origin,
                targetPoint,
                targetPoint - origin);

            actorIds[0] = 100;

            CollectionAssert.AreEqual(new[] { 12, 7 }, cmd._targetInstanceIdArr);
            Assert.AreEqual(origin, cmd._castOrigin);
            Assert.AreEqual(targetPoint, cmd._targetPoint);
            Assert.AreEqual(Vector3.right, cmd._direction);
            Assert.IsTrue(cmd._hasCastOrigin);
            Assert.IsTrue(cmd._hasTargetPoint);
            Assert.IsTrue(cmd._hasDirection);

            cmd.Clear();

            Assert.IsEmpty(cmd._targetInstanceIdArr);
            Assert.AreEqual(Vector3.zero, cmd._castOrigin);
            Assert.AreEqual(Vector3.zero, cmd._targetPoint);
            Assert.AreEqual(Vector3.zero, cmd._direction);
            Assert.IsFalse(cmd._hasCastOrigin);
            Assert.IsFalse(cmd._hasTargetPoint);
            Assert.IsFalse(cmd._hasDirection);
            ReferencePool.Release(cmd);
        }

        [Test]
        public void EffectSpec_CopiesSpatialContext_WithoutHoldingCastCommand()
        {
            var effect = new TargetContextTestEffect();
            var origin = new Vector3(2f, 0f, 4f);
            var targetPoint = new Vector3(7f, 0f, 4f);

            effect.Init(
                default,
                null,
                null,
                origin,
                targetPoint,
                Vector3.right,
                true,
                true,
                true);

            Assert.AreEqual(origin, effect.CastOrigin);
            Assert.AreEqual(targetPoint, effect.TargetPoint);
            Assert.AreEqual(Vector3.right, effect.Direction);
            Assert.IsTrue(effect.HasCastOrigin);
            Assert.IsTrue(effect.HasTargetPoint);
            Assert.IsTrue(effect.HasDirection);

            effect.Clear();

            Assert.AreEqual(Vector3.zero, effect.CastOrigin);
            Assert.IsFalse(effect.HasCastOrigin);
            Assert.IsFalse(effect.HasTargetPoint);
            Assert.IsFalse(effect.HasDirection);
        }

        [Test]
        public void SelectorEnum_HasExplicitMappingsAndPrefabs()
        {
            var supportedMethod = typeof(Object_AbilitySelectorBase).GetMethod(
                "IsSupportedSelectorType",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(supportedMethod);

            foreach (AbilitySelectType selectType in Enum.GetValues(typeof(AbilitySelectType)))
            {
                Assert.IsTrue((bool)supportedMethod.Invoke(null, new object[] { selectType }), selectType.ToString());
                var path = $"Assets/Res/Prefab/AbilitySelector/Object_AbilitySelector{selectType}.prefab";
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                Assert.IsNotNull(prefab, path);
                Assert.IsNotNull(prefab.GetComponent<AbilitySelectorDriver>(), path);
            }
        }

        [Test]
        public void ActorTargetRequirement_DistinguishesSpatialAndActorSelectors()
        {
            Assert.IsTrue(CastCmd.RequiresActorTargets(AbilitySelectType.Single));
            Assert.IsTrue(CastCmd.RequiresActorTargets(AbilitySelectType.Circle));
            Assert.IsTrue(CastCmd.RequiresActorTargets(AbilitySelectType.SecondaryActor));
            Assert.IsFalse(CastCmd.RequiresActorTargets(AbilitySelectType.Point));
            Assert.IsFalse(CastCmd.RequiresActorTargets(AbilitySelectType.Direction));
            Assert.IsFalse(CastCmd.RequiresActorTargets(AbilitySelectType.Line));
            Assert.IsFalse(CastCmd.RequiresActorTargets(AbilitySelectType.GlobalActor));
        }

        private sealed class TargetContextTestEffect : EffectSpec_Base
        {
        }
    }
}
