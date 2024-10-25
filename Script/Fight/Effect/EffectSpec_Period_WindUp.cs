using Aquila.Event;
using Aquila.Module;
using Cfg.Enum;
using UnityEditor;
using Tools = Aquila.Toolkit.Tools;

namespace Aquila.Fight
{
    /// <summary>
    /// 吟唱effect
    /// </summary>
    public class EffectSpec_Period_WindUp : EffectSpec_Base
    {
        public override void OnEffectAwake(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            //吟唱生效时，添加吟唱tag
            //找目标身上是否有windup tag，有就不添加
            var mainType = ActorTagType.Ability;
            var tagToAdd = (ushort)ActorTagSubType_Ability.WindUp;
            if(target.Actor.HasTag(mainType,tagToAdd))
                return;
            
            target.Actor.AddTag(mainType,tagToAdd);
            GameEntry.Event.Fire(this,EventArg_WindUp.CreateStartEventArg(Meta.Duration,target.Actor.ActorID));
        }

        public override void OnEffectEnd(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            //检查角色身上的所有tag，如果自己是最后一个才移除tag
            if (!GameEntry.Impact.FilterSpecEffect(target.Actor.ActorID, FindOtherWindUpEffect))
            {
                var mainType = ActorTagType.Ability;
                var tagToRemove = (ushort)ActorTagSubType_Ability.WindUp;
                target.Actor.RemoveTag(mainType,tagToRemove);
                GameEntry.Event.Fire(this,EventArg_WindUp.CreateStopEventArg());
            }
        }

        /// <summary>
        /// 筛选函数，找出带有附加WindUp tag的effect，有返回true
        /// </summary>
        private bool FindOtherWindUpEffect(EffectSpec_Base effect)
        {
            if (effect is null)
                return false;

            //跳过自己
            //如果还有同类型的effect
            if (effect == this)
                return false;
            
            if (effect is EffectSpec_Period_ActorTag)
            {
                //wind up 身上还有wind up类型的effect，返回true，表示有
                var mainType = effect.Meta.ExtensionParam.IntParam_1;
                var subType = effect.Meta.ExtensionParam.IntParam_2;
                
                if (mainType == (int)ActorTagType.Ability && subType == (int)ActorTagSubType_Ability.WindUp)
                    return true;
            }
            
            return false;
        }
    }
   
}
