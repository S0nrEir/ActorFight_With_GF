using System;
using UnityEngine;

namespace Editor.AbilityEditor.Config
{
    /// <summary>
    /// 当前配置的全局访问器 / Global accessor for the currently generated AbilityConfig
    ///  为编辑器工具和自定义导出提供能力配置数据的集中访问 / Provides centralized access to ability configuration data for editor tools and custom export systems
    /// </summary>
    public static class AbilityConfigAccessor
    {
        /// <summary>
        /// 获取当前配置（如果没有生成则为空） / Get the current ability config (null if not generated)
        /// </summary>
        public static AbilityConfig Current => _current;

        /// <summary>
        /// 设定当前配置 / Set the current config and fire change event
        /// </summary>
        public static void SetConfig(AbilityConfig config)
        {
            if (config == null)
            {
                Debug.LogWarning("[AbilityConfigAccessor] SetConfig called with null config. Use Clear() instead.");
                return;
            }
            
            if (!config.Validate(out string errorMessage))
            {
                Debug.LogError($"[AbilityConfigAccessor] Cannot set invalid config: {errorMessage}");
                return;
            }

            _current = config;

            Debug.Log($"[AbilityConfigAccessor] Config set: {config}");
            OnConfigChanged?.Invoke(_current);
        }

        /// <summary>
        /// 清理配置 / Clear the current config
        /// </summary>
        public static void Clear()
        {
            if (_current == null)
            {
                Debug.LogWarning("[AbilityConfigAccessor] Clear called but no config is loaded");
                return;
            }
            Debug.Log($"[AbilityConfigAccessor] Clearing config: {_current}");
            _current = null;
            
            OnConfigChanged?.Invoke(null);
        }
        
        public static string ToString()
        {
            if (_current == null)
                return "[No config loaded]";

            return $"AbilityConfig Summary:\n" +
                   $"  ID: {_current.AbilityID}\n" +
                   $"  Name: {_current.Name}\n" +
                   $"  Duration: {_current.TimelineDuration:F2}s\n" +
                   $"  Triggers: {_current.Triggers.Count}\n" +
                   $"  Effects: {_current.Effects.Count}\n" +
                   $"  Skills: {_current.Skills.Count}\n" +
                   $"  Audios: {_current.Audios.Count}\n" +
                   $"  VFXs: {_current.VFXs.Count}";
        }
        
        public static event Action<AbilityConfig> OnConfigChanged;
        private static AbilityConfig _current;
    }
}
