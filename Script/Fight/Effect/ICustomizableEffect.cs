using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 自定义数值类型effect
    /// </summary>
    public interface ICustomizableEffect
    {
        /// <summary>
        /// 设置数值修改器的数值
        /// </summary
        public void SetModifier( EffectSpec_Base parent);
    }

}
