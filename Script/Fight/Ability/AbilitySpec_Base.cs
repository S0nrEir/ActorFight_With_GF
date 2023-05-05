using System;
using System.Collections.Generic;
using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.GameTag;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.common;
using Cfg.Enum;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 技能逻辑基类
    /// </summary>
    public /*abstract*/ class AbilitySpecBase : IReference
    {
        //CoolDown和Cost：拿CD和Cost类型的GE数据
        //根据GE数据创建对应GESpec
        //Spec持有数据
        //EffectScriptableObject翻译成配表，包含CD和Cost
        //#todo为tag添加移除和添加时的回调
        //子弹类技能怎么配置：技能表添加类型，比如召唤物，子弹类，蓄力，位移等，根绝类型生成特殊的spec，使用技能时加载actor，路径就放在
        
        /// <summary>
        /// 移除tag
        /// </summary>
        public void Remove(UInt32 bit_to_remove)
        {
            _ability_tag.Remove(bit_to_remove - 1);
        }

        /// <summary>
        /// 添加tag
        /// </summary>
        public void Add(UInt32 bit_to_add)
        {
            _ability_tag.Add(bit_to_add - 1);
        }

        /// <summary>
        /// 包含某个tag
        /// </summary>
        public bool ContainsTag(UInt32 bit_tag)
        {
            return _ability_tag.Contains(bit_tag);
        }

        public virtual void Setup(AbilityBase meta)
        {
            Meta = meta;
            InitEffectSpec();
        }
        
        /// <summary>
        /// 使用技能
        /// </summary>
        public virtual bool UseAbility(Module_Proxy_Actor.ActorInstance instance,ref AbilityResult result)
        {
            if (!OnPreAbility(ref result))
                return false;
            
            //刷新CD
            if (_cd_effect != null)
                _cd_effect._remain = _cd_effect._total_duration;
            
            //扣除cost
            if(_cost_effect != null)
                _cost_effect.Apply(_owner,ref result);

            foreach (var effect in _effect_list)
                effect.Apply(effect.Meta.Target_None == 1 ? instance : _owner,ref result);

            if (!OnAfterAbility(ref result))
                return false;
            
            result.SetState(AbilityResultDescTypeEnum.HIT);
            return true;
        }

        /// <summary>
        /// 使用技能前置函数
        /// </summary>
        public virtual bool OnPreAbility(ref AbilityResult result)
        {
            return true;
        }

        /// <summary>
        /// 使用技能后置函数
        /// </summary>
        public virtual bool OnAfterAbility(ref AbilityResult result)
        {
            return true;
        }

        /// <summary>
        /// 是否可使用技能，可以返回true
        /// </summary>
        public virtual bool CanUseAbility(ref AbilityResult result)
        {
            var succ = true;
            if (!CostOK())
            {
                succ = false;
                result.SetState(AbilityResultDescTypeEnum.COST_NOT_ENOUGH);
            }

            if (!CDOK())
            {
                succ = false;
                result.SetState(AbilityResultDescTypeEnum.CD_NOT_OK);
            }

            return succ;
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public virtual void Clear()
        {
            if (_effect_list != null && _effect_list.Count != 0)
            {
                foreach (var effect in _effect_list)
                    effect?.Clear();
            
                _effect_list.Clear();    
            }

            _effect_list = null;
            Meta         = null;
            _ability_tag = null;
            _cd_effect   = null;
            _cost_effect = null;
            _owner       = null;
            //处理CD和Cost
            _cost_effect?.Clear();
            _cd_effect?.Clear();
        }

        /// <summary>
        /// 刷帧，处理CD
        /// </summary>
        public virtual void OnUpdate(float delta_time)
        {
            _cd_effect._remain -= delta_time;
        }

        //-------------------priv-------------------
        /// <summary>
        /// 检查技能冷却
        /// </summary>
        private bool CDOK()
        {
            return _cd_effect._remain <= 0f;
        }
        
        /// <summary>
        /// 拿到该技能持有的指定effectSpec，拿不到返回null
        /// </summary>
        protected EffectSpec_Base GetSpec(int id)
        {
            foreach (var effect_spec in _effect_list)
            {
                if (effect_spec.Meta.id == id)
                    return effect_spec;
            }

            return null;
        }

        /// <summary>
        /// 检查技能消耗
        /// </summary>
        private bool CostOK()
        {
            if (_cost_effect is null)
                return true;
            
            //#todo技能消耗目前暂时只消耗魔法值，剩下的后面再补
            //因为都独立开来了，拿不到技能所属的addon和actor，暂时想到的解决办法：通过proxy拿他们的代理实例
            var attr_addon = _owner.GetAddon<Addon_BaseAttrNumric>();
            if (attr_addon is null)
                return false;
            
            var res = attr_addon.GetCorrectionFinalValue(Actor_Attr.Curr_MP,0f);
            if(!res.get_succ)
                Log.Warning("!res.get_succ");
            
            return _cost_effect.Calc(res.value) > 0;
        }

        /// <summary>
        /// 该技能持有的effect逻辑集合
        /// </summary>
        //#todo:能不能改成不用list
        private List<EffectSpec_Base> _effect_list = null;
        
        /// <summary>
        /// 表数据
        /// </summary>
        public AbilityBase Meta { get; private set; } = null;

        /// <summary>
        /// 该技能持有的tag
        /// </summary>
        private TagContainer _ability_tag = null;

        /// <summary>
        /// 技能CD
        /// </summary>
        private EffectSpec_CoolDown _cd_effect = null;

        /// <summary>
        /// 技能消耗
        /// </summary>
        private EffectSpec_Cost _cost_effect = null;
        
        /// <summary>
        /// 持有的actor代理实例，技能的持有者
        /// </summary>
        public Module_Proxy_Actor.ActorInstance _owner = null;
        
        public AbilitySpecBase()
        {
            _ability_tag = new TagContainer();
        }

        // private bool _active = false;
        /// <summary>
        /// 初始化Cost相关逻辑
        /// </summary>
        private void InitCostEffect(Effect effect)
        {
            _cost_effect = new EffectSpec_Cost(effect);
        }
        
        /// <summary>
        /// 初始化技能CD相关逻辑
        /// </summary>
        private void InitCDEffect(Effect effect)
        {
            _cd_effect = new EffectSpec_CoolDown(effect);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitEffectSpec()
        {
            if(Meta is null || Meta.effects is null)
                return;

            //默认持有16个effect
            _effect_list = new List<EffectSpec_Base>(16);
            Effect effect_meta = null;
            foreach (var effect_id in Meta.effects)
            {
                effect_meta = GameEntry.DataTable.Table<TB_Effect>().Get(effect_id);
                if (effect_meta is null)
                {
                    Log.Warning("InitEffectSpec--->effect_meta is null");
                    continue;
                }

                switch (effect_meta.Type)
                {
                    case EffectType.Cost:
                        InitCostEffect(effect_meta);
                        break;
                    
                    case EffectType.CoolDown:
                        InitCDEffect(effect_meta);
                        break;
                    
                    default:
                        _effect_list.Add(Tools.Ability.CreateEffectSpec(effect_meta));
                        break;
                }
            }
        }
        
        /// <summary>
        /// 根据表格配置生成一个spec实例
        /// </summary>
        /// <param name="meta">技能元数据</param>
        /// <param name="instance">携带的各个组件</param>
        /// <returns></returns>
        public static AbilitySpecBase Gen( AbilityBase meta,Module_Proxy_Actor.ActorInstance instance)
        {
            var spec = ReferencePool.Acquire<AbilitySpecBase>();
            spec.Setup(meta);
            spec._owner = instance;
            return null;
        }
    }
}

