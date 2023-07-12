using Aquila.Config;
using Aquila.Module;
using Aquila.ObjectPool;
using Aquila.Toolkit;
using Cfg.Common;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections.Generic;
using UGFExtensions;
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
        /// HPBar加载完成
        /// </summary>
        public void LoadHPBarFinish()
        {
            _handler.HPBarLoadFinish();
            OnPreLoadFinished();
        }

        /// <summary>
        /// 伤害数字加载完成
        /// </summary>
        public void LoadDmgNumberFinish()
        {
            _handler.DmgNumberLoadFinish();
            OnPreLoadFinished();
        }

        /// <summary>
        /// 当任意模块资源预加载完成
        /// </summary>
        private void OnPreLoadFinished()
        {
            if ( !_handler.PreLoadFinish() )
                return;

            System.GC.Collect();

            //测试进入战斗流程
            NextProcedure();
        }

        protected override void OnInit( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnInit( procedureOwner );
            _procedureOwner = procedureOwner;
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            _handler = new PreloadHandler( Configs );

            GameEntry.Event.Subscribe( LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSucc );

            PreLoadTables();
            PreloadInternalTable();
            PreloadInfoBoard();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            GameEntry.Event.Unsubscribe( LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSucc );
            _handler = null;
            base.OnLeave( procedureOwner, isShutdown );
        }

        private void PreloadInfoBoard()
        {
            GameEntry.InfoBoard.Preload();
        }

        /// <summary>
        /// 预加载内部数据表
        /// </summary>
        private void PreloadInternalTable()
        {
            foreach ( var tableName in Configs )
            {
                //#todo：配置成常量路径
                var assetPath = @$"Assets/Res/Config/{tableName}.txt";
                GameEntry.DataTable.LoadDataTable( tableName, assetPath, null );
            }
        }

        /// <summary>
        /// 预加载数据表
        /// </summary>
        private void PreLoadTables()
        {
            // _preload_flags = Tools.SetBitValue( _preload_flags, _table_load_flag_bit_offset, false );
            //_preloadFlag |= _tableLoadFinish;
            //#todo别的预加载逻辑依赖luban数据表，所以luban数据表一开始先加载，不在预加载逻辑做，这里抽空改了
            //if ( !GameEntry.LuBan.LoadDataTable() )
            //    return;

            _handler.LoadDataTableFinish();
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
                var scene_script_meta = GameEntry.LuBan.Table<Scripts>().Get( 10000 );
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
        ///  表加载成功回调
        /// </summary>
        private void OnLoadDataTableSucc( object sender, GameEventArgs e )
        {
            var arg = e as LoadDataTableSuccessEventArgs;
            if ( arg is null )
                return;

            _handler.OnDataTableLoadSucc( arg.DataTableAssetName );
            OnPreLoadFinished();
        }

        /// <summary>
        /// 状态机拥有者
        /// </summary>
        private IFsm<IProcedureManager> _procedureOwner = null;

        /// <summary>
        /// 预加载处理器
        /// </summary>
        private PreloadHandler _handler = null;

        //#todo放到config里
        /// <summary>
        /// 预加载的form配置
        /// </summary>
        public static readonly string[] Configs = new string[]
            {
                "UIForm"
            };
    }

    /// <summary>
    /// 预加载处理器
    /// </summary>
    internal class PreloadHandler
    {
        /// <summary>
        /// 加载数据表完成
        /// </summary>
        public void LoadDataTableFinish()
        {
            _preloadFlag |= _tableLoadFinish;
        }

        /// <summary>
        /// HPBar加载完成
        /// </summary>
        public void HPBarLoadFinish()
        {
            _preloadFlag |= _infoboardHPBarLoadFinish;
        }

        /// <summary>
        /// 伤害数字加载完成
        /// </summary>
        public void DmgNumberLoadFinish()
        {
            _preloadFlag |= _infoboardDmgNumberLoadFinish;
        }

        /// <summary>
        /// 内部数据表加载成功
        /// </summary>
        public void OnDataTableLoadSucc( string assetName )
        {
            var iter = _datatableLoadedSet.GetEnumerator();
            while ( iter.MoveNext() )
            {
                if ( assetName.Contains( iter.Current ) )
                {
                    //#考虑不要用contains检查，抽空改了
                    _datatableLoadedSet.Remove( iter.Current );
                    break;
                }
            }

            if ( _datatableLoadedSet.Count == 0 )
                _preloadFlag |= _datatableLoadFinish;
        }

        /// <summary>
        /// 预加载全部完成
        /// </summary>
        public bool PreLoadFinish()
        {
            return _preloadFlag == _preloadStateFinish;
        }


        public PreloadHandler( string[] internalTableNames )
        {
            _preloadFlag = 0;

            _datatableLoadedSet = new HashSet<string>();
            foreach ( var name in internalTableNames )
                _datatableLoadedSet.Add( name );
        }

        //------------fields------------
        /// <summary>
        /// 各个资源模块的加载标记
        /// </summary>
        private int _preloadFlag = 0;

        /// <summary>
        /// 数据表加载完成
        /// </summary>
        private const int _tableLoadFinish = 0b_0000_0000_0001;

        /// <summary>
        /// 伤害数字加载完成
        /// </summary>
        public const int _infoboardDmgNumberLoadFinish = 0b_0000_0000_0010;

        /// <summary>
        /// 数据表加载标记
        /// </summary>
        private const int _datatableLoadFinish = 0b_0000_0000_0100;

        /// <summary>
        /// hpbar加载完成
        /// </summary>
        public const int _infoboardHPBarLoadFinish = 0b_0000_0000_1000;

        /// <summary>
        /// 加载完成状态
        /// </summary>
        private const int _preloadStateFinish = 0b_0000_0000_1111;

        /// <summary>
        /// 保存未加载完成的数据表
        /// </summary>
        private HashSet<string> _datatableLoadedSet = null;
    }
}
