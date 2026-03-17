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
            EffectSpec_Base newEffect = null;
            foreach ( var effectID in _effectData.GetDeriveEffects() )
            {
                if (GameEntry.AbilityPool.TryGetEffect(effectID, out var effectData))
                {
                    newEffect = Tools.Ability.CreateEffectSpecByReferencePool(effectData, castor, target);
                }
                else
                {
                    Log.Warning($"<color=yellow>EffectSpec_Period_DerivingStack.Apply --> faild to get deriveEffect,id:{effectID}</color>");
                    // 回退到 LuBan
                    // var tempMeta = GameEntry.LuBan.Tables.Effect.Get(effectID);
                    // if (tempMeta == null)
                    // {
                    //     Log.Warning($"<color=yellow>EffectSpec_Period_DerivingStack.Apply()--->effect not found, id:{effectID}</color>");
                    //     continue;
                    // }
                    // newEffect = Tools.Ability.CreateEffectSpecByReferencePool(tempMeta, castor, target);
                }
                
                if ( newEffect is null )
                {
                    Log.Warning( $"<color=yellow>EffectSpec_Period_Deriving.Apply()--->newEffect is null, effectID:{effectID}</color>" );
                    continue;
                }

                if ( newEffect.Policy != DurationPolicy.Instant )
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

        public override void Init(EffectData data, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null)
        {
            base.Init(data, castor, target);
            StackLimit = _effectData.GetIntParam1();
            ResetWhenOverride = _effectData.GetIntParam2() == 1;
        }
        
        // public override void Init( Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
        //     Module_ProxyActor.ActorInstance target = null )
        // {
        //     base.Init( meta ,castor,target);
        //     StackLimit = IntParam1;
        //     ResetWhenOverride = _effectData.GetIntParam2() == 1;
        // }

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
