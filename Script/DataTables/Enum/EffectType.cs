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
    /// Effect的类型
    /// </summary>
    public enum EffectType
    {
        /// <summary>
        /// 冷却类effect
        /// </summary>
        Period_CoolDown = 1,
        /// <summary>
        /// 消耗类Effect
        /// </summary>
        Instant_Cost = 2,
        /// <summary>
        /// 物理伤害
        /// </summary>
        Instant_PhyDamage = 3,
        /// <summary>
        /// 召唤物_投射物
        /// </summary>
        Instant_Summon_Projectile = 4,
        /// <summary>
        /// 周期性固定数值伤害
        /// </summary>
        Period_FixedDamage = 5,
        /// <summary>
        /// 百分比生命值移除
        /// </summary>
        Instant_PercentageRemoveHealth = 6,
        /// <summary>
        /// 周期性生效effect，派发子effect
        /// </summary>
        Period_DerivingStack = 7,
        /// <summary>
        /// 为actor添加tag
        /// </summary>
        Period_ActorTag = 8,
        /// <summary>
        /// 为Ability添加tag
        /// </summary>
        Period_AbilityTag = 9,
        /// <summary>
        /// 吟唱
        /// </summary>
        Period_WindUp = 10,
        /// <summary>
        /// 受击修改属性effect
        /// </summary>
        OnHitted_Trigger_ModifyAttr = 11,
    }
}
