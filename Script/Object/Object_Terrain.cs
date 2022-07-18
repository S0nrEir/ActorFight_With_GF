using Aquila.Config;
using GameFramework;
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
        /// 生成一个object
        /// </summary>
        public static Object_Terrain Gen( GameObject go )
        {
            var obj = ReferencePool.Acquire<Object_Terrain>();
            obj.Initialize( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME, go );
            return obj;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        protected override void OnSpawn()
        {
            base.OnSpawn();
            _mesh_render = Tools.GetComponent<MeshRenderer>( Target_GO, "Mesh" );
            if ( _mesh_render == null )
            {
                var child = Target_GO.transform.Find( "Mesh" );
                if ( child == null )
                    throw new GameFrameworkException( "faild to get Mesh child!" );

                if(!Tools.TryAddComponent<MeshRenderer>( child.gameObject ,out _mesh_render))
                    throw new GameFrameworkException( "faild to add Mesh child!" );
            }
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
            _mesh_render = null;
            base.Release( isShutdown );
        }


        /// <summary>
        /// 缓存网格渲染器
        /// </summary>
        private MeshRenderer _mesh_render = null;
    }

}
