using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquila.AbilityEditor;
using Aquila.AbilityEditor.Config;
using Aquila.Fight;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Tools
{
    /// <summary>
    /// AbilityData 组装验证工具
    /// 读取生产环境的 .ablt 和 .efct 文件，组装成 AbilityData 结构，
    /// 与编辑器 SO 数据进行比较，同时交叉验证 .ablt 内联 effect 数据与独立 .efct 数据的一致性
    /// </summary>
    public static class AbilityDataAssemblyVerifier
    {
        private const float FLOAT_TOLERANCE = 0.0001f;

        /// <summary>
        /// 执行组装验证流程
        /// </summary>
        public static AssemblyVerificationResult VerifyAssembly(string logPath)
        {
            var result = new AssemblyVerificationResult();

            try
            {
                var editorAbilities = LoadEditorAbilities();
                var editorEffects = LoadEditorEffects();

                if (editorAbilities.Count == 0)
                {
                    result.ErrorMessage = "No editor ability data found";
                    return result;
                }

                var productionEffects = ReadProductionEffects();
                foreach (var kvp in editorAbilities)
                {
                    int abilityId = kvp.Key;
                    var editorAbility = kvp.Value;
                    result.TotalAbilities++;

                    string abltFile = Path.Combine(Misc.ABILITY_BIN_ASSET_PATH, $"{abilityId}.ablt");

                    if (!File.Exists(abltFile))
                    {
                        result.Failures.Add(new AssemblyFailure
                        {
                            AbilityId = abilityId,
                            ErrorMessage = $"Production .ablt file not found: {abltFile}"
                        });
                        continue;
                    }

                    try
                    {
                        var prodAbility = AbilityVerificationTool.ReadBinaryFile(abltFile);
                        var differences = new List<string>();

                        // 验证基础 ability 字段
                        CompareBasicAbilityFields(editorAbility, prodAbility, differences);

                        // 验证 track/clip 结构并交叉检查 effect 数据
                        VerifyTracksAndCrossCheck(editorAbility, prodAbility, productionEffects, editorEffects, differences);

                        // 第 3 步：组装 AbilityData
                        var assembled = AssembleAbilityData(prodAbility, productionEffects);

                        // 第 4 步：用组装后的 AbilityData 与编辑器 SO 比较
                        CompareAssembledWithEditor(assembled, editorAbility, differences);

                        if (differences.Count > 0)
                        {
                            result.Failures.Add(new AssemblyFailure
                            {
                                AbilityId = abilityId,
                                Differences = differences
                            });
                        }
                        else
                        {
                            result.SuccessfulAbilities.Add(abilityId);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Failures.Add(new AssemblyFailure
                        {
                            AbilityId = abilityId,
                            ErrorMessage = $"Failed to verify assembly: {ex.Message}"
                        });
                    }
                }

                GenerateReport(result, logPath);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"Assembly verification failed: {ex.Message}\n{ex.StackTrace}";
                Aquila.Toolkit.Tools.Logger.Error($"[AbilityDataAssemblyVerifier] {result.ErrorMessage}");
            }

            return result;
        }

        #region Data Loading

        private static Dictionary<int, AbilityEditorSOData> LoadEditorAbilities()
        {
            var map = new Dictionary<int, AbilityEditorSOData>();
            string[] guids = AssetDatabase.FindAssets("t:AbilityEditorSOData", new[] { Misc.ABILITY_ASSET_BASE_PATH });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var data = AssetDatabase.LoadAssetAtPath<AbilityEditorSOData>(path);
                if (data != null && data.Id > 0)
                    map[data.Id] = data;
            }

            return map;
        }

        private static Dictionary<int, EffectEditorSOData> LoadEditorEffects()
        {
            var map = new Dictionary<int, EffectEditorSOData>();
            string[] guids = AssetDatabase.FindAssets("t:EffectEditorSOData", new[] { Misc.EFFECT_ASSET_BASE_PATH });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var data = AssetDatabase.LoadAssetAtPath<EffectEditorSOData>(path);
                if (data != null && data.id > 0)
                    map[data.id] = data;
            }

            return map;
        }

        private static Dictionary<int, EffectVerificationTool.CachedEffectData> ReadProductionEffects()
        {
            var map = new Dictionary<int, EffectVerificationTool.CachedEffectData>();
            string efctDir = Misc.EFFECT_BIN_ASSET_PATH;

            if (!Directory.Exists(efctDir))
            {
                Aquila.Toolkit.Tools.Logger.Warning($"[AbilityDataAssemblyVerifier] Production effect directory not found: {efctDir}");
                return map;
            }

            foreach (string file in Directory.GetFiles(efctDir, "*.efct"))
            {
                try
                {
                    var data = EffectVerificationTool.ReadBinaryFile(file);
                    map[data.Id] = data;
                }
                catch (Exception ex)
                {
                    Aquila.Toolkit.Tools.Logger.Warning($"[AbilityDataAssemblyVerifier] Failed to read .efct: {file} - {ex.Message}");
                }
            }

            Aquila.Toolkit.Tools.Logger.Info($"[AbilityDataAssemblyVerifier] Loaded {map.Count} production .efct files");
            return map;
        }

        #endregion

        #region Assembly

        /// <summary>
        /// 从生产 .ablt 数据 + .efct 字典组装成运行时 AbilityData 结构体
        /// clip 级字段（startTime, endTime, stackCount, canStack）来自 .ablt，
        /// effect 配置字段来自 .efct（找不到时回退使用 .ablt 内联数据）
        /// </summary>
        private static AbilityData AssembleAbilityData(
            AbilityVerificationTool.CachedAbilityData prodAbility,
            Dictionary<int, EffectVerificationTool.CachedEffectData> productionEffects)
        {
            var effectDataList = new List<EffectData>();

            if (prodAbility.Tracks != null)
            {
                foreach (var track in prodAbility.Tracks)
                {
                    if (track.Clips == null) continue;
                    foreach (var clip in track.Clips)
                    {
                        if (!(clip is AbilityVerificationTool.CachedEffectClipData effectClip))
                            continue;

                        EffectData effectData;
                        if (productionEffects.TryGetValue(effectClip.EffectId, out var standaloneEffect))
                        {
                            effectData = new EffectData(
                                effectId: effectClip.EffectId,
                                stackLimit: effectClip.StackCount,
                                canStack: effectClip.CanStack,
                                startTime: effectClip.StartTime,
                                endTime: effectClip.EndTime,
                                effectType: standaloneEffect.Type,
                                modifierType: standaloneEffect.ModifierType,
                                affectedAttribute: standaloneEffect.EffectType,
                                target: standaloneEffect.Target,
                                duration: standaloneEffect.Duration,
                                period: standaloneEffect.Period,
                                policy: standaloneEffect.Policy,
                                effectOnAwake: standaloneEffect.EffectOnAwake,
                                deriveEffects: standaloneEffect.DeriveEffects,
                                awakeEffects: standaloneEffect.AwakeEffects,
                                floatParam1: standaloneEffect.Float1,
                                floatParam2: standaloneEffect.Float2,
                                floatParam3: standaloneEffect.Float3,
                                floatParam4: standaloneEffect.Float4,
                                intParam1: standaloneEffect.Int1,
                                intParam2: standaloneEffect.Int2,
                                intParam3: standaloneEffect.Int3,
                                intParam4: standaloneEffect.Int4);
                        }
                        else
                        {
                            effectData = new EffectData(
                                effectId: effectClip.EffectId,
                                stackLimit: effectClip.StackCount,
                                canStack: effectClip.CanStack,
                                startTime: effectClip.StartTime,
                                endTime: effectClip.EndTime,
                                effectType: effectClip.EffectType,
                                modifierType: effectClip.ModifierType,
                                affectedAttribute: effectClip.AffectedAttribute,
                                target: effectClip.Target,
                                duration: effectClip.Duration,
                                period: effectClip.Period,
                                policy: effectClip.Policy,
                                effectOnAwake: effectClip.EffectOnAwake,
                                deriveEffects: effectClip.DeriveEffects,
                                awakeEffects: effectClip.AwakeEffects,
                                floatParam1: effectClip.FloatParam1,
                                floatParam2: effectClip.FloatParam2,
                                floatParam3: effectClip.FloatParam3,
                                floatParam4: effectClip.FloatParam4,
                                intParam1: effectClip.IntParam1,
                                intParam2: effectClip.IntParam2,
                                intParam3: effectClip.IntParam3,
                                intParam4: effectClip.IntParam4);
                        }

                        effectDataList.Add(effectData);
                    }
                }
            }

            return new AbilityData(
                id: prodAbility.Id,
                costEffectID: prodAbility.CostEffectID,
                coolDownEffectID: prodAbility.CoolDownEffectID,
                targetType: prodAbility.TargetType,
                timelineID: prodAbility.TimelineID,
                timelineDuration: prodAbility.TimelineDuration,
                effects: effectDataList.ToArray());
        }

        #endregion

        #region Assembly Comparison

        /// <summary>
        /// 用组装好的 AbilityData 与编辑器 SO 数据逐字段比较
        /// </summary>
        private static void CompareAssembledWithEditor(
            AbilityData assembled,
            AbilityEditorSOData editor,
            List<string> differences)
        {
            if (assembled.GetId() != editor.Id)
                differences.Add($"[AbilityData] Id | Assembled: {assembled.GetId()} | Editor: {editor.Id}");

            if (assembled.GetCostEffectID() != editor.CostEffectID)
                differences.Add($"[AbilityData] CostEffectID | Assembled: {assembled.GetCostEffectID()} | Editor: {editor.CostEffectID}");

            if (assembled.GetCoolDownEffectID() != editor.CoolDownEffectID)
                differences.Add($"[AbilityData] CoolDownEffectID | Assembled: {assembled.GetCoolDownEffectID()} | Editor: {editor.CoolDownEffectID}");

            if (assembled.GetTargetType() != editor.TargetType)
                differences.Add($"[AbilityData] TargetType | Assembled: {assembled.GetTargetType()} | Editor: {editor.TargetType}");

            if (assembled.GetTimelineID() != editor.TimelineID)
                differences.Add($"[AbilityData] TimelineID | Assembled: {assembled.GetTimelineID()} | Editor: {editor.TimelineID}");

            if (!FloatEquals(assembled.GetTimelineDuration(), editor.TimelineDuration))
                differences.Add($"[AbilityData] TimelineDuration | Assembled: {assembled.GetTimelineDuration()} | Editor: {editor.TimelineDuration}");

            // 收集编辑器中所有 EffectClipData
            var editorEffectClips = new List<EffectClipData>();
            if (editor.Tracks != null)
            {
                foreach (var track in editor.Tracks)
                {
                    if (track.Clips == null) continue;
                    foreach (var clip in track.Clips)
                    {
                        if (clip is EffectClipData efc)
                            editorEffectClips.Add(efc);
                    }
                }
            }

            var assembledEffects = assembled.GetEffects();
            int assembledCount = assembledEffects?.Count ?? 0;

            if (assembledCount != editorEffectClips.Count)
            {
                differences.Add($"[AbilityData] EffectData Count | Assembled: {assembledCount} | Editor: {editorEffectClips.Count}");
                return;
            }

            for (int i = 0; i < assembledCount; i++)
            {
                var ae = assembledEffects[i];
                var ee = editorEffectClips[i];
                string prefix = $"[AbilityData] Effect[{i}]";

                if (ae.GetEffectId() != ee.EffectId)
                    differences.Add($"{prefix} EffectId | Assembled: {ae.GetEffectId()} | Editor: {ee.EffectId}");

                if (ae.GetStackLimit() != ee.StackCount)
                    differences.Add($"{prefix} StackCount | Assembled: {ae.GetStackLimit()} | Editor: {ee.StackCount}");

                if (ae.GetCanStack() != ee.CanStack)
                    differences.Add($"{prefix} CanStack | Assembled: {ae.GetCanStack()} | Editor: {ee.CanStack}");

                if (!FloatEquals(ae.GetStartTime(), ee.StartTime))
                    differences.Add($"{prefix} StartTime | Assembled: {ae.GetStartTime()} | Editor: {ee.StartTime}");

                if (!FloatEquals(ae.GetEndTime(), ee.EndTime))
                    differences.Add($"{prefix} EndTime | Assembled: {ae.GetEndTime()} | Editor: {ee.EndTime}");

                if (ae.GetEffectType() != ee.EffectType)
                    differences.Add($"{prefix} EffectType | Assembled: {ae.GetEffectType()} | Editor: {ee.EffectType}");

                if (ae.GetModifierType() != ee.ModifierType)
                    differences.Add($"{prefix} ModifierType | Assembled: {ae.GetModifierType()} | Editor: {ee.ModifierType}");

                if (ae.GetAffectedAttribute() != ee.AffectedAttribute)
                    differences.Add($"{prefix} AffectedAttribute | Assembled: {ae.GetAffectedAttribute()} | Editor: {ee.AffectedAttribute}");

                if (ae.GetTarget() != ee.Target)
                    differences.Add($"{prefix} Target | Assembled: {ae.GetTarget()} | Editor: {ee.Target}");

                if (!FloatEquals(ae.GetDuration(), ee.Duration))
                    differences.Add($"{prefix} Duration | Assembled: {ae.GetDuration()} | Editor: {ee.Duration}");

                if (!FloatEquals(ae.GetPeriod(), ee.Period))
                    differences.Add($"{prefix} Period | Assembled: {ae.GetPeriod()} | Editor: {ee.Period}");

                if (ae.GetPolicy() != ee.Policy)
                    differences.Add($"{prefix} Policy | Assembled: {ae.GetPolicy()} | Editor: {ee.Policy}");

                if (ae.GetEffectOnAwake() != ee.EffectOnAwake)
                    differences.Add($"{prefix} EffectOnAwake | Assembled: {ae.GetEffectOnAwake()} | Editor: {ee.EffectOnAwake}");

                if (ee.ExtensionParam != null)
                {
                    if (!FloatEquals(ae.GetFloatParam1(), ee.ExtensionParam.FloatParam_1))
                        differences.Add($"{prefix} FloatParam1 | Assembled: {ae.GetFloatParam1()} | Editor: {ee.ExtensionParam.FloatParam_1}");

                    if (!FloatEquals(ae.GetFloatParam2(), ee.ExtensionParam.FloatParam_2))
                        differences.Add($"{prefix} FloatParam2 | Assembled: {ae.GetFloatParam2()} | Editor: {ee.ExtensionParam.FloatParam_2}");

                    if (!FloatEquals(ae.GetFloatParam3(), ee.ExtensionParam.FloatParam_3))
                        differences.Add($"{prefix} FloatParam3 | Assembled: {ae.GetFloatParam3()} | Editor: {ee.ExtensionParam.FloatParam_3}");

                    if (!FloatEquals(ae.GetFloatParam4(), ee.ExtensionParam.FloatParam_4))
                        differences.Add($"{prefix} FloatParam4 | Assembled: {ae.GetFloatParam4()} | Editor: {ee.ExtensionParam.FloatParam_4}");

                    if (ae.GetIntParam1() != ee.ExtensionParam.IntParam_1)
                        differences.Add($"{prefix} IntParam1 | Assembled: {ae.GetIntParam1()} | Editor: {ee.ExtensionParam.IntParam_1}");

                    if (ae.GetIntParam2() != ee.ExtensionParam.IntParam_2)
                        differences.Add($"{prefix} IntParam2 | Assembled: {ae.GetIntParam2()} | Editor: {ee.ExtensionParam.IntParam_2}");

                    if (ae.GetIntParam3() != ee.ExtensionParam.IntParam_3)
                        differences.Add($"{prefix} IntParam3 | Assembled: {ae.GetIntParam3()} | Editor: {ee.ExtensionParam.IntParam_3}");

                    if (ae.GetIntParam4() != ee.ExtensionParam.IntParam_4)
                        differences.Add($"{prefix} IntParam4 | Assembled: {ae.GetIntParam4()} | Editor: {ee.ExtensionParam.IntParam_4}");
                }

                // 比较数组需要从 IReadOnlyList 转为 int[]
                int[] assembledDerive = ReadOnlyListToArray(ae.GetDeriveEffects());
                int[] assembledAwake = ReadOnlyListToArray(ae.GetAwakeEffects());

                if (!ArrayEquals(assembledDerive, ee.DeriveEffects))
                    differences.Add($"{prefix} DeriveEffects | Assembled: [{FormatArray(assembledDerive)}] | Editor: [{FormatArray(ee.DeriveEffects)}]");

                if (!ArrayEquals(assembledAwake, ee.AwakeEffects))
                    differences.Add($"{prefix} AwakeEffects | Assembled: [{FormatArray(assembledAwake)}] | Editor: [{FormatArray(ee.AwakeEffects)}]");
            }
        }

        #endregion

        #region Comparison

        private static void CompareBasicAbilityFields(
            AbilityEditorSOData editor,
            AbilityVerificationTool.CachedAbilityData prod,
            List<string> differences)
        {
            if (editor.Id != prod.Id)
                differences.Add($"[Assembly] Id | Editor: {editor.Id} | Production: {prod.Id}");

            if (editor.CostEffectID != prod.CostEffectID)
                differences.Add($"[Assembly] CostEffectID | Editor: {editor.CostEffectID} | Production: {prod.CostEffectID}");

            if (editor.CoolDownEffectID != prod.CoolDownEffectID)
                differences.Add($"[Assembly] CoolDownEffectID | Editor: {editor.CoolDownEffectID} | Production: {prod.CoolDownEffectID}");

            if (editor.TargetType != prod.TargetType)
                differences.Add($"[Assembly] TargetType | Editor: {editor.TargetType} | Production: {prod.TargetType}");

            if (editor.TimelineID != prod.TimelineID)
                differences.Add($"[Assembly] TimelineID | Editor: {editor.TimelineID} | Production: {prod.TimelineID}");

            if (!FloatEquals(editor.TimelineDuration, prod.TimelineDuration))
                differences.Add($"[Assembly] TimelineDuration | Editor: {editor.TimelineDuration} | Production: {prod.TimelineDuration}");
        }

        private static void VerifyTracksAndCrossCheck(
            AbilityEditorSOData editor,
            AbilityVerificationTool.CachedAbilityData prod,
            Dictionary<int, EffectVerificationTool.CachedEffectData> productionEffects,
            Dictionary<int, EffectEditorSOData> editorEffects,
            List<string> differences)
        {
            int editorTrackCount = editor.Tracks?.Count ?? 0;
            int prodTrackCount = prod.Tracks?.Count ?? 0;

            if (editorTrackCount != prodTrackCount)
            {
                differences.Add($"[Assembly] Track Count | Editor: {editorTrackCount} | Production: {prodTrackCount}");
                return;
            }

            for (int t = 0; t < editorTrackCount; t++)
            {
                var editorTrack = editor.Tracks[t];
                var prodTrack = prod.Tracks[t];

                int editorClipCount = editorTrack.Clips?.Count ?? 0;
                int prodClipCount = prodTrack.Clips?.Count ?? 0;

                if (editorClipCount != prodClipCount)
                {
                    differences.Add($"[Assembly] Track[{t}] Clip Count | Editor: {editorClipCount} | Production: {prodClipCount}");
                    continue;
                }

                for (int c = 0; c < editorClipCount; c++)
                {
                    var editorClip = editorTrack.Clips[c];
                    var prodClip = prodTrack.Clips[c];
                    string prefix = $"[Assembly] Track[{t}] Clip[{c}]";

                    if (editorClip.ClipType != prodClip.ClipType)
                    {
                        differences.Add($"{prefix} ClipType mismatch | Editor: {editorClip.ClipType} | Production: {prodClip.ClipType}");
                        continue;
                    }

                    if (editorClip is EffectClipData editorEffect &&
                        prodClip is AbilityVerificationTool.CachedEffectClipData prodEffect)
                    {
                        CompareEffectClipWithProduction(editorEffect, prodEffect, prefix, differences);
                        CrossCheckWithStandaloneEffect(prodEffect, productionEffects, editorEffects, prefix, differences);
                    }
                    else if (editorClip is AudioClipData editorAudio &&
                             prodClip is AbilityVerificationTool.CachedAudioClipData prodAudio)
                    {
                        CompareAudioClipWithProduction(editorAudio, prodAudio, prefix, differences);
                    }
                    else if (editorClip is VFXClipData editorVfx &&
                             prodClip is AbilityVerificationTool.CachedVFXClipData prodVfx)
                    {
                        CompareVfxClipWithProduction(editorVfx, prodVfx, prefix, differences);
                    }
                }
            }
        }

        private static void CompareEffectClipWithProduction(
            EffectClipData editor,
            AbilityVerificationTool.CachedEffectClipData prod,
            string prefix,
            List<string> differences)
        {
            if (!FloatEquals(editor.StartTime, prod.StartTime))
                differences.Add($"{prefix} StartTime | Editor: {editor.StartTime} | Production: {prod.StartTime}");

            if (!FloatEquals(editor.EndTime, prod.EndTime))
                differences.Add($"{prefix} EndTime | Editor: {editor.EndTime} | Production: {prod.EndTime}");

            if (editor.EffectId != prod.EffectId)
                differences.Add($"{prefix} EffectId | Editor: {editor.EffectId} | Production: {prod.EffectId}");

            if (editor.StackCount != prod.StackCount)
                differences.Add($"{prefix} StackCount | Editor: {editor.StackCount} | Production: {prod.StackCount}");

            if (editor.CanStack != prod.CanStack)
                differences.Add($"{prefix} CanStack | Editor: {editor.CanStack} | Production: {prod.CanStack}");

            if (editor.EffectType != prod.EffectType)
                differences.Add($"{prefix} EffectType | Editor: {editor.EffectType} | Production: {prod.EffectType}");

            if (editor.ModifierType != prod.ModifierType)
                differences.Add($"{prefix} ModifierType | Editor: {editor.ModifierType} | Production: {prod.ModifierType}");

            if (editor.AffectedAttribute != prod.AffectedAttribute)
                differences.Add($"{prefix} AffectedAttribute | Editor: {editor.AffectedAttribute} | Production: {prod.AffectedAttribute}");

            if (editor.Target != prod.Target)
                differences.Add($"{prefix} Target | Editor: {editor.Target} | Production: {prod.Target}");

            if (!FloatEquals(editor.Duration, prod.Duration))
                differences.Add($"{prefix} Duration | Editor: {editor.Duration} | Production: {prod.Duration}");

            if (!FloatEquals(editor.Period, prod.Period))
                differences.Add($"{prefix} Period | Editor: {editor.Period} | Production: {prod.Period}");

            if (editor.Policy != prod.Policy)
                differences.Add($"{prefix} Policy | Editor: {editor.Policy} | Production: {prod.Policy}");

            if (editor.EffectOnAwake != prod.EffectOnAwake)
                differences.Add($"{prefix} EffectOnAwake | Editor: {editor.EffectOnAwake} | Production: {prod.EffectOnAwake}");

            // Extension params
            if (editor.ExtensionParam != null)
            {
                if (!FloatEquals(editor.ExtensionParam.FloatParam_1, prod.FloatParam1))
                    differences.Add($"{prefix} FloatParam1 | Editor: {editor.ExtensionParam.FloatParam_1} | Production: {prod.FloatParam1}");

                if (!FloatEquals(editor.ExtensionParam.FloatParam_2, prod.FloatParam2))
                    differences.Add($"{prefix} FloatParam2 | Editor: {editor.ExtensionParam.FloatParam_2} | Production: {prod.FloatParam2}");

                if (!FloatEquals(editor.ExtensionParam.FloatParam_3, prod.FloatParam3))
                    differences.Add($"{prefix} FloatParam3 | Editor: {editor.ExtensionParam.FloatParam_3} | Production: {prod.FloatParam3}");

                if (!FloatEquals(editor.ExtensionParam.FloatParam_4, prod.FloatParam4))
                    differences.Add($"{prefix} FloatParam4 | Editor: {editor.ExtensionParam.FloatParam_4} | Production: {prod.FloatParam4}");

                if (editor.ExtensionParam.IntParam_1 != prod.IntParam1)
                    differences.Add($"{prefix} IntParam1 | Editor: {editor.ExtensionParam.IntParam_1} | Production: {prod.IntParam1}");

                if (editor.ExtensionParam.IntParam_2 != prod.IntParam2)
                    differences.Add($"{prefix} IntParam2 | Editor: {editor.ExtensionParam.IntParam_2} | Production: {prod.IntParam2}");

                if (editor.ExtensionParam.IntParam_3 != prod.IntParam3)
                    differences.Add($"{prefix} IntParam3 | Editor: {editor.ExtensionParam.IntParam_3} | Production: {prod.IntParam3}");

                if (editor.ExtensionParam.IntParam_4 != prod.IntParam4)
                    differences.Add($"{prefix} IntParam4 | Editor: {editor.ExtensionParam.IntParam_4} | Production: {prod.IntParam4}");
            }

            // Arrays
            if (!ArrayEquals(editor.DeriveEffects, prod.DeriveEffects))
                differences.Add($"{prefix} DeriveEffects | Editor: [{FormatArray(editor.DeriveEffects)}] | Production: [{FormatArray(prod.DeriveEffects)}]");

            if (!ArrayEquals(editor.AwakeEffects, prod.AwakeEffects))
                differences.Add($"{prefix} AwakeEffects | Editor: [{FormatArray(editor.AwakeEffects)}] | Production: [{FormatArray(prod.AwakeEffects)}]");
        }

        /// <summary>
        /// 交叉验证：.ablt 内联 effect 数据 vs 独立 .efct 文件数据 vs 编辑器 EffectEditorSOData
        /// </summary>
        private static void CrossCheckWithStandaloneEffect(
            AbilityVerificationTool.CachedEffectClipData inlineEffect,
            Dictionary<int, EffectVerificationTool.CachedEffectData> productionEffects,
            Dictionary<int, EffectEditorSOData> editorEffects,
            string prefix,
            List<string> differences)
        {
            int effectId = inlineEffect.EffectId;
            string crossPrefix = $"{prefix} [CrossCheck EffectId={effectId}]";

            // 交叉检查 .ablt inline vs .efct standalone
            if (productionEffects.TryGetValue(effectId, out var standaloneEffect))
            {
                if (inlineEffect.EffectType != standaloneEffect.Type)
                    differences.Add($"{crossPrefix} EffectType | .ablt: {inlineEffect.EffectType} | .efct: {standaloneEffect.Type}");

                if (inlineEffect.ModifierType != standaloneEffect.ModifierType)
                    differences.Add($"{crossPrefix} ModifierType | .ablt: {inlineEffect.ModifierType} | .efct: {standaloneEffect.ModifierType}");

                if (inlineEffect.AffectedAttribute != standaloneEffect.EffectType)
                    differences.Add($"{crossPrefix} AffectedAttribute | .ablt: {inlineEffect.AffectedAttribute} | .efct: {standaloneEffect.EffectType}");

                if (inlineEffect.Target != standaloneEffect.Target)
                    differences.Add($"{crossPrefix} Target | .ablt: {inlineEffect.Target} | .efct: {standaloneEffect.Target}");

                if (!FloatEquals(inlineEffect.Duration, standaloneEffect.Duration))
                    differences.Add($"{crossPrefix} Duration | .ablt: {inlineEffect.Duration} | .efct: {standaloneEffect.Duration}");

                if (!FloatEquals(inlineEffect.Period, standaloneEffect.Period))
                    differences.Add($"{crossPrefix} Period | .ablt: {inlineEffect.Period} | .efct: {standaloneEffect.Period}");

                if (inlineEffect.Policy != standaloneEffect.Policy)
                    differences.Add($"{crossPrefix} Policy | .ablt: {inlineEffect.Policy} | .efct: {standaloneEffect.Policy}");

                if (inlineEffect.EffectOnAwake != standaloneEffect.EffectOnAwake)
                    differences.Add($"{crossPrefix} EffectOnAwake | .ablt: {inlineEffect.EffectOnAwake} | .efct: {standaloneEffect.EffectOnAwake}");

                if (!FloatEquals(inlineEffect.FloatParam1, standaloneEffect.Float1))
                    differences.Add($"{crossPrefix} Float1 | .ablt: {inlineEffect.FloatParam1} | .efct: {standaloneEffect.Float1}");

                if (!FloatEquals(inlineEffect.FloatParam2, standaloneEffect.Float2))
                    differences.Add($"{crossPrefix} Float2 | .ablt: {inlineEffect.FloatParam2} | .efct: {standaloneEffect.Float2}");

                if (!FloatEquals(inlineEffect.FloatParam3, standaloneEffect.Float3))
                    differences.Add($"{crossPrefix} Float3 | .ablt: {inlineEffect.FloatParam3} | .efct: {standaloneEffect.Float3}");

                if (!FloatEquals(inlineEffect.FloatParam4, standaloneEffect.Float4))
                    differences.Add($"{crossPrefix} Float4 | .ablt: {inlineEffect.FloatParam4} | .efct: {standaloneEffect.Float4}");

                if (inlineEffect.IntParam1 != standaloneEffect.Int1)
                    differences.Add($"{crossPrefix} Int1 | .ablt: {inlineEffect.IntParam1} | .efct: {standaloneEffect.Int1}");

                if (inlineEffect.IntParam2 != standaloneEffect.Int2)
                    differences.Add($"{crossPrefix} Int2 | .ablt: {inlineEffect.IntParam2} | .efct: {standaloneEffect.Int2}");

                if (inlineEffect.IntParam3 != standaloneEffect.Int3)
                    differences.Add($"{crossPrefix} Int3 | .ablt: {inlineEffect.IntParam3} | .efct: {standaloneEffect.Int3}");

                if (inlineEffect.IntParam4 != standaloneEffect.Int4)
                    differences.Add($"{crossPrefix} Int4 | .ablt: {inlineEffect.IntParam4} | .efct: {standaloneEffect.Int4}");

                if (!ArrayEquals(inlineEffect.DeriveEffects, standaloneEffect.DeriveEffects))
                    differences.Add($"{crossPrefix} DeriveEffects | .ablt: [{FormatArray(inlineEffect.DeriveEffects)}] | .efct: [{FormatArray(standaloneEffect.DeriveEffects)}]");

                if (!ArrayEquals(inlineEffect.AwakeEffects, standaloneEffect.AwakeEffects))
                    differences.Add($"{crossPrefix} AwakeEffects | .ablt: [{FormatArray(inlineEffect.AwakeEffects)}] | .efct: [{FormatArray(standaloneEffect.AwakeEffects)}]");
            }
            else
            {
                differences.Add($"{crossPrefix} Production .efct file not found for EffectId={effectId}");
            }

            // 交叉检查 .efct standalone vs 编辑器 EffectEditorSOData
            if (editorEffects.TryGetValue(effectId, out var editorEffect))
            {
                if (productionEffects.TryGetValue(effectId, out var prodEffect))
                {
                    string editorPrefix = $"{prefix} [EditorEffect EffectId={effectId}]";

                    if (editorEffect.Type != prodEffect.Type)
                        differences.Add($"{editorPrefix} Type | EditorSO: {editorEffect.Type} | .efct: {prodEffect.Type}");

                    if (editorEffect.ModifierType != prodEffect.ModifierType)
                        differences.Add($"{editorPrefix} ModifierType | EditorSO: {editorEffect.ModifierType} | .efct: {prodEffect.ModifierType}");

                    if (editorEffect.EffectOnAwake != prodEffect.EffectOnAwake)
                        differences.Add($"{editorPrefix} EffectOnAwake | EditorSO: {editorEffect.EffectOnAwake} | .efct: {prodEffect.EffectOnAwake}");

                    if (editorEffect.Policy != prodEffect.Policy)
                        differences.Add($"{editorPrefix} Policy | EditorSO: {editorEffect.Policy} | .efct: {prodEffect.Policy}");

                    if (!FloatEquals(editorEffect.Period, prodEffect.Period))
                        differences.Add($"{editorPrefix} Period | EditorSO: {editorEffect.Period} | .efct: {prodEffect.Period}");

                    if (!FloatEquals(editorEffect.Duration, prodEffect.Duration))
                        differences.Add($"{editorPrefix} Duration | EditorSO: {editorEffect.Duration} | .efct: {prodEffect.Duration}");

                    if (editorEffect.Target != prodEffect.Target)
                        differences.Add($"{editorPrefix} Target | EditorSO: {editorEffect.Target} | .efct: {prodEffect.Target}");

                    if (editorEffect.AffectedAttribute != prodEffect.EffectType)
                        differences.Add($"{editorPrefix} EffectType(Attribute) | EditorSO: {editorEffect.AffectedAttribute} | .efct: {prodEffect.EffectType}");

                    if (editorEffect.ExtensionParam != null)
                    {
                        if (!FloatEquals(editorEffect.ExtensionParam.float_1, prodEffect.Float1))
                            differences.Add($"{editorPrefix} Float1 | EditorSO: {editorEffect.ExtensionParam.float_1} | .efct: {prodEffect.Float1}");

                        if (!FloatEquals(editorEffect.ExtensionParam.float_2, prodEffect.Float2))
                            differences.Add($"{editorPrefix} Float2 | EditorSO: {editorEffect.ExtensionParam.float_2} | .efct: {prodEffect.Float2}");

                        if (!FloatEquals(editorEffect.ExtensionParam.float_3, prodEffect.Float3))
                            differences.Add($"{editorPrefix} Float3 | EditorSO: {editorEffect.ExtensionParam.float_3} | .efct: {prodEffect.Float3}");

                        if (!FloatEquals(editorEffect.ExtensionParam.float_4, prodEffect.Float4))
                            differences.Add($"{editorPrefix} Float4 | EditorSO: {editorEffect.ExtensionParam.float_4} | .efct: {prodEffect.Float4}");

                        if (editorEffect.ExtensionParam.int_1 != prodEffect.Int1)
                            differences.Add($"{editorPrefix} Int1 | EditorSO: {editorEffect.ExtensionParam.int_1} | .efct: {prodEffect.Int1}");

                        if (editorEffect.ExtensionParam.int_2 != prodEffect.Int2)
                            differences.Add($"{editorPrefix} Int2 | EditorSO: {editorEffect.ExtensionParam.int_2} | .efct: {prodEffect.Int2}");

                        if (editorEffect.ExtensionParam.int_3 != prodEffect.Int3)
                            differences.Add($"{editorPrefix} Int3 | EditorSO: {editorEffect.ExtensionParam.int_3} | .efct: {prodEffect.Int3}");

                        if (editorEffect.ExtensionParam.int_4 != prodEffect.Int4)
                            differences.Add($"{editorPrefix} Int4 | EditorSO: {editorEffect.ExtensionParam.int_4} | .efct: {prodEffect.Int4}");
                    }

                    if (!ArrayEquals(editorEffect.DeriveEffects, prodEffect.DeriveEffects))
                        differences.Add($"{editorPrefix} DeriveEffects | EditorSO: [{FormatArray(editorEffect.DeriveEffects)}] | .efct: [{FormatArray(prodEffect.DeriveEffects)}]");

                    if (!ArrayEquals(editorEffect.AwakeEffects, prodEffect.AwakeEffects))
                        differences.Add($"{editorPrefix} AwakeEffects | EditorSO: [{FormatArray(editorEffect.AwakeEffects)}] | .efct: [{FormatArray(prodEffect.AwakeEffects)}]");
                }
            }
            else
            {
                differences.Add($"{prefix} [EditorEffect] Editor EffectEditorSOData not found for EffectId={effectId}");
            }
        }

        private static void CompareAudioClipWithProduction(
            AudioClipData editor,
            AbilityVerificationTool.CachedAudioClipData prod,
            string prefix,
            List<string> differences)
        {
            if (!FloatEquals(editor.StartTime, prod.StartTime))
                differences.Add($"{prefix} (Audio) StartTime | Editor: {editor.StartTime} | Production: {prod.StartTime}");

            if (!FloatEquals(editor.EndTime, prod.EndTime))
                differences.Add($"{prefix} (Audio) EndTime | Editor: {editor.EndTime} | Production: {prod.EndTime}");

            if (editor.AudioPath != prod.AudioPath)
                differences.Add($"{prefix} (Audio) AudioPath | Editor: {editor.AudioPath} | Production: {prod.AudioPath}");

            if (!FloatEquals(editor.Volume, prod.Volume))
                differences.Add($"{prefix} (Audio) Volume | Editor: {editor.Volume} | Production: {prod.Volume}");

            if (editor.Loop != prod.Loop)
                differences.Add($"{prefix} (Audio) Loop | Editor: {editor.Loop} | Production: {prod.Loop}");

            if (!FloatEquals(editor.FadeInDuration, prod.FadeInDuration))
                differences.Add($"{prefix} (Audio) FadeInDuration | Editor: {editor.FadeInDuration} | Production: {prod.FadeInDuration}");

            if (!FloatEquals(editor.FadeOutDuration, prod.FadeOutDuration))
                differences.Add($"{prefix} (Audio) FadeOutDuration | Editor: {editor.FadeOutDuration} | Production: {prod.FadeOutDuration}");
        }

        private static void CompareVfxClipWithProduction(
            VFXClipData editor,
            AbilityVerificationTool.CachedVFXClipData prod,
            string prefix,
            List<string> differences)
        {
            if (!FloatEquals(editor.StartTime, prod.StartTime))
                differences.Add($"{prefix} (VFX) StartTime | Editor: {editor.StartTime} | Production: {prod.StartTime}");

            if (!FloatEquals(editor.EndTime, prod.EndTime))
                differences.Add($"{prefix} (VFX) EndTime | Editor: {editor.EndTime} | Production: {prod.EndTime}");

            if (editor.VfxPath != prod.VfxPath)
                differences.Add($"{prefix} (VFX) VfxPath | Editor: {editor.VfxPath} | Production: {prod.VfxPath}");

            if (editor.AttachPoint != prod.AttachPoint)
                differences.Add($"{prefix} (VFX) AttachPoint | Editor: {editor.AttachPoint} | Production: {prod.AttachPoint}");

            if (!Vector3Equals(editor.PositionOffset, prod.PositionOffset))
                differences.Add($"{prefix} (VFX) PositionOffset | Editor: {editor.PositionOffset} | Production: {prod.PositionOffset}");

            if (!Vector3Equals(editor.RotationOffset, prod.RotationOffset))
                differences.Add($"{prefix} (VFX) RotationOffset | Editor: {editor.RotationOffset} | Production: {prod.RotationOffset}");

            if (!Vector3Equals(editor.Scale, prod.Scale))
                differences.Add($"{prefix} (VFX) Scale | Editor: {editor.Scale} | Production: {prod.Scale}");

            if (editor.FollowAttachPoint != prod.FollowAttachPoint)
                differences.Add($"{prefix} (VFX) FollowAttachPoint | Editor: {editor.FollowAttachPoint} | Production: {prod.FollowAttachPoint}");
        }

        #endregion

        #region Utility

        private static bool FloatEquals(float a, float b)
        {
            return Mathf.Abs(a - b) < FLOAT_TOLERANCE;
        }

        private static bool Vector3Equals(Vector3 a, Vector3 b)
        {
            return FloatEquals(a.x, b.x) && FloatEquals(a.y, b.y) && FloatEquals(a.z, b.z);
        }

        private static bool ArrayEquals(int[] a, int[] b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        private static string FormatArray(int[] arr)
        {
            if (arr == null) return "";
            return string.Join(", ", arr);
        }

        private static int[] ReadOnlyListToArray(IReadOnlyList<int> list)
        {
            if (list == null) return new int[0];
            var arr = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
                arr[i] = list[i];
            return arr;
        }

        #endregion

        #region Report

        private static void GenerateReport(AssemblyVerificationResult result, string logPath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("========== AbilityData Assembly Verification Report ==========");
            sb.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine($"Total Abilities: {result.TotalAbilities}");
            sb.AppendLine($"Verified: {result.SuccessfulAbilities.Count}");
            sb.AppendLine($"Failed: {result.Failures.Count}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                sb.AppendLine($"[ERROR] {result.ErrorMessage}");
                sb.AppendLine();
            }

            foreach (var failure in result.Failures)
            {
                sb.AppendLine($"[FAILED] Ability ID: {failure.AbilityId}");

                if (!string.IsNullOrEmpty(failure.ErrorMessage))
                    sb.AppendLine($"  Error: {failure.ErrorMessage}");

                foreach (var diff in failure.Differences)
                    sb.AppendLine($"  {diff}");

                sb.AppendLine();
            }

            if (result.SuccessfulAbilities.Count > 0)
                sb.AppendLine($"[SUCCESS] Ability IDs: {string.Join(", ", result.SuccessfulAbilities)}");

            sb.AppendLine("========== End of Assembly Verification Report ==========");

            string reportContent = sb.ToString();

            EnsureDirectoryExists(Path.GetDirectoryName(logPath));
            File.AppendAllText(logPath, "\n\n" + reportContent);

            Aquila.Toolkit.Tools.Logger.Info($"[AbilityDataAssemblyVerifier] Report:\n{reportContent}");
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        #endregion

        #region Data Structures

        public class AssemblyVerificationResult
        {
            public int TotalAbilities;
            public List<int> SuccessfulAbilities = new List<int>();
            public List<AssemblyFailure> Failures = new List<AssemblyFailure>();
            public string ErrorMessage;

            public bool IsSuccess => Failures.Count == 0 && string.IsNullOrEmpty(ErrorMessage);
        }

        public class AssemblyFailure
        {
            public int AbilityId;
            public string ErrorMessage;
            public List<string> Differences = new List<string>();
        }

        #endregion
    }
}
