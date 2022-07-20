using Aquila.Config;
using Aquila.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// 战斗地形的模组类，负责处理地块的生成，管理，回收，逻辑
    /// </summary>
    public class Module_Terrain : GameFrameworkModuleBase
    {
        #region public
        //#todo所有的地块获取，都要从对象池里拿TerrainObject

        /// <summary>
        /// 生成战斗地块场景
        /// </summary>
        /// <param name="x_width">x方向上的长度</param>
        /// <param name="y_width">z方向上的长度</param>
        public void GenerateFightSceneTerrain( int x_width, int z_width )
        {
            if ( _generate_flag )
            {
                Log.Info( $"already generate terrain!", LogColorTypeEnum.Orange );
                return;
            }

            EnsureInit();
            _terrain_cache_dic.Clear();
            Object_Terrain terrain = null;
            var pool = GameEntry.ObjectPool.GetObjectPool<ObjectPool.Object_Terrain>( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
            var x_offset = 0f;
            var z_offset = 0f;
            for ( int z = 0; z < z_width; z++ )
            {
                z_offset = GameConfig.Scene.TERRAIN_BLOCK_OFFSET_DISTANCE * z;
                x_offset = 0f;
                for ( int x = 0; x < x_width; x++ )
                {
                    x_offset = GameConfig.Scene.TERRAIN_BLOCK_OFFSET_DISTANCE * x;
                    terrain = pool.Spawn( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
                    //safety chk
                    if ( terrain == null )
                    {
                        Log.Error( $"faild to spawn a terrain object!" );
                        continue;
                    }
                    //init settting
                    terrain.SetParent( Root_GO.transform );
                    terrain.SetCoordinate( x, z );
                    terrain.SetHeight( 0 );
                    terrain.SetPosition( x_offset, z_offset );
                    AddToCache( terrain );
                }
            }//end for
            _generate_flag = true;
        }

        /// <summary>
        /// 移除所有地块
        /// </summary>
        public void RemoveAll()
        {
            if ( _terrain_cache_dic is null || _terrain_cache_dic.Count == 0 )
                return;

            int key = 0;
            var iter = _terrain_cache_dic.GetEnumerator();
            while ( iter.MoveNext() )
            {
                key = iter.Current.Key;
                RemoveFromCache( key );
            }
            _generate_flag = false;
        }

        /// <summary>
        /// 根据xz坐标获取一个地块对象，失败返回空
        /// </summary>
        public Object_Terrain Get( int x, int z )
        {
            return Get( Tools.Fight.Coord2UniqueKey( x, z ) );
        }

        /// <summary>
        /// 根据uniqueKey获取一个地块对象，失败返回空
        /// </summary>
        public Object_Terrain Get( int uniqueKey )
        {
            _terrain_cache_dic.TryGetValue( uniqueKey, out var terrain );
            return terrain;
        }

        #endregion

        #region private 

        /// <summary>
        /// 从地块缓存中移除
        /// </summary>
        private bool RemoveFromCache( int key )
        {
            var terrain = Get( key );
            if ( terrain is null )
                return false;

            var pool = GameEntry.ObjectPool.GetObjectPool<ObjectPool.Object_Terrain>( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
            pool.Unspawn( terrain.Target );
            return _terrain_cache_dic.Remove( key );
        }

        /// <summary>
        /// 添加到地块缓存
        /// </summary>
        private bool AddToCache( Object_Terrain terrain )
        {
            if ( terrain is null )
                return false;

            if ( _terrain_cache_dic.ContainsKey( terrain.UniqueKey ) )
                return false;

            _terrain_cache_dic.Add( terrain.UniqueKey, terrain );
            return true;
        }

        #endregion

        public override void OnClose()
        {
            _root_go = null;
            _terrain_cache_dic?.Clear();
            _terrain_cache_dic = null;
            _generate_flag = false;
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            if ( _terrain_cache_dic is null )
                _terrain_cache_dic = new Dictionary<int, Object_Terrain>();

            _generate_flag = false;
        }

        /// <summary>
        /// 地块缓存
        /// </summary>
        private Dictionary<int, Object_Terrain> _terrain_cache_dic = null;

        /// <summary>
        /// 所有地块的根节点
        /// </summary>
        public GameObject Root_GO
        {
            get
            {
                if ( _root_go == null )
                {
                    //拿不到就找
                    _root_go = GameObject.FindGameObjectWithTag( GameConfig.Tags.TERRAIN_ROOT );
                    //找不到就创建
                    if ( _root_go == null )
                    {
                        _root_go = new GameObject( GameConfig.Tags.TERRAIN_ROOT );
                        _root_go.transform.position = Vector3.zero;
                        _root_go.transform.localScale = Vector3.one;
                        _root_go.transform.eulerAngles = Vector3.zero;
                        Object.DontDestroyOnLoad( _root_go );
                    }
                }
                return _root_go;
            }
        }

        /// <summary>
        /// 地块根节点
        /// </summary>
        private GameObject _root_go = null;

        /// <summary>
        /// 生成标记
        /// </summary>
        private bool _generate_flag = false;
    }
}
