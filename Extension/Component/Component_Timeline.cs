using Aquila.ObjectPool;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// timeline缁勪欢
    /// </summary>
    public class Component_Timeline : GameFrameworkComponent
    {
        public void Play( string assetPath, PlayableDirector director )
        {
            if ( string.IsNullOrEmpty( assetPath ) )
            {
                Log.Warning( "<color=yellow>Compoent_Timeline.Play()--->string.IsNullOrEmpty( path )</color>" );
                return;
            }

            if ( _assetCache.TryGetValue( assetPath, out var cachedAsset ) )
            {
                PlayWithDirector( assetPath, cachedAsset, director );
                return;
            }

            var timelineObject = _assetPool.Spawn( assetPath );
            if ( timelineObject == null )
            {
                EnqueuePendingPlayRequest( assetPath, director );
                if ( !_loadingAssetNames.Add( assetPath ) )
                {
                    return;
                }

                // 池里没有就加载
                GameEntry.Resource.LoadAsset( assetPath, _loadAssetCallBack, null );
                return;
            }

            var pooledAsset = timelineObject.Target as PlayableAsset;
            if ( pooledAsset != null && !_assetCache.ContainsKey( assetPath ) )
            {
                _assetCache.Add( assetPath, pooledAsset );
                _registeredAssets.Add( pooledAsset );
            }

            PlayWithDirector( assetPath, pooledAsset, director );
        }

        private void OnLoadAssetSucc( string assetName, object asset, float duration, object userData )
        {
            var playableAsset = asset as PlayableAsset;
            if ( playableAsset is null )
            {
                Log.Warning( "<color=yellow>Component_Timeline.OnLoadAssetSucc()--->playableAsset is null </color>" );
                ClearLoadingState( assetName );
                return;
            }

            RegisterPlayableAssetIfNeeded( assetName, playableAsset );
            DrainPendingPlayRequests( assetName );
        }

        private void RegisterPlayableAssetIfNeeded( string assetName, PlayableAsset playableAsset )
        {
            if ( _assetCache.TryGetValue( assetName, out var cachedAsset ) )
            {
                if ( !ReferenceEquals( cachedAsset, playableAsset ) )
                {
                    Log.Warning( $"<color=yellow>Component_Timeline.OnLoadAssetSucc()--->duplicate asset callback ignored, asset:{assetName}</color>" );
                }

                return;
            }

            if ( _registeredAssets.Add( playableAsset ) )
            {
                var obj = Object_PlayableAsset.Create( assetName, playableAsset );
                _assetPool.Register( obj, false );
            }
            else
            {
                Log.Warning( $"<color=yellow>Component_Timeline.OnLoadAssetSucc()--->asset target already registered, asset:{assetName}</color>" );
            }

            _assetCache.Add( assetName, playableAsset );
        }

        private void PlayWithDirector( string assetName, PlayableAsset playableAsset, PlayableDirector director )
        {
            if ( director == null )
            {
                Log.Warning( $"<color=yellow>Component_Timeline.Play()--->director is null, asset:{assetName}</color>" );
                return;
            }

            director.playableAsset = playableAsset;
            if ( director.playableAsset is null )
            {
                Log.Warning( "<color=yellow>Component_Timeline.Play()--->director.playableAsset is null </color>" );
                return;
            }

            director.Play();
        }

        private void OnLoadAssetFaild( string assetName, LoadResourceStatus status, string errorMessage, object userData )
        {
            Log.Warning( $"<color=yellow>Component_Timeline.OnLoadAssetFaild()--->asset:{assetName},status:{status},error:{errorMessage}</color>" );
            ClearLoadingState( assetName );
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _assetPool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<Object_PlayableAsset>( nameof( Object_PlayableAsset ) );
            _assetPool.ExpireTime = 3600f;
            _loadAssetCallBack = new LoadAssetCallbacks( OnLoadAssetSucc, OnLoadAssetFaild );
        }

        private Dictionary<string, PlayableAsset> _assetCache = new Dictionary<string, PlayableAsset>();
        private IObjectPool<Object_PlayableAsset> _assetPool = null;
        private LoadAssetCallbacks _loadAssetCallBack = null;
        private HashSet<string> _loadingAssetNames = new HashSet<string>();
        private Dictionary<string, List<PlayableDirector>> _pendingPlayRequests = new Dictionary<string, List<PlayableDirector>>();
        private HashSet<PlayableAsset> _registeredAssets = new HashSet<PlayableAsset>();

        private void EnqueuePendingPlayRequest( string assetPath, PlayableDirector director )
        {
            if ( director == null )
            {
                return;
            }

            if ( !_pendingPlayRequests.TryGetValue( assetPath, out var pendingDirectors ) )
            {
                pendingDirectors = new List<PlayableDirector>();
                _pendingPlayRequests.Add( assetPath, pendingDirectors );
            }

            pendingDirectors.Add( director );
        }

        private void DrainPendingPlayRequests( string assetPath )
        {
            _loadingAssetNames.Remove( assetPath );
            if ( !_pendingPlayRequests.TryGetValue( assetPath, out var pendingDirectors ) )
            {
                return;
            }

            _pendingPlayRequests.Remove( assetPath );
            foreach ( var pendingDirector in pendingDirectors )
            {
                if ( pendingDirector == null )
                {
                    continue;
                }

                Play( assetPath, pendingDirector );
            }
        }

        private void ClearLoadingState( string assetPath )
        {
            _loadingAssetNames.Remove( assetPath );
            _pendingPlayRequests.Remove( assetPath );
        }
    }
}
