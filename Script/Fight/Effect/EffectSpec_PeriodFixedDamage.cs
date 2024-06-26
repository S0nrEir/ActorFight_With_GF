using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Common;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 周期性固定伤害
    /// </summary>
    public class EffectSpec_Period_FixedDamage : EffectSpec_Base
    {
        public override void Init( Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null )
        {
            base.Init( meta ,castor,target );
            _modifier.Setup( meta.ModifierType, meta.ExtensionParam.FloatParam_1 );
        }

        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            //get damage
            var addon = target.GetAddon<Addon_BaseAttrNumric>();
            if ( addon is null )
            {
                Log.Warning( $"EffectSpec_PeriodFixedDamage.Apply()--->addon is null" );
                return;
            }

            var currHP = addon.GetCurrHPCorrection();
            currHP = _modifier.Calc( currHP );
            addon.SetCurrHP( currHP );
            result._dealedDamage = Tools.Fight.AddDealedDamage( result._dealedDamage, ( int ) Mathf.Abs( _modifier.ValueFac() ) );
        }
    }
}