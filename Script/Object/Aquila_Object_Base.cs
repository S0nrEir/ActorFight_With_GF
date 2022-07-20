using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Aquila.ObjectPool
{

    /// <summary>
    /// 对象池对象扩展基类,Target基于GameObject
    /// </summary>
    public abstract class Aquila_Object_Base : ObjectBase
    {
        /// <summary>
        /// 生成一个object
        /// </summary>
        //public abstract object Gen( GameObject go );

        ///// <summary>
        ///// 对象池内的类型名称
        ///// </summary>
        //public abstract string ObjectName { get; }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Target_GO = Target as GameObject;
            if ( Target_GO == null )
                throw new GameFrameworkException( "faild to convert Target as GameObject!!!" );

            Tools.SetActive( Target_GO, true );
        }

        protected override void OnUnspawn()
        {
            base.OnUnspawn();
            if ( Target_GO == null )
                return;

            Tools.SetActive( Target_GO, false );
        }

        /// <summary>
        /// 释放
        /// </summary>
        protected override void Release( bool isShutdown )
        {
            if ( Target_GO == null )
                return;

            Target_GO = null;
            Object.Destroy( Target as GameObject );
        }

        /// <summary>
        /// target game object
        /// </summary>
        protected GameObject Target_GO = null;
    }
}
