using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Aquila.GameTag;
using Cfg.common;
using GameFramework;
using UnityEngine;

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
            return true;
        }

        public virtual void Clear()
        {
            //处理CD和Cost
            Meta = null;
            _ability_tag = null;
        }
        
        /// <summary>
        /// 表数据
        /// </summary>
        public AbilityBase Meta { get; private set; } = null;

        /// <summary>
        /// 该技能持有的tag
        /// </summary>
        private TagContainer _ability_tag = null; 
        
        public AbilitySpecBase()
        {
            _ability_tag = new TagContainer();
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

