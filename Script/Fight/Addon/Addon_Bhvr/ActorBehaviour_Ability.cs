using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.Toolkit;
using Cfg.Fight;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight
{
    /// <summary>
    /// 行为 使用技能
    /// </summary>
    public class ActorBehaviour_Ability : ActorBehaviour_Base
    {
        public override void Exec( object param )
        {
            var addon = _instance.GetAddon<Addon_FSM>();
            if ( addon is null )
            {
                Log.Warning( $"fsm addon is null" );
                return;
            }

            if ( addon.CurrState != FSM.ActorStateTypeEnum.IDLE_STATE && addon.CurrState != FSM.ActorStateTypeEnum.MOVE_STATE )
            {
                Log.Warning( $" addon.CurrState != FSM.ActorStateTypeEnum.IDLE_STATE || addon.CurrState != FSM.ActorStateTypeEnum.MOVE_STATE" );
                return;
            }

            addon.SwitchTo( ActorStateTypeEnum.ABILITY_STATE, param, null );
        }

        public ActorBehaviour_Ability( ActorInstance instance ) : base( instance )
        {

        }

        public override ActorBehaviourTypeEnum BehaviourType => ActorBehaviourTypeEnum.ABILITY;

        /// <summary>
        /// 施法者ActorID
        /// </summary>
        private int _castorID = -1;

        /// <summary>
        /// 使用状态结果
        /// </summary>
        private AbilityResult_Use _result = null;

        /// <summary>
        /// 技能数据
        /// </summary>
        private Table_AbilityBase _abilityMeta = null;

        /// <summary>
        /// 技能timeline配置
        /// </summary>
        private Table_AbilityTimeline _timelineMeta = null;
    }

    /// <summary>
    /// 技能行为 参数
    /// </summary>
    public class ActorBehaviour_Ability_Param
    {
        public int _abilityMetaID = -1;
    }

}