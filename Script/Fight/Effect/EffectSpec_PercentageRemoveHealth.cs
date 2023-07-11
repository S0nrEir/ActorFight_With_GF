using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Cfg.Enum;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 基于生命值上限百分比移除生命值
    /// </summary>
    public class EffectSpec_RemoveHealth : EffectSpec_Base, ICustomizeEffect
    {
        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            base.Apply( castor, target, result );
            var addon = target.GetAddon<Addon_BaseAttrNumric>();
            if ( addon is null )
            {
                Log.Warning( $"<color=yellow>EffectSpec_RemoveHealth.Apply()--->addon is null</color>" );
                return;
            }
            if ( !_initFlag )
            {
                var res = addon.GetCorrectionFinalValue( Actor_Base_Attr.HP, 0f );
                _maxHP = res.value;
            }

            var removeVal = _modifier.Calc( _maxHP );
            var curr = addon.GetCurrHPCorrection();
            addon.SetCurrHP( curr - removeVal );
            result._dealedDamage += ( int ) removeVal;
        }

        public void SetModifier( float fac )
        {
            _modifier.Setup( Meta.ModifierType, fac );
        }

        public override void Clear()
        {
            _maxHP = -1f;
            _initFlag = false;
            base.Clear();
        }

        /// <summary>
        /// 记录buff生效时的最大hp
        /// </summary>
        private float _maxHP = -1f;

        /// <summary>
        /// 初始化标记
        /// </summary>
        private bool _initFlag = false;
    }
}
