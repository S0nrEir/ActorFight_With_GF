using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Cfg.Common;

namespace Aquila.Fight
{
    /// <summary>
    /// 消耗类effect
    /// </summary>
    public class EffectSpec_Cost : EffectSpec_Base
    {
        public override void Apply( Module_ProxyActor.ActorInstance instance, AbilityResult_Hit result )
        {
            var attr_addon = instance.GetAddon<Addon_BaseAttrNumric>();
            if(attr_addon is null)
                return;
             
            var curr_value = attr_addon.GetCurrMPCorrection();
            curr_value += Meta.ExtensionParam.FloatParam_1;
            attr_addon.SetCurrMP(curr_value);
        }

        /// <summary>
        /// 计算消耗后的值
        /// </summary>
        public float Calc(float valToModify)
        {
            return _modifier.Calc(valToModify);
        }

        public override void Init( Table_Effect meta )
        {
            base.Init( meta );
            _modifier = new Numric.Numric_Modifier( meta.ModifierType,meta.ExtensionParam.FloatParam_1 );
        }

        public EffectSpec_Cost()
        {

        }
    }
   
}
