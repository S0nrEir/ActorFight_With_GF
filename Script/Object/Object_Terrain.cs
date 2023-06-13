using Aquila.Config;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;

namespace Aquila.ObjectPool
{
    /// <summary>
    /// 地块对象
    /// </summary>
    public class Object_Terrain : Object_Base
    {
        public bool SetNameTest( string name )
        {
            if ( string.IsNullOrEmpty( name ) )
                return false;

            _targetGameObject.name = name;
            return true;
        }

        /// <summary>
        /// 设置地块gameobject的名称
        /// </summary>
        public void SetName( string name )
        {
            _targetGameObject.name = name;
        }

        /// <summary>
        /// 设置地块状态
        /// </summary>
        public void SetState( TerrainStateTypeEnum type )
        {
            State = type;
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        public void SetParent( Transform parent )
        {
            Tools.SetParent( _targetGameObject.transform, parent );
        }

        /// <summary>
        /// 设置地块在根节点下的本地坐标
        /// </summary>
        public void SetLocalPosition( float x, float z )
        {
            _targetGameObject.transform.localPosition = new Vector3( x, 0, z );
            WorldHangPosition = WorldPosition + GameConfig.Terrain.TERRAIN_HANG_POINT_OFFSET;
        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        public void SetCoordinate( int x, int z )
        {
            Coordinate = new Vector3Int( x, 0, z );
            UniqueKey = Tools.Fight.Coord2UniqueKey( Coordinate.x, Coordinate.z );
            _targetGameObject.name = $"{x}_{z}";
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
            Tools.SetTag( GameConfig.Tags.TERRAIN_BLOCK, _targetGameObject, true );
            Tools.SetLayer( GameConfig.Layer.LAYER_TERRAIN_BLOCK, _targetGameObject, true );
            _mesh_render = Tools.GetComponent<MeshRenderer>( _targetGameObject, "Mesh" );
            if ( _mesh_render == null )
            {
                var child = _targetGameObject.transform.Find( "Mesh" );
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
            State = TerrainStateTypeEnum.INVALID;
            base.Release( isShutdown );
        }
        #endregion

        /// <summary>
        /// 缓存网格渲染器
        /// </summary>
        private MeshRenderer _mesh_render = null;

        /// <summary>
        /// 地块状态
        /// </summary>
        public TerrainStateTypeEnum State { get; private set; } = TerrainStateTypeEnum.INVALID;

        /// <summary>
        /// 地块世界坐标
        /// </summary>
        public Vector3 WorldPosition
        {
            get { return _targetGameObject.transform.position; }
        }

        /// <summary>
        /// 世界物体挂点坐标
        /// </summary>
        public Vector3 WorldHangPosition { get; private set; } = Vector3.zero;
    }

    /// <summary>
    /// 地块状态枚举
    /// </summary>
    public enum TerrainStateTypeEnum
    {
        /// <summary>
        /// 无效
        /// </summary>
        INVALID = -1,

        /// <summary>
        /// 初始状态，啥都没有
        /// </summary>
        NONE = 0,

        /// <summary>
        /// 地块上拥有actor
        /// </summary>
        ACTOR,

    }
}
