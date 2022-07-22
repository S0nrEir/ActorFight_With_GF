using Aquila.Config;
using GameFramework;
using UnityEngine;

namespace Aquila.ObjectPool
{
    /// <summary>
    /// 地块对象
    /// </summary>
    public class Object_Terrain : Aquila_Object_Base
    {
        public bool SetNameTest( string name )
        {
            if(string.IsNullOrEmpty(name))
                return false;

            Target_GO.name = name;
            return true;
        }

        /// <summary>
        /// 测试函数
        /// </summary>
        public void SetName( string name )
        {
            Target_GO.name = name;
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        public void SetParent( Transform parent )
        {
            Tools.SetParent( Target_GO.transform, parent );
        }

        /// <summary>
        /// 设置地块的世界位置
        /// </summary>
        public void SetPosition( float x, float z )
        {
            Target_GO.transform.localPosition = new Vector3( x, 0, z );
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public void SetCoordinate( int x, int y )
        {
            Coordinate = new Vector3Int( x, 0, y );
            UniqueKey = Tools.Fight.Coord2UniqueKey( Coordinate.x, Coordinate.y );
            Target_GO.name = $"{x}_{y}";
        }

        /// <summary>
        /// 设置地块高度，默认0
        /// </summary>
        public void SetHeight( int height )
        {
            Coordinate.Set( Coordinate.x, height, Coordinate.z );
        }

        /// <summary>
        /// 坐标位置，y=地块高度
        /// </summary>
        public Vector3Int Coordinate { get; private set; } = Vector3Int.zero;

        /// <summary>
        /// 坐标key
        /// </summary>
        public int UniqueKey { get; private set; } = 0;

        /// <summary>
        /// 生成一个object
        /// </summary>
        public static Object_Terrain Gen( GameObject go )
        {
            var obj = ReferencePool.Acquire<Object_Terrain>();
            obj.Initialize( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME, go );
            return obj;
        }

        #region
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

                if ( !Tools.TryAddComponent<MeshRenderer>( child.gameObject, out _mesh_render ) )
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
        #endregion

        /// <summary>
        /// 缓存网格渲染器
        /// </summary>
        private MeshRenderer _mesh_render = null;
    }

}
