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
    public class Procedure_ResourcePreload : ProcedureBase
    {
        /// <summary>
        /// FSM 数据键
        /// </summary>
        public const string KEY_RESOURCE_PRELOAD_VARIABLE = nameof( Procedure_ResourcePreload_Variable );

        /// <summary>
        /// 启动参数化预加载（泛型入口）
        /// </summary>
        public static bool StartWith<TNextProcedure>( IFsm<IProcedureManager> procedureOwner, params string[] resourceAssetPaths ) where TNextProcedure : ProcedureBase
        {
            return StartWith( procedureOwner, typeof( TNextProcedure ), resourceAssetPaths );
        }

        /// <summary>
        /// 启动参数化预加载（运行时类型入口）
        /// </summary>
        public static bool StartWith( IFsm<IProcedureManager> procedureOwner, Type nextProcedureType, IList<string> resourceAssetPaths )
        {
            if ( procedureOwner == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] StartWith failed: procedureOwner is null." );
                return false;
            }

            if ( nextProcedureType == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] StartWith failed: nextProcedureType is null." );
                return false;
            }

            if ( !typeof( ProcedureBase ).IsAssignableFrom( nextProcedureType ) )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] StartWith failed: next procedure type '{nextProcedureType.FullName}' does not inherit ProcedureBase." );
                return false;
            }

            if ( !procedureOwner.Owner.HasProcedure( nextProcedureType ) )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] StartWith failed: next procedure '{nextProcedureType.FullName}' is not registered in ProcedureComponent." );
                return false;
            }

            if ( resourceAssetPaths == null || resourceAssetPaths.Count == 0 )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] StartWith failed: resourceAssetPaths is null or empty." );
                return false;
            }

            var paths = new string[resourceAssetPaths.Count];
            for ( int i = 0; i < resourceAssetPaths.Count; i++ )
            {
                var path = resourceAssetPaths[i];
                if ( string.IsNullOrEmpty( path ) )
                {
                    Tools.Logger.Error( $"[Procedure_ResourcePreload] StartWith failed: resource path at index {i} is null or empty." );
                    return false;
                }

                paths[i] = path;
            }

            if ( procedureOwner.HasData( KEY_RESOURCE_PRELOAD_VARIABLE ) )
            {
                procedureOwner.RemoveData( KEY_RESOURCE_PRELOAD_VARIABLE );
            }

            var variable = ReferencePool.Acquire<Procedure_ResourcePreload_Variable>();
            variable.SetValue( new Procedure_ResourcePreload_Data
            {
                ResourceAssetPaths = paths,
                NextProcedureType = nextProcedureType
            } );
            procedureOwner.SetData( KEY_RESOURCE_PRELOAD_VARIABLE, variable );
            return true;
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            _procedureOwner = procedureOwner;
            ResetRuntimeState();

            if ( !TryReadPreloadParams( procedureOwner ) )
            {
                return;
            }

            DispatchPreloadRequests();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            if ( procedureOwner.HasData( KEY_RESOURCE_PRELOAD_VARIABLE ) )
            {
                procedureOwner.RemoveData( KEY_RESOURCE_PRELOAD_VARIABLE );
            }

            _procedureOwner = null;
            _nextProcedureType = null;
            _resourceAssetPaths = null;
            _loadedResourceIndexSet = null;
            _hasLoadFailed = false;
            _hasChangedState = false;
            _loadFinishFlag = 0UL;
            _loadFinishFlagMask = 0UL;

            base.OnLeave( procedureOwner, isShutdown );
        }

        /// <summary>
        /// 重置运行时状态
        /// </summary>
        private void ResetRuntimeState()
        {
            _hasLoadFailed = false;
            _hasChangedState = false;
            _loadFinishFlag = 0UL;
            _loadFinishFlagMask = 0UL;
        }

        /// <summary>
        /// 读取并校验参数
        /// </summary>
        private bool TryReadPreloadParams( IFsm<IProcedureManager> procedureOwner )
        {
            if ( !procedureOwner.HasData( KEY_RESOURCE_PRELOAD_VARIABLE ) )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] missing preload parameter variable." );
                return false;
            }

            var variable = procedureOwner.GetData( KEY_RESOURCE_PRELOAD_VARIABLE ) as Procedure_ResourcePreload_Variable;
            if ( variable == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] preload variable type mismatch." );
                return false;
            }

            var data = variable.GetValue() as Procedure_ResourcePreload_Data;
            if ( data == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] preload data is null." );
                return false;
            }

            if ( data.NextProcedureType == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] next procedure type is null." );
                return false;
            }

            if ( !typeof( ProcedureBase ).IsAssignableFrom( data.NextProcedureType ) )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] next procedure type '{data.NextProcedureType.FullName}' does not inherit ProcedureBase." );
                return false;
            }

            if ( !procedureOwner.Owner.HasProcedure( data.NextProcedureType ) )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] next procedure '{data.NextProcedureType.FullName}' is not registered in ProcedureComponent." );
                return false;
            }

            if ( data.ResourceAssetPaths == null || data.ResourceAssetPaths.Length == 0 )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] resource list is null or empty." );
                return false;
            }

            if ( data.ResourceAssetPaths.Length > 63 )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] resource list is too large ({data.ResourceAssetPaths.Length}), max supported count is 63." );
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

            _nextProcedureType = data.NextProcedureType;
            _resourceAssetPaths = data.ResourceAssetPaths;
            _loadedResourceIndexSet = new HashSet<int>( _resourceAssetPaths.Length );
            _loadFinishFlagMask = ( 1UL << _resourceAssetPaths.Length ) - 1UL;
            return true;
        }

        /// <summary>
        /// 派发预加载请求
        /// </summary>
        private void DispatchPreloadRequests()
        {
            if ( _resourceAssetPaths == null || _resourceAssetPaths.Length == 0 )
            {
                return;
            }

            if ( _loadAssetCallbacks == null )
            {
                _loadAssetCallbacks = new LoadAssetCallbacks( OnLoadAssetSuccess, OnLoadAssetFailed );
            }

            for ( int i = 0; i < _resourceAssetPaths.Length; i++ )
            {
                GameEntry.Resource.LoadAsset( _resourceAssetPaths[i], _loadAssetCallbacks, i );
            }
        }

        /// <summary>
        /// 资源加载成功回调
        /// </summary>
        private void OnLoadAssetSuccess( string assetName, object asset, float duration, object userData )
        {
            if ( _hasLoadFailed || _hasChangedState )
            {
                return;
            }

            if ( !( userData is int index ) )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] load success callback userData type invalid, asset: {assetName}." );
                return;
            }

            if ( _resourceAssetPaths == null || index < 0 || index >= _resourceAssetPaths.Length )
            {
                Tools.Logger.Error( $"[Procedure_ResourcePreload] load success callback index out of range, asset: {assetName}, index: {index}." );
                return;
            }

            if ( _loadedResourceIndexSet != null && !_loadedResourceIndexSet.Add( index ) )
            {
                Tools.Logger.Warning( $"[Procedure_ResourcePreload] duplicate success callback ignored, asset: {assetName}, index: {index}." );
                return;
            }

            _loadFinishFlag |= ( 1UL << index );
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
            {
                return;
            }

            if ( _loadFinishFlag != _loadFinishFlagMask )
            {
                return;
            }

            if ( _procedureOwner == null || _nextProcedureType == null )
            {
                Tools.Logger.Error( "[Procedure_ResourcePreload] cannot change state, owner or next procedure type is null." );
                return;
            }

            _hasChangedState = true;
            ChangeState( _procedureOwner, _nextProcedureType );
        }

        /// <summary>
        /// 流程状态机拥有者
        /// </summary>
        private IFsm<IProcedureManager> _procedureOwner;

        /// <summary>
        /// 目标流程类型
        /// </summary>
        private Type _nextProcedureType;

        /// <summary>
        /// 待加载资源路径
        /// </summary>
        private string[] _resourceAssetPaths;

        /// <summary>
        /// 已成功加载的资源索引集合（用于防重复回调）
        /// </summary>
        private HashSet<int> _loadedResourceIndexSet;

        /// <summary>
        /// 已完成加载标记位
        /// </summary>
        private ulong _loadFinishFlag;

        /// <summary>
        /// 预期完成加载标记位
        /// </summary>
        private ulong _loadFinishFlagMask;

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
    }

    /// <summary>
    /// 参数化资源预加载流程数据
    /// </summary>
    internal class Procedure_ResourcePreload_Data : IReference
    {
        public string[] ResourceAssetPaths;
        public Type NextProcedureType;

        public void Clear()
        {
            ResourceAssetPaths = null;
            NextProcedureType = null;
        }
    }

    /// <summary>
    /// 参数化资源预加载流程数据变量容器
    /// </summary>
    internal class Procedure_ResourcePreload_Variable : Variable<Procedure_ResourcePreload_Data>
    {
        public override Type Type => typeof( Procedure_ResourcePreload_Variable );

        public override void Clear()
        {
            if ( Value is IReference )
            {
                ReferencePool.Release( Value );
            }

            Value = null;
        }
    }
}