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
        public EffectSpec_CoolDown(Effect meta_) : base(meta_)
        {
        }

        public override void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
   
}
