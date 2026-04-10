using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquila.Fight;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.AbilityPool
{
    /// <summary>
    /// 技能池组件，运行时集中管理所有技能和效果的静态配置数据
    /// </summary>
    public class Component_AbilityPool : GameFrameworkComponent
    {
        //----------------------- pub -----------------------
        public void Init()
        {
            if (_initialized)
            {
                Tools.Logger.Info("[AbilityPool] Init skipped. Already initialized.");
                return;
            }

            EffectSpecFactory.EnsureInitialized();
            _abilityPool = new Dictionary<int, AbilityData>(_defaultCapacity);
            _effectPool  = new Dictionary<int, EffectData>(_defaultCapacity);

            LoadAllEffects();
            LoadAllAbilities();

            _initialized = true;
            Tools.Logger.Info($"[AbilityPool] Init complete. Abilities={_abilityPool.Count}, Effects={_effectPool.Count}");
        }

        public bool TryGetAbility(int abilityId, out AbilityData data)
        {
            return _abilityPool.TryGetValue(abilityId, out data);
        }

        public bool GetAbility(int abilityId,out AbilityData abilityData)
        {
            abilityData = default;
            if (_abilityPool.TryGetValue(abilityId, out abilityData))
            {
                return true;
            }

            Tools.Logger.Warning($"[AbilityPool] Ability not found: {abilityId}");
            return false;
        }

        public bool HasAbility(int abilityId)
        {
            return _abilityPool.ContainsKey(abilityId);
        }

        /// <summary>
        /// <para>传入角色 MetaID，查 LuBan 角色表拿到该角色的所有技能ID，从技能池中返回对应的 AbilityData 数组。AbilityData 是 readonly struct，返回的是值拷贝，修改不影响池内数据。</para>
        /// <para>Pass in the character MetaID, query the LuBan character table to get all skill IDs for that character,and return the corresponding AbilityData array from the skill pool.AbilityData is a readonly struct, so a value copy is returned, and modifications do not affect the data in the pool.</para>
        /// </summary>
        public AbilityData[] GetAbilities(int roleMetaId)
        {
            var roleMeta = GameEntry.LuBan.Tables.RoleMeta.Get(roleMetaId);
            if (roleMeta == null)
            {
                Tools.Logger.Warning($"[AbilityPool] GetAbilitiesByRoleId: RoleMeta not found for id={roleMetaId}");
                return Array.Empty<AbilityData>();
            }
            if (roleMeta.AbilityBaseID == null || roleMeta.AbilityBaseID.Length <= 0)
                return Array.Empty<AbilityData>();

            var result = new AbilityData[roleMeta.AbilityBaseID.Length];
            var count = 0;
            foreach (var id in roleMeta.AbilityBaseID)
            {
                if (_abilityPool.TryGetValue(id, out var data))
                {
                    result[count++] = data;
                }
                else
                {
                    Tools.Logger.Warning($"[AbilityPool] GetAbilitiesByRoleId: ability id={id} not found in pool (roleMetaId={roleMetaId})");
                    return Array.Empty<AbilityData>();
                }
            }

            if (count < result.Length)
            {
                var trimmed = new AbilityData[count];
                Array.Copy(result, trimmed, count);
                return trimmed;
            }

            return result;
        }

        public IReadOnlyCollection<int> GetAllAbilityIds()
        {
            return _abilityPool.Keys;
        }

        public bool TryGetEffect(int effectId, out EffectData data)
        {
            return _effectPool.TryGetValue(effectId, out data);
        }

        public EffectData GetEffect(int effectId)
        {
            if (_effectPool.TryGetValue(effectId, out var data))
                return data;

            Tools.Logger.Warning($"[AbilityPool] Effect not found: {effectId}");
            return default;
        }

        public EffectSpec_Base CreateEffectSpecByReferencePool(
            EffectData data,
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target)
        {
            return EffectSpecFactory.CreateEffectSpecByReferencePool(data, castor, target);
        }

        public T CreateEffectSpecByReferencePool<T>() where T : EffectSpec_Base
        {
            return EffectSpecFactory.CreateEffectSpecByReferencePool<T>();
        }

        /// <summary>
        /// 从指定 .ablt 文件路径加载并组装 AbilityData 写入池。（二进制形式）
        /// 若该 Ability 依赖的 Effect 尚未在 _effectPool 中，可额外传入 effectFilePaths 预先加载。
        /// </summary>
        public void LoadBinaryAbilityFromPath(string abltFilePath, string[] effectFilePaths = null)
        {
            if (string.IsNullOrEmpty(abltFilePath))
            {
                Tools.Logger.Error("[AbilityPool] LoadAbilityFromPath: abltFilePath is null or empty");
                return;
            }

            if (!File.Exists(abltFilePath))
            {
                Tools.Logger.Error($"[AbilityPool] LoadAbilityFromPath: file not found: {abltFilePath}");
                return;
            }

            // 先加载额外的 Effect 文件
            if (effectFilePaths != null)
            {
                foreach (var efctPath in effectFilePaths)
                {
                    if (string.IsNullOrEmpty(efctPath) || !File.Exists(efctPath))
                    {
                        Tools.Logger.Warning($"[AbilityPool] LoadAbilityFromPath: effect file not found: {efctPath}");
                        continue;
                    }

                    try
                    {
                        if (TryReadEffect(efctPath, out var effectData))
                            _effectPool[effectData.GetEffectId()] = effectData;
                    }
                    catch (Exception ex)
                    {
                        Tools.Logger.Error($"[AbilityPool] LoadAbilityFromPath: failed to read effect {efctPath}: {ex.Message}");
                    }
                }
            }
            
            if (!TryReadAbility(abltFilePath, out var abilityData))
                return;

            if (_abilityPool.ContainsKey(abilityData.GetId()))
                Tools.Logger.Warning($"[AbilityPool] LoadAbilityFromPath: overwriting existing ability {abilityData.GetId()}");

            _abilityPool[abilityData.GetId()] = abilityData;
            Tools.Logger.Info($"[AbilityPool] Loaded ability {abilityData.GetId()} from {abltFilePath}");
        }

        /// <summary>
        /// 从沙盒目录加.ablt 和同目录 .efct 文件，校验后组装 AbilityData 写入池，并返回组装结
        /// </summary>
        public bool LoadSandBoxAbility(string sandBoxDir, out AbilityData abilityData)
        {
            abilityData = default;

            string abltPath = Path.Combine(sandBoxDir, "sand_box.ablt");
            if (!File.Exists(abltPath))
            {
                Tools.Logger.Error($"[AbilityPool] LoadSandBoxAbility: .ablt not found: {abltPath}");
                return false;
            }

            if (!TryReadAbility(abltPath, out var rawAbility))
            {
                Tools.Logger.Error($"[AbilityPool] LoadSandBoxAbility: failed to read ability: {abltPath}");
                return false;
            }

            // 收集 ability 中所有需要的 effectID
            var requiredEffectIds = new HashSet<int>();
            if (rawAbility.GetCostEffectID() > 0)
                requiredEffectIds.Add(rawAbility.GetCostEffectID());
            
            if (rawAbility.GetCoolDownEffectID() > 0)
                requiredEffectIds.Add(rawAbility.GetCoolDownEffectID());
            
            var effects = rawAbility.GetEffects();
            if (effects != null)
            {
                foreach (var e in effects)
                {
                    if (e.GetEffectId() > 0)
                        requiredEffectIds.Add(e.GetEffectId());
                }
            }

            // 扫描沙盒目录下所有.efct 文件，文件名effectID
            string[] efctFiles = Directory.GetFiles(sandBoxDir, "*.efct");
            var availableEffectIds = new HashSet<int>();
            var efctPathMap = new Dictionary<int, string>();
            foreach (var efctPath in efctFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(efctPath);
                if (int.TryParse(fileName, out int parsedId))
                {
                    availableEffectIds.Add(parsedId);
                    efctPathMap[parsedId] = efctPath;
                }
                else
                {
                    Tools.Logger.Error($"[AbilityPool] LoadSandBoxAbility: invalid .efct filename (not an int): {efctPath}");
                    return false;
                }
            }

            // 校验：required 与 available 必须完全一致
            foreach (var id in requiredEffectIds)
            {
                if (!availableEffectIds.Contains(id))
                {
                    Tools.Logger.Error($"[AbilityPool] LoadSandBoxAbility: required effect {id} not found in sandbox dir");
                    return false;
                }
            }
            foreach (var id in availableEffectIds)
            {
                if (!requiredEffectIds.Contains(id))
                {
                    Tools.Logger.Error($"[AbilityPool] LoadSandBoxAbility: extra .efct file {id} not referenced by ability");
                    return false;
                }
            }

            // 加载所有 .efct 写入池
            foreach (var kvp in efctPathMap)
            {
                if (!TryReadEffect(kvp.Value, out var effectData))
                {
                    Tools.Logger.Error($"[AbilityPool] LoadSandBoxAbility: failed to read effect: {kvp.Value}");
                    return false;
                }
                if (effectData.GetEffectId() != kvp.Key)
                {
                    Tools.Logger.Error($"[AbilityPool] LoadSandBoxAbility: effect file {kvp.Value} contains id={effectData.GetEffectId()}, expected {kvp.Key}");
                    return false;
                }
                _effectPool[effectData.GetEffectId()] = effectData;
            }

            _abilityPool[rawAbility.GetId()] = rawAbility;
            abilityData = rawAbility;
            Tools.Logger.Info($"[AbilityPool] LoadSandBoxAbility: loaded ability {rawAbility.GetId()} with {requiredEffectIds.Count} effects");
            return true;
        }
        
        //----------------------- priv -----------------------
        /// <summary>
        /// 加载所有 .efct 文件到 _effectPool
        /// </summary>
        private void LoadAllEffects()
        {
            string fullDir = Path.Combine(Application.dataPath, EFFECT_DIR);
            if (!Directory.Exists(fullDir))
            {
                Tools.Logger.Warning($"[AbilityPool] Effect directory not found: {fullDir}");
                return;
            }

            string[] files = Directory.GetFiles(fullDir, "*.efct");
            foreach (var filePath in files)
            {
                if (!TryReadEffect(filePath, out var data))
                    continue;

                _effectPool[data.GetEffectId()] = data;
            }
        }

        /// <summary>
        /// 加载所有 .ablt 文件到 _abilityPool
        /// </summary>
        private void LoadAllAbilities()
        {
            string fullDir = Path.Combine(Application.dataPath, ABILITY_DIR);
            if (!Directory.Exists(fullDir))
            {
                Tools.Logger.Warning($"[AbilityPool] Ability directory not found: {fullDir}");
                return;
            }

            string[] files = Directory.GetFiles(fullDir, "*.ablt");
            foreach (var filePath in files)
            {
                if (!TryReadAbility(filePath, out var data))
                    continue;

                _abilityPool[data.GetId()] = data;
            }
        }

        /// <summary>
        /// 读取单个 .efct 文件，格式与 EffectBinaryExporter 写出顺序一致
        /// </summary>
        private bool TryReadEffect(string filePath, out EffectData data)
        {
            data = default;
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fs, Encoding.UTF8);

            // Header
            string magic = Encoding.ASCII.GetString(reader.ReadBytes(6));
            byte version = reader.ReadByte();
            if (magic != EFFECT_MAGIC || version != EFFECT_VERSION)
            {
                Tools.Logger.Error($"[AbilityPool] Invalid effect header (magic={magic}, version={version}) in {filePath}");
                return false;
            }

            // Basic Info（顺序与 EffectBinaryExporter.ExportEffect 一致）
            int id              = reader.ReadInt32();
            var effectType      = (EffectType)reader.ReadInt32();         // Type EffectType 枚举（effect 类型
            var modifierType    = (NumricModifierType)reader.ReadUInt16();
            bool effectOnAwake  = reader.ReadBoolean();
            var policy          = (DurationPolicy)reader.ReadUInt16();
            float period        = reader.ReadSingle();
            float duration      = reader.ReadSingle();
            int target          = reader.ReadInt32();
            int resolveTypeId   = reader.ReadInt32();
            var affectedAttr    = (actor_attribute)reader.ReadInt32();   // EffectType actor_attribute 字段

            // Extension Params
            float f1 = reader.ReadSingle();
            float f2 = reader.ReadSingle();
            float f3 = reader.ReadSingle();
            float f4 = reader.ReadSingle();
            int i1   = reader.ReadInt32();
            int i2   = reader.ReadInt32();
            int i3   = reader.ReadInt32();
            int i4   = reader.ReadInt32();

            // Derive Effects
            int deriveCount = reader.ReadInt32();
            int[] deriveEffects = new int[deriveCount];
            for (int i = 0; i < deriveCount; i++)
                deriveEffects[i] = reader.ReadInt32();

            // Awake Effects
            int awakeCount = reader.ReadInt32();
            int[] awakeEffects = new int[awakeCount];
            for (int i = 0; i < awakeCount; i++)
                awakeEffects[i] = reader.ReadInt32();

            data = new EffectData(
                effectId:          id,
                stackLimit:        1,
                canStack:          false,
                startTime:         0f,
                endTime:           0f,
                effectType:        effectType,
                modifierType:      modifierType,
                affectedAttribute: affectedAttr,
                target:            target,
                duration:          duration,
                period:            period,
                policy:            policy,
                effectOnAwake:     effectOnAwake,
                deriveEffects:     deriveEffects,
                awakeEffects:      awakeEffects,
                floatParam1:       f1,
                floatParam2:       f2,
                floatParam3:       f3,
                floatParam4:       f4,
                intParam1:         i1,
                intParam2:         i2,
                intParam3:         i3,
                intParam4:         i4,
                resolveTypeID:    resolveTypeId
            );
            return true;
        }

        /// <summary>
        /// 读取单个 .ablt 文件，格式与 AbilityBinaryExporter.ExportAbility 写出顺序一致
        /// </summary>
        private bool TryReadAbility(string filePath, out AbilityData data)
        {
            data = default;
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fs, Encoding.UTF8);

            // Header
            string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            byte version = reader.ReadByte();
            if (magic != ABILITY_MAGIC || version != ABILITY_VERSION)
            {
                Tools.Logger.Error($"[AbilityPool] Invalid ability header (magic={magic}, version={version}) in {filePath}");
                return false;
            }

            // Basic Info
            int abilityId       = reader.ReadInt32();
            int costEffectId    = reader.ReadInt32();
            int coolDownId      = reader.ReadInt32();
            var targetType      = (AbilityTargetType)reader.ReadInt32();
            int timelineId      = reader.ReadInt32();
            float duration      = reader.ReadSingle();
            
            // Tracks → collect all EffectClipData
            var effectList = new List<EffectData>();
            int trackCount = reader.ReadInt32();

            for (int t = 0; t < trackCount; t++)
            {
                int clipCount = reader.ReadInt32();
                for (int c = 0; c < clipCount; c++)
                {
                    int clipType    = reader.ReadInt32();
                    float startTime = reader.ReadSingle();
                    float endTime   = reader.ReadSingle();

                    if (clipType == CLIP_TYPE_EFFECT)
                    {
                        var effectData = ReadEffectClip(reader, startTime, endTime);
                        if (effectData.HasValue)
                            effectList.Add(effectData.Value);
                    }
                    else
                    {
                        // 跳过非 Effect clip 的数据（Audio / VFX）
                        SkipNonEffectClip(reader, clipType);
                    }
                }
            }

            data = new AbilityData(
                abilityId,
                costEffectId,
                coolDownId,
                targetType,
                timelineId,
                duration,
                effectList.ToArray()
            );
            return true;
        }

        /// <summary>
        /// 读取 Effect Clip 字段（顺序与 AbilityBinaryExporter.WriteEffectClip 一致）
        /// </summary>
        private EffectData? ReadEffectClip(BinaryReader reader, float startTime, float endTime)
        {
            int effectId        = reader.ReadInt32();
            int stackLimit      = reader.ReadInt32();
            bool canStack       = reader.ReadBoolean();

            var effectType      = (EffectType)reader.ReadInt32();
            var modifierType    = (NumricModifierType)reader.ReadUInt16();
            var affectedAttr    = (actor_attribute)reader.ReadInt32();
            int target          = reader.ReadInt32();
            int resolveTypeId   = reader.ReadInt32();
            float clipDuration  = reader.ReadSingle();
            float period        = reader.ReadSingle();
            var policy          = (DurationPolicy)reader.ReadUInt16();
            bool effectOnAwake  = reader.ReadBoolean();

            float f1 = reader.ReadSingle();
            float f2 = reader.ReadSingle();
            float f3 = reader.ReadSingle();
            float f4 = reader.ReadSingle();
            int i1   = reader.ReadInt32();
            int i2   = reader.ReadInt32();
            int i3   = reader.ReadInt32();
            int i4   = reader.ReadInt32();

            int deriveCount = reader.ReadInt32();
            int[] deriveEffects = new int[deriveCount];
            for (int i = 0; i < deriveCount; i++)
                deriveEffects[i] = reader.ReadInt32();

            int awakeCount = reader.ReadInt32();
            int[] awakeEffects = new int[awakeCount];
            for (int i = 0; i < awakeCount; i++)
                awakeEffects[i] = reader.ReadInt32();

            if (effectId <= 0)
            {
                Tools.Logger.Warning($"[AbilityPool] Skipping effect clip with invalid ID: {effectId}");
                return null;
            }

            return new EffectData(
                effectId:          effectId,
                stackLimit:        stackLimit,
                canStack:          canStack,
                startTime:         startTime,
                endTime:           endTime,
                effectType:        effectType,
                modifierType:      modifierType,
                affectedAttribute: affectedAttr,
                target:            target,
                duration:          clipDuration,
                period:            period,
                policy:            policy,
                effectOnAwake:     effectOnAwake,
                deriveEffects:     deriveEffects,
                awakeEffects:      awakeEffects,
                floatParam1:       f1,
                floatParam2:       f2,
                floatParam3:       f3,
                floatParam4:       f4,
                intParam1:         i1,
                intParam2:         i2,
                intParam3:         i3,
                intParam4:         i4,
                resolveTypeID:    resolveTypeId
            );
        }

        /// <summary>
        /// 跳过 Audio / VFX clip 的字节，保持 BinaryReader 位置正确
        /// </summary>
        private void SkipNonEffectClip(BinaryReader reader, int clipType)
        {
            if (clipType == CLIP_TYPE_AUDIO)
            {
                reader.ReadInt32();     // AudioId
                reader.ReadSingle();    // Volume
                reader.ReadBoolean();   // Loop
                reader.ReadSingle();    // FadeIn
                reader.ReadSingle();    // FadeOut
            }
            else if (clipType == CLIP_TYPE_VFX)
            {
                ReadString(reader);     // VfxPath
                ReadString(reader);     // AttachPoint
                reader.ReadSingle(); reader.ReadSingle(); reader.ReadSingle(); // Position
                reader.ReadSingle(); reader.ReadSingle(); reader.ReadSingle(); // Rotation
                reader.ReadSingle(); reader.ReadSingle(); reader.ReadSingle(); // Scale
                reader.ReadBoolean();   // FollowAttachPoint
            }
        }

        private string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length <= 0)
                return string.Empty;
            
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }

        //----------------------- fields -----------------------

        private bool _initialized;
        private Dictionary<int, AbilityData> _abilityPool;
        private Dictionary<int, EffectData>  _effectPool;

        [SerializeField] private int _defaultCapacity = 32;

        /// <summary>
        /// 相对于 Application.dataPath 的 Effect 二进制目录
        /// </summary>
        private const string EFFECT_DIR  = "Res/Config/Effect";

        /// <summary>
        /// 相对于 Application.dataPath 的 Ability 二进制目录
        /// </summary>
        private const string ABILITY_DIR = "Res/Config/Ability";

        private const string EFFECT_MAGIC  = "EFFECT";
        private const string ABILITY_MAGIC = "ABLT";
        private const byte EFFECT_VERSION  = 0x02;
        private const byte ABILITY_VERSION = 0x02;

        private const int CLIP_TYPE_EFFECT = 1;
        private const int CLIP_TYPE_AUDIO  = 2;
        private const int CLIP_TYPE_VFX    = 3;
        
    /// <summary>
    /// EffectSpec 统一初始化注册校验实现
    /// </summary>
    private class EffectSpecFactory
    {
        public static void Initialize()
        {
            lock (_initLock)
            {
                if (_initialized)
                {
                    return;
                }

                BuildRegistrationMap();
                ValidateRegistrationMap();
                _initialized = true;

                Tools.Logger.Info($"[EffectSpecFactory] Initialize complete. Registered={_effectSpecTypeByEffectType.Count}");
            }
        }

        public static void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            Initialize();
        }

        public static EffectSpec_Base CreateEffectSpecByReferencePool(
            EffectData data,
            Module_ProxyActor.ActorInstance castor,
            Module_ProxyActor.ActorInstance target)
        {
            EnsureInitialized();

            var effectType = data.GetEffectType();
            if (!_effectSpecTypeByEffectType.TryGetValue(effectType, out var specType))
            {
                Tools.Logger.Warning($"[EffectSpecFactory] No EffectSpec registered for EffectType={effectType}, EffectID={data.GetEffectId()}.");
                return null;
            }

            var effect = ReferencePool.Acquire(specType) as EffectSpec_Base;
            if (effect == null)
            {
                var message = $"[EffectSpecFactory] Acquire failed. EffectType={effectType}, SpecType={specType.FullName}.";
                Tools.Logger.Error(message);
                throw new InvalidOperationException(message);
            }

            effect.Init(data, castor, target);
            return effect;
        }

        public static T CreateEffectSpecByReferencePool<T>() where T : EffectSpec_Base
        {
            EnsureInitialized();

            var specType = typeof(T);
            if (!_effectTypeByEffectSpecType.ContainsKey(specType))
            {
                Tools.Logger.Warning($"[EffectSpecFactory] EffectSpec type is not registered: {specType.FullName}.");
                return null;
            }

            return ReferencePool.Acquire(specType) as T;
        }

        private static void BuildRegistrationMap()
        {
            _effectSpecTypeByEffectType.Clear();
            _effectTypeByEffectSpecType.Clear();

            Register<EffectSpec_Period_CoolDown>(EffectType.Period_CoolDown);
            Register<EffectSpec_Instant_Cost>(EffectType.Instant_Cost);
            Register<EffectSpec_Instant_PhyDamage>(EffectType.Instant_PhyDamage);
            Register<EffectSpec_Instant_Summon_Projectile>(EffectType.Instant_Summon_Projectile);
            Register<EffectSpec_Period_FixedDamage>(EffectType.Period_FixedDamage);
            Register<EffectSpec_Instant_PercentageRemoveHealth>(EffectType.Instant_PercentageRemoveHealth);
            Register<EffectSpec_Period_DerivingStack>(EffectType.Period_DerivingStack);
            Register<EffectSpec_Period_ActorTag>(EffectType.Period_ActorTag);
            Register<EffectSpec_Period_AbilityTag>(EffectType.Period_AbilityTag);
            Register<EffectSpec_Period_WindUp>(EffectType.Period_WindUp);
            Register<EffectSpec_OnHitted_Trigger_ModifyAttr>(EffectType.OnHitted_Trigger_ModifyAttr);
        }

        private static void Register<T>(EffectType effectType) where T : EffectSpec_Base
        {
            var specType = typeof(T);

            if (!typeof(EffectSpec_Base).IsAssignableFrom(specType))
            {
                var message = $"[EffectSpecFactory] Invalid registration. {specType.FullName} is not EffectSpec_Base.";
                Tools.Logger.Error(message);
                throw new InvalidOperationException(message);
            }

            if (_effectSpecTypeByEffectType.ContainsKey(effectType))
            {
                var message = $"[EffectSpecFactory] Duplicate EffectType registration: {effectType}.";
                Tools.Logger.Error(message);
                throw new InvalidOperationException(message);
            }

            if (_effectTypeByEffectSpecType.ContainsKey(specType))
            {
                var message = $"[EffectSpecFactory] Duplicate EffectSpec type registration: {specType.FullName}.";
                Tools.Logger.Error(message);
                throw new InvalidOperationException(message);
            }

            _effectSpecTypeByEffectType.Add(effectType, specType);
            _effectTypeByEffectSpecType.Add(specType, effectType);
        }

        private static void ValidateRegistrationMap()
        {
            var missing = new List<EffectType>();
            foreach (EffectType effectType in Enum.GetValues(typeof(EffectType)))
            {
                if (effectType == EffectType.Invalid)
                {
                    continue;
                }

                if (!_effectSpecTypeByEffectType.TryGetValue(effectType, out var specType))
                {
                    missing.Add(effectType);
                    continue;
                }

                if (specType == null || !typeof(EffectSpec_Base).IsAssignableFrom(specType))
                {
                    var message = $"[EffectSpecFactory] Invalid mapped type for {effectType}.";
                    Tools.Logger.Error(message);
                    throw new InvalidOperationException(message);
                }
            }

            if (missing.Count > 0)
            {
                var message =
                    $"[EffectSpecFactory] Registration incomplete. Missing EffectType: {string.Join(", ", missing)}";
                Tools.Logger.Error(message);
                throw new InvalidOperationException(message);
            }
        }

        private static readonly Dictionary<EffectType, Type> _effectSpecTypeByEffectType =
            new Dictionary<EffectType, Type>(16);

        private static readonly Dictionary<Type, EffectType> _effectTypeByEffectSpecType =
            new Dictionary<Type, EffectType>(16);

        private static readonly object _initLock = new object();
        private static bool _initialized;
    }
        
    }

    
}

