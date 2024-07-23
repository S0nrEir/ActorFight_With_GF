using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aquila.Editor
{
    /// <summary>
    /// effect数据容器
    /// </summary>
    public static class EffectDataMgr
    {
        /// <summary>
        /// 设置一个group携带的effects，会替换原来的
        /// </summary>
        public static void SetEffects(AbilityEditorEffectGroupNode node,List<AbilityEffect> newEffects)
        {
            var hashCode = node.GetHashCode();
            if (!_abilityNodeToEffects.ContainsKey(hashCode))
            {
                Debug.LogError($"EffectDataMgr.cs: SetEffect: nodeHash:{hashCode} already exists.");
                return;
            }

            _abilityNodeToEffects[hashCode] = newEffects;
        }

        // public static bool SetEffect(AbilityEditorEffectGroupNode group, AbilityEffect effect)
        // {
        //     var hashCode = group.GetHashCode();
        //     if (!_abilityNodeToEffects.TryGetValue(hashCode,out var effects))
        //     {
        //         Debug.LogError($"EffectDataMgr.cs: SetEffect: nodeHash:{hashCode} already exists.");
        //         return false;
        //     }
        //     
        //     var cnt = effects.Count;
        //     for (var i = 0; i < cnt; i++)
        //     {
        //         if (effects[i].GUID == effect.GUID)
        //         {
        //             effects[i] = effect;
        //             return true;
        //         }
        //     }
        //     return false;
        // }

        public static void Clear()
        {
            var itor = _abilityNodeToEffects.GetEnumerator();
            while (itor.MoveNext())
            {
                itor.Current.Value.Clear();
            }
            itor.Dispose();
            _abilityNodeToEffects.Clear();
            _abilityNodeToEffects = null;
        }

        /// <summary>
        /// 获取一个nodeGroup下的所有effect
        /// </summary>
        public static List<AbilityEffect> GetEffects(AbilityEditorEffectGroupNode group)
        {
            var hashCode = group.GetHashCode();
            if (!_abilityNodeToEffects.TryGetValue(hashCode, out var effects))
            {
                Debug.LogError($"EffectDataMgr.cs: GetEffects: nodeHash:{hashCode} not found.");
                return null;
            }

            return effects;
        }
        
        /// <summary>
        /// 移除一个nodeGroup
        /// </summary>
        public static bool RemoveNodeGroup(AbilityEditorEffectGroupNode group)
        {
            var hashCode = group.GetHashCode();
            if (!_abilityNodeToEffects.ContainsKey(hashCode.GetHashCode()))
            {
                Debug.LogError($"EffectDataMgr.cs: RemoveNodeGroup: nodeHash:{hashCode} not found.");
                return false;
            }
            
            _abilityNodeToEffects[hashCode].Clear();
            _abilityNodeToEffects.Remove(hashCode);
            return true;
        }
        
        /// <summary>
        /// 添加一个nodeGroup
        /// </summary>
        public static bool AddNodeGroup(AbilityEditorEffectGroupNode group)
        {
            var hashCode = group.GetHashCode();
            if (_abilityNodeToEffects is null)
                _abilityNodeToEffects = new Dictionary<int, List<AbilityEffect>>();
            
            if (_abilityNodeToEffects.ContainsKey(hashCode.GetHashCode()))
            {
                Debug.LogError($"EffectDataMgr.cs: AddNodeGroup: nodeHash:{hashCode} already exists.");
                return false;
            }

            _abilityNodeToEffects.Add(hashCode, new List<AbilityEffect>());
            return true;
        }
        
        /// <summary>
        /// 为一个nodeGroup移除一个effect
        /// </summary>
        public static bool RemoveEffect(AbilityEditorEffectGroupNode group, AbilityEffect effect)
        {
            var hashCode = group.GetHashCode();
            if (!_abilityNodeToEffects.TryGetValue(hashCode.GetHashCode(), out var effects))
            {
                Debug.LogError($"EffectDataMgr.cs: AddEffects: nodeHash:{hashCode} not found.");
                return false;
            }

            if (effects is null || effects.Count == 0)
            {
                Debug.LogError($"effects is null or effects count = 0,node name:{group.name}");
                return false;
            }

            effects.Remove(effect);
            return true;
        }

        /// <summary>
        /// 为一个nodeGroup添加effect
        /// </summary>
        public static bool AddEffect(AbilityEditorEffectGroupNode group,AbilityEffect effect)
        {
            var hashCode = group.GetHashCode();
            if (!_abilityNodeToEffects.TryGetValue(hashCode.GetHashCode(), out var effects))
            {
                Debug.LogError($"EffectDataMgr.cs: AddEffects: nodeHash:{hashCode} not found.");
                return false;
            }

            if (effects is null)
            {
                Debug.LogError($"effects is null,node name:{group.name}");
                effects = new List<AbilityEffect>();
            }

            effects.Add(effect);
            return true;
        }

        static EffectDataMgr()
        {
            _abilityNodeToEffects = new Dictionary<int, List<AbilityEffect>>();
        }

        public static Dictionary<int, List<AbilityEffect>> _abilityNodeToEffects = null;
    }
}
