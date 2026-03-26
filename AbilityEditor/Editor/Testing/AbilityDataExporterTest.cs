using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Aquila.AbilityEditor;
using Editor.AbilityEditor.Config;
using Cfg.Enum;

namespace Editor.AbilityEditor.Testing
{
    // AbilityDataExporter 测试工具
    public static class AbilityDataExporterTest
    {
        [MenuItem("Aquila/AbilityEditor/Testing/测试资产导出 - 创建新资产")]
        public static void TestExportNew()
        {
            Debug.Log("[测试] 开始测试创建新资产...");

            var config = CreateTestConfig(999, "测试技能_新建");
            var tracks = CreateTestTracks();

            AbilityDataExporter.ExportToAsset(config, tracks);

            Debug.Log("[测试] 测试完成! 请在 Assets/AbilityEditor/Editor/Config/Ability/ 下查看 10001.asset");
        }

        [MenuItem("Aquila/AbilityEditor/Testing/测试资产导出 - 覆盖已有资产")]
        public static void TestExportOverwrite()
        {
            Debug.Log("[测试] 开始测试覆盖已有资产...");

            var config = CreateTestConfig(10001, "测试技能_覆盖版本");
            config.Desc = "这是覆盖后的描述信息";
            var tracks = CreateTestTracks();

            AbilityDataExporter.ExportToAsset(config, tracks);

            Debug.Log("[测试] 测试完成! 请在 Inspector 中查看 10001.asset 的数据是否更新");
        }

        // 创建测试用的 AbilityConfig
        private static AbilityConfig CreateTestConfig(int id, string name)
        {
            var config = new AbilityConfig
            {
                AbilityID = id,
                Name = name,
                Desc = "这是一个测试技能描述",
                CostEffectID = 1001,
                CoolDownEffectID = 1002,
                TargetType = AbilityTargetType.Enemy,
                TimelineID = 5001,
                TimelineDuration = 3.5f
            };

            return config;
        }

        // 创建测试用的 Tracks
        private static List<TimelineTrackItem> CreateTestTracks()
        {
            var tracks = new List<TimelineTrackItem>();

            // Track 1: Effect Track
            var effectTrack = new TimelineTrackItem("Effect Track", Color.red, true);
            var effectClip1 = new EffectClipData("伤害效果", 1.0f, 1002);
            var effectClip2 = new EffectClipData("治疗效果", 2.0f, 1002);
            effectTrack.AddClip(effectClip1);
            effectTrack.AddClip(effectClip2);
            tracks.Add(effectTrack);

            // Track 2: Audio Track
            // var audioTrack = new TimelineTrackItem("Audio Track", Color.blue, true);
            // var audioClip = new AudioClipData("技能音效", 0.5f, 2.5f, "audio/skill_cast");
            // audioTrack.AddClip(audioClip);
            // tracks.Add(audioTrack);

            // Track 3: VFX Track
            // var vfxTrack = new TimelineTrackItem("VFX Track", Color.yellow, true);
            // var vfxClip = new VFXClipData("技能特效", 1.5f, 2.5f, "vfx/skill_impact");
            // vfxTrack.AddClip(vfxClip);
            // tracks.Add(vfxTrack);

            return tracks;
        }
    }
}
