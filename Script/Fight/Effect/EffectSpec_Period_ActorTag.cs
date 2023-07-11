using Aquila.Event;
using Aquila.Module;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 为actor添加tag
    /// </summary>
    public class EffectSpec_Period_ActorTag : EffectSpec_Base
    {
        public override void OnEffectAwake( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            base.OnEffectAwake( castor, target );
            //添加tag
            target.Actor.AddTag( (ushort) (ActorTagType) Meta.ExtensionParam.IntParam_1 );
        }

        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            base.Apply( castor, target, result );
        }

        public override void OnEffectEnd( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            base.OnEffectEnd( castor, target );
            //移除tag
            target.Actor.RemoveTag( ( ushort ) ( ActorTagType ) Meta.ExtensionParam.IntParam_1 );
        }
    }

}
