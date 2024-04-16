using Aquila.Fight.Addon;
using Aquila.Module;
using System.Collections.Generic;
using UnityEngine;
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

        // public override void OnAdd()
        // {
        //     base.OnAdd();
        //     _actorInstance.GetAddon<Addon_Event>().Register((int)AddonEventTypeEnum.ON_ACTOR_SHOW, (int)AddonType, OnActorShow);
        // }

        private void OnActorShow(int addonType, object param)
        {
            //hero的state暂时先转换到第一个state
            SwitchTo( StateList[0]._stateID, null, null );
        }

        public override void Reset()
        {
            base.Reset();
            _actorInstance.GetAddon<Addon_Event>().Register((int)AddonEventTypeEnum.ON_ACTOR_SHOW, (int)EventAddonPrioerityTypeEnum.ADDON_STATE_MATCHINE, OnActorShow);
        }
    }
    
}