using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight
{
    /// <summary>
    /// 行为 死亡
    /// </summary>
    public class ActorBehaviour_Die : ActorBehaviour_Base
    {
        public override void Exec( object param )
        {
            var addon = _instance.GetAddon<Addon_FSM>();
            if ( addon is null )
            {
                Log.Warning( $"fsm addon is null" );
                return;
            }
            //找到所有可以取消的addon
            //_instance.Actor.Notify( ( int ) AddonEventTypeEnum.CANCEL_BHVR, null );
            addon.SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
        }

        public ActorBehaviour_Die( ActorInstance ins ) : base( ins )
        {
        }

        public override ActorBehaviourTypeEnum BehaviourType => ActorBehaviourTypeEnum.DIE;
    }

}
