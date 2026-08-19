using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aquila.AbilityEditor;
using Aquila.AbilityPool;
using Aquila.Extension;
using Aquila.Fight;
using Editor.AbilityEditor.Config;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    public static class GameplayCueSelfTestRunner
    {
        public static void Run()
        {
            TestTagHierarchy();
            TestMontageMarkers();
            TestCueRouting();
            TestBinaryContract();
            TestEffectBinaryContract();
            TestPhysicalAttackConfiguration();
            TestNotifies();

            var resultPath = Path.Combine(Directory.GetCurrentDirectory(), "Temp/GameplayCueSelfTest.result");
            File.WriteAllText(resultPath, "PASS");
            Debug.Log("[GameplayCueSelfTest] PASS");
        }

        private static void TestTagHierarchy()
        {
            var exactFirst = CreateNotify("a.b.c");
            var exactSecond = CreateNotify("a.b.c");
            var parent = CreateNotify("a.b");
            var root = CreateNotify("a");
            var index = new GameplayCueTagIndex();
            var resolved = new List<GameplayCueNotifyBase>();

            index.Build(new GameplayCueNotifyBase[] { exactFirst, exactSecond, exactFirst, parent, root });
            index.Resolve("a.b.c", resolved);
            Require(resolved.Count == 4, "hierarchy resolution count");
            Require(resolved[0] == exactFirst && resolved[1] == exactSecond && resolved[2] == parent && resolved[3] == root, "hierarchy resolution order");

            index.Resolve("a.b", resolved);
            Require(resolved.Count == 2 && resolved[0] == parent && resolved[1] == root, "parent must not resolve child notify");

            ExpectArgumentException("");
            ExpectArgumentException(".a");
            ExpectArgumentException("a.");
            ExpectArgumentException("a..b");
            Require(GameplayCueTagIndex.IsValid("a.b") && !GameplayCueTagIndex.IsValid("a..b"), "non-throwing tag validation");

            Destroy(exactFirst, exactSecond, parent, root);
        }

        private static void TestMontageMarkers()
        {
            var montage = AbilityMontage.Create(
                new MontageEventData[]
                {
                    new MontageEventData(0f, 0, "zero", "Event.Zero"),
                    new MontageEventData(0.2f, 2, "second", "Event.Second"),
                    new MontageEventData(0.2f, 1, "first", "Event.First"),
                    new MontageEventData(0.4f, 0, "last", "Event.Last")
                },
                10,
                100,
                1,
                new[] { 2, 3 });

            var emitted = new List<string>();
            montage.GameplayEvent += gameplayEvent => emitted.Add(gameplayEvent.MarkerId);
            montage.Start();
            montage.Advance(0.1f, 0.5f);
            montage.Stop();
            montage.Advance(0.5f, 1f);

            Require(string.Join(",", emitted) == "zero,first,second,last", "montage marker crossing/order/stop");
            ReferencePool.Release(montage);
        }

        private static void TestCueRouting()
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
            var requests = new List<SelfTestCueRequest>();
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

            AbilityCueRouter.Route(
                bindings,
                gameplayEvent,
                id => positions[id],
                (tag, parameters) => requests.Add(new SelfTestCueRequest(tag, parameters)));

            Require(requests.Count == 4, "caster/primary/each request count");
            Require(requests[0].CueTag == "Cue.Caster" && requests[0].Parameters.TargetActorId == 1, "caster policy");
            Require(requests[1].CueTag == "Cue.Primary" && requests[1].Parameters.Location == new Vector3(3f, 1f, 1f), "primary/location policy");
            Require(requests[2].Parameters.TargetActorId == 2 && requests[3].Parameters.TargetActorId == 3, "each target policy");
        }

        private static void TestBinaryContract()
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

            var config = AbilityConfigGenerator.Generate(ability);
            var tracks = new List<TimelineTrackItem> { ability.Tracks[0].ToTrackItem() };
            var exported = AbilityDataExporter.CreateAbilityData(config, tracks);
            Require(exported.MontageEvents[0].EventTag == "Event.Attack.Hit", "montage export flow");
            Require(exported.CueBindings[0].CueTag == "GameplayCue.Attack.Hit.Vfx", "cue export flow");

            var abilityPath = Path.Combine(Path.GetTempPath(), "gameplay-cue-reserved.ablt");
            var invalidMagicPath = Path.Combine(Path.GetTempPath(), "gameplay-cue-invalid-magic.ablt");
            var retiredPath = Path.Combine(Path.GetTempPath(), "gameplay-cue-retired-clip.ablt");
            AbilityBinaryExporter.ExportAbility(exported, abilityPath);
            var abilityBytes = File.ReadAllBytes(abilityPath);
            abilityBytes[4] = 0xA7;
            File.WriteAllBytes(abilityPath, abilityBytes);
            var invalidMagicBytes = (byte[])abilityBytes.Clone();
            invalidMagicBytes[0] = (byte)'X';
            File.WriteAllBytes(invalidMagicPath, invalidMagicBytes);
            WriteRetiredClip(retiredPath);

            var parseAbility = typeof(Aquila.Toolkit.Tools.Ability).GetMethod("ParseAbilityBinary", BindingFlags.NonPublic | BindingFlags.Static);
            var parsed = (AbilityData)parseAbility.Invoke(null, new object[] { abilityBytes, new Dictionary<int, EffectData>() });
            Require(parsed.GetEffects().Count == 1, "Tools.Ability effect roundtrip");
            Require(parsed.GetMontageEvents()[0].EventTag == "Event.Attack.Hit", "Tools.Ability montage roundtrip");
            Require(parsed.GetCueBindings()[0].CueTag == "GameplayCue.Attack.Hit.Vfx", "Tools.Ability VFX cue roundtrip");
            Require(parsed.GetCueBindings()[1].CueTag == "GameplayCue.Attack.Hit.Audio", "Tools.Ability Audio cue roundtrip");
            Require(parsed.GetCueBindings()[0].EventType == GameplayCueEventType.Execute, "Tools.Ability cue event type roundtrip");
            Require(((AbilityData)parseAbility.Invoke(null, new object[] { invalidMagicBytes, new Dictionary<int, EffectData>() })).GetId() == 0, "Tools.Ability invalid magic rejection");
            Require(((AbilityData)parseAbility.Invoke(null, new object[] { File.ReadAllBytes(retiredPath), new Dictionary<int, EffectData>() })).GetId() == 0, "Tools.Ability retired clip rejection");

            var gameObject = new GameObject("AbilityPoolSelfTest");
            var abilityPool = gameObject.AddComponent<Component_AbilityPool>();
            var tryReadAbility = typeof(Component_AbilityPool).GetMethod("TryReadAbility", BindingFlags.NonPublic | BindingFlags.Instance);
            var abilityArgs = new object[] { abilityPath, null };
            Require((bool)tryReadAbility.Invoke(abilityPool, abilityArgs), "Component_AbilityPool read");
            Require(((AbilityData)abilityArgs[1]).GetEffects().Count == 1, "Component_AbilityPool effect roundtrip");
            Require(((AbilityData)abilityArgs[1]).GetCueBindings()[0].CueTag == "GameplayCue.Attack.Hit.Vfx", "Component_AbilityPool VFX cue roundtrip");
            Require(((AbilityData)abilityArgs[1]).GetCueBindings()[1].CueTag == "GameplayCue.Attack.Hit.Audio", "Component_AbilityPool Audio cue roundtrip");
            Require(((AbilityData)abilityArgs[1]).GetCueBindings()[0].EventType == GameplayCueEventType.Execute, "Component_AbilityPool cue event type roundtrip");
            var invalidMagicArgs = new object[] { invalidMagicPath, null };
            Require(!(bool)tryReadAbility.Invoke(abilityPool, invalidMagicArgs), "Component_AbilityPool invalid magic rejection");
            var retiredArgs = new object[] { retiredPath, null };
            Require(!(bool)tryReadAbility.Invoke(abilityPool, retiredArgs), "Component_AbilityPool retired clip rejection");

            Destroy(gameObject, exported, ability);
            File.Delete(abilityPath);
            File.Delete(invalidMagicPath);
            File.Delete(retiredPath);
        }

        private static void TestEffectBinaryContract()
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

            var parseEffect = typeof(Aquila.Toolkit.Tools.Ability).GetMethod("ParseEffectBinary", BindingFlags.NonPublic | BindingFlags.Static);
            var parsed = (EffectData)parseEffect.Invoke(null, new object[] { effectBytes });
            Require(parsed.GetEffectId() == 7002, "Tools.Ability effect reserved byte");
            Require(parsed.GetFormulaID() == 12345, "Tools.Ability effect FormulaID");
            Require(((EffectData)parseEffect.Invoke(null, new object[] { invalidMagicBytes })).GetEffectId() == 0, "Tools.Ability effect invalid magic rejection");

            var gameObject = new GameObject("EffectPoolSelfTest");
            var abilityPool = gameObject.AddComponent<Component_AbilityPool>();
            var tryReadEffect = typeof(Component_AbilityPool).GetMethod("TryReadEffect", BindingFlags.NonPublic | BindingFlags.Instance);
            var effectArgs = new object[] { effectPath, null };
            Require((bool)tryReadEffect.Invoke(abilityPool, effectArgs), "Component_AbilityPool effect read");
            Require(((EffectData)effectArgs[1]).GetFormulaID() == 12345, "Component_AbilityPool effect FormulaID");
            var invalidMagicArgs = new object[] { invalidMagicPath, null };
            Require(!(bool)tryReadEffect.Invoke(abilityPool, invalidMagicArgs), "Component_AbilityPool effect invalid magic rejection");

            Destroy(gameObject);
            File.Delete(effectPath);
            File.Delete(invalidMagicPath);
        }

        private static void TestNotifies()
        {
            var prefab = new GameObject("CueVfxPrefab");
            var vfxNotify = ScriptableObject.CreateInstance<GameplayCueVfxNotify>();
            SetCueTag(vfxNotify, "Cue.Visual");
            var vfxObject = new SerializedObject(vfxNotify);
            vfxObject.FindProperty("_prefab").objectReferenceValue = prefab;
            vfxObject.FindProperty("_lifeTime").floatValue = 0f;
            vfxObject.ApplyModifiedPropertiesWithoutUndo();

            var audioNotify = ScriptableObject.CreateInstance<GameplayCueSelfTestAudioNotify>();
            SetCueTag(audioNotify, "Cue.Visual");
            audioNotify.ResolvedAssetPath = "Assets/Test/Cue.wav";
            var audioObject = new SerializedObject(audioNotify);
            audioObject.FindProperty("_soundEffectId").intValue = 20001;
            audioObject.FindProperty("_soundGroup").stringValue = "Effect";
            audioObject.FindProperty("_volume").floatValue = 0.75f;
            audioObject.ApplyModifiedPropertiesWithoutUndo();

            var throwingNotify = ScriptableObject.CreateInstance<GameplayCueSelfTestThrowingNotify>();
            SetCueTag(throwingNotify, "Cue.Visual");
            var afterFailureNotify = CreateNotify("Cue.Visual");
            var gameObject = new GameObject("GameplayCueSelfTest");
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

            component.ExecuteGameplayCue("Cue.Visual", new GameplayCueParameters { Location = new Vector3(5f, 0f, 0f) });
            var vfxInstance = GameObject.Find("CueVfxPrefab(Clone)");
            Require(vfxInstance != null, "VFX one-shot execution");
            Require(audioNotify.PlayCount == 1 && audioNotify.ResolvedSoundEffectId == 20001 && audioNotify.AssetPath == "Assets/Test/Cue.wav" && audioNotify.Location == new Vector3(5f, 0f, 0f), "Audio one-shot execution");
            Require(afterFailureNotify.ExecuteCount == 1, "presentation failure isolation");

            component.ExecuteGameplayCue("Cue..Visual", new GameplayCueParameters { AbilityId = 7, ActivationId = 9 });
            Require(afterFailureNotify.ExecuteCount == 1, "invalid cue tag isolation");

            Destroy(vfxInstance, gameObject, vfxNotify, audioNotify, throwingNotify, afterFailureNotify, prefab);
        }

        private static void TestPhysicalAttackConfiguration()
        {
            const string abilityPath = "Assets/AbilityEditor/Editor/Config/Ability/1000.asset";
            const string notifyPath = "Assets/Res/GameplayCue/Skill/PhysicalAttackHitAudio.asset";
            const string audioPath = "Assets/Res/Audio/Fight/hitted.mp3";
            var ability = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(abilityPath);
            var notify = AssetDatabase.LoadAssetAtPath<GameplayCueAudioNotify>(notifyPath);
            Require(ability != null && notify != null, "physical attack source assets");
            Require(
                ability.MontageEvents.Count == 1 &&
                ability.MontageEvents[0].Time == 2f &&
                ability.MontageEvents[0].Sequence == 0 &&
                ability.MontageEvents[0].MarkerId == "physical_attack_hit" &&
                ability.MontageEvents[0].EventTag == "Event.Ability.PhysicalAttack.Hit",
                "physical attack montage event");
            Require(
                ability.CueBindings.Count == 1 &&
                ability.CueBindings[0].EventTag == "Event.Ability.PhysicalAttack.Hit" &&
                ability.CueBindings[0].CueTag == "GameplayCue.Ability.PhysicalAttack.Hit" &&
                ability.CueBindings[0].EventType == GameplayCueEventType.Execute &&
                ability.CueBindings[0].TargetPolicy == GameplayCueTargetPolicy.PrimaryTarget &&
                ability.CueBindings[0].LocationPolicy == GameplayCueLocationPolicy.Target &&
                ability.CueBindings[0].Magnitude == 1f &&
                ability.CueBindings[0].LocationOffset == Vector3.zero,
                "physical attack cue binding");

            var notifyObject = new SerializedObject(notify);
            Require(
                notify.CueTag == "GameplayCue.Ability.PhysicalAttack.Hit" &&
                notifyObject.FindProperty("_soundEffectId").intValue == 20001 &&
                notifyObject.FindProperty("_soundGroup").stringValue == "Effect" &&
                notifyObject.FindProperty("_volume").floatValue == 1f,
                "physical attack audio notify");

            var soundEffectMap = new Cfg.Common.SoundEffectMap(
                new Bright.Serialization.ByteBuf(File.ReadAllBytes("Assets/Res/DataTables/common_soundeffectmap.bytes")));
            var soundEffect = soundEffectMap.GetOrDefault(20001);
            Require(soundEffect != null && soundEffect.asset_path == audioPath, "SoundEffectMap 20001");
            Require(AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath) != null, "physical attack audio asset");
            Require(
                Array.IndexOf(AssetDatabase.GetDependencies("Assets/Res/Scene/Start.unity", true), notifyPath) >= 0 &&
                Array.IndexOf(AssetDatabase.GetDependencies("Assets/AbilityEditor/AbilityEditorEntry.unity", true), notifyPath) >= 0,
                "physical attack notify scene registration");

            var parseAbility = typeof(Aquila.Toolkit.Tools.Ability).GetMethod(
                "ParseAbilityBinary",
                BindingFlags.NonPublic | BindingFlags.Static);
            var abilityPoolObject = new GameObject("PhysicalAttackAbilityPoolSelfTest");
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
                var parsed = (AbilityData)parseAbility.Invoke(
                    null,
                    new object[] { File.ReadAllBytes(binaryPath), new Dictionary<int, EffectData>() });
                RequirePhysicalAttackCueData(parsed, $"Tools.Ability {binaryPath}");
                var readArgs = new object[] { binaryPath, null };
                Require((bool)tryReadAbility.Invoke(abilityPool, readArgs), $"Component_AbilityPool read {binaryPath}");
                RequirePhysicalAttackCueData((AbilityData)readArgs[1], $"Component_AbilityPool {binaryPath}");
            }

            var simulatedNotify = ScriptableObject.CreateInstance<GameplayCueSelfTestAudioNotify>();
            simulatedNotify.ResolvedAssetPath = soundEffect.asset_path;
            var simulatedObject = new SerializedObject(simulatedNotify);
            simulatedObject.FindProperty("_soundEffectId").intValue = 20001;
            simulatedObject.FindProperty("_soundGroup").stringValue = "Effect";
            simulatedObject.FindProperty("_volume").floatValue = 1f;
            simulatedObject.ApplyModifiedPropertiesWithoutUndo();
            var montage = AbilityMontage.Create(ability.MontageEvents, ability.Id, 10, 1, new[] { 2 });
            montage.GameplayEvent += gameplayEvent => AbilityCueRouter.Route(
                ability.CueBindings,
                gameplayEvent,
                actorId => actorId == 1 ? Vector3.zero : new Vector3(3f, 4f, 5f),
                (cueTag, parameters) =>
                {
                    Require(cueTag == "GameplayCue.Ability.PhysicalAttack.Hit", "physical attack routed cue tag");
                    simulatedNotify.Execute(parameters);
                });
            montage.Start();
            montage.Advance(0f, 1.9f);
            Require(simulatedNotify.PlayCount == 0, "physical attack cue before marker");
            montage.Advance(1.9f, 2.1f);
            montage.Advance(2.1f, 5f);
            Require(
                simulatedNotify.PlayCount == 1 &&
                simulatedNotify.ResolvedSoundEffectId == 20001 &&
                simulatedNotify.AssetPath == audioPath &&
                simulatedNotify.Location == new Vector3(3f, 4f, 5f),
                "physical attack cue simulation");

            ReferencePool.Release(montage);
            Destroy(simulatedNotify, abilityPoolObject);
        }

        private static void RequirePhysicalAttackCueData(AbilityData ability, string source)
        {
            Require(ability.GetId() == 1000, $"{source} ability id");
            Require(
                ability.GetMontageEvents().Count == 1 &&
                ability.GetMontageEvents()[0].Time == 2f &&
                ability.GetMontageEvents()[0].Sequence == 0 &&
                ability.GetMontageEvents()[0].MarkerId == "physical_attack_hit" &&
                ability.GetMontageEvents()[0].EventTag == "Event.Ability.PhysicalAttack.Hit",
                $"{source} montage event");
            Require(
                ability.GetCueBindings().Count == 1 &&
                ability.GetCueBindings()[0].EventTag == "Event.Ability.PhysicalAttack.Hit" &&
                ability.GetCueBindings()[0].CueTag == "GameplayCue.Ability.PhysicalAttack.Hit" &&
                ability.GetCueBindings()[0].EventType == GameplayCueEventType.Execute &&
                ability.GetCueBindings()[0].TargetPolicy == GameplayCueTargetPolicy.PrimaryTarget &&
                ability.GetCueBindings()[0].LocationPolicy == GameplayCueLocationPolicy.Target &&
                ability.GetCueBindings()[0].Magnitude == 1f &&
                ability.GetCueBindings()[0].LocationOffset == Vector3.zero,
                $"{source} cue binding");
        }

        private static GameplayCueSelfTestNotify CreateNotify(string cueTag)
        {
            var notify = ScriptableObject.CreateInstance<GameplayCueSelfTestNotify>();
            SetCueTag(notify, cueTag);
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

        private static void SetCueTag(GameplayCueNotifyBase notify, string cueTag)
        {
            var serialized = new SerializedObject(notify);
            serialized.FindProperty("_cueTag").stringValue = cueTag;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ExpectArgumentException(string cueTag)
        {
            try
            {
                GameplayCueTagIndex.Validate(cueTag);
            }
            catch (ArgumentException)
            {
                return;
            }

            throw new InvalidOperationException($"Expected invalid tag rejection: '{cueTag}'");
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"GameplayCue self test failed: {message}");
        }

        private static void Destroy(params UnityEngine.Object[] objects)
        {
            for (var i = 0; i < objects.Length; i++)
            {
                if (objects[i] != null)
                    UnityEngine.Object.DestroyImmediate(objects[i]);
            }
        }

        private readonly struct SelfTestCueRequest
        {
            public SelfTestCueRequest(string cueTag, GameplayCueParameters parameters)
            {
                CueTag = cueTag;
                Parameters = parameters;
            }

            public string CueTag { get; }
            public GameplayCueParameters Parameters { get; }
        }
    }

    public sealed class GameplayCueSelfTestNotify : GameplayCueNotifyBase
    {
        public int ExecuteCount { get; private set; }

        public override void Execute(in GameplayCueParameters parameters)
        {
            ExecuteCount++;
        }
    }

    public sealed class GameplayCueSelfTestThrowingNotify : GameplayCueNotifyBase
    {
        public override void Execute(in GameplayCueParameters parameters)
        {
            throw new InvalidOperationException("simulated presentation failure");
        }
    }

    public sealed class GameplayCueSelfTestAudioNotify : GameplayCueAudioNotify
    {
        public int PlayCount { get; private set; }
        public int ResolvedSoundEffectId { get; private set; }
        public string ResolvedAssetPath { get; set; }
        public string AssetPath { get; private set; }
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
            Location = location;
        }
    }
}
