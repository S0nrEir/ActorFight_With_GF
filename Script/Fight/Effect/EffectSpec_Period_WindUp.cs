using Aquila.Event;
using Aquila.Module;
using Cfg.Enum;

namespace Aquila.Fight
{
    /// <summary>
    /// 吟唱effect
    /// </summary>
    public class EffectSpec_Period_WindUp : EffectSpec_Base
    {
        public override void OnEffectAwake(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            //直接上tag
            target.Actor.AddTag((int)ActorTagType.WindUp);
            GameEntry.Event.Fire(this,EventArg_WindUp.CreateStartEventArg(Meta.ExtensionParam.FloatParam_1,target.Actor.ActorID));
        }

        public override void OnEffectEnd(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            //检查角色身上的所有tag，如果自己是最后一个才移除tag
            if (!GameEntry.Impact.FilterSpecEffect(target.Actor.ActorID, FilterWindUpEffect))
                target.Actor.RemoveTag((int)ActorTagType.WindUp);
        }

        /// <summary>
        /// 筛选函数，找出带有附加WindUp tag的effect，有返回true
        /// </summary>
        private bool FilterWindUpEffect(EffectSpec_Base effect)
        {
            if (effect is null)
                return false;

            if (effect is EffectSpec_Period_ActorTag)
            {
                //wind up 身上还有wind up类型的effect，返回true，表示有
                var tagType = (int)Meta.ExtensionParam.FloatParam_1;
                if (tagType == (int)ActorTagType.WindUp)
                    return true;
            }
            
            return false;
        }
    }
   
}
