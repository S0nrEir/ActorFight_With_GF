using Aquila.Fight.Addon;
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

        }

        public ActorBehaviour_Ability( ActorInstance instance ) : base( instance )
        {

        }

        public override ActorBehaviourTypeEnum BehaviourType => ActorBehaviourTypeEnum.ABILITY;
    }

    /// <summary>
    /// 技能行为 参数
    /// </summary>
    public class ActorBehaviour_Ability_Param
    {
        public int _abilityMetaID = -1;
    }

}