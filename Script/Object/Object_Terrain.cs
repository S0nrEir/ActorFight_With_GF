using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.ObjectPool
{
    /// <summary>
    /// 地块对象
    /// </summary>
    public class Object_Terrain : Aquila_Object_Base
    { 
        /// <summary>
        /// 获取对象
        /// </summary>
        protected override void OnSpawn()
        {
            base.OnSpawn();
        }

        /// <summary>
        /// 回收
        /// </summary>
        protected override void OnUnspawn()
        {
            base.OnUnspawn();
        }

        /// <summary>
        /// 释放
        /// </summary>
        protected override void Release( bool isShutdown )
        {
            base.Release( isShutdown );
        }



    }

}
