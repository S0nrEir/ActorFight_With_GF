using System.Collections;
using System.Collections.Generic;
using Aquila.Fight.FSM;
using Cfg.common;
using UnityEngine;

namespace Aquila.Fight.Actor
{
    //hero actor的各个实现
    public partial class HeroActor : 
        ISwitchStateBehavior,
        IDieBehavior,
        IDoAbilityBehavior
    {
        /// <summary>
        /// 死亡
        /// </summary>
        public void Die()
        {
            SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
        }
        
        /// <summary>
        /// 切换状态
        /// </summary>
        public void SwitchTo( ActorStateTypeEnum state_type, object[] enter_param, object[] exist_param )
        {
            _fsm_addon.SwitchTo( state_type, enter_param, exist_param );
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        public void UseAbility(object param)
        {   
            SwitchTo(ActorStateTypeEnum.ABILITY_STATE,new object[]{param},null);
        }
    }
}
