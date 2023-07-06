using Aquila.Event;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Common;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 定期派发子Effect
    /// </summary>
    public class EffectSpec_Period_Deriving : EffectSpec_Base
    {
        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            base.Apply( castor,target, result );
            Cfg.Common.Table_Effect meta = null;
            EffectSpec_Base newEffect = null;
            //这里并不能复用AbilitySpec的逻辑
            foreach ( var effectID in Meta.DeriveEffects )
            {
                meta = GameEntry.LuBan.Tables.Effect.Get( effectID );
                if ( meta is null )
                {
                    Log.Warning( $"<color=yellow>EffectSpec_Period_Deriging.Apply()--->meta is null,id:{effectID}</color>" );
                    continue;
                }
                newEffect = Tools.Ability.CreateEffectSpecByReferencePool( meta );
                if ( newEffect is null )
                {
                    Log.Warning( $"EffectSpec_Period_Deriving.Apply()--->newEffect is null,effectMeta:{meta.ToString()}" );
                    break;
                }

                if ( newEffect.Meta.Policy != DurationPolicy.Instant )
                {
                    GameEntry.Impact.Attach( newEffect, castor.Actor.ActorID, target.Actor.ActorID );
                }
                else
                {
                    newEffect.Apply( castor, target, result );
                    newEffect.OnEffectEnd();
                    ReferencePool.Release( newEffect );
                }
            }
        }

        public override void Init( Table_Effect meta )
        {
            base.Init( meta );
            StackLimit = meta.ExtensionParam.IntParam_1;
            ResetWhenOverride = meta.ExtensionParam.IntParam_2 == 1;
        }

        public override void Clear()
        {
            StackCount = 1;
            base.Clear();
        }
    }
}
