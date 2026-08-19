using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aquila;
using Aquila.AbilityEditor;
using Aquila.AbilityPool;
using Aquila.Extension;
using Aquila.Fight;
using Editor.AbilityEditor;
using Editor.AbilityEditor.Config;
using Editor.AbilityEditor.Tools;
using GameFramework;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

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
        public void AbilityPresentationData_FlowsThroughGeneratorAndExporter()
        {
            var source = ScriptableObject.CreateInstance<AbilityEditorSOData>();
            source.Id = 9000;
            source.Name = "Presentation Flow";
            source.TimelineID = 1;
            source.TimelineDuration = 1f;
            source.SetTracks(new List<SerializedTrackData>
            {
                new SerializedTrackData
                {
                    TrackName = "Effect Track",
                    Clips = new List<TimelineClipData>
                    {
                        new EffectClipData("SimulatedEffect", 0.1f, 7000) { ResolveTypeID = 1 }
                    }
                }
            });
            source.SetMontageEvents(new List<MontageEventData>
            {
                new MontageEventData(0.2f, 5, "impact", "Event.Attack.Hit"),
                new MontageEventData(0.4f, 7, "finish", "Event.Attack.Finish")
            });
            source.SetCueBindings(new List<AbilityCueBindingData>
            {
                new AbilityCueBindingData(
                    "Event.Attack.Hit",
                    "GameplayCue.Attack.Hit.Vfx",
                    GameplayCueTargetPolicy.PrimaryTarget,
                    GameplayCueLocationPolicy.Target,
                    1.5f,
                    new Vector3(1f, 2f, 3f),
                    GameplayCueEventType.Execute),
                new AbilityCueBindingData(
                    "Event.Attack.Finish",
                    "GameplayCue.Attack.Finish.Audio",
                    GameplayCueTargetPolicy.Caster,
                    GameplayCueLocationPolicy.Source,
                    0.75f,
                    new Vector3(4f, 5f, 6f),
                    GameplayCueEventType.Remove)
            });

            var config = AbilityConfigGenerator.Generate(source);
            var tracks = new List<TimelineTrackItem> { source.Tracks[0].ToTrackItem() };
            source.MontageEvents[1].MarkerId = "mutated";
            source.CueBindings[1].CueTag = "GameplayCue.Mutated";
            var exported = AbilityDataExporter.CreateAbilityData(config, tracks);

            Assert.AreEqual(2, config.MontageEvents.Count);
            Assert.AreEqual("finish", config.MontageEvents[1].MarkerId);
            Assert.AreEqual(2, exported.MontageEvents.Count);
            Assert.AreEqual(0.4f, exported.MontageEvents[1].Time);
            Assert.AreEqual(7, exported.MontageEvents[1].Sequence);
            Assert.AreEqual("finish", exported.MontageEvents[1].MarkerId);
            Assert.AreEqual("Event.Attack.Finish", exported.MontageEvents[1].EventTag);
            Assert.AreEqual(2, exported.CueBindings.Count);
            Assert.AreEqual("Event.Attack.Finish", exported.CueBindings[1].EventTag);
            Assert.AreEqual("GameplayCue.Attack.Finish.Audio", exported.CueBindings[1].CueTag);
            Assert.AreEqual(GameplayCueEventType.Remove, exported.CueBindings[1].EventType);
            Assert.AreEqual(GameplayCueTargetPolicy.Caster, exported.CueBindings[1].TargetPolicy);
            Assert.AreEqual(GameplayCueLocationPolicy.Source, exported.CueBindings[1].LocationPolicy);
            Assert.AreEqual(0.75f, exported.CueBindings[1].Magnitude);
            Assert.AreEqual(new Vector3(4f, 5f, 6f), exported.CueBindings[1].LocationOffset);

            UnityEngine.Object.DestroyImmediate(exported);
            UnityEngine.Object.DestroyImmediate(source);
        }

        [Test]
        public void AbilityConfigInitialize_ReplacesCollections_AndNullPresentationDataClearsThem()
        {
            var config = new AbilityConfig();
            config.Initialize(
                new List<TriggerData> { new TriggerData(0.1f, new List<int> { 7000 }) },
                new List<EffectClipData> { new EffectClipData("FirstEffect", 0.1f, 7000) },
                new[] { new MontageEventData(0.2f, 1, "first", "Event.First") },
                new[]
                {
                    new AbilityCueBindingData(
                        "Event.First",
                        "GameplayCue.First",
                        GameplayCueTargetPolicy.Caster,
                        GameplayCueLocationPolicy.Source,
                        1f,
                        Vector3.zero)
                });

            config.Initialize(
                new List<TriggerData> { new TriggerData(0.3f, new List<int> { 7001 }) },
                new List<EffectClipData> { new EffectClipData("SecondEffect", 0.3f, 7001) },
                new[] { new MontageEventData(0.4f, 2, "second", "Event.Second") },
                new[]
                {
                    new AbilityCueBindingData(
                        "Event.Second",
                        "GameplayCue.Second",
                        GameplayCueTargetPolicy.PrimaryTarget,
                        GameplayCueLocationPolicy.Target,
                        1f,
                        Vector3.zero)
                });

            Assert.AreEqual(1, config.Triggers.Count);
            Assert.AreEqual(0.3f, config.Triggers[0].TriggerTime);
            Assert.AreEqual(1, config.Effects.Count);
            Assert.AreEqual(7001, config.Effects[0].EffectId);
            Assert.AreEqual(1, config.MontageEvents.Count);
            Assert.AreEqual("second", config.MontageEvents[0].MarkerId);
            Assert.AreEqual(1, config.CueBindings.Count);
            Assert.AreEqual("GameplayCue.Second", config.CueBindings[0].CueTag);

            config.Initialize(null, null, config.MontageEvents, config.CueBindings);

            Assert.AreEqual("second", config.MontageEvents[0].MarkerId);
            Assert.AreEqual("GameplayCue.Second", config.CueBindings[0].CueTag);

            config.Initialize(null, null);

            Assert.IsEmpty(config.Triggers);
            Assert.IsEmpty(config.Effects);
            Assert.IsEmpty(config.MontageEvents);
            Assert.IsEmpty(config.CueBindings);
        }

        [Test]
        public void AbilityPresentationData_IsEmptyWhenEditorHasNoCurrentAbilityData()
        {
            var editor = ScriptableObject.CreateInstance<AbilityEditorWindow>();
            var editorType = typeof(AbilityEditorWindow);
            editorType.GetField("_abilityIDTextField", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(editor, new TextField { value = "9003" });
            editorType.GetField("_abilityDescTextField", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(editor, new TextField { value = "No Current Ability Data" });
            editorType.GetField("_timelineIDTextField", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(editor, new TextField { value = "1" });
            editorType.GetField("_durationTextField", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(editor, new TextField { value = "1" });
            editorType.GetField("_timelineTrackItems", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(editor, new List<TimelineTrackItem>());

            var config = AbilityConfigGenerator.Generate(editor);
            var exported = AbilityDataExporter.CreateAbilityData(config, null);

            Assert.IsEmpty(config.MontageEvents);
            Assert.IsEmpty(config.CueBindings);
            Assert.IsEmpty(exported.MontageEvents);
            Assert.IsEmpty(exported.CueBindings);

            UnityEngine.Object.DestroyImmediate(exported);
            UnityEngine.Object.DestroyImmediate(editor);
        }

        [Test]
        public void AbilityBinary_ReservedByteIsIgnored_AndBadMagicIsRejectedByBothRuntimeReaders()
        {
            var ability = ScriptableObject.CreateInstance<AbilityEditorSOData>();
            ability.Id = 9001;
            ability.TimelineID = 1;
            ability.TimelineDuration = 1f;
            var effect = new EffectClipData("SimulatedEffect", 0.1f, 7001)
            {
                ResolveTypeID = 1
            };
            ability.SetTracks(new List<SerializedTrackData>
            {
                new SerializedTrackData
                {
                    TrackName = "Effect Track",
                    Clips = new List<TimelineClipData> { effect }
                }
            });
            ability.SetMontageEvents(new List<MontageEventData>
            {
                new MontageEventData(0.2f, 5, "impact", "Event.Attack.Hit")
            });
            ability.SetCueBindings(new List<AbilityCueBindingData>
            {
                new AbilityCueBindingData(
                    "Event.Attack.Hit",
                    "GameplayCue.Attack.Hit.Vfx",
                    GameplayCueTargetPolicy.PrimaryTarget,
                    GameplayCueLocationPolicy.Target,
                    1.5f,
                    new Vector3(1f, 2f, 3f),
                    GameplayCueEventType.Execute),
                new AbilityCueBindingData(
                    "Event.Attack.Hit",
                    "GameplayCue.Attack.Hit.Audio",
                    GameplayCueTargetPolicy.PrimaryTarget,
                    GameplayCueLocationPolicy.Target,
                    0.75f,
                    Vector3.zero,
                    GameplayCueEventType.Execute)
            });

            var abilityPath = Path.Combine(Path.GetTempPath(), "gameplay-cue-reserved.ablt");
            var invalidMagicPath = Path.Combine(Path.GetTempPath(), "gameplay-cue-invalid-magic.ablt");
            var retiredPath = Path.Combine(Path.GetTempPath(), "gameplay-cue-retired-clip.ablt");
            AbilityBinaryExporter.ExportAbility(ability, abilityPath);
            var abilityBytes = File.ReadAllBytes(abilityPath);
            abilityBytes[4] = 0xA7;
            File.WriteAllBytes(abilityPath, abilityBytes);
            var invalidMagicBytes = (byte[])abilityBytes.Clone();
            invalidMagicBytes[0] = (byte)'X';
            File.WriteAllBytes(invalidMagicPath, invalidMagicBytes);
            WriteRetiredClip(retiredPath);

            var verified = AbilityVerificationTool.ReadBinaryFile(abilityPath);
            Assert.AreEqual(9001, verified.Id);
            Assert.AreEqual("Event.Attack.Hit", verified.MontageEvents[0].EventTag);
            Assert.AreEqual("GameplayCue.Attack.Hit.Audio", verified.CueBindings[1].CueTag);
            Assert.Throws<InvalidDataException>(() => AbilityVerificationTool.ReadBinaryFile(invalidMagicPath));

            var parseAbility = typeof(Aquila.Toolkit.Tools.Ability).GetMethod("ParseAbilityBinary", BindingFlags.NonPublic | BindingFlags.Static);
            var parsed = (AbilityData)parseAbility.Invoke(null, new object[] { abilityBytes, new Dictionary<int, EffectData>() });
            Assert.AreEqual(1, parsed.GetEffects().Count);
            Assert.AreEqual("Event.Attack.Hit", parsed.GetMontageEvents()[0].EventTag);
            Assert.AreEqual("GameplayCue.Attack.Hit.Vfx", parsed.GetCueBindings()[0].CueTag);
            Assert.AreEqual("GameplayCue.Attack.Hit.Audio", parsed.GetCueBindings()[1].CueTag);
            Assert.AreEqual(GameplayCueEventType.Execute, parsed.GetCueBindings()[0].EventType);
            LogAssert.ignoreFailingMessages = true;
            Assert.AreEqual(0, ((AbilityData)parseAbility.Invoke(null, new object[] { invalidMagicBytes, new Dictionary<int, EffectData>() })).GetId());
            Assert.AreEqual(0, ((AbilityData)parseAbility.Invoke(null, new object[] { File.ReadAllBytes(retiredPath), new Dictionary<int, EffectData>() })).GetId());

            var gameObject = new GameObject("AbilityPoolTest");
            var abilityPool = gameObject.AddComponent<Component_AbilityPool>();
            var tryReadAbility = typeof(Component_AbilityPool).GetMethod("TryReadAbility", BindingFlags.NonPublic | BindingFlags.Instance);
            var abilityArgs = new object[] { abilityPath, null };
            Assert.IsTrue((bool)tryReadAbility.Invoke(abilityPool, abilityArgs));
            Assert.AreEqual(1, ((AbilityData)abilityArgs[1]).GetEffects().Count);
            Assert.AreEqual("GameplayCue.Attack.Hit.Vfx", ((AbilityData)abilityArgs[1]).GetCueBindings()[0].CueTag);
            Assert.AreEqual("GameplayCue.Attack.Hit.Audio", ((AbilityData)abilityArgs[1]).GetCueBindings()[1].CueTag);
            Assert.AreEqual(GameplayCueEventType.Execute, ((AbilityData)abilityArgs[1]).GetCueBindings()[0].EventType);
            var invalidMagicArgs = new object[] { invalidMagicPath, null };
            Assert.IsFalse((bool)tryReadAbility.Invoke(abilityPool, invalidMagicArgs));
            var retiredArgs = new object[] { retiredPath, null };
            Assert.IsFalse((bool)tryReadAbility.Invoke(abilityPool, retiredArgs));
            LogAssert.ignoreFailingMessages = false;

            UnityEngine.Object.DestroyImmediate(gameObject);
            UnityEngine.Object.DestroyImmediate(ability);
            File.Delete(abilityPath);
            File.Delete(invalidMagicPath);
            File.Delete(retiredPath);
        }

        [Test]
        public void EffectBinary_ReservedByteIsIgnored_FormulaIdIsRead_AndBadMagicIsRejected()
        {
            var effect = new EffectClipData("FormulaEffect", 0.1f, 7002)
            {
                ResolveTypeID = 3,
                FormulaID = 12345
            };
            var effectPath = Path.Combine(Path.GetTempPath(), "effect-reserved.efct");
            var invalidMagicPath = Path.Combine(Path.GetTempPath(), "effect-invalid-magic.efct");
            EffectBinaryExporter.ExportEffect(effect, effectPath);
            var effectBytes = File.ReadAllBytes(effectPath);
            effectBytes[6] = 0xD3;
            File.WriteAllBytes(effectPath, effectBytes);
            var invalidMagicBytes = (byte[])effectBytes.Clone();
            invalidMagicBytes[0] = (byte)'X';
            File.WriteAllBytes(invalidMagicPath, invalidMagicBytes);

            var verified = EffectVerificationTool.ReadBinaryFile(effectPath);
            Assert.AreEqual(7002, verified.Id);
            Assert.AreEqual(12345, verified.FormulaID);
            Assert.Throws<InvalidDataException>(() => EffectVerificationTool.ReadBinaryFile(invalidMagicPath));

            var parseEffect = typeof(Aquila.Toolkit.Tools.Ability).GetMethod("ParseEffectBinary", BindingFlags.NonPublic | BindingFlags.Static);
            var parsed = (EffectData)parseEffect.Invoke(null, new object[] { effectBytes });
            Assert.AreEqual(7002, parsed.GetEffectId());
            Assert.AreEqual(12345, parsed.GetFormulaID());
            LogAssert.ignoreFailingMessages = true;
            Assert.AreEqual(0, ((EffectData)parseEffect.Invoke(null, new object[] { invalidMagicBytes })).GetEffectId());

            var gameObject = new GameObject("EffectPoolTest");
            var abilityPool = gameObject.AddComponent<Component_AbilityPool>();
            var tryReadEffect = typeof(Component_AbilityPool).GetMethod("TryReadEffect", BindingFlags.NonPublic | BindingFlags.Instance);
            var effectArgs = new object[] { effectPath, null };
            Assert.IsTrue((bool)tryReadEffect.Invoke(abilityPool, effectArgs));
            Assert.AreEqual(12345, ((EffectData)effectArgs[1]).GetFormulaID());
            var invalidMagicArgs = new object[] { invalidMagicPath, null };
            Assert.IsFalse((bool)tryReadEffect.Invoke(abilityPool, invalidMagicArgs));
            LogAssert.ignoreFailingMessages = false;

            UnityEngine.Object.DestroyImmediate(gameObject);
            File.Delete(effectPath);
            File.Delete(invalidMagicPath);
        }

        [Test]
        public void AbilityBinaryAssets_HaveStableLayoutAndAreReadableByBothRuntimeReaders()
        {
            Assert.AreEqual(0, (int)TimelineClipType.Ability);
            Assert.AreEqual(1, (int)TimelineClipType.Buff);
            Assert.AreEqual(4, (int)TimelineClipType.Animation);
            Assert.AreEqual(5, (int)TimelineClipType.Custom);

            var paths = new List<string>(Directory.GetFiles("Assets/Res/Config/Ability", "*.ablt"));
            paths.Add("Assets/AbilityEditor/SandBox/sand_box.ablt");
            Assert.AreEqual(9, paths.Count);

            var parseAbility = typeof(Aquila.Toolkit.Tools.Ability).GetMethod("ParseAbilityBinary", BindingFlags.NonPublic | BindingFlags.Static);
            var gameObject = new GameObject("AbilityPoolAssetTest");
            var abilityPool = gameObject.AddComponent<Component_AbilityPool>();
            var tryReadAbility = typeof(Component_AbilityPool).GetMethod("TryReadAbility", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var path in paths)
            {
                AssertStableAbilityFileLayout(path);

                var parsed = (AbilityData)parseAbility.Invoke(null, new object[] { File.ReadAllBytes(path), new Dictionary<int, EffectData>() });
                Assert.Greater(parsed.GetId(), 0, path);

                var args = new object[] { path, null };
                Assert.IsTrue((bool)tryReadAbility.Invoke(abilityPool, args), path);
                Assert.AreEqual(parsed.GetId(), ((AbilityData)args[1]).GetId(), path);
            }

            UnityEngine.Object.DestroyImmediate(gameObject);
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
            audioNotify.ResolvedAssetPath = "Assets/Test/Cue.wav";
            var audioObject = new SerializedObject(audioNotify);
            audioObject.FindProperty("_soundEffectId").intValue = 20001;
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
            Assert.AreEqual(20001, audioNotify.ResolvedSoundEffectId);
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

        [Test]
        public void GameplayCueAudioNotify_MissingSoundEffect_LogsAndSkipsPlayback()
        {
            var notify = ScriptableObject.CreateInstance<TestGameplayCueAudioNotify>();
            SetCueTag(notify, "GameplayCue.Missing.Audio");
            var notifyObject = new SerializedObject(notify);
            notifyObject.FindProperty("_soundEffectId").intValue = 99999;
            notifyObject.ApplyModifiedPropertiesWithoutUndo();

            LogAssert.Expect(
                LogType.Error,
                "<color=orange>[GameplayCueAudioNotify] Sound effect lookup failed, SoundEffectId=99999, CueTag=GameplayCue.Missing.Audio</color>");
            notify.Execute(new GameplayCueParameters { Location = Vector3.one });

            Assert.AreEqual(99999, notify.ResolvedSoundEffectId);
            Assert.AreEqual(0, notify.PlayCount);
            UnityEngine.Object.DestroyImmediate(notify);
        }

        [Test]
        public void PhysicalAttackAudioCue_SourceAssetsScenesAndSimulation_AreConfigured()
        {
            const string abilityPath = "Assets/AbilityEditor/Editor/Config/Ability/1000.asset";
            const string notifyPath = "Assets/Res/GameplayCue/Skill/PhysicalAttackHitAudio.asset";
            const string audioPath = "Assets/Res/Audio/Fight/hitted.mp3";
            var ability = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(abilityPath);
            var notify = AssetDatabase.LoadAssetAtPath<GameplayCueAudioNotify>(notifyPath);

            Assert.IsNotNull(ability);
            Assert.AreEqual(1, ability.MontageEvents.Count);
            Assert.AreEqual(2f, ability.MontageEvents[0].Time);
            Assert.AreEqual(0, ability.MontageEvents[0].Sequence);
            Assert.AreEqual("physical_attack_hit", ability.MontageEvents[0].MarkerId);
            Assert.AreEqual("Event.Ability.PhysicalAttack.Hit", ability.MontageEvents[0].EventTag);
            Assert.AreEqual(1, ability.CueBindings.Count);
            Assert.AreEqual("Event.Ability.PhysicalAttack.Hit", ability.CueBindings[0].EventTag);
            Assert.AreEqual("GameplayCue.Ability.PhysicalAttack.Hit", ability.CueBindings[0].CueTag);
            Assert.AreEqual(GameplayCueEventType.Execute, ability.CueBindings[0].EventType);
            Assert.AreEqual(GameplayCueTargetPolicy.PrimaryTarget, ability.CueBindings[0].TargetPolicy);
            Assert.AreEqual(GameplayCueLocationPolicy.Target, ability.CueBindings[0].LocationPolicy);
            Assert.AreEqual(1f, ability.CueBindings[0].Magnitude);
            Assert.AreEqual(Vector3.zero, ability.CueBindings[0].LocationOffset);

            Assert.IsNotNull(notify);
            Assert.AreEqual("GameplayCue.Ability.PhysicalAttack.Hit", notify.CueTag);
            var notifyObject = new SerializedObject(notify);
            Assert.AreEqual(20001, notifyObject.FindProperty("_soundEffectId").intValue);
            Assert.AreEqual("Effect", notifyObject.FindProperty("_soundGroup").stringValue);
            Assert.AreEqual(1f, notifyObject.FindProperty("_volume").floatValue);

            var soundEffectMap = new Cfg.Common.SoundEffectMap(
                new Bright.Serialization.ByteBuf(File.ReadAllBytes("Assets/Res/DataTables/common_soundeffectmap.bytes")));
            var soundEffect = soundEffectMap.GetOrDefault(20001);
            Assert.IsNotNull(soundEffect);
            Assert.AreEqual(audioPath, soundEffect.asset_path);
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath));
            CollectionAssert.Contains(AssetDatabase.GetDependencies("Assets/Res/Scene/Start.unity", true), notifyPath);
            CollectionAssert.Contains(AssetDatabase.GetDependencies("Assets/AbilityEditor/AbilityEditorEntry.unity", true), notifyPath);

            var parseAbility = typeof(Aquila.Toolkit.Tools.Ability).GetMethod(
                "ParseAbilityBinary",
                BindingFlags.NonPublic | BindingFlags.Static);
            var abilityPoolObject = new GameObject("PhysicalAttackAbilityPoolTest");
            var abilityPool = abilityPoolObject.AddComponent<Component_AbilityPool>();
            var tryReadAbility = typeof(Component_AbilityPool).GetMethod(
                "TryReadAbility",
                BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var binaryPath in new[]
                     {
                         "Assets/Res/Config/Ability/1000.ablt",
                         "Assets/AbilityEditor/SandBox/sand_box.ablt"
                     })
            {
                AssertStableAbilityFileLayout(binaryPath);
                var bytes = File.ReadAllBytes(binaryPath);
                var parsed = (AbilityData)parseAbility.Invoke(
                    null,
                    new object[] { bytes, new Dictionary<int, EffectData>() });
                AssertPhysicalAttackCueData(parsed, binaryPath);

                var readArgs = new object[] { binaryPath, null };
                Assert.IsTrue((bool)tryReadAbility.Invoke(abilityPool, readArgs), binaryPath);
                AssertPhysicalAttackCueData((AbilityData)readArgs[1], binaryPath);
            }
            UnityEngine.Object.DestroyImmediate(abilityPoolObject);

            var simulatedNotify = ScriptableObject.CreateInstance<TestGameplayCueAudioNotify>();
            simulatedNotify.ResolvedAssetPath = soundEffect.asset_path;
            var simulatedObject = new SerializedObject(simulatedNotify);
            simulatedObject.FindProperty("_soundEffectId").intValue = 20001;
            simulatedObject.FindProperty("_soundGroup").stringValue = "Effect";
            simulatedObject.FindProperty("_volume").floatValue = 1f;
            simulatedObject.ApplyModifiedPropertiesWithoutUndo();
            var positions = new Dictionary<int, Vector3>
            {
                { 1, Vector3.zero },
                { 2, new Vector3(3f, 4f, 5f) }
            };
            var montage = AbilityMontage.Create(ability.MontageEvents, ability.Id, 10, 1, new[] { 2 });
            montage.GameplayEvent += gameplayEvent => AbilityCueRouter.Route(
                ability.CueBindings,
                gameplayEvent,
                actorId => positions[actorId],
                (cueTag, parameters) =>
                {
                    Assert.AreEqual("GameplayCue.Ability.PhysicalAttack.Hit", cueTag);
                    simulatedNotify.Execute(parameters);
                });
            montage.Start();
            montage.Advance(0f, 1.9f);
            Assert.AreEqual(0, simulatedNotify.PlayCount);
            montage.Advance(1.9f, 2.1f);
            montage.Advance(2.1f, 5f);

            Assert.AreEqual(1, simulatedNotify.PlayCount);
            Assert.AreEqual(20001, simulatedNotify.ResolvedSoundEffectId);
            Assert.AreEqual(audioPath, simulatedNotify.AssetPath);
            Assert.AreEqual("Effect", simulatedNotify.SoundGroup);
            Assert.AreEqual(1f, simulatedNotify.Volume);
            Assert.AreEqual(new Vector3(3f, 4f, 5f), simulatedNotify.Location);
            ReferencePool.Release(montage);
            UnityEngine.Object.DestroyImmediate(simulatedNotify);
        }

        private static void AssertPhysicalAttackCueData(AbilityData ability, string source)
        {
            Assert.AreEqual(1000, ability.GetId(), source);
            Assert.AreEqual(1, ability.GetMontageEvents().Count, source);
            Assert.AreEqual(2f, ability.GetMontageEvents()[0].Time, source);
            Assert.AreEqual(0, ability.GetMontageEvents()[0].Sequence, source);
            Assert.AreEqual("physical_attack_hit", ability.GetMontageEvents()[0].MarkerId, source);
            Assert.AreEqual("Event.Ability.PhysicalAttack.Hit", ability.GetMontageEvents()[0].EventTag, source);
            Assert.AreEqual(1, ability.GetCueBindings().Count, source);
            Assert.AreEqual("Event.Ability.PhysicalAttack.Hit", ability.GetCueBindings()[0].EventTag, source);
            Assert.AreEqual("GameplayCue.Ability.PhysicalAttack.Hit", ability.GetCueBindings()[0].CueTag, source);
            Assert.AreEqual(GameplayCueEventType.Execute, ability.GetCueBindings()[0].EventType, source);
            Assert.AreEqual(GameplayCueTargetPolicy.PrimaryTarget, ability.GetCueBindings()[0].TargetPolicy, source);
            Assert.AreEqual(GameplayCueLocationPolicy.Target, ability.GetCueBindings()[0].LocationPolicy, source);
            Assert.AreEqual(1f, ability.GetCueBindings()[0].Magnitude, source);
            Assert.AreEqual(Vector3.zero, ability.GetCueBindings()[0].LocationOffset, source);
        }

        private static TestGameplayCueNotify CreateNotify(string tag)
        {
            var notify = ScriptableObject.CreateInstance<TestGameplayCueNotify>();
            SetCueTag(notify, tag);
            return notify;
        }

        private static void WriteRetiredClip(string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(new byte[] { (byte)'A', (byte)'B', (byte)'L', (byte)'T' });
                writer.Write((byte)0x7F);
                writer.Write(9002);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0f);
                writer.Write(1);
                writer.Write(1f);
                writer.Write(1);
                writer.Write(1);
                writer.Write(2);
                writer.Write(0f);
                writer.Write(0f);
            }
        }

        private static void AssertStableAbilityFileLayout(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                Assert.AreEqual("ABLT", new string(reader.ReadChars(4)), path);
                reader.ReadByte();
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadSingle();
                reader.ReadInt32();
                reader.ReadSingle();

                var trackCount = reader.ReadInt32();
                for (var trackIndex = 0; trackIndex < trackCount; trackIndex++)
                {
                    var clipCount = reader.ReadInt32();
                    for (var clipIndex = 0; clipIndex < clipCount; clipIndex++)
                    {
                        Assert.AreEqual(1, reader.ReadInt32(), $"{path} track={trackIndex} clip={clipIndex}");
                        reader.ReadSingle();
                        reader.ReadSingle();
                        SkipEffectClip(reader);
                    }
                }

                var montageCount = reader.ReadInt32();
                for (var i = 0; i < montageCount; i++)
                {
                    reader.ReadSingle();
                    reader.ReadInt32();
                    ReadLengthPrefixedString(reader);
                    ReadLengthPrefixedString(reader);
                }

                var bindingCount = reader.ReadInt32();
                for (var i = 0; i < bindingCount; i++)
                {
                    ReadLengthPrefixedString(reader);
                    ReadLengthPrefixedString(reader);
                    reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadSingle();
                    reader.ReadSingle();
                    reader.ReadSingle();
                    reader.ReadSingle();
                }

                Assert.AreEqual(stream.Length, stream.Position, path);
            }
        }

        private static void SkipEffectClip(BinaryReader reader)
        {
            reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadBoolean();
            reader.ReadInt32();
            reader.ReadUInt16();
            reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadSingle();
            reader.ReadSingle();
            reader.ReadUInt16();
            reader.ReadBoolean();
            for (var i = 0; i < 4; i++)
                reader.ReadSingle();
            for (var i = 0; i < 4; i++)
                reader.ReadInt32();
            var deriveCount = reader.ReadInt32();
            for (var i = 0; i < deriveCount; i++)
                reader.ReadInt32();
            var awakeCount = reader.ReadInt32();
            for (var i = 0; i < awakeCount; i++)
                reader.ReadInt32();
            reader.ReadInt32();
        }

        private static string ReadLengthPrefixedString(BinaryReader reader)
        {
            var length = reader.ReadInt32();
            return length <= 0 ? string.Empty : System.Text.Encoding.UTF8.GetString(reader.ReadBytes(length));
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
        public int ResolvedSoundEffectId { get; private set; }
        public string ResolvedAssetPath { get; set; }
        public string AssetPath { get; private set; }
        public string SoundGroup { get; private set; }
        public float Volume { get; private set; }
        public Vector3 Location { get; private set; }

        protected override string ResolveAssetPath(int soundEffectId)
        {
            ResolvedSoundEffectId = soundEffectId;
            return ResolvedAssetPath;
        }

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
