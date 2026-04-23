using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;

namespace  Aquila.Fight
{
    /// <summary>
    /// 受击触发修改属性effect
    /// </summary>
    public class EffectSpec_OnHitted_Trigger_ModifyAttr : EffectSpec_Base/*,IHitted_Trigger_Effect*/
    {
        public bool CanApplyEffect(HittedTriggerEffectParam param)
        {
            _cumulation += param._effectedValue;
            var canApply = false;
            switch ((effect_mod_attr_condition)_effectData.GetIntParam1())
            {
                //按百分比，要拿出基数计算一下
                case effect_mod_attr_condition.Percentage:
                {
                    // var actionInstnace = Target == 0 ? param._castor : param._target;
                    var addon = param._target.GetAddon<Addon_BaseAttrNumric>();
                    //拿要检查的属性
                    var attrType = _effectData.GetIntParam3();
                    if (attrType >= (int)actor_attribute.Max ||
                        attrType < 0)
                    {
                        Tools.Logger.Error("<color=red>attrType >= (int)actor_attribute.Max || attrType < 0</color>");
                        return false;
                    }

                    var baseVal = addon.GetCorrectionValue((actor_attribute)attrType, 0f);
                    canApply = Tools.Ability.CanApplyModifyAttrByEffect_ByPercentage
                        (
                            baseVal,
                            _effectData.GetFloatParam1(),
                            _cumulation
                        );
                }
                break;
                
                //按固定数值
                case effect_mod_attr_condition.FixedValue:
                    canApply = Tools.Ability.CanApplyModifyAttrEffect_ByFixed(_effectData.GetFloatParam1(), _cumulation);
                    break;
            }
            
            if (canApply)
                _cumulation = 0f;
            
            return canApply;
        }

        public override void Init(EffectData data, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null)
        {
            base.Init(data, castor, target);
            _cumulation = 0f;
        }
        
        // public override void Init(Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
        //     Module_ProxyActor.ActorInstance target = null)
        // {
        //     base.Init(meta, castor, target);
        //     _cumulation = 0f;
        // }

        public override void OnEffectEnd(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            base.OnEffectEnd(castor, target);
        }

        /// <summary>
        /// 应用effect
        /// </summary>
        public override void Apply(Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target)
        {
            base.Apply(castor, target);
        }

        public override void Clear()
        {
            base.Clear();
            _cumulation = 0f;
        }
        
        /// <summary>
        /// 当actor受击触发       
        /// </summary>
        private void OnHitted(int eventType, object param)
        {
            //#todo这块的逻辑被注释掉了，因为不打算要ihitted接口了，后面处理下
            // var hitParam = param as OnActorHittedParam;
            // if (hitParam is null)
            //     return;
            //
            // var canApplyParam = ReferencePool.Acquire<HittedTriggerEffectParam>();
            // canApplyParam._effectedValue += hitParam._dealedDamage;
            // canApplyParam._castor         = hitParam._castor;
            // canApplyParam._target         = hitParam._target;
            // if(CanApplyEffect(canApplyParam))
            //     GameEntry.Module.GetModule<Module_ProxyActor>().ApplyEffect(hitParam._castor,hitParam._target,this);
        }
        
        /// <summary>
        /// 累计值
        /// </summary>
        private float _cumulation;
    }
    
    public class EffectSpec_OnHitted_Trigger_ModifyAttrParam : IReference
    {
        public void Clear()
        {
            _changedAttr = actor_attribute.Invalid;
            _dirtyCorrectionValue = 0f;
        }

        /// <summary>
        /// 发生变更之前的脏值
        /// </summary>
        public float _dirtyCorrectionValue;
        
        /// <summary>改变的属性 </summary>
        public actor_attribute _changedAttr = actor_attribute.Invalid;
    }
    
    public class OnActorHittedParam : IReference
    {
        public void Clear()
        {
            _castor = null;
            _target = null;
            _dealedDamage = 0;
        }
        
        public Module_ProxyActor.ActorInstance _castor;
        public Module_ProxyActor.ActorInstance _target;
        public int _dealedDamage;
    }
}
