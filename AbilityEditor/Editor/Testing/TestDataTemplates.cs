using System.Collections.Generic;
using Aquila.AbilityEditor;
using Cfg.Enum;
using UnityEngine;

namespace Editor.AbilityEditor.Testing
{
    /// <summary>
    /// 测试场景数据结构
    /// </summary>
    public class TestScenario
    {
        public TestScenario()
        {
            Tracks = new List<TrackTemplate>();
        }
        
        public int AbilityID;
        public string Name;
        public string Description;
        public int CostEffectID;
        public int CoolDownEffectID;
        public AbilityTargetType TargetType;
        public int TimelineID;
        public float Duration;
        public List<TrackTemplate> Tracks;
    }

    /// <summary>
    /// 轨道模板数据
    /// </summary>
    public class TrackTemplate
    {
        public string TrackName;
        public Color TrackColor;
        public bool IsEnabled;
        public List<ClipTemplate> Clips;

        public TrackTemplate(string name, Color color, bool enabled = true)
        {
            TrackName = name;
            TrackColor = color;
            IsEnabled = enabled;
            Clips = new List<ClipTemplate>();
        }
    }

    /// <summary>
    /// Clip 模板数据基类
    /// </summary>
    public abstract class ClipTemplate
    {
        public string ClipName;
        public float StartTime;
        public bool IsEnabled;

        protected ClipTemplate(string name, float startTime, bool enabled = true)
        {
            ClipName = name;
            StartTime = startTime;
            IsEnabled = enabled;
        }

        /// <summary>
        /// 创建实际的 ClipData 对象
        /// </summary>
        public abstract TimelineClipData CreateClipData();
    }

    /// <summary>
    /// Effect Clip 模板
    /// </summary>
    public class EffectClipTemplate : ClipTemplate
    {
        public int EffectID;
        public EffectType EffectType;
        public NumricModifierType ModifierType;
        public actor_attribute AffectedAttribute;
        public int Target;
        public float Duration;

        public EffectClipTemplate(string name, float startTime, int effectId, bool enabled = true)
            : base(name, startTime, enabled)
        {
            EffectID = effectId;
            EffectType = EffectType.Instant_Cost;
            ModifierType = NumricModifierType.Sum;
            AffectedAttribute = actor_attribute.Curr_HP;
            Target = 0;
            Duration = -1f;
        }

        public override TimelineClipData CreateClipData()
        {
            var clip = new EffectClipData(ClipName, StartTime, EffectID)
            {
                IsEnabled = IsEnabled,
                EffectType = EffectType,
                ModifierType = ModifierType,
                AffectedAttribute = AffectedAttribute,
                Target = Target,
                Duration = Duration
            };
            return clip;
        }
    }

    /// <summary>
    /// Audio Clip 模板
    /// </summary>
    public class AudioClipTemplate : ClipTemplate
    {
        public int AudioId;

        public AudioClipTemplate(string name, float startTime, int audioId = 0, bool enabled = true)
            : base(name, startTime, enabled)
        {
            AudioId = audioId;
        }

        public override TimelineClipData CreateClipData()
        {
            var clip = new AudioClipData(ClipName, StartTime, StartTime + 1f, AudioId)
            {
                IsEnabled = IsEnabled
            };
            return clip;
        }
    }

    /// <summary>
    /// VFX Clip 模板
    /// </summary>
    public class VFXClipTemplate : ClipTemplate
    {
        public string VFXPath;

        public VFXClipTemplate(string name, float startTime, string vfxPath = "", bool enabled = true)
            : base(name, startTime, enabled)
        {
            VFXPath = vfxPath;
        }

        public override TimelineClipData CreateClipData()
        {
            var clip = new VFXClipData(ClipName, StartTime, StartTime + 1f, VFXPath)
            {
                IsEnabled = IsEnabled,
                VfxPath = VFXPath
            };
            return clip;
        }
    }

    /// <summary>
    /// 测试数据模板库
    /// </summary>
    public static class TestDataTemplates
    {
        /// <summary>
        /// 创建简单测试场景
        /// - 1个轨道
        /// - 2个 EffectClip (0.5s, 1.0s)
        /// </summary>
        public static TestScenario CreateSimpleScenario()
        {
            var scenario = new TestScenario
            {
                AbilityID = 10001,
                Name = "简单测试技能",
                Description = "用于基本功能测试",
                CostEffectID = 1001,
                CoolDownEffectID = 1002,
                TargetType = AbilityTargetType.Enemy,
                TimelineID = 1,
                Duration = 5.0f
            };

            var track1 = new TrackTemplate("effect track", new Color(0.8f, 0.4f, 0.8f));
            track1.Clips.Add(new EffectClipTemplate("dmg effect 1", 0.5f, 2001));
            track1.Clips.Add(new EffectClipTemplate("dmg effect 2", 1.0f, 2002));

            scenario.Tracks.Add(track1);

            return scenario;
        }

        /// <summary>
        /// 创建标准测试场景
        /// - 3个轨道
        /// - 包含 Effect, Audio, VFX 多种类型
        /// </summary>
        public static TestScenario CreateStandardScenario()
        {
            var scenario = new TestScenario
            {
                AbilityID = 10002,
                Name = "标准测试技能",
                Description = "测试多轨道和多类型 Clip",
                CostEffectID = 1001,
                CoolDownEffectID = 1002,
                TargetType = AbilityTargetType.Enemy,
                TimelineID = 2,
                Duration = 5.0f
            };

            // Track 1: Effect Clips
            var track1 = new TrackTemplate("效果轨道", new Color(0.8f, 0.4f, 0.8f));
            track1.Clips.Add(new EffectClipTemplate("伤害效果1", 0.5f, 2001));
            track1.Clips.Add(new EffectClipTemplate("伤害效果2", 1.5f, 2002));

            // Track 2: Audio Clip
            var track2 = new TrackTemplate("音效轨道", new Color(0.4f, 0.8f, 0.8f));
            track2.Clips.Add(new AudioClipTemplate("攻击音效", 1.0f, 0));

            // Track 3: VFX Clip
            var track3 = new TrackTemplate("特效轨道", new Color(0.8f, 0.8f, 0.4f));
            track3.Clips.Add(new VFXClipTemplate("爆炸特效", 2.0f, "VFX/Explosion_01"));

            scenario.Tracks.Add(track1);
            scenario.Tracks.Add(track2);
            scenario.Tracks.Add(track3);

            return scenario;
        }

