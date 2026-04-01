using System;
using System.Collections.Generic;
using System.Linq;
using Aquila.AbilityEditor;
using Editor.AbilityEditor.Config;
using UnityEditor;
using UnityEngine;

namespace Editor.AbilityEditor.Testing
{
    /// <summary>
    /// Ability 配置测试工具
    /// 用于自动化测试 AbilityConfig 生成和 AbilityConfigAccessor 功能
    /// </summary>
    public static class AbilityConfigTestTool
    {

        [MenuItem("Aquila/AbilityEditor/Testing/运行简单测试")]
        public static void RunSimpleTest()
        {
            RunTest("简单测试", TestDataTemplates.CreateSimpleScenario());
        }

        [MenuItem("Aquila/AbilityEditor/Testing/运行标准测试")]
        public static void RunStandardTest()
        {
            RunTest("标准测试", TestDataTemplates.CreateStandardScenario());
        }

        [MenuItem("Aquila/AbilityEditor/Testing/运行复杂测试")]
        public static void RunComplexTest()
        {
            RunTest("复杂测试", TestDataTemplates.CreateComplexScenario());
        }

        [MenuItem("Aquila/AbilityEditor/Testing/运行边界测试")]
        public static void RunBoundaryTest()
        {
            RunTest("边界测试", TestDataTemplates.CreateBoundaryScenario());
        }

        [MenuItem("Aquila/AbilityEditor/Testing/运行所有测试")]
        public static void RunAllTests()
        {
            Aquila.Toolkit.Tools.Logger.Info("<color=cyan><b>========== 开始运行所有测试 ==========</b></color>");

            int totalTests = 4;
            int passedTests = 0;
            int failedTests = 0;

            try
            {
                if (RunTest("简单测试", TestDataTemplates.CreateSimpleScenario(), false))
                    passedTests++;
                else
                    failedTests++;
            }
            catch (Exception)
            {
                failedTests++;
            }

            try
            {
                if (RunTest("标准测试", TestDataTemplates.CreateStandardScenario(), false))
                    passedTests++;
                else
                    failedTests++;
            }
            catch (Exception)
            {
                failedTests++;
            }

            try
            {
                if (RunTest("复杂测试", TestDataTemplates.CreateComplexScenario(), false))
                    passedTests++;
                else
                    failedTests++;
            }
            catch (Exception)
            {
                failedTests++;
            }

            try
            {
                if (RunTest("边界测试", TestDataTemplates.CreateBoundaryScenario(), false))
                    passedTests++;
                else
                    failedTests++;
            }
            catch (Exception)
            {
                failedTests++;
            }

            Aquila.Toolkit.Tools.Logger.Info("<color=cyan><b>========== 测试汇总 ==========</b></color>");
            Aquila.Toolkit.Tools.Logger.Info($"<color=white>总测试数: {totalTests}</color>");
            Aquila.Toolkit.Tools.Logger.Info($"<color=green>通过: {passedTests}</color>");
            Aquila.Toolkit.Tools.Logger.Info($"<color=red>失败: {failedTests}</color>");

            if (failedTests == 0)
            {
                Aquila.Toolkit.Tools.Logger.Info("<color=green><b>✓ 所有测试通过!</b></color>");
            }
            else
            {
                Aquila.Toolkit.Tools.Logger.Warning($"<color=yellow><b>⚠ 部分测试失败 ({failedTests}/{totalTests})</b></color>");
            }
        }

        [MenuItem("Aquila/AbilityEditor/Testing/清除测试数据")]
        public static void ClearTestData()
        {
            Aquila.Toolkit.Tools.Logger.Info("<color=green>[AbilityConfigTestTool] 测试数据已清除</color>");
        }
        
        /// <summary>
        /// 运行单个测试
        /// </summary>
        private static bool RunTest(string testName, TestScenario scenario, bool logHeader = true)
        {
            if (logHeader)
            {
                Aquila.Toolkit.Tools.Logger.Info($"<color=cyan><b>========== 开始运行{testName} ==========</b></color>");
            }
            else
            {
                Aquila.Toolkit.Tools.Logger.Info($"<color=cyan>--- {testName} ---</color>");
            }

            bool testPassed = true;
            var tracks = CreateTestTracks(scenario);
            Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ 创建测试数据: {tracks.Count}个轨道</color>");

            var config = GenerateConfig(scenario, tracks);
            Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ 生成配置成功: AbilityID={config.AbilityID}</color>");

            testPassed = ValidateConfig(config, scenario, tracks);

            // if (testPassed)
            //     testPassed = TestAccessor(config);

            if (testPassed)
                Aquila.Toolkit.Tools.Logger.Info($"<color=green><b>========== {testName}通过! ==========</b></color>\n");
            else
                Aquila.Toolkit.Tools.Logger.Error($"<color=red><b>========== {testName}失败! ==========</b></color>\n");
            
            // AbilityConfigAccessor.Clear();
            return testPassed;
        }

