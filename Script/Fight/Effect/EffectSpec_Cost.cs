using System.Collections;
using System.Collections.Generic;
using Aquila.Numric;
using Cfg.common;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 消耗类effect
    /// </summary>
    public class EffectSpec_Cost : EffectSpec_Base
    {
        /// <summary>
        /// 计算消耗后的值
        /// </summary>
        public float Calc(float val_to_modify_)
        {
            return _modifier.Calc(val_to_modify_);
        }

        public EffectSpec_Cost(Effect meta_) : base(meta_)
        {
            _modifier = ReferencePool.Acquire<Numric_Modifier>();
            _modifier.Setup(meta_.ModifierType,meta_.ModifierNumric);
        }
        
        public override void Clear()
        {
            ReferencePool.Release(_modifier);
            _modifier = null;
        }
        
        /// <summary>
        /// 对应的数值修改器
        /// </summary>
        private Numric_Modifier _modifier = null;

    }
   
}
