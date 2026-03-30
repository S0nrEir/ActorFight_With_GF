using Aquila.Module;
using Aquila.Toolkit;
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
            var mainType = (ActorTagType)_effectData.GetIntParam1();
            var tagToAdd = _effectData.GetIntParam2();
            target.Actor.AddTag( mainType,(ushort)tagToAdd ,
                (currTag, tagToRemove, isAdd) =>
                {
                    Tools.Logger.Info($"<color=white>add tag , main type:{mainType} , sub type:{tagToAdd}</color>");
                });
            target.Actor.AddTag(mainType,(ushort)tagToAdd, (currTag, tagToRemove, isAdd) =>
            {
                Tools.Logger.Info($"<color=white>add tag , main type:{mainType} , sub type:{tagToAdd}</color>");
                Tools.Logger.Info($"<color=green>add tag , main type:{mainType} , sub type:{tagToAdd}</color>");
            });
        }

        public override void OnEffectEnd( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            base.OnEffectEnd( castor, target );
            //移除tag
            var mainType = (ActorTagType)_effectData.GetIntParam1();
            var tagToRemove = _effectData.GetIntParam2();
            target.Actor.RemoveTag( mainType,tagToRemove ,
                (currTag, tagToRemove, isAdd) =>
                {
                    Tools.Logger.Info($"<color=white>remove tag , main type:{mainType} , sub type:{tagToRemove}</color>");
                });
        }
    }

}
