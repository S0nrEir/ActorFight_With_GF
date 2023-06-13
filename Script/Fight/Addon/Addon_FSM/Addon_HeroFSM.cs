using Aquila.Fight.Addon;
using System.Collections.Generic;
namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 英雄状态addon by yhc
    /// </summary>
    public class Addon_HeroFSM : Addon_FSM
    {
        public override List<ActorState_Base> StateList => new List<ActorState_Base>
        {
            new ActorState_HeroIdle((int)ActorStateTypeEnum.IDLE_STATE),
            new ActorState_HeroMove((int)ActorStateTypeEnum.MOVE_STATE),
            new ActorState_HeroAbility((int)ActorStateTypeEnum.ABILITY_STATE),
            new ActorState_HeroDie((int)ActorStateTypeEnum.DIE_STATE),
        };

        public override void Reset()
        {
            base.Reset();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
    
}