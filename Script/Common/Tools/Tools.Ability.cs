using System.Collections.Generic;
using System.IO;
using Aquila.Fight;
using Aquila.Module;
using Cfg.Enum;
using UnityEngine;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// 技能工具类
        /// <para>#todo此处暂时存放了一些技能数据，工具类不该存放这些技能数据，之后要处理一下</para>
        /// </summary>
        public static class Ability
        {
            /// <summary>
            /// 是否可以通过百分比计算应用修改属性的effect
            /// </summary>
            /// <param name="baseValue">要用于百分比检查的基础值</param>
            /// <param name="conditionValue">条件值</param>
            /// <param name="primitive">当前积累的值</param>
            /// <returns></returns>
            public static bool CanApplyModifyAttrByEffect_ByPercentage(float baseValue,float conditionValue, float primitive)
            {
                primitive /= baseValue;
                return primitive >= conditionValue;
            }

            /// <summary>
            /// 是否可以按固定值计算应用修改属性的effect
            /// </summary>
            public static bool CanApplyModifyAttrEffect_ByFixed(float conditionValue,float valueToCheck)
            {
                return valueToCheck >= conditionValue;
            }
            
            /// <summary>
            /// 初始化effect生成器
            /// </summary>
            // public static void InitEffectSpecGenerator()
            // {
            //     EffectSpecFactory.EnsureInitialized();
            // }

            /// <summary>
            /// 根据配表类型获取对应的effect逻辑实例，拿不到返回null
            /// </summary>
            // public static EffectSpec_Base CreateEffectSpecByReferencePool( Table_Effect meta,Module_ProxyActor.ActorInstance castor,Module_ProxyActor.ActorInstance target)
            // {
            //     var type = ( int ) meta.Type;
            //
            //     if (!_effectTypeDic.TryGetValue(type, out var effectType))
            //     {
            //         Aquila.Toolkit.Tools.Logger.Warning( $"Tools.Ability.CreateEffectSpecByReferencePool()--->!_effectTypeDic.ContainsKey( ( int ) meta.Type ),key{meta.Type}" );
            //         return null;
            //     }
            //
            //     var effect = ReferencePool.Acquire(effectType) as EffectSpec_Base;
            //     if (effect is null)
            //     {
            //         Aquila.Toolkit.Tools.Logger.Warning( $"Tools.Ability.CreateEffectSpecByReferencePool()--->effect is null" );
            //         return null;
            //     }
            //
            //     effect.Init(meta, castor, target);
            //     return effect;
            // }

            /// <summary>
            /// 根据 EffectData 创建对应的 effect 逻辑实例，拿不到返回 null
            /// </summary>
            public static EffectSpec_Base CreateEffectSpecByReferencePool(EffectData data, Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
            {
                if (GameEntry.AbilityPool == null)
                {
                    Logger.Error("Tools.Ability.CreateEffectSpecByReferencePool()--->GameEntry.AbilityPool is null");
                    return null;
                }

                return GameEntry.AbilityPool.CreateEffectSpecByReferencePool(data, castor, target);
            }
                #region nouse

                //EffectSpec_Base effect = null;
                //switch ( meta.Type )
                //{
                //    case EffectType.Instant_PhyDamage:
                //        //return new EffectSpec_PhyDamage(meta);
                //        effect = ReferencePool.Acquire<EffectSpec_Instant_PhyDamage>();
                //        break;

                //    case EffectType.Period_FixedDamage:
                //        effect = ReferencePool.Acquire<EffectSpec_Period_FixedDamage>();
                //        break;

                //    case EffectType.Period_DerivingStack:
                //        effect = ReferencePool.Acquire<EffectSpec_Period_DerivingStack>();
                //        break;

                //    case EffectType.Instant_PercentageRemoveHealth:
                //        effect = ReferencePool.Acquire<EffectSpec_Instant_PercentageRemoveHealth>();
                //        break;

                //    case EffectType.Period_ActorTag:
                //        effect = ReferencePool.Acquire<EffectSpec_Period_ActorTag>();
                //        break;

                //    case EffectType.Instant_Summon:
                //        break;

                //    default:
                //        return null;
                //}

                //effect.Init( meta );
                //return effect;

                #endregion
            //}

            /// <summary>
            /// 创建对应的effect逻辑实例，创建失败返回null
            /// </summary>
            public static T CreateEffectSpecByReferencePool<T>() where T : EffectSpec_Base
            {
                if (GameEntry.AbilityPool == null)
                {
                    Logger.Error("Tools.Ability.CreateEffectSpecByReferencePool<T>()--->GameEntry.AbilityPool is null");
                    return null;
                }

                return GameEntry.AbilityPool.CreateEffectSpecByReferencePool<T>();
            }

            /// <summary>
            /// 创建对应的effect逻辑实例，创建失败返回null
            /// </summary>
            // public static T CreateEffectSpecByReferencePool<T>(Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
            //     Module_ProxyActor.ActorInstance target = null) where T : EffectSpec_Base
            // {
            //     return CreateEffectSpecByReferencePool(meta,castor,target) as T;
            // }

            #region Binary Loading
#if UNITY_EDITOR
            /// <summary>
            /// 从二进制文件加载所有技能数据，组装成运行时 AbilityData
            /// </summary>
            public static IReadOnlyDictionary<int, AbilityData> LoadAllAbilityData_Editor()
            {
                var effectTemplates = new Dictionary<int, EffectData>();
                var result = new Dictionary<int, AbilityData>();
                string effectDir = System.IO.Path.Combine(Application.dataPath, EFFECT_BIN_DIR);
                string abilityDir = System.IO.Path.Combine(Application.dataPath, ABILITY_BIN_DIR);

                if (Directory.Exists(effectDir))
                {
                    foreach (var file in Directory.GetFiles(effectDir, "*.efct"))
                    {
                        var effectData = ParseEffectBinary(File.ReadAllBytes(file));
                        if (effectData.GetEffectId() != 0)
                            effectTemplates[effectData.GetEffectId()] = effectData;
                    }
                }

                if (Directory.Exists(abilityDir))
                {
                    foreach (var file in Directory.GetFiles(abilityDir, "*.ablt"))
                    {
                        var abilityData = ParseAbilityBinary(File.ReadAllBytes(file), effectTemplates);
                        if (abilityData.GetId() != 0)
                            result[abilityData.GetId()] = abilityData;
                    }
                }
                return result;
            }
#endif
            private static EffectData ParseEffectBinary(byte[] data)
            {
                using (var reader = new ByteReader(data))
                {
                    string magic = reader.ReadFixedString(6);
                    byte version = reader.ReadByte();

                    if (magic != EFCT_MAGIC || version != BIN_VERSION_3)
                    {
                        Logger.Warning($"Tools.Ability.ParseEffectBinary: invalid header (magic={magic}, version={version})");
                        return default;
                    }

                    int id = reader.ReadInt32();
                    var type = (EffectType)reader.ReadInt32();
                    var modifierType = (NumricModifierType)reader.ReadUInt16();
                    bool effectOnAwake = reader.ReadBoolean();
                    var policy = (DurationPolicy)reader.ReadUInt16();
                    float period = reader.ReadSingle();
                    float duration = reader.ReadSingle();
                    int target = reader.ReadInt32();
                    int resolveTypeID = reader.ReadInt32();
                    var affectedAttribute = (actor_attribute)reader.ReadInt32();
                    float float1 = reader.ReadSingle();
                    float float2 = reader.ReadSingle();
                    float float3 = reader.ReadSingle();
                    float float4 = reader.ReadSingle();
                    int int1 = reader.ReadInt32();
                    int int2 = reader.ReadInt32();
                    int int3 = reader.ReadInt32();
                    int int4 = reader.ReadInt32();

                    int deriveCount = reader.ReadInt32();
                    var deriveEffects = new int[deriveCount];
                    for (int i = 0; i < deriveCount; i++)
                        deriveEffects[i] = reader.ReadInt32();

                    int awakeCount = reader.ReadInt32();
                    var awakeEffects = new int[awakeCount];
                    for (int i = 0; i < awakeCount; i++)
                        awakeEffects[i] = reader.ReadInt32();

                    int formulaID = -1;
                    if (version >= BIN_VERSION_3 && !reader.IsEnd)
                        formulaID = reader.ReadInt32();

                    return new EffectData(
                        effectId: id,
                        stackLimit: 0,
                        canStack: false,
                        startTime: 0f,
                        endTime: 0f,
                        effectType: type,
                        modifierType: modifierType,
                        affectedAttribute: affectedAttribute,
                        target: target,
                        duration: duration,
                        period: period,
                        policy: policy,
                        effectOnAwake: effectOnAwake,
                        deriveEffects: deriveEffects,
                        awakeEffects: awakeEffects,
                        floatParam1: float1,
                        floatParam2: float2,
                        floatParam3: float3,
                        floatParam4: float4,
                        intParam1: int1,
                        intParam2: int2,
                        intParam3: int3,
                        intParam4: int4,
                        resolveTypeID: resolveTypeID,
                        formulaID: formulaID);
                }
            }

            private static AbilityData ParseAbilityBinary(byte[] data, Dictionary<int, EffectData> effectTemplates)
            {
                using (var reader = new ByteReader(data))
                {
                    string magic = reader.ReadFixedString(4);
                    byte version = reader.ReadByte();

                    if (magic != ABLT_MAGIC || version != BIN_VERSION_3)
                    {
                        Logger.Warning($"Tools.Ability.ParseAbilityBinary: invalid header (magic={magic}, version={version})");
                        return default;
                    }

                    int id = reader.ReadInt32();
                    int costEffectID = reader.ReadInt32();
                    int coolDownEffectID = reader.ReadInt32();
                    var targetType = (AbilityTargetType)reader.ReadInt32();
                    int timelineID = reader.ReadInt32();
                    float timelineDuration = reader.ReadSingle();

                    var effectDataList = new List<EffectData>();
                    int trackCount = reader.ReadInt32();

                    for (int t = 0; t < trackCount; t++)
                    {
                        int clipCount = reader.ReadInt32();
                        for (int c = 0; c < clipCount; c++)
                        {
                            int clipType = reader.ReadInt32();
                            float startTime = reader.ReadSingle();
                            float endTime = reader.ReadSingle();

                            switch (clipType)
                            {
                                case 1:
                                    ReadEffectClip(reader, startTime, endTime, effectTemplates, effectDataList);
                                    break;
                                
                                case 2:
                                    SkipAudioClip(reader);
                                    break;
                                
                                case 3:
                                    SkipVfxClip(reader);
                                    break;
                                
                                default:
                                    Logger.Warning($"Tools.Ability.ParseAbilityBinary: unknown clip type {clipType} in ability {id}");
                                    break;
                            }
                        }
                    }

                    return new AbilityData(
                        id: id,
                        costEffectID: costEffectID,
                        coolDownEffectID: coolDownEffectID,
                        targetType: targetType,
                        timelineID: timelineID,
                        timelineDuration: timelineDuration,
                        effects: effectDataList.ToArray());
                }
            }

            /// <summary>
            /// 从 .ablt 二进制流中读取一个 Effect Clip，
            /// 优先使用 .efct 模板的配置字段，找不到则回退使用 .ablt 内联数据
            /// </summary>
            private static void ReadEffectClip(
                ByteReader reader,
                float startTime,
                float endTime,
                Dictionary<int, EffectData> effectTemplates,
                List<EffectData> effectDataList)
            {
                int effectId = reader.ReadInt32();
                int stackCount = reader.ReadInt32();
                bool canStack = reader.ReadBoolean();
                var inlineType = (EffectType)reader.ReadInt32();
                var inlineModifier = (NumricModifierType)reader.ReadUInt16();
                var inlineAttr = (actor_attribute)reader.ReadInt32();
                int inlineTarget = reader.ReadInt32();
                int inlineResolveTypeID = reader.ReadInt32();
                float inlineDuration = reader.ReadSingle();
                float inlinePeriod = reader.ReadSingle();
                var inlinePolicy = (DurationPolicy)reader.ReadUInt16();
                bool inlineOnAwake = reader.ReadBoolean();
                float f1 = reader.ReadSingle();
                float f2 = reader.ReadSingle();
                float f3 = reader.ReadSingle();
                float f4 = reader.ReadSingle();
                int i1 = reader.ReadInt32();
                int i2 = reader.ReadInt32();
                int i3 = reader.ReadInt32();
                int i4 = reader.ReadInt32();

                int deriveCount = reader.ReadInt32();
                var inlineDerives = new int[deriveCount];
                for (int i = 0; i < deriveCount; i++)
                    inlineDerives[i] = reader.ReadInt32();

                int awakeCount = reader.ReadInt32();
                var inlineAwakes = new int[awakeCount];
                for (int i = 0; i < awakeCount; i++)
                    inlineAwakes[i] = reader.ReadInt32();
                
                int inlineFormulaID = reader.ReadInt32();

                EffectData effectData;
                if (effectTemplates.TryGetValue(effectId, out var tmpl))
                {
                    var srcDerives = tmpl.GetDeriveEffects();
                    var dArr = new int[srcDerives.Count];
                    for (int i = 0; i < srcDerives.Count; i++)
                        dArr[i] = srcDerives[i];

                    var srcAwakes = tmpl.GetAwakeEffects();
                    var aArr = new int[srcAwakes.Count];
                    for (int i = 0; i < srcAwakes.Count; i++)
                        aArr[i] = srcAwakes[i];

                    effectData = new EffectData(
                        effectId: effectId,
                        stackLimit: stackCount,
                        canStack: canStack,
                        startTime: startTime,
                        endTime: endTime,
                        effectType: tmpl.GetEffectType(),
                        modifierType: tmpl.GetModifierType(),
                        affectedAttribute: tmpl.GetAffectedAttribute(),
                        target: tmpl.GetTarget(),
                        duration: tmpl.GetDuration(),
                        period: tmpl.GetPeriod(),
                        policy: tmpl.GetPolicy(),
                        effectOnAwake: tmpl.GetEffectOnAwake(),
                        deriveEffects: dArr,
                        awakeEffects: aArr,
                        floatParam1: tmpl.GetFloatParam1(),
                        floatParam2: tmpl.GetFloatParam2(),
                        floatParam3: tmpl.GetFloatParam3(),
                        floatParam4: tmpl.GetFloatParam4(),
                        intParam1: tmpl.GetIntParam1(),
                        intParam2: tmpl.GetIntParam2(),
                        intParam3: tmpl.GetIntParam3(),
                        intParam4: tmpl.GetIntParam4(),
                        resolveTypeID: tmpl.GetResolveTypeID(),
                        formulaID: tmpl.GetFormulaID());
                }
                else
                {
                    effectData = new EffectData(
                        effectId: effectId,
                        stackLimit: stackCount,
                        canStack: canStack,
                        startTime: startTime,
                        endTime: endTime,
                        effectType: inlineType,
                        modifierType: inlineModifier,
                        affectedAttribute: inlineAttr,
                        target: inlineTarget,
                        duration: inlineDuration,
                        period: inlinePeriod,
                        policy: inlinePolicy,
                        effectOnAwake: inlineOnAwake,
                        deriveEffects: inlineDerives,
                        awakeEffects: inlineAwakes,
                        floatParam1: f1,
                        floatParam2: f2,
                        floatParam3: f3,
                        floatParam4: f4,
                        intParam1: i1,
                        intParam2: i2,
                        intParam3: i3,
                        intParam4: i4,
                        resolveTypeID: inlineResolveTypeID,
                        formulaID: inlineFormulaID);
                }

                effectDataList.Add(effectData);
            }

            private static void SkipAudioClip(ByteReader reader)
            {
                reader.ReadString();
                reader.ReadSingle();
                reader.ReadBoolean();
                reader.ReadSingle();
                reader.ReadSingle();
            }

            private static void SkipVfxClip(ByteReader reader)
            {
                reader.ReadString();
                reader.ReadString();
                reader.ReadVector3();
                reader.ReadVector3();
                reader.ReadVector3();
                reader.ReadBoolean();
            }

            #endregion

            private const string ABILITY_BIN_DIR = "Res/Config/Ability";
            private const string EFFECT_BIN_DIR = "Res/Config/Effect";
            private const string ABLT_MAGIC = "ABLT";
            private const string EFCT_MAGIC = "EFFECT";
            private const byte BIN_VERSION_3 = 0x03;
        }//end class Ability
    }//end class Tools
}