        /// <summary>
        /// 创建复杂测试场景
        /// - 5个轨道
        /// - 大量 Clip,包含相同时间点的多个 Effect (测试 Trigger 合并)
        /// </summary>
        public static TestScenario CreateComplexScenario()
        {
            var scenario = new TestScenario
            {
                AbilityID = 10003,
                Name = "复杂测试技能",
                Description = "测试大数据量和 Trigger 合并",
                CostEffectID = 1001,
                CoolDownEffectID = 1002,
                TargetType = AbilityTargetType.Enemy,
                TimelineID = 3,
                Duration = 10.0f
            };

            // Track 1: 多个 Effect (包含相同时间点)
            var track1 = new TrackTemplate("主效果轨道", new Color(0.8f, 0.4f, 0.8f));
            track1.Clips.Add(new EffectClipTemplate("伤害1", 0.5f, 2001));
            track1.Clips.Add(new EffectClipTemplate("伤害2", 0.5f, 2002)); // 相同时间
            track1.Clips.Add(new EffectClipTemplate("伤害3", 1.0f, 2003));
            track1.Clips.Add(new EffectClipTemplate("伤害4", 2.0f, 2004));
            track1.Clips.Add(new EffectClipTemplate("伤害5", 2.0f, 2005)); // 相同时间

            // Track 2: 次要效果
            var track2 = new TrackTemplate("次要效果轨道", new Color(0.6f, 0.3f, 0.6f));
            track2.Clips.Add(new EffectClipTemplate("减速", 1.5f, 3001));
            track2.Clips.Add(new EffectClipTemplate("眩晕", 3.0f, 3002));
            track2.Clips.Add(new EffectClipTemplate("流血", 4.0f, 3003));

            // Track 3: 多个音效
            var track3 = new TrackTemplate("音效轨道", new Color(0.4f, 0.8f, 0.8f));
            track3.Clips.Add(new AudioClipTemplate("蓄力音效", 0.0f, 0));
            track3.Clips.Add(new AudioClipTemplate("释放音效", 0.5f, 0));
            track3.Clips.Add(new AudioClipTemplate("爆炸音效", 2.0f, 0));

            // Track 4: 多个特效
            var track4 = new TrackTemplate("特效轨道", new Color(0.8f, 0.8f, 0.4f));
            track4.Clips.Add(new VFXClipTemplate("蓄力光环", 0.0f, "VFX/ChargeAura"));
            track4.Clips.Add(new VFXClipTemplate("冲击波", 0.5f, "VFX/Shockwave"));
            track4.Clips.Add(new VFXClipTemplate("爆炸", 2.0f, "VFX/Explosion"));
            track4.Clips.Add(new VFXClipTemplate("余波", 3.0f, "VFX/Aftershock"));

            // Track 5: 额外效果
            var track5 = new TrackTemplate("持续效果轨道", new Color(0.5f, 0.5f, 0.8f));
            track5.Clips.Add(new EffectClipTemplate("持续伤害1", 5.0f, 4001));
            track5.Clips.Add(new EffectClipTemplate("持续伤害2", 6.0f, 4002));
            track5.Clips.Add(new EffectClipTemplate("持续伤害3", 7.0f, 4003));

            scenario.Tracks.Add(track1);
            scenario.Tracks.Add(track2);
            scenario.Tracks.Add(track3);
            scenario.Tracks.Add(track4);
            scenario.Tracks.Add(track5);

            return scenario;
        }

        /// <summary>
        /// 创建边界测试场景
        /// - 空轨道
        /// - 禁用的轨道和 Clip
        /// - 时间为 0 的 Clip
        /// </summary>
        public static TestScenario CreateBoundaryScenario()
        {
            var scenario = new TestScenario
            {
                AbilityID = 10004,
                Name = "边界测试技能",
                Description = "测试边界情况和过滤逻辑",
                CostEffectID = 1001,
                CoolDownEffectID = 1002,
                TargetType = AbilityTargetType.Self,
                TimelineID = 4,
                Duration = 5.0f
            };

            // Track 1: 空轨道 (没有 Clip)
            var track1 = new TrackTemplate("空轨道", Color.gray);

            // Track 2: 禁用的轨道 (包含 Clip 但不应被处理)
            var track2 = new TrackTemplate("禁用轨道", Color.red, false);
            track2.Clips.Add(new EffectClipTemplate("不应生效的效果", 1.0f, 9001));

            // Track 3: 包含禁用 Clip 的启用轨道
            var track3 = new TrackTemplate("混合轨道", new Color(0.8f, 0.4f, 0.8f));
            track3.Clips.Add(new EffectClipTemplate("正常效果", 0.5f, 2001));
            track3.Clips.Add(new EffectClipTemplate("禁用效果", 1.0f, 2002, false)); // 禁用
            track3.Clips.Add(new EffectClipTemplate("正常效果2", 1.5f, 2003));

            // Track 4: 包含时间为 0 的 Clip
            var track4 = new TrackTemplate("起始效果轨道", new Color(0.6f, 0.6f, 0.8f));
            track4.Clips.Add(new EffectClipTemplate("起始效果", 0.0f, 3001)); // 时间为0

            scenario.Tracks.Add(track1);
            scenario.Tracks.Add(track2);
            scenario.Tracks.Add(track3);
            scenario.Tracks.Add(track4);

            return scenario;
        }
    }
}
