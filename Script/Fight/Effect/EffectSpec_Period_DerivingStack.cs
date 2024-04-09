using Aquila.Event;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Common;
using Cfg.Enum;
using Cfg.Bean;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 定期派发子Effect,带叠层，effect生效时叠层效果加给子effect
    /// </summary>
    public class EffectSpec_Period_DerivingStack : EffectSpec_Base
    {
        public override void OnEffectAwake( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            base.OnEffectAwake( castor, target );
        }

        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            base.Apply( castor, target, result );
            Cfg.Common.Table_Effect tempMeta = null;
            EffectSpec_Base newEffect = null;
            //这里并不能复用AbilitySpec的逻辑
            foreach ( var effectID in Meta.DeriveEffects )
            {
                tempMeta = GameEntry.LuBan.Tables.Effect.Get( effectID );
                if ( tempMeta is null )
                {
                    Log.Warning( $"<color=yellow>EffectSpec_Period_Deriging.Apply()--->meta is null,id:{effectID}</color>" );
                    continue;
                }
                newEffect = Tools.Ability.CreateEffectSpecByReferencePool( tempMeta );
                if ( newEffect is null )
                {
                    Log.Warning( $"<color=yellow>EffectSpec_Period_Deriving.Apply()--->newEffect is null,effectMeta:{tempMeta.ToString()}</color>" );
                    break;
                }

                if ( newEffect.Meta.Policy != DurationPolicy.Instant )
                {
                    GameEntry.Impact.Attach( newEffect, castor.Actor.ActorID, target.Actor.ActorID );
                }
                else
                {
                    //叠层
                    if ( newEffect is ICustomizableEffect )
                        ( newEffect as ICustomizableEffect ).SetModifier( this );

                    GameEntry.Module.GetModule<Module_ProxyActor>().ApplyEffect( castor, target, newEffect );
                    GameEntry.Module.GetModule<Module_ProxyActor>().InvalidEffect( castor, target, newEffect );
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

        /// <summary>
        /// 叠加层数映射到对应的配置数值
        /// </summary>
        private static float FacToParam( int stackCount, int limit, EffectExtensionParam extension )
        {
            switch ( stackCount )
            {
                case 1:
                    return extension.FloatParam_1;

                case 2:
                    return extension.FloatParam_2;

                case 3:
                    return extension.FloatParam_3;

                case 4:
                    return extension.FloatParam_4;
            }
            return 0f;
        }
    }
}
