using System.Collections;
using System.Collections.Generic;
using Aquila.Numric;
using Cfg.common;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 冷却类effect
    /// </summary>
    public class EffectSpec_CoolDown : EffectSpec_Base
    {
        public EffectSpec_CoolDown(Effect meta) : base(meta)
        {
            _total_duration = meta.ModifierNumric;
            _remain = 0f;
        }
        
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float _remain = 0f;
        
        /// <summary>
        /// cool down
        /// </summary>
        public float _total_duration = 0f;
    }
   
}
