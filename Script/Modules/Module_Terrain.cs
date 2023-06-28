using Aquila.Config;
using Aquila.Extension;
using Aquila.ObjectPool;
using Aquila.Toolkit;
using System.Collections.Generic;
using Cfg.Common;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// 战斗模块，地块类
    /// </summary>
    public class Module_Terrain : GameFrameworkModuleBase
    {
        //-----------------------public-----------------------

        public override void Start( object param )
        {
            base.Start( param );
            var temp = param as Fight_Param;
            GenerateFightSceneTerrain( temp.x_width,temp.z_width );
            //地形设置在地块加载之后
            var meta = GameEntry.LuBan.Table<Scripts>().Get( 10000 );
            GameEntry.Lua.Load( meta );
            //GameEntry.Lua.Load( GameEntry.DataTable.Tables.TB_Scripts.Get( 10000 ), GameConfig.Module.MODULE_TERRAIN_ENVIR_KEY );

            _genFlag = true;
        }

        public override void End()
        {
            RemoveAll();
            //GameEntry.Lua.UnLoad( GameConfig.Module.MODULE_TERRAIN_ENVIR_KEY );
            GameEntry.Lua.UnLoadAllRunningData();
            _genFlag = false;
            base.End();
        }

        //固定地块加载用嵌入实现
        /// <summary>
        /// 生成战斗地块场景
        /// </summary>
        private void GenerateFightSceneTerrain( int x_width, int z_width )
        {
            if ( _genFlag )
            {
                Log.Info( $"already generate terrain!", LogColorTypeEnum.Orange );
                return;
            }

            EnsureInit();
            _terrainCacheDic.Clear();
            _terrainGoCacheDic.Clear();
            Object_Terrain terrain = null;
            var pool = GameEntry.ObjectPool.GetObjectPool<ObjectPool.Object_Terrain>( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
            var xOffset = 0f;
            var zOffset = 0f;
            var scene_config = GameEntry.LuBan.Tables.SceneConfig;
            for ( int z = 0; z < z_width; z++ )
            {
                zOffset = scene_config.Terrain_Block_Offset_Distance * z;
                xOffset = 0f;
                for ( int x = 0; x < x_width; x++ )
                {
                    xOffset = scene_config.Terrain_Block_Offset_Distance * x;
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
                    terrain.SetLocalPosition( xOffset, zOffset );
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
            if ( _terrainGoCacheDic != null )
                _terrainGoCacheDic.Clear();

            if ( _terrainCacheDic != null )
            {
                foreach( var kv in _terrainCacheDic )
                    RemoveFromCache( kv.Key );

                _terrainCacheDic.Clear();
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
            _terrainCacheDic.TryGetValue( uniqueKey, out var terrain );
            return terrain;
        }

        /// <summary>
        /// 根据持有的GameObject引用获取一个地块对象
        /// </summary>
        public Object_Terrain Get( GameObject go )
        {
            if ( go == null )
                return null;

            _terrainGoCacheDic.TryGetValue( go, out var terrain );
            return terrain;
        }

        //-----------------------private----------------------- 

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
            return _terrainCacheDic.Remove( key );
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

            if ( _terrainCacheDic.ContainsKey( terrain.UniqueKey ) || _terrainGoCacheDic.ContainsKey( go ) )
                return false;

            _terrainCacheDic.Add( terrain.UniqueKey, terrain );
            _terrainGoCacheDic.Add( go, terrain );
            return true;
        }
        
        //-----------------------override-----------------------
        public override void OnClose()
        {
            _rootGameObject = null;
            _terrainCacheDic?.Clear();
            Log.Info( "_terrainCacheDic?.Clear()", LogColorTypeEnum.White );
            _terrainCacheDic = null;

            _terrainGoCacheDic?.Clear();
            _terrainGoCacheDic = null;
        }

        public override void EnsureInit()
        {
            var scene_config = GameEntry.LuBan.Tables.SceneConfig;
            var size = scene_config.Fight_Scene_Default_X_Width * scene_config.Fight_Scene_Default_Y_Width;
            if ( _terrainCacheDic is null )
                _terrainCacheDic = new Dictionary<int, Object_Terrain>( size );

            if ( _terrainGoCacheDic is null )
                _terrainGoCacheDic = new Dictionary<GameObject, Object_Terrain>( size );
        }

        /// <summary>
        /// 地块缓存
        /// </summary>
        private Dictionary<int, Object_Terrain> _terrainCacheDic = null;

        /// <summary>
        /// 地块gameobject索引
        /// </summary>
        private Dictionary<GameObject, Object_Terrain> _terrainGoCacheDic = null;

        /// <summary>
        /// 所有地块的根节点
        /// </summary>
        public GameObject Root_GO
        {
            get
            {
                if ( _rootGameObject == null )
                {
                    //拿不到就找
                    _rootGameObject = GameObject.FindGameObjectWithTag( GameConfig.Tags.TERRAIN_ROOT );
                    //找不到就创建
                    if ( _rootGameObject == null )
                    {
                        _rootGameObject                      = new GameObject( GameConfig.Tags.TERRAIN_ROOT );
                        _rootGameObject.transform.position   = Vector3.zero;
                        _rootGameObject.transform.localScale = Vector3.one;
                        _rootGameObject.transform.rotation   = Quaternion.Euler( Vector3.zero );
                        Object.DontDestroyOnLoad( _rootGameObject );
                    }
                }
                return _rootGameObject;
            }
        }

        /// <summary>
        /// 地块根节点
        /// </summary>
        private GameObject _rootGameObject = null;

        /// <summary>
        /// 生成标记
        /// </summary>
        private bool _genFlag = false;
    }//end of class
}
