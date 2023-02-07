using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Numric
{
    #region pub

    /// <summary>
    /// ����һ����ֵ����
    /// </summary>
    public class Numric : IReference
    {
        /// <summary>
        /// ���ռ�������ֵ
        /// </summary>
        public float Value
        {
            get
            {
                if ( _changed_flag )
                {
                    _changed_flag = false;
                    ReCalc();
                }
                return _value;
            }
        }

        /// <summary>
        /// ������ֵ
        /// </summary>
        public void Setup(float base_val,float equip_add,float buff_add,float class_add)
        {
            _base_val  = base_val;
            _class_add = class_add;
            _equip_add = equip_add;
            _buff_add  = buff_add;
            ReCalc();
        }

        /// <summary>
        /// ���û���ֵ
        /// </summary>
        public void SetBaseVal( float val )
        {
            _base_val = val;
            _changed_flag = true;
        }

        public void SetClassAdd( float val )
        {
            _class_add = val;
            _changed_flag = true;
        }

        /// <summary>
        /// ����buff����
        /// </summary>
        public void SetBuffAdd( float val )
        {
            _buff_add = val;
            _changed_flag = true;
        }

        /// <summary>
        /// ����װ������
        /// </summary>
        public void SetEquipAdd( float val )
        {
            _equip_add = val;
            _changed_flag = true;
        }

        /// <summary>
        /// �������
        /// </summary>
        public void Clear()
        {
            _base_val = 0;
            _class_add = 0;
            _equip_add = 0;
            _buff_add = 0;
            _changed_flag = false;
        }

        #endregion

        #region priv

        /// <summary>
        /// ���¼���������ֵ�����õ�����ֵ
        /// </summary>
        private float ReCalc()
        {
            _value = _base_val + _equip_add + _buff_add + _class_add;
            return _value;
        }

        #endregion

        #region fields

        /// <summary>
        /// ������
        /// </summary>
        private bool _changed_flag = false;

        /// <summary>
        /// ����ֵ
        /// </summary>
        private float _base_val = 0f;

        /// <summary>
        /// װ������
        /// </summary>
        private float _equip_add = 0f;

        /// <summary>
        /// ְҵ����
        /// </summary>
        private float _class_add = 0f;

        /// <summary>
        /// buff����
        /// </summary>
        private float _buff_add = 0f;

        /// <summary>
        /// ���ռ�������ֵ
        /// </summary>
        private float _value = 0f;

        #endregion
    }
}