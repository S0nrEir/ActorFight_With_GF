using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace  Aquila.Extension
{
    /// <summary>
    /// 全局组件，存储一些全局可访问的实例和变量
    /// </summary>
    public class Component_GlobalVar : GameFrameworkComponent
    {
        //--------------fields--------------

        /// <summary>
        /// 表示无效的位置
        /// </summary>
        public readonly Vector3 InvalidPosition = new Vector3( 999f, 999f, 999f );

        /// <summary>
        /// 无效的GUID
        /// </summary>
        public const UInt64 InvalidGUID = 0xFFFFFFFFFFFFFFFFul;

        /// <summary>
        /// 无效ID
        /// </summary>
        public const int InvalidID = -1;

        //--------------override--------------
        protected override void Awake()
        {
            base.Awake();
        }
    }

}
