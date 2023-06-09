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
            var res = attr_addon.SetCurrMP(curr_value);
        }

        /// <summary>
        /// 计算消耗后的值
        /// </summary>
        public float Calc(float val_to_modify)
        {
            return _modifier.Calc(val_to_modify);
        }

        public EffectSpec_Cost(Table_Effect meta) : base(meta)
        {
        }

    }
   
}
