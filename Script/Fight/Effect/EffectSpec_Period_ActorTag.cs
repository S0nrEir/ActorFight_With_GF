using Aquila.Event;
using Aquila.Module;
using Cfg.Enum;
using UnityEngine;
using UnityGameFramework.Runtime;

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
            var mainType = (ActorTagType)Meta.ExtensionParam.IntParam_1;
            var tagToAdd = Meta.ExtensionParam.IntParam_2;
            target.Actor.AddTag( mainType,(ushort)tagToAdd ,
                (currTag, tagToRemove, isAdd) =>
                {
                    Log.Info($"<color=white>add tag , main type:{mainType} , sub type:{tagToAdd}</color>");
                });
        }

        public override void OnEffectEnd( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            base.OnEffectEnd( castor, target );
            //移除tag
            var mainType = (ActorTagType)Meta.ExtensionParam.IntParam_1;
            var tagToRemove = Meta.ExtensionParam.IntParam_2;
            target.Actor.RemoveTag( mainType,tagToRemove ,
                (currTag, tagToRemove, isAdd) =>
                {
                    Log.Info($"<color=white>remove tag , main type:{mainType} , sub type:{tagToRemove}</color>");
                });
        }
    }

}
