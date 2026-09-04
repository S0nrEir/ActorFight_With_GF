using Aquila.Combat.Resolve;
using Cfg.Enum;
using NUnit.Framework;

namespace Aquila.Tests.Editor
{
    public sealed class CombatDamageResolutionTests
    {
        [TestCase(100f, 30f, 20f, ResolveSourceType.EffectDirect, 70f)]
        [TestCase(100f, 30f, 20f, ResolveSourceType.MagicDamage, 80f)]
        [TestCase(100f, 30f, 20f, ResolveSourceType.TrueDamage, 100f)]
        [TestCase(10f, 30f, 20f, ResolveSourceType.MagicDamage, 0f)]
        public void ApplyDefense_UsesDamageSourceRules(
            float inputDelta,
            float physicalDefense,
            float magicDefense,
            ResolveSourceType sourceType,
            float expected)
        {
            var actual = CombatDamageCalculator.ApplyDefense(
                inputDelta,
                physicalDefense,
                magicDefense,
                sourceType);

            Assert.AreEqual(expected, actual, 0.001f);
        }

        [TestCase(0.9f, 0f)]
        [TestCase(-10f, 0f)]
        [TestCase(10.9f, 10f)]
        public void NormalizeDamage_DoesNotRaiseSubUnitDamage(
            float inputDelta,
            float expected)
        {
            Assert.AreEqual(expected, CombatDamageCalculator.NormalizeDamage(inputDelta), 0.001f);
        }
    }
}
