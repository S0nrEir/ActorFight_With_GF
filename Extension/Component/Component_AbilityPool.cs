using System.Collections.Generic;
using System.IO;
using System.Text;
using Aquila.Fight;
using Cfg.Enum;
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
            _abilityPool = new Dictionary<int, AbilityData>(_defaultCapacity);
            _effectPool  = new Dictionary<int, EffectData>(_defaultCapacity);

            LoadAllEffects();
            LoadAllAbilities();

            Log.Info($"[AbilityPool] Init complete. Abilities={_abilityPool.Count}, Effects={_effectPool.Count}");
        }

        public bool TryGetAbility(int abilityId, out AbilityData data)
        {
            return _abilityPool.TryGetValue(abilityId, out data);
        }

        public AbilityData GetAbility(int abilityId)
        {
            if (_abilityPool.TryGetValue(abilityId, out var data))
                return data;

            Log.Warning($"[AbilityPool] Ability not found: {abilityId}");
            return default;
        }

        public bool HasAbility(int abilityId)
        {
            return _abilityPool.ContainsKey(abilityId);
        }

        /// <summary>
        /// <para>传入角色 MetaID，查 LuBan 角色表拿到该角色的所有技能 ID，从技能池中返回对应的 AbilityData 数组。AbilityData 是 readonly struct，返回的是值拷贝，修改不影响池内数据。</para>
        /// <para>Pass in the character MetaID, query the LuBan character table to get all skill IDs for that character,and return the corresponding AbilityData array from the skill pool.AbilityData is a readonly struct, so a value copy is returned, and modifications do not affect the data in the pool.</para>
        /// </summary>
        public AbilityData[] GetAbilities(int roleMetaId)
        {
            var roleMeta = GameEntry.LuBan.Tables.RoleMeta.Get(roleMetaId);
            if (roleMeta == null)
            {
                Log.Warning($"[AbilityPool] GetAbilitiesByRoleId: RoleMeta not found for id={roleMetaId}");
                return System.Array.Empty<AbilityData>();
            }
            if (roleMeta.AbilityBaseID == null || roleMeta.AbilityBaseID.Length == 0)
                return System.Array.Empty<AbilityData>();

            var result = new AbilityData[roleMeta.AbilityBaseID.Length];
            var count = 0;
            foreach (var id in roleMeta.AbilityBaseID)
            {
                if (_abilityPool.TryGetValue(id, out var data))
                {result[count++] = data;
                }
                else
                {
                    Log.Warning($"[AbilityPool] GetAbilitiesByRoleId: ability id={id} not found in pool (roleMetaId={roleMetaId})");
                    return System.Array.Empty<AbilityData>();
                }
            }

            if (count < result.Length)
            {
                var trimmed = new AbilityData[count];
                System.Array.Copy(result, trimmed, count);
                return trimmed;
            }

            return result;
        }

        public IReadOnlyCollection<int> GetAllAbilityIds()
        {
            return _abilityPool.Keys;
        }

        /// <summary>
        /// 从指定 .ablt 文件路径加载并组装 AbilityData 写入池。（二进制形式）
        /// 若该 Ability 依赖的 Effect 尚未在 _effectPool 中，可额外传入 effectFilePaths 预先加载。
        /// </summary>
        public void LoadBinaryAbilityFromPath(string abltFilePath, string[] effectFilePaths = null)
        {
            if (string.IsNullOrEmpty(abltFilePath))
            {
                Log.Error("[AbilityPool] LoadAbilityFromPath: abltFilePath is null or empty");
                return;
            }

            if (!File.Exists(abltFilePath))
            {
                Log.Error($"[AbilityPool] LoadAbilityFromPath: file not found: {abltFilePath}");
                return;
            }

            // 先加载额外的 Effect 文件
            if (effectFilePaths != null)
            {
                foreach (var efctPath in effectFilePaths)
                {
                    if (string.IsNullOrEmpty(efctPath) || !File.Exists(efctPath))
                    {
                        Log.Warning($"[AbilityPool] LoadAbilityFromPath: effect file not found: {efctPath}");
                        continue;
                    }

                    try
                    {
                        if (TryReadEffect(efctPath, out var effectData))
                            _effectPool[effectData.GetEffectId()] = effectData;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"[AbilityPool] LoadAbilityFromPath: failed to read effect {efctPath}: {ex.Message}");
                    }
                }
            }
            
            if (!TryReadAbility(abltFilePath, out var abilityData))
                return;

            if (_abilityPool.ContainsKey(abilityData.GetId()))
                Log.Warning($"[AbilityPool] LoadAbilityFromPath: overwriting existing ability {abilityData.GetId()}");

            _abilityPool[abilityData.GetId()] = abilityData;
            Log.Info($"[AbilityPool] Loaded ability {abilityData.GetId()} from {abltFilePath}");
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
                Log.Warning($"[AbilityPool] Effect directory not found: {fullDir}");
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
                Log.Warning($"[AbilityPool] Ability directory not found: {fullDir}");
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
            if (magic != EFFECT_MAGIC)
            {
                Log.Error($"[AbilityPool] Invalid effect magic '{magic}' in {filePath}");
                return false;
            }

            // Basic Info（顺序与 EffectBinaryExporter.ExportEffect 一致）
            int id              = reader.ReadInt32();
            var effectType      = (EffectType)reader.ReadInt32();         // Type → EffectType 枚举（effect 类型）
            var modifierType    = (NumricModifierType)reader.ReadUInt16();
            bool effectOnAwake  = reader.ReadBoolean();
            var policy          = (DurationPolicy)reader.ReadUInt16();
            float period        = reader.ReadSingle();
            float duration      = reader.ReadSingle();
            int target          = reader.ReadInt32();
            var affectedAttr    = (actor_attribute)reader.ReadInt32();   // EffectType → actor_attribute 字段

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
                stackCount:        1,
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
                intParam4:         i4
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
            if (magic != ABILITY_MAGIC)
            {
                Log.Error($"[AbilityPool] Invalid ability magic '{magic}' in {filePath}");
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
            int stackCount      = reader.ReadInt32();
            bool canStack       = reader.ReadBoolean();

            var effectType      = (EffectType)reader.ReadInt32();
            var modifierType    = (NumricModifierType)reader.ReadUInt16();
            var affectedAttr    = (actor_attribute)reader.ReadInt32();
            int target          = reader.ReadInt32();
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
                Log.Warning($"[AbilityPool] Skipping effect clip with invalid ID: {effectId}");
                return null;
            }

            return new EffectData(
                effectId:          effectId,
                stackCount:        stackCount,
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
                intParam4:         i4
            );
        }

        /// <summary>
        /// 跳过 Audio / VFX clip 的字节，保持 BinaryReader 位置正确
        /// </summary>
        private void SkipNonEffectClip(BinaryReader reader, int clipType)
        {
            if (clipType == CLIP_TYPE_AUDIO)
            {
                ReadString(reader);     // AudioPath
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

        private const int CLIP_TYPE_EFFECT = 1;
        private const int CLIP_TYPE_AUDIO  = 2;
        private const int CLIP_TYPE_VFX    = 3;
    }
}
