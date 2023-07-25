using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight
{
    public class ActorBehaviour_Die : ActorBehaviour_Base
    {
        public ActorBehaviour_Die( ActorInstance ins ) : base( ins )
        {
        }

        public override void Exec( object param )
        {
        }

        public override ActorBehaviourTypeEnum BehaviourType => ActorBehaviourTypeEnum.DIE;
    }

}