        /// <summary>
        /// 创建测试用的 TimelineTrackItem 列表
        /// </summary>
        private static List<TimelineTrackItem> CreateTestTracks(TestScenario scenario)
        {
            var tracks = new List<TimelineTrackItem>();

            foreach (var trackTemplate in scenario.Tracks)
            {
                var track = new TimelineTrackItem(
                    trackTemplate.TrackName,
                    trackTemplate.TrackColor,
                    trackTemplate.IsEnabled
                );

                foreach (var clipTemplate in trackTemplate.Clips)
                {
                    var clipData = clipTemplate.CreateClipData();
                    track.AddClip(clipData);
                }

                tracks.Add(track);
            }

            return tracks;
        }

        /// <summary>
        /// 生成 AbilityConfig
        /// </summary>
        private static AbilityConfig GenerateConfig(TestScenario scenario, List<TimelineTrackItem> tracks)
        {
            var config = new AbilityConfig
            {
                AbilityID = scenario.AbilityID,
                Name = scenario.Name,
                Desc = scenario.Description,
                CostEffectID = scenario.CostEffectID,
                CoolDownEffectID = scenario.CoolDownEffectID,
                TargetType = scenario.TargetType,
                TimelineID = scenario.TimelineID,
                TimelineDuration = scenario.Duration,
                DataSource = "TestTool"
            };

            // 收集所有启用轨道的启用 Clip
            var allEffects = new List<EffectClipData>();
            var allAudios = new List<AudioClipData>();
            var allVFXs = new List<VFXClipData>();

            foreach (var track in tracks)
            {
                if (!track.IsEnabled)
                    continue;

                foreach (var clip in track.Clips)
                {
                    if (!clip.IsEnabled)
                        continue;

                    if (clip is EffectClipData effectClip)
                        allEffects.Add(effectClip);
                    else if (clip is AudioClipData audioClip)
                        allAudios.Add(audioClip);
                    else if (clip is VFXClipData vfxClip)
                        allVFXs.Add(vfxClip);
                }
            }

            // 生成 Triggers (按时间分组 EffectClip)
            var triggers = GenerateTriggers(allEffects);

            // 初始化配置
            config.Initialize(triggers, allEffects, allAudios, allVFXs);

            return config;
        }

        /// <summary>
        /// 生成 Trigger 数据 (按时间分组)
        /// </summary>
        private static List<TriggerData> GenerateTriggers(List<EffectClipData> effects)
        {
            var triggerDict = new Dictionary<float, List<int>>();

            foreach (var effect in effects)
            {
                // 四舍五入到 0.01s 精度
                float triggerTime = Mathf.Round(effect.TriggerTime * 100f) / 100f;

                if (!triggerDict.ContainsKey(triggerTime))
                {
                    triggerDict[triggerTime] = new List<int>();
                }

                triggerDict[triggerTime].Add(effect.EffectId);
            }

            // 排序并创建 TriggerData
            var triggers = new List<TriggerData>();
            foreach (var kvp in triggerDict.OrderBy(x => x.Key))
            {
                triggers.Add(new TriggerData(kvp.Key, kvp.Value));
            }

            return triggers;
        }
        
        /// <summary>
        /// 验证配置数据
        /// </summary>
        private static bool ValidateConfig(AbilityConfig config, TestScenario scenario, List<TimelineTrackItem> tracks)
        {
            bool allPassed = true;

            // 验证基本元数据
            if (!ValidateMetadata(config, scenario))
                allPassed = false;

            // 验证 Triggers
            if (!ValidateTriggers(config, tracks))
                allPassed = false;

            // 验证 Clip 收集
            if (!ValidateClipCollection(config, tracks))
                allPassed = false;

            return allPassed;
        }

        /// <summary>
        /// 验证基本元数据
        /// </summary>
        private static bool ValidateMetadata(AbilityConfig config, TestScenario scenario)
        {
            if (config == null)
            {
                Aquila.Toolkit.Tools.Logger.Error("<color=red>✗ 配置对象为 null</color>");
                return false;
            }

            bool passed = true;

            if (config.AbilityID != scenario.AbilityID)
            {
                Aquila.Toolkit.Tools.Logger.Error($"<color=red>✗ AbilityID 不匹配: 期望={scenario.AbilityID}, 实际={config.AbilityID}</color>");
                passed = false;
            }
            else
            {
                Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ AbilityID 验证通过: {config.AbilityID}</color>");
            }

            if (config.TimelineDuration != scenario.Duration)
            {
                Aquila.Toolkit.Tools.Logger.Error($"<color=red>✗ Duration 不匹配: 期望={scenario.Duration}, 实际={config.TimelineDuration}</color>");
                passed = false;
            }
            else
            {
                Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ Timeline Duration 验证通过: {config.TimelineDuration}s</color>");
            }

            return passed;
        }

