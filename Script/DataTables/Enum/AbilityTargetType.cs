//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cfg.Enum
{
    /// <summary>
    /// Ability目标类型
    /// </summary>
    [System.Flags]
    public enum AbilityTargetType
    {
        /// <summary>
        /// 自身
        /// </summary>
        Self = 1,
        /// <summary>
        /// 敌方
        /// </summary>
        Enemy = 2,
        /// <summary>
        /// 友方
        /// </summary>
        Ally = 4,
        /// <summary>
        /// 中立
        /// </summary>
        Neutral = 8,
        /// <summary>
        /// 自身和友方单位
        /// </summary>
        Self_Ally = Self|Ally,
    }
}
