using Aquila.Config;
using Aquila.Module;
using Aquila.ObjectPool;
using Aquila.Toolkit;
using Cfg.Common;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 预加载流程
    /// </summary>
    public class Procedure_Prelaod : ProcedureBase
    {
        /// <summary>
        /// 主动通知某一个加载标记完成，并且检查预加载状态
        /// </summary>
        public void NotifyFlag(int flag)
        {
            _preloadFlag |= flag;
            OnPreLoadFinished();
        }

        /// <summary>
        /// 当任意模块资源预加载完成
        /// </summary>
        private void OnPreLoadFinished()
        {
            if ( _preloadFlag != _preloadStateFinish )
                return;

            System.GC.Collect();

            //测试进入战斗流程
            NextProcedure();
        }

        protected override void OnInit( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnInit( procedureOwner );
            _load_terrain_callBack = new GameFramework.Resource.LoadAssetCallbacks( LoadTerrainSuccCallBack, LoadAssetFaildCallBack );
            _procedureOwner = procedureOwner;
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            _preloadFlag = _preloadStateInit;

            PreLoadTables();
            PreLoadObejct();
            PreloadInfoBoard();
            //测试配表
            //GameEntry.DataTable.Test();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            base.OnLeave( procedureOwner, isShutdown );
        }

        private void PreloadInfoBoard()
        {
            GameEntry.InfoBoard.Preload();
        }

        /// <summary>
        /// 预加载数据表
        /// </summary>
        private void PreLoadTables()
        {
            // _preload_flags = Tools.SetBitValue( _preload_flags, _table_load_flag_bit_offset, false );
            _preloadFlag |= _tableLoadFinish;
            OnPreLoadFinished();
        }

        /// <summary>
        /// 预加载地形的对象池对象
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
            //这里是为了提前创建好对象池和对象池内的数量，以便在后续的流程中使用，
            //最后把他们释放掉也是因为如此
            var go = asset as GameObject;
            if ( go == null )
                throw new GameFrameworkException( "terrain game object is null!" );

            //默认的地块创建数量
            var scene_config = Tools.Table.GetSceneConfig();
            var default_create_count = scene_config.Fight_Scene_Default_X_Width * scene_config.Fight_Scene_Default_Y_Width + 10;
            //默认创建四十个地块
            var pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ObjectPool.Object_Terrain>( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME, default_create_count, 3600f );

            Object_Base[] obj_arr = new Object_Base[default_create_count];
            ObjectPool.Object_Terrain temp_obj = null;
            GameObject temp_go = null;
            var root_go = GameEntry.Module.GetModule<Module_Terrain>().Root_GO;
            if ( root_go == null )
                throw new GameFrameworkException( "root_go == null" );

            for ( var i = 0; i < default_create_count; i++ )
            {
                temp_obj = pool.Spawn( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
                if ( temp_obj == null )
                {
                    //第一次生成，池里没有现成的object，先生成新的，然后注册到池里，直到把池内填满
                    //下次拿的时候就不用重新生成
                    temp_go = Object.Instantiate( go );
                    temp_go.tag = GameConfig.Tags.TERRAIN_BLOCK;
                    temp_go.transform.SetParent( root_go.transform );
                    pool.Register( Object_Terrain.Gen( temp_go ), false );
                    temp_obj = pool.Spawn( GameConfig.ObjectPool.OBJECT_POOL_TERRAIN_NAME );
                }
                obj_arr[i] = temp_obj;
            }
            foreach ( var obj in obj_arr )
                pool.Unspawn( obj.Target );

            obj_arr = null;
            // _preload_flags = Tools.SetBitValue( _preload_flags, _terrain_load_flag_bit_offset, false );
            _preloadFlag |= _terrainLoadFinish;
            OnPreLoadFinished();
        }

        /// <summary>
        /// 下一个流程
        /// </summary>
        private void NextProcedure()
        {
            ChangeState<Procedure_Test_Fight>( _procedureOwner );
            return;

#pragma warning disable CS0162 // 检测到无法访问的代码
            if ( GameEntry.Procedure._is_enter_test_scene )
#pragma warning restore CS0162 // 检测到无法访问的代码
            {
                ChangeState<Procedure_Test>( _procedureOwner );
            }
            else
            {
                var procedure_variable = ReferencePool.Acquire<Procedure_Fight_Variable>();
                var scene_script_meta = GameEntry.DataTable.Table<Scripts>().Get( 10000 );
                procedure_variable.SetValue( new Procedure_Fight_Data()
                {
                    _sceneScriptMeta = scene_script_meta,
                    _chunkName = Tools.Lua.GetChunkName( scene_script_meta.AssetPath )
                } );
                _procedureOwner.SetData( typeof( Procedure_Fight_Variable ).Name, procedure_variable );
                ChangeState<Procedure_Fight>( _procedureOwner );
            }
        }

        /// <summary>
        /// 加载资源失败回调
        /// </summary>
        private void LoadAssetFaildCallBack( string assetName, GameFramework.Resource.LoadResourceStatus status, string errorMessage, object userData ) => throw new GameFrameworkException( $"Load asset {assetName} faild!" );

        /// <summary>
        /// 地块资源加载回调
        /// </summary>
        private GameFramework.Resource.LoadAssetCallbacks _load_terrain_callBack = null;

        /// <summary>
        /// 各个资源模块的加载标记
        /// </summary>
        private int _preloadFlag = 0;

        /// <summary>
        /// 加载完成状态
        /// </summary>
        private const int _preloadStateFinish = 0b_0000_0111;

        /// <summary>
        /// 预加载初始化标记
        /// </summary>
        private const int _preloadStateInit = 0b_0000_0000_0000;

        /// <summary>
        /// 地块加载完成标记
        /// </summary>
        private const int _terrainLoadFinish = 0b_0000_0000_0001;

        /// <summary>
        /// 数据表加载完成
        /// </summary>
        private const int _tableLoadFinish = 0b_0000_0000_0010;

        /// <summary>
        /// hpbar加载完成
        /// </summary>
        public const int _infoboardHPBarLoadFinish = 0b_0000_0000_0100;

        /// <summary>
        /// 伤害数字加载完成
        /// </summary>
        public const int _infoboardDmgNumberLoadFinish = 0b_0000_0000_1000;
        
        /// <summary>
        /// 状态机拥有者
        /// </summary>
        private IFsm<IProcedureManager> _procedureOwner = null;
    }

}