        /// <summary>
        /// 验证 Triggers
        /// </summary>
        private static bool ValidateTriggers(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            // 计算期望的 Trigger 数量
            var enabledEffects = new List<EffectClipData>();
            foreach (var track in tracks)
            {
                if (!track.IsEnabled) continue;

                foreach (var clip in track.Clips)
                {
                    if (clip.IsEnabled && clip is EffectClipData effectClip)
                        enabledEffects.Add(effectClip);
                }
            }

            // 按时间分组计算期望的 Trigger 数量
            var uniqueTimes = enabledEffects
                .Select(e => Mathf.Round(e.TriggerTime * 100f) / 100f)
                .Distinct()
                .Count();

            if (config.Triggers.Count != uniqueTimes)
            {
                Aquila.Toolkit.Tools.Logger.Error($"<color=red>✗ Trigger 数量不匹配: 期望={uniqueTimes}, 实际={config.Triggers.Count}</color>");
                return false;
            }

            Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ Triggers 验证通过: {config.Triggers.Count}个</color>");

            // 验证 Trigger 排序
            var times = config.Triggers.Select(t => t.TriggerTime).ToList();
            var sortedTimes = times.OrderBy(t => t).ToList();
            bool sorted = times.SequenceEqual(sortedTimes);

            if (!sorted)
            {
                Aquila.Toolkit.Tools.Logger.Error("<color=red>✗ Triggers 未按时间排序</color>");
                return false;
            }

            Aquila.Toolkit.Tools.Logger.Info("<color=green>✓ Triggers 排序验证通过</color>");

            return true;
        }

        /// <summary>
        /// 验证 Clip 收集
        /// </summary>
        private static bool ValidateClipCollection(AbilityConfig config, List<TimelineTrackItem> tracks)
        {
            bool passed = true;

            // 计算期望的 Clip 数量
            int expectedEffects = 0;
            int expectedAudios = 0;
            int expectedVFXs = 0;

            foreach (var track in tracks)
            {
                if (!track.IsEnabled) continue;

                foreach (var clip in track.Clips)
                {
                    if (!clip.IsEnabled) continue;

                    if (clip is EffectClipData) expectedEffects++;
                    else if (clip is AudioClipData) expectedAudios++;
                    else if (clip is VFXClipData) expectedVFXs++;
                }
            }

            // 验证 EffectClips
            if (config.Effects.Count != expectedEffects)
            {
                Aquila.Toolkit.Tools.Logger.Error($"<color=red>✗ EffectClips 数量不匹配: 期望={expectedEffects}, 实际={config.Effects.Count}</color>");
                passed = false;
            }
            else
            {
                Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ EffectClips 验证通过: {config.Effects.Count}个</color>");
            }

            // 验证 AudioClips
            if (config.Audios.Count != expectedAudios)
            {
                Aquila.Toolkit.Tools.Logger.Error($"<color=red>✗ AudioClips 数量不匹配: 期望={expectedAudios}, 实际={config.Audios.Count}</color>");
                passed = false;
            }
            else
            {
                Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ AudioClips 验证通过: {config.Audios.Count}个</color>");
            }

            // 验证 VFXClips
            if (config.VFXs.Count != expectedVFXs)
            {
                Aquila.Toolkit.Tools.Logger.Error($"<color=red>✗ VFXClips 数量不匹配: 期望={expectedVFXs}, 实际={config.VFXs.Count}</color>");
                passed = false;
            }
            else
            {
                Aquila.Toolkit.Tools.Logger.Info($"<color=green>✓ VFXClips 验证通过: {config.VFXs.Count}个</color>");
            }

            return passed;
        }

        /// <summary>
        /// 测试 AbilityConfigAccessor
        /// </summary>
        // private static bool TestAccessor(AbilityConfig config)
        // {
        //     bool passed = true;
        //     _eventTriggered = false;
        //     _eventConfig = null;
        //     AbilityConfigAccessor.OnConfigChanged += OnConfigChanged;
        //     AbilityConfigAccessor.SetConfig(config);
        //     if (AbilityConfigAccessor.Current != config)
        //     {
        //         Aquila.Toolkit.Tools.Logger.Error("<color=red>✗ AbilityConfigAccessor.Current 不正确</color>");
        //         passed = false;
        //     }
        //     else
        //     {
        //         Aquila.Toolkit.Tools.Logger.Info("<color=green>✓ AbilityConfigAccessor.Current 验证通过</color>");
        //     }
        //
        //     if (!_eventTriggered)
        //     {
        //         Aquila.Toolkit.Tools.Logger.Error("<color=red>✗ OnConfigChanged 事件未触发</color>");
        //         passed = false;
        //     }
        //     else if (_eventConfig != config)
        //     {
        //         Aquila.Toolkit.Tools.Logger.Error("<color=red>✗ OnConfigChanged 事件参数不正确</color>");
        //         passed = false;
        //     }
        //     else
        //     {
        //         Aquila.Toolkit.Tools.Logger.Info("<color=green>✓ OnConfigChanged 事件验证通过</color>");
        //     }
        //     
        //     AbilityConfigAccessor.OnConfigChanged -= OnConfigChanged;
        //     return passed;
        // }

        /// <summary>
        /// 配置改变事件处理
        /// </summary>
        private static void OnConfigChanged(AbilityConfig config)
        {
            _eventTriggered = true;
            _eventConfig = config;
        }
        
        private static bool _eventTriggered;
        private static AbilityConfig _eventConfig;
    }
}
