using Aquila.Event;
using Aquila.Module;
using Cfg.Enum;

namespace Aquila.Fight
{
    public class EffectSpec_Period_WinUp : EffectSpec_Base
    {
        public override void OnEffectAwake(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            //直接上tag
            target.Actor.AddTag((int)ActorTagType.WindUp);
            GameEntry.Event.Fire(this,EventArg_StartWindUp.Create(Meta.ExtensionParam.FloatParam_1,target.Actor.ActorID));
        }

        public override void OnEffectEnd(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            //直接删除tag
            target.Actor.RemoveTag((int)ActorTagType.WindUp);
        }
    }
   
}
