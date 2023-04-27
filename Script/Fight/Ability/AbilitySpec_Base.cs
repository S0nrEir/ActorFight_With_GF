using System;
using Aquila.GameTag;
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
        public void Remove(UInt32 bit_to_remove_)
        {
            _ability_tag.Remove(bit_to_remove_ - 1);
        }

        /// <summary>
        /// 添加tag
        /// </summary>
        public void Add(UInt32 bit_to_add_)
        {
            _ability_tag.Add(bit_to_add_ - 1);
        }

        /// <summary>
        /// 包含某个tag
        /// </summary>
        public bool ContainsTag(UInt32 bit_tag_)
        {
            return _ability_tag.Contains(bit_tag_);
        }

        public virtual void Setup(AbilityBase meta_)
        {
            Meta = meta_;
            InitCDEffect();
            InitCostEffect();
        }
        
        /// <summary>
        /// 使用技能
        /// </summary>
        public virtual bool UseAbility()
        {
            OnPreAbility();
            //刷新CD
            _cd_effect._remain = _cd_effect._total_duration;
            //todo扣除cost
            OnAfterAbility();
            return true;
        }

        /// <summary>
        /// 使用技能前置函数
        /// </summary>
        public virtual void OnPreAbility()
        {
            
        }

        /// <summary>
        /// 使用技能后置函数
        /// </summary>
        public virtual void OnAfterAbility()
        {
            
        }

        /// <summary>
        /// 是否可使用技能，可以返回true
        /// </summary>
        public virtual bool CanUseAbility()
        {
            return CostOK() && CDOK();
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public virtual void Clear()
        {
            //处理CD和Cost
            Meta = null;
            _ability_tag = null;
            _cd_effect.Clear();
            _cost_effect.Clear();
            _cd_effect = null;
            _cost_effect = null;
        }

        /// <summary>
        /// 刷帧，处理CD
        /// </summary>
        public virtual void OnUpdate(float delta_time_)
        {
            _cd_effect._remain -= delta_time_;
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
        /// 检查技能消耗
        /// </summary>
        private bool CostOK()
        {
            return true;
            // if (_cost_effect is null)
            //     return true;
            //
            // //#todo技能消耗目前暂时只消耗魔法值，剩下的后面再补
            // //因为都独立开来了，拿不到技能所属的addon和actor，暂时想到的解决办法：通过proxy拿他们的代理实例
            // var meta = _cost_effect.Meta;
            // if(meta.ModifierType == )
            // return _cost_effect.Calc() > 0;
        }

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
        
        public AbilitySpecBase()
        {
            _ability_tag = new TagContainer();
        }

        // private bool _active = false;
        
        /// <summary>
        /// 初始化技能CD相关逻辑
        /// </summary>
        private void InitCDEffect()
        {
            _cd_effect = null;
            if(Meta is null)
                return;

            Effect effect_meta = null;
            foreach (var effect_id in Meta.effects)
            {
                effect_meta = GameEntry.DataTable.Table<TB_Effect>().Get(effect_id);
                if (effect_meta is null)
                {
                    Log.Warning($"effect_meta is null,id:{effect_id}");
                    continue;
                }

                if (effect_meta.Type == EffectType.CoolDown)
                {
                    _cd_effect = new EffectSpec_CoolDown(effect_meta);
                    return;
                }
            }
        }

        /// <summary>
        /// 初始化Cost相关逻辑
        /// </summary>
        private void InitCostEffect()
        {
            _cost_effect = null;
            if(Meta is null)
                return;

            Effect effect_meta = null;
            foreach (var effect_id in Meta.effects)
            {
                effect_meta = GameEntry.DataTable.Table<TB_Effect>().Get(effect_id);
                if (effect_meta is null)
                {
                    Log.Warning($"effect_meta is null,id:{effect_id}");
                    continue;
                }

                if (effect_meta.Type == EffectType.Cost)
                {
                    _cost_effect = new EffectSpec_Cost(effect_meta);
                    return;
                }
            }
        }
        
        /// <summary>
        /// 根据表格配置生成一个spec实例
        /// </summary>
        /// <param name="meta_">技能元数据</param>
        /// <param name="addon_arr_">携带的各个组件</param>
        /// <returns></returns>
        public static AbilitySpecBase Gen( AbilityBase meta_)
        {
            var spec = ReferencePool.Acquire<AbilitySpecBase>();
            spec.Setup(meta_);
            return null;
        }
    }
}

