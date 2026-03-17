using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Cfg.Common;

namespace Aquila.Fight
{
    /// <summary>
    /// 消耗类effect
    /// </summary>
    public class EffectSpec_Instant_Cost : EffectSpec_Base
    {
        public override void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            var attr_addon = target.GetAddon<Addon_BaseAttrNumric>();
            if(attr_addon is null)
                return;
             
            var curr_value = attr_addon.GetCurrMPCorrection();
            curr_value += _effectData.GetFloatParam1();
            attr_addon.SetCurrMP(curr_value);
        }

        /// <summary>
        /// 计算消耗后的值
        /// </summary>
        public float Calc(float valToModify)
        {
            return _modifier.Calc(valToModify);
        }
        
        public override void Init(EffectData data, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null)
        {
            base.Init(data, castor, target);
            _modifier.Setup(ModifierType, _effectData.GetFloatParam1());
        }
        
        // public override void Init(Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
        //     Module_ProxyActor.ActorInstance target = null)
        // {
        //     base.Init(meta, castor, target);
        //     _modifier.Setup(ModifierType, _effectData.GetFloatParam1());
        // }

        public EffectSpec_Instant_Cost()
        {

        }
    }
   
}
