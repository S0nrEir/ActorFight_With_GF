using System;
using System.Collections.Generic;
using Aquila.Toolkit;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 参数化资源预加载流程
    /// </summary>
    public class Procedure_PreloadResource : ProcedureBase
    {

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            _procedureOwner = procedureOwner;
            ResetRuntimeState();

            if ( !TryReadPreloadParams( procedureOwner ) )
                return;

            DispatchPreloadRequests();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            if ( procedureOwner.HasData( KEY_RESOURCE_PRELOAD_VARIABLE ) )
            {
                procedureOwner.RemoveData( KEY_RESOURCE_PRELOAD_VARIABLE );
            }

            _procedureOwner = null;
            _nextProcedure = null;
            _resourceAssetPaths = null;
            _pendingIndexSet = null;
            _loadedIndexSet = null;
            _hasLoadFailed = false;
            _hasChangedState = false;

            base.OnLeave( procedureOwner, isShutdown );
        }

        /// <summary>
        /// 重置运行时状态
        /// </summary>
        private void ResetRuntimeState()
        {
            _hasLoadFailed = false;
            _hasChangedState = false;
            _pendingIndexSet = null;
            _loadedIndexSet = null;
        }

        /// <summary>
        /// 读取并校验参数
        /// </summary>
        private bool TryReadPreloadParams( IFsm<IProcedureManager> procedureOwner )
        {
            var variable = procedureOwner.GetData( KEY_RESOURCE_PRELOAD_VARIABLE ) as Procedure_ResourcePreload_Variable;
            var data = variable.GetValue() as Procedure_ResourcePreload_Data;
            if ( data == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] preload data is null." );
                return false;
            }

            if ( data.ResourceAssetPaths == null || data.ResourceAssetPaths.Length == 0 )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] resource list is null or empty." );
                return false;
            }

            for ( int i = 0; i < data.ResourceAssetPaths.Length; i++ )
            {
                if ( string.IsNullOrEmpty( data.ResourceAssetPaths[i] ) )
                {
                    Tools.Logger.Error( $"[Procedure_ResourcePreload] resource path at index {i} is null or empty." );
                    return false;
                }
            }

            _nextProcedure = data.NextProcedureType;
            _resourceAssetPaths = data.ResourceAssetPaths;
            _pendingIndexSet = new HashSet<int>( _resourceAssetPaths.Length );
            _loadedIndexSet = new HashSet<int>( _resourceAssetPaths.Length );
            for ( int i = 0; i < _resourceAssetPaths.Length; i++ )
                _pendingIndexSet.Add( i );
            return true;
        }

        /// <summary>
        /// 派发预加载请求
        /// </summary>
        private void DispatchPreloadRequests()
        {
            if ( _resourceAssetPaths == null || _resourceAssetPaths.Length == 0 )
                return;

            if ( _loadAssetCallbacks == null )
                _loadAssetCallbacks = new LoadAssetCallbacks( OnLoadAssetSuccess, OnLoadAssetFailed );

            for ( int i = 0; i < _resourceAssetPaths.Length; i++ )
                GameEntry.Resource.LoadAsset( _resourceAssetPaths[i], _loadAssetCallbacks, i );
        }

        /// <summary>
        /// 资源加载成功回调
        /// </summary>
        private void OnLoadAssetSuccess( string assetName, object asset, float duration, object userData )
        {
            if ( _hasLoadFailed || _hasChangedState )
                return;
            
            if ( !( userData is int index ) )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] load success callback userData type invalid, asset: {assetName}." );
                return;
            }

            if ( !_loadedIndexSet.Add( index ) )
            {
                Tools.Logger.Warning( $"[Procedure_ResourcePreload] duplicate success callback ignored, asset: {assetName}, index: {index}." );
                return;
            }

            TryGotoNextProcedure();
        }

        /// <summary>
        /// 资源加载失败回调
        /// </summary>
        private void OnLoadAssetFailed( string assetName, LoadResourceStatus status, string errorMessage, object userData )
        {
            _hasLoadFailed = true;
            Tools.Logger.Error( $"[Procedure_ResourcePreload] load asset failed, asset: {assetName}, status: {status}, error: {errorMessage}." );
        }

        /// <summary>
        /// 全部资源完成后跳转到目标流程
        /// </summary>
        private void TryGotoNextProcedure()
        {
            if ( _hasLoadFailed || _hasChangedState )
                return;

            if ( _loadedIndexSet.Count != _pendingIndexSet.Count )
                return;

            if ( _procedureOwner == null || _nextProcedure == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] cannot change state, owner or next procedure type is null." );
                return;
            }

            _hasChangedState = true;
            ChangeState( _procedureOwner, _nextProcedure );
        }

        /// <summary>
        /// 流程状态机拥有者
        /// </summary>
        private IFsm<IProcedureManager> _procedureOwner;

        /// <summary>
        /// 加载完成后要进入的流程
        /// </summary>
        private Type _nextProcedure;

        /// <summary>
        /// 待加载资源路径
        /// </summary>
        private string[] _resourceAssetPaths;

        /// <summary>
        /// 待加载资源索引集合
        /// </summary>
        private HashSet<int> _pendingIndexSet;

        /// <summary>
        /// 已成功加载的资源索引集合
        /// </summary>
        private HashSet<int> _loadedIndexSet;

        /// <summary>
        /// 是否出现加载失败
        /// </summary>
        private bool _hasLoadFailed;

        /// <summary>
        /// 是否已触发跳转
        /// </summary>
        private bool _hasChangedState;

        /// <summary>
        /// 加载回调
        /// </summary>
        private LoadAssetCallbacks _loadAssetCallbacks;
        
        /// <summary>
        /// FSM 数据键
        /// </summary>
        public const string KEY_RESOURCE_PRELOAD_VARIABLE = nameof( Procedure_ResourcePreload_Variable );
    }

    /// <summary>
    /// 参数化资源预加载流程数据
    /// </summary>
    public class Procedure_ResourcePreload_Data : IReference
    {
        public void Clear()
        {
            ResourceAssetPaths = null;
            NextProcedureType = null;
        }
        public string[] ResourceAssetPaths;
        public Type NextProcedureType;
    }

    /// <summary>
    /// 参数化资源预加载流程数据变量容器
    /// </summary>
    public class Procedure_ResourcePreload_Variable : Variable<Procedure_ResourcePreload_Data>
    {
        public override Type Type => typeof( Procedure_ResourcePreload_Variable );

        public override void Clear()
        {
            if ( Value is IReference )
                ReferencePool.Release( Value );

            Value = null;
        }
    }
}