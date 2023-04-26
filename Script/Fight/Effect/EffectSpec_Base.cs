using System.Collections;
using System.Collections.Generic;
using Aquila.Numric;
using Cfg.common;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// effect逻辑基类
    /// </summary>
    public class EffectSpec_Base
    {
        /// <summary>
        /// 获取modifier修改后的值
        /// </summary>
        public float GetValueAfterModify(float val_to_modify_)
        {
            return Modifier.Calc(val_to_modify_);
        }

        public EffectSpec_Base(Effect meta_)
        {
            Meta = meta_;
            Modifier = ReferencePool.Acquire<Numric_Modifier>();
            Modifier.Setup(meta_.ModifierType,meta_.ModifierNumric);
        }

        public Effect Meta { get; private set; } = null;

        public Numric_Modifier Modifier { get; private set; } = null;
    }
   
}
