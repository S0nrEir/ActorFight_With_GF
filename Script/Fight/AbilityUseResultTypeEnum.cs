namespace Aquila.Fight
{

    /// <summary>
    /// 使用技能结果状态描述
    /// </summary>
    public enum AbilityUseResultTypeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        SUCC = 0,

        /// <summary>
        /// 无目标
        /// </summary>
        NO_TARGET = 1,

        /// <summary>
        /// 无施法者
        /// </summary>
        NO_CASTOR = 2,

        /// <summary>
        /// 技能消耗不够
        /// </summary>
        COST_NOT_ENOUGH,

        /// <summary>
        /// 技能冷却中
        /// </summary>
        CD_NOT_OK,

        /// <summary>
        /// 没有技能参数
        /// </summary>
        NONE_PARAM,

        /// <summary>
        /// 没有技能配置参数
        /// </summary>
        NONE_ABILITY_META,

        /// <summary>
        /// 没有timeline配置参数
        /// </summary>
        NONE_TIMELINE_META,
    }


}