using Aquila.Config;
using Aquila.Module;
using Aquila.ObjectPool;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;

namespace Aquila.Procedure
{
    /// <summary>
    /// 预加载流程
    /// </summary>
    public class Procedure_Prelaod : ProcedureBase
    {
        protected override void OnInit( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnInit( procedureOwner );
            _load_terrain_callBack = new GameFramework.Resource.LoadAssetCallbacks( LoadTerrainSuccCallBack, LoadAssetFaildCallBack );
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            //加载数据表
            if ( !GameEntry.DataTable.LoadDataTable() )
                throw new GameFrameworkException( "load data table faild!" );

            PreLoadObejct();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            base.OnLeave( procedureOwner, isShutdown );
        }

        /// <summary>
        /// 预加载对象池对象
        /// </summary>
        private void PreLoadObejct()
        {
            GameEntry.Resource.LoadAsset
                (
                    //#todo这里的路径看配置成常量静态内部表
                    @"Assets/Res/Prefab/Terrain/EmptyTerrain.prefab",
                    _load_terrain_callBack
                );
        }

        /// <summary>
        /// 加载地块成功回调
        /// </summary>
        private void LoadTerrainSuccCallBack( string assetName, object asset, float duration, object userData )
        {
            var go = asset as GameObject;
            if ( go == null )
                throw new GameFrameworkException( "terrain game object is null!" );

            //默认的地块创建数量
            var default_create_count = 40;
            //默认创建四十个地块
            var pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ObjectPool.Object_Terrain>( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME, default_create_count, 3600f );

            Aquila_Object_Base[] obj_arr = new Aquila_Object_Base[default_create_count];
            ObjectPool.Object_Terrain temp_obj = null;
            GameObject temp_go = null;
            var root_go = GameFrameworkModule.GetModule<Module_Terrain>().Root_GO;
            for ( var i = 0; i < default_create_count; i++ )
            {
                temp_obj = pool.Spawn( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
                if ( temp_obj == null )
                {
                    temp_go = Object.Instantiate( go );
                    temp_go.tag = GameConfig.Tags.TERRAIN_BLOCK;
                    temp_go.transform.SetParent( root_go.transform );
                    pool.Register( Object_Terrain.Gen( temp_go ) , false );
                    temp_obj = pool.Spawn( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
                }
                obj_arr[i] = temp_obj;
            }
            foreach ( var obj in obj_arr )
                pool.Unspawn( obj.Target );

            obj_arr = null;
        }

        /// <summary>
        /// 加载资源失败回调
        /// </summary>
        private void LoadAssetFaildCallBack( string assetName, GameFramework.Resource.LoadResourceStatus status, string errorMessage, object userData ) => throw new GameFrameworkException( $"Load asset {assetName} faild!" );

        private GameFramework.Resource.LoadAssetCallbacks _load_terrain_callBack = null;
    }

}
