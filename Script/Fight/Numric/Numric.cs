using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Numric
{
    #region pub

    /// <summary>
    /// 描述一个数值类型
    /// </summary>
    public class Numric : IReference
    {
        /// <summary>
        /// 最终计算后的数值
        /// </summary>
        public float Value => _value;

        /// <summary>
        /// 设置数值
        /// </summary>
        public void Setup(float base_val,float equip_add,float buff_add,float class_add)
        {
            _base_val  = base_val;
            _class_add = class_add;
            _equip_add = equip_add;
            _buff_add  = buff_add;
            ReCalc();
        }

        public void Clear()
        {
        }

        #endregion

        #region priv

        /// <summary>
        /// 重新计算所有数值并设置到最终值
        /// </summary>
        private float ReCalc()
        {
            _value = _base_val + _equip_add + _buff_add + _class_add;
            return _value;
        }

        #endregion

        #region fields

        /// <summary>
        /// 基础值
        /// </summary>
        private float _base_val = 0f;

        /// <summary>
        /// 装备修正
        /// </summary>
        private float _equip_add = 0f;

        /// <summary>
        /// 职业修正
        /// </summary>
        private float _class_add = 0f;

        /// <summary>
        /// buff修正
        /// </summary>
        private float _buff_add = 0f;

        /// <summary>
        /// 最终计算后的数值
        /// </summary>
        private float _value = 0f;

        #endregion
    }
}