using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Common;
using Cfg.Enum;
using GameFramework;
using UnityGameFramework.Runtime;

namespace  Aquila.Fight
{
    /// <summary>
    /// 受击触发修改属性effect
    /// </summary>
    public class EffectSpec_OnHitted_Trigger_ModifyAttr : EffectSpec_Base,IHitted_Trigger_Effect
    {
        public bool CanApplyEffect(HittedTriggerEffectParam param)
        {
            _cumulation += param._effectedValue;
            var canApply = false;
            switch ((Cfg.Enum.effect_mod_attr_condition)Meta.ExtensionParam.IntParam_1)
            {
                //按百分比，要拿出基数计算一下
                case effect_mod_attr_condition.Percentage:
                {
                    var actionInstnace = Meta.Target == 0 ? param._castor : param._target;
                    var addon = actionInstnace.GetAddon<Addon_BaseAttrNumric>();
                    var baseVal = addon.GetCorrectionValue((actor_attribute)Meta.ExtensionParam.IntParam_3, 0f);
                    canApply = Tools.Ability.CanApplyModifyAttrByEffect_ByPercentage
                        (
                            baseVal,
                            _cumulation,
                            Meta.ExtensionParam.FloatParam_1
                        );
                }
                break;
                
                //按固定数值
                case effect_mod_attr_condition.FixedValue:
                    canApply = Tools.Ability.CanApplyModifyAttrEffect_ByFixed(Meta.ExtensionParam.FloatParam_1, _cumulation);
                    break;
            }
            
            if (canApply)
                _cumulation = 0f;
            
            return canApply;
        }

        public override void Init(Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null)
        {
            base.Init(meta,castor,target);
            _cumulation = 0f;
        }

        public override void OnEffectEnd(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            base.OnEffectEnd(castor, target);
            //移除时移除修饰器
            Aquila.GameEntry.Module.GetModule<Module_ProxyActor>().RemoveModifierFromActor_Effect
                (
                    Meta.Target == 0 ? castor : target,
                    _modifier,
                    (actor_attribute)Meta.ExtensionParam.IntParam_2 
                );
        }

        /// <summary>
        /// 应用effect
        /// </summary>
        public override void Apply(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result)
        {
            base.Apply(castor, target, result);
            _modifier.Setup(Meta.ModifierType,Meta.ExtensionParam.FloatParam_2);
            if (!Aquila.GameEntry.Module.GetModule<Module_ProxyActor>().AddModifierToActor_Effect
                (
                    Meta.Target == 0 ? castor : target,
                    _modifier,
                    (actor_attribute)Meta.ExtensionParam.IntParam_2
                ))
                Log.Warning("<color=yellow>faild to modify actor attribute</color>");
        }

        public override void Clear()
        {
            base.Clear();
            _cumulation = 0f;
        }

        /// <summary>
        /// 累计值
        /// </summary>
        private float _cumulation = 0f;
    }
    
    public class EffectSpec_OnHitted_Trigger_ModifyAttrParam : IReference
    {
        public EffectSpec_OnHitted_Trigger_ModifyAttrParam()
        {
        }

        public void Clear()
        {
            _changedAttr = actor_attribute.Invalid;
            _dirtyCorrectionValue = 0f;
        }

        /// <summary>
        /// 发生变更之前的值
        /// </summary>
        public float _dirtyCorrectionValue = 0f;
        
        /// <summary>改变的属性 </summary>
        public actor_attribute _changedAttr = actor_attribute.Invalid;
    }
}
