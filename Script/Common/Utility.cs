using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utility
    {
        /// <summary>
        /// 设置一个gameObject的active
        /// </summary>
        public static void SetActive(GameObject go,bool active)
        {
            if ( go == null )
                return;

            if ( go.activeSelf != active )
                go.SetActive( active );
        }
    }



}
