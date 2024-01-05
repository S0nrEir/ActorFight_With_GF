using Aquila.Fight.Addon;
using Aquila.Module;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 英雄状态addon by yhc
    /// </summary>
    public class Addon_FSM_Hero : Addon_FSM
    {
        public override List<ActorState_Base> StateList => new List<ActorState_Base>
        {
            new ActorState_HeroIdle   ((int)ActorStateTypeEnum.IDLE_STATE),
            new ActorState_HeroMove   ((int)ActorStateTypeEnum.MOVE_STATE),
            new ActorState_HeroAbility((int)ActorStateTypeEnum.ABILITY_STATE),
            new ActorState_HeroDie    ((int)ActorStateTypeEnum.DIE_STATE),
        };

    }
    
}