using Aquila.Event;
using Aquila.Module;

namespace Aquila.Fight
{
    /// <summary>
    /// 为actor添加tag
    /// </summary>
    public class EffectSpec_PeriodActorTag : EffectSpec_Base
    {
        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            base.Apply( castor, target, result );
            //添加tag
        }

        public override void OnEffectEnd( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            base.OnEffectEnd( castor, target );
        }
    }

}
