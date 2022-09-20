using Aquila.Config;
using Aquila.Extension;
using Aquila.ObjectPool;
using Aquila.ToolKit;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{

    /// <summary>
    /// 战斗模块，地块类
    /// </summary>
    public class Module_Terrain : GameFrameworkModuleBase
    {
        #region public
        //#todo所有的地块获取，都要从对象池里拿TerrainObject

        public override void Start( object param )
        {
            base.Start( param );
            var temp = param as Fight_Param;
            GenerateFightSceneTerrain( temp.x_width,temp.z_width );
            //地形设置在地块加载之后
            GameEntry.Lua.Load( GameEntry.DataTable.Tables.TB_Scripts.Get( 10000 ), GameConfig.Module.MODULE_TERRAIN_ENVIR_KEY );
            _gen_flag = true;
        }

        public override void End()
        {
            RemoveAll();
            GameEntry.Lua.UnLoad( GameConfig.Module.MODULE_TERRAIN_ENVIR_KEY );
            _gen_flag = false;
            base.End();
        }

        //固定地块加载用嵌入实现
        /// <summary>
        /// 生成战斗地块场景
        /// </summary>
        private void GenerateFightSceneTerrain( int x_width, int z_width )
        {
            if ( _gen_flag )
            {
                Log.Info( $"already generate terrain!", LogColorTypeEnum.Orange );
                return;
            }

            EnsureInit();
            _terrain_cache_dic.Clear();
            _terrain_go_cache_dic.Clear();
            Object_Terrain terrain = null;
            var pool = GameEntry.ObjectPool.GetObjectPool<ObjectPool.Object_Terrain>( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
            var x_offset = 0f;
            var z_offset = 0f;
            var scene_config = GameEntry.DataTable.Tables.TB_SceneConfig;
            for ( int z = 0; z < z_width; z++ )
            {
                z_offset = scene_config.Terrain_Block_Offset_Distance * z;
                x_offset = 0f;
                for ( int x = 0; x < x_width; x++ )
                {
                    x_offset = scene_config.Terrain_Block_Offset_Distance * x;
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
                    terrain.SetLocalPosition( x_offset, z_offset );
                    terrain.SetState( TerrainStateTypeEnum.NONE );
                    AddToCache( terrain );
                }
            }//end for
        }

        /// <summary>
        /// 移除所有地块
        /// </summary>
        private void RemoveAll()
        {
            if ( _terrain_go_cache_dic != null )
                _terrain_go_cache_dic.Clear();

            if ( _terrain_cache_dic != null )
            {
                foreach( var kv in _terrain_cache_dic )
                    RemoveFromCache( kv.Key );

                _terrain_cache_dic.Clear();
            }
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

        /// <summary>
        /// 根据持有的GameObject引用获取一个地块对象
        /// </summary>
        public Object_Terrain Get( GameObject go )
        {
            if ( go == null )
                return null;

            _terrain_go_cache_dic.TryGetValue( go, out var terrain );
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

            var go = terrain.Target as GameObject;
            if ( go == null )
                return false;

            if ( _terrain_cache_dic.ContainsKey( terrain.UniqueKey ) || _terrain_go_cache_dic.ContainsKey( go ) )
                return false;

            _terrain_cache_dic.Add( terrain.UniqueKey, terrain );
            _terrain_go_cache_dic.Add( go, terrain );
            return true;
        }

        #endregion

        public override void OnClose()
        {
            _root_go = null;
            _terrain_cache_dic?.Clear();
            Log.Info( "_terrain_cache_dic?.Clear()", LogColorTypeEnum.White );
            _terrain_cache_dic = null;

            _terrain_go_cache_dic?.Clear();
            _terrain_go_cache_dic = null;
        }

        public override void EnsureInit()
        {
            var scene_config = GameEntry.DataTable.Tables.TB_SceneConfig;
            var size = scene_config.Fight_Scene_Default_X_Width * scene_config.Fight_Scene_Default_Y_Width;
            if ( _terrain_cache_dic is null )
                _terrain_cache_dic = new Dictionary<int, Object_Terrain>( size );

            if ( _terrain_go_cache_dic is null )
                _terrain_go_cache_dic = new Dictionary<GameObject, Object_Terrain>( size );
        }

        /// <summary>
        /// 地块缓存
        /// </summary>
        private Dictionary<int, Object_Terrain> _terrain_cache_dic = null;

        /// <summary>
        /// 地块gameobject索引
        /// </summary>
        private Dictionary<GameObject, Object_Terrain> _terrain_go_cache_dic = null;

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
                        _root_go.transform.rotation = Quaternion.Euler( Vector3.zero );
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
        private bool _gen_flag = false;
    }//end of class
}
