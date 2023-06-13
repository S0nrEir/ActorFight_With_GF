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
        
        ///// <summary>
        ///// 施法者不存在
        ///// </summary>
        //NO_CASTOR,
        
        ///// <summary>
        ///// 没有目标
        ///// </summary>
        //NO_TARGET,
        
        ///// <summary>
        ///// 无法使用
        ///// </summary>
        //CANT_USE,

        /// <summary>
        /// 造成了暴击
        /// </summary>
        CRITICAL,
    }
}
