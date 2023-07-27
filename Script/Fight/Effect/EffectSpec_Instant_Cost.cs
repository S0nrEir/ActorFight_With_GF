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
            //todo_modifier每次初始化都要走子类的重写，这块能不能改一下
            //_modifier = new Numric.Numric_Modifier( meta.ModifierType,meta.ExtensionParam.FloatParam_1 );
            _modifier.Setup( meta.ModifierType, meta.ExtensionParam.FloatParam_1 );
        }

        public EffectSpec_Instant_Cost()
        {

        }
    }
   
}
