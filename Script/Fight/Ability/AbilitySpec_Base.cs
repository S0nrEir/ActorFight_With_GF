using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Aquila.GameTag;
using Cfg.common;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
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
            return CanUseAbility();
        }

        /// <summary>
        /// 是否可使用技能，可以返回true
        /// </summary>
        public virtual bool CanUseAbility()
        {
            return CostOK() && CDOK();
        }

        public virtual void Clear()
        {
            //处理CD和Cost
            Meta = null;
            _ability_tag = null;
            _cd_effect = null;
            _cost_effect = null;
        }
        
        //-------------------priv-------------------
        /// <summary>
        /// 检查技能冷却
        /// </summary>
        private bool CDOK()
        {
            
        }

        /// <summary>
        /// 检查技能消耗
        /// </summary>
        private bool CostOK()
        {
            if (_cost_effect != null)
            {
                
            }

            return true;
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
        private EffectSpec_Base _cd_effect = null;

        /// <summary>
        /// 技能消耗
        /// </summary>
        private EffectSpec_Base _cost_effect = null;
        
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
                    _cd_effect = new EffectSpec_Base(effect_meta);
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
                    _cost_effect = new EffectSpec_Base(effect_meta);
                    return;
                }
            }
        }

        /// <summary>
        /// 根据表格配置生成一个spec实例
        /// </summary>
        public static AbilitySpecBase Gen( AbilityBase meta_ )
        {
            var spec = ReferencePool.Acquire<AbilitySpecBase>();
            spec.Setup(meta_);
            return null;
        }
    }
}

