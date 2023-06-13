namespace Aquila.Fight.FSM
{
    /// <summary>
    /// Actor通用状态枚举
    /// </summary>
    public enum ActorStateTypeEnum
    {
        /// <summary>
        /// 无效状态
        /// </summary>
        INVALID = -1,

        /// <summary>
        /// 英雄待机
        /// </summary>
        IDLE_STATE = 0,

        /// <summary>
        /// 移动
        /// </summary>
        MOVE_STATE = 1,

        /// <summary>
        /// 技能 SKILL_1
        /// </summary>
        ABILITY_STATE = 2,

        /// <summary>
        /// 死亡
        /// </summary>
        DIE_STATE = 3,

        ///// <summary>
        ///// 胜利
        ///// </summary>
        //WIN_STATE = 20,

        ///// <summary>
        ///// 索敌
        ///// </summary>
        //SEARCHING_STATE = 25,
    }
}

