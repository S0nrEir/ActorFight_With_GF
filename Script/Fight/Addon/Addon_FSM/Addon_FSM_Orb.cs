using System.Collections.Generic;
using Aquila.Fight.Addon;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 法球的状态类型
    /// </summary>
    public class Addon_FSM_Orb : Addon_FSM
    {
        public override List<ActorState_Base> StateList => new List<ActorState_Base>()
        {
            new ActorState_Orb_Ability((int)ActorStateTypeEnum.ABILITY_STATE)
        };
    }
}
