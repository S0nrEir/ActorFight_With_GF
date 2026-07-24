using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aquila;
using Aquila.AbilityEditor;
using Aquila.AbilityPool;
using Aquila.Extension;
using Aquila.Fight;
using Editor.AbilityEditor.Tools;
using GameFramework;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Aquila.Tests.Editor
{
    public sealed class GameplayCuePresentationTests
    {
        [Test]
        public void TagIndex_ResolvesExactThenParents_DeduplicatesNotify_AndRejectsInvalidTags()
        {
            var exactFirst = CreateNotify("a.b.c");
            var exactSecond = CreateNotify("a.b.c");
            var parent = CreateNotify("a.b");
            var root = CreateNotify("a");
            var index = new GameplayCueTagIndex();
            var resolved = new List<GameplayCueNotifyBase>();

            index.Build(new GameplayCueNotifyBase[] { exactFirst, exactSecond, exactFirst, parent, root });
            index.Resolve("a.b.c", resolved);

            CollectionAssert.AreEqual(new GameplayCueNotifyBase[] { exactFirst, exactSecond, parent, root }, resolved);

            index.Resolve("a.b", resolved);
            CollectionAssert.AreEqual(new GameplayCueNotifyBase[] { parent, root }, resolved);

            Assert.Throws<ArgumentException>(() => GameplayCueTagIndex.Validate(""));
            Assert.Throws<ArgumentException>(() => GameplayCueTagIndex.Validate(".a"));
            Assert.Throws<ArgumentException>(() => GameplayCueTagIndex.Validate("a."));
            Assert.Throws<ArgumentException>(() => GameplayCueTagIndex.Validate("a..b"));
            Assert.IsTrue(GameplayCueTagIndex.IsValid("a.b"));
            Assert.IsFalse(GameplayCueTagIndex.IsValid("a..b"));

            DestroyNotify(exactFirst, exactSecond, parent, root);
        }

        [Test]
        public void AbilityMontage_EmitsZeroAndLargeDeltaMarkersInStableOrder_AndStopsAfterInterrupt()
        {
            var markers = new MontageEventData[]
            {
                new MontageEventData(0f, 0, "zero", "Event.Zero"),
                new MontageEventData(0.2f, 2, "second", "Event.Second"),
                new MontageEventData(0.2f, 1, "first", "Event.First"),
                new MontageEventData(0.4f, 0, "last", "Event.Last")
            };
            var montage = AbilityMontage.Create(markers, 10, 100, 1, new[] { 2, 3 });
            var emitted = new List<string>();
            montage.GameplayEvent += gameplayEvent => emitted.Add(gameplayEvent.MarkerId);

            montage.Start();
            montage.Advance(0.1f, 0.5f);
            montage.Stop();
            montage.Advance(0.5f, 1f);

            CollectionAssert.AreEqual(new[] { "zero", "first", "second", "last" }, emitted);
            ReferencePool.Release(montage);
        }

        [Test]
        public void AbilityCueRouter_MapsEventAndResolvesCasterPrimaryAndEachTargetParameters()
        {
            var bindings = new AbilityCueBindingData[]
            {
                new AbilityCueBindingData("Event.Hit", "Cue.Caster", GameplayCueTargetPolicy.Caster, GameplayCueLocationPolicy.Source, 1f, Vector3.zero),
                new AbilityCueBindingData("Event.Hit", "Cue.Primary", GameplayCueTargetPolicy.PrimaryTarget, GameplayCueLocationPolicy.Target, 2f, Vector3.one),
                new AbilityCueBindingData("Event.Hit", "Cue.Each", GameplayCueTargetPolicy.EachTarget, GameplayCueLocationPolicy.Target, 3f, Vector3.zero)
            };
            var positions = new Dictionary<int, Vector3>
            {
                { 1, new Vector3(1f, 0f, 0f) },
                { 2, new Vector3(2f, 0f, 0f) },
                { 3, new Vector3(3f, 0f, 0f) }
            };
            var requests = new List<(string cueTag, GameplayCueParameters parameters)>();
            var gameplayEvent = new MontageGameplayEvent
            {
                EventTag = "Event.Hit",
                EventTime = 0.25f,
                AbilityId = 7,
                ActivationId = 9,
                SourceActorId = 1,
                TargetActorIds = new[] { 2, 3 },
                Sequence = 4
            };

            AbilityCueRouter.Route(bindings, gameplayEvent, id => positions[id], (tag, parameters) => requests.Add((tag, parameters)));

            Assert.AreEqual(4, requests.Count);
            Assert.AreEqual("Cue.Caster", requests[0].cueTag);
            Assert.AreEqual(1, requests[0].parameters.TargetActorId);
            Assert.AreEqual(new Vector3(1f, 0f, 0f), requests[0].parameters.Location);
            Assert.AreEqual("Cue.Primary", requests[1].cueTag);
            Assert.AreEqual(2, requests[1].parameters.TargetActorId);
            Assert.AreEqual(new Vector3(3f, 1f, 1f), requests[1].parameters.Location);
            Assert.AreEqual("Cue.Each", requests[2].cueTag);
            Assert.AreEqual(2, requests[2].parameters.TargetActorId);
            Assert.AreEqual("Cue.Each", requests[3].cueTag);
            Assert.AreEqual(3, requests[3].parameters.TargetActorId);
        }

        [Test]
        public void AbilityBinaryV5_RoundTripsMontageAndCueData_AndRejectsV4InBothRuntimeReaders()
        {
            var ability = ScriptableObject.CreateInstance<AbilityEditorSOData>();
            ability.Id = 9001;
            ability.TimelineID = 1;
            ability.TimelineDuration = 1f;
            ability.SetTracks(new List<SerializedTrackData>());
            ability.SetMontageEvents(new List<MontageEventData>
            {
                new MontageEventData(0.2f, 5, "impact", "Event.Attack.Hit")
            });
            ability.SetCueBindings(new List<AbilityCueBindingData>
            {
                new AbilityCueBindingData(
                    "Event.Attack.Hit",
                    "GameplayCue.Attack.Hit",
                    GameplayCueTargetPolicy.PrimaryTarget,
                    GameplayCueLocationPolicy.Target,
                    1.5f,
                    new Vector3(1f, 2f, 3f),
                    GameplayCueEventType.Add)
            });

            var v5Path = Path.Combine(Path.GetTempPath(), "gameplay-cue-v5.ablt");
            var v4Path = Path.Combine(Path.GetTempPath(), "gameplay-cue-v4.ablt");
            AbilityBinaryExporter.ExportAbility(ability, v5Path);
            File.WriteAllBytes(v4Path, new byte[] { (byte)'A', (byte)'B', (byte)'L', (byte)'T', 0x04 });

            var parseAbility = typeof(Aquila.Toolkit.Tools.Ability).GetMethod("ParseAbilityBinary", BindingFlags.NonPublic | BindingFlags.Static);
            var parsed = (AbilityData)parseAbility.Invoke(null, new object[] { File.ReadAllBytes(v5Path), new Dictionary<int, EffectData>() });
            Assert.AreEqual("Event.Attack.Hit", parsed.GetMontageEvents()[0].EventTag);
            Assert.AreEqual("GameplayCue.Attack.Hit", parsed.GetCueBindings()[0].CueTag);
            Assert.AreEqual(GameplayCueEventType.Add, parsed.GetCueBindings()[0].EventType);
            LogAssert.ignoreFailingMessages = true;
            Assert.AreEqual(0, ((AbilityData)parseAbility.Invoke(null, new object[] { File.ReadAllBytes(v4Path), new Dictionary<int, EffectData>() })).GetId());

            var gameObject = new GameObject("AbilityPoolTest");
            var abilityPool = gameObject.AddComponent<Component_AbilityPool>();
            var tryReadAbility = typeof(Component_AbilityPool).GetMethod("TryReadAbility", BindingFlags.NonPublic | BindingFlags.Instance);
            var v5Args = new object[] { v5Path, null };
            Assert.IsTrue((bool)tryReadAbility.Invoke(abilityPool, v5Args));
            Assert.AreEqual("GameplayCue.Attack.Hit", ((AbilityData)v5Args[1]).GetCueBindings()[0].CueTag);
            Assert.AreEqual(GameplayCueEventType.Add, ((AbilityData)v5Args[1]).GetCueBindings()[0].EventType);
            var v4Args = new object[] { v4Path, null };
            Assert.IsFalse((bool)tryReadAbility.Invoke(abilityPool, v4Args));
            LogAssert.ignoreFailingMessages = false;

            UnityEngine.Object.DestroyImmediate(gameObject);
            UnityEngine.Object.DestroyImmediate(ability);
            File.Delete(v5Path);
            File.Delete(v4Path);
        }

        [Test]
        public void GameplayCue_ExecutesVfx_AndIsolatesPresentationFailures()
        {
            var prefab = new GameObject("CueVfxPrefab");
            var vfxNotify = ScriptableObject.CreateInstance<GameplayCueVfxNotify>();
            SetCueTag(vfxNotify, "Cue.Visual");
            var notifyObject = new SerializedObject(vfxNotify);
            notifyObject.FindProperty("_prefab").objectReferenceValue = prefab;
            notifyObject.FindProperty("_lifeTime").floatValue = 0f;
            notifyObject.ApplyModifiedPropertiesWithoutUndo();

            var audioNotify = ScriptableObject.CreateInstance<TestGameplayCueAudioNotify>();
            SetCueTag(audioNotify, "Cue.Visual");
            var audioObject = new SerializedObject(audioNotify);
            audioObject.FindProperty("_assetPath").stringValue = "Assets/Test/Cue.wav";
            audioObject.FindProperty("_soundGroup").stringValue = "Effect";
            audioObject.FindProperty("_volume").floatValue = 0.75f;
            audioObject.ApplyModifiedPropertiesWithoutUndo();
            var throwingNotify = ScriptableObject.CreateInstance<ThrowingGameplayCueNotify>();
            SetCueTag(throwingNotify, "Cue.Visual");
            var afterFailureNotify = CreateNotify("Cue.Visual");
            var gameObject = new GameObject("GameplayCueTest");
            var component = gameObject.AddComponent<Component_GameplayCue>();
            var componentObject = new SerializedObject(component);
            var notifies = componentObject.FindProperty("_notifies");
            notifies.arraySize = 4;
            notifies.GetArrayElementAtIndex(0).objectReferenceValue = vfxNotify;
            notifies.GetArrayElementAtIndex(1).objectReferenceValue = audioNotify;
            notifies.GetArrayElementAtIndex(2).objectReferenceValue = throwingNotify;
            notifies.GetArrayElementAtIndex(3).objectReferenceValue = afterFailureNotify;
            componentObject.ApplyModifiedPropertiesWithoutUndo();
            component.RebuildIndex();

            LogAssert.ignoreFailingMessages = true;
            Assert.DoesNotThrow(() => component.ExecuteGameplayCue("Cue.Visual", new GameplayCueParameters { Location = new Vector3(5f, 0f, 0f) }));
            LogAssert.ignoreFailingMessages = false;
            var vfxInstance = GameObject.Find("CueVfxPrefab(Clone)");
            Assert.IsNotNull(vfxInstance);
            Assert.AreEqual(1, audioNotify.PlayCount);
            Assert.AreEqual("Assets/Test/Cue.wav", audioNotify.AssetPath);
            Assert.AreEqual("Effect", audioNotify.SoundGroup);
            Assert.AreEqual(0.75f, audioNotify.Volume);
            Assert.AreEqual(new Vector3(5f, 0f, 0f), audioNotify.Location);
            Assert.AreEqual(1, afterFailureNotify.ExecuteCount);

            UnityEngine.Object.DestroyImmediate(vfxInstance);
            UnityEngine.Object.DestroyImmediate(gameObject);
            UnityEngine.Object.DestroyImmediate(vfxNotify);
            UnityEngine.Object.DestroyImmediate(audioNotify);
            UnityEngine.Object.DestroyImmediate(throwingNotify);
            UnityEngine.Object.DestroyImmediate(afterFailureNotify);
            UnityEngine.Object.DestroyImmediate(prefab);
        }

        private static TestGameplayCueNotify CreateNotify(string tag)
        {
            var notify = ScriptableObject.CreateInstance<TestGameplayCueNotify>();
            SetCueTag(notify, tag);
            return notify;
        }

        private static void SetCueTag(GameplayCueNotifyBase notify, string tag)
        {
            var serialized = new SerializedObject(notify);
            serialized.FindProperty("_cueTag").stringValue = tag;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void DestroyNotify(params UnityEngine.Object[] notifies)
        {
            for (var i = 0; i < notifies.Length; i++)
                UnityEngine.Object.DestroyImmediate(notifies[i]);
        }

    }

    public sealed class TestGameplayCueNotify : GameplayCueNotifyBase
    {
        public int ExecuteCount { get; private set; }

        public override void Execute(in GameplayCueParameters parameters)
        {
            ExecuteCount++;
        }
    }

    public sealed class ThrowingGameplayCueNotify : GameplayCueNotifyBase
    {
        public override void Execute(in GameplayCueParameters parameters)
        {
            throw new InvalidOperationException("simulated presentation failure");
        }
    }

    public sealed class TestGameplayCueAudioNotify : GameplayCueAudioNotify
    {
        public int PlayCount { get; private set; }
        public string AssetPath { get; private set; }
        public string SoundGroup { get; private set; }
        public float Volume { get; private set; }
        public Vector3 Location { get; private set; }

        protected override void Play(string assetPath, string soundGroup, float volume, Vector3 location)
        {
            PlayCount++;
            AssetPath = assetPath;
            SoundGroup = soundGroup;
            Volume = volume;
            Location = location;
        }
    }

}
