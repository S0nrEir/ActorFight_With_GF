using Aquila.Toolkit;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace Aquila.ObjectPool
{
    /// <summary>
    /// 对象池对象扩展基类,Target基于GameObject
    /// </summary>
    public abstract class Object_Base : ObjectBase
    {
        
        public virtual void Setup(GameObject go)
        {
        }
        
        protected override void OnSpawn()
        {
            base.OnSpawn();
            _targetGameObject = Target as GameObject;
            if ( _targetGameObject == null )
                throw new GameFrameworkException( "faild to convert Target as GameObject!!!" );

            Tools.SetActive( _targetGameObject, true );
        }

        protected override void OnUnspawn()
        {
            base.OnUnspawn();
            if ( _targetGameObject == null )
                return;

            Tools.SetActive( _targetGameObject, false );
        }

        /// <summary>
        /// 释放
        /// </summary>
        protected override void Release( bool isShutdown )
        {
            if ( _targetGameObject == null )
                return;

            _targetGameObject = null;
            Object.Destroy( Target as GameObject );
        }

        /// <summary>
        /// target game object
        /// </summary>
        protected GameObject _targetGameObject = null;
    }
}
