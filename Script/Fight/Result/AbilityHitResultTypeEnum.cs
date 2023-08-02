using UnityGameFramework.Runtime;

namespace Aquila.Fight
{   
    /// <summary>
    /// 技能命中结果
    /// </summary>
    public enum AbilityHitResultTypeEnum
    {
        ///// <summary>
        ///// 无效
        ///// </summary>
        //INVALID = -1,
        
        /// <summary>
        /// 命中
        /// </summary>
        HIT = 0,
        
        /// <summary>
        /// 没有技能实例
        /// </summary>
        NONE_SPEC,
        
        /// <summary>
        /// 造成了暴击
        /// </summary>
        CRITICAL,

        /// <summary>
        /// 造成了治疗
        /// </summary>
        HEALING,

        /// <summary>
        /// 包含位置
        /// </summary>
        CONTAINS_POSITION,
    }
}
