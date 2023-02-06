using Aquila.Numric;
using Cfg.Enum;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    public partial class Addon_Data
    {
        #region priv
        
        private void ResetNumricArr(Cfg.role.RoleMeta meta)
        {
            var len = ( int ) Numric_Type.Max - 1;
            _numric_arr = new Numric.Numric[len];
            Numric.Numric temp = null;
            for ( int i = 0; i < len; i++ )
            {
                temp = ReferencePool.Acquire<Numric.Numric>();
                _numric_arr[i] = temp;
                temp.Setup( 0f, 0f, 0f, 0f );
            }
        }

        #endregion

        #region fields

        //#todo_���ݷֳ��ĸ����֣�������ֵ��װ����buff����ֵ
        /// <summary>
        /// ��ֵ����
        /// </summary>
        private Numric.Numric[] _numric_arr = null;

        #endregion
    }
}
