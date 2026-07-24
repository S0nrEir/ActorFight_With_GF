using System.Collections.Generic;
using Aquila.ObjectPool;
using Aquila.Toolkit;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    public readonly struct TimelinePlayRequestHandle
    {
        internal TimelinePlayRequestHandle(Component_Timeline owner, long requestId, int generation)
        {
            Owner = owner;
            RequestId = requestId;
            Generation = generation;
        }

        public bool IsValid => Owner != null && RequestId > 0;

        public void Cancel()
        {
            Owner?.Cancel(this);
        }

        internal Component_Timeline Owner { get; }
        internal long RequestId { get; }
        internal int Generation { get; }
    }

    public class Component_Timeline : GameFrameworkComponent
    {
        public TimelinePlayRequestHandle Play(string assetPath, PlayableDirector director)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                Tools.Logger.Warning("<color=yellow>Component_Timeline.Play()--->asset path is empty</color>");
                return default;
            }

            if (director == null)
            {
                Tools.Logger.Warning($"<color=yellow>Component_Timeline.Play()--->director is null, asset:{assetPath}</color>");
                return default;
            }

            var handle = new TimelinePlayRequestHandle(this, ++_nextRequestId, _generation);
            if (_assetCache.TryGetValue(assetPath, out var cachedAsset))
            {
                PlayWithDirector(assetPath, cachedAsset, director);
                return handle;
            }

            var timelineObject = _assetPool.Spawn(assetPath);
            if (timelineObject != null)
            {
                var pooledAsset = timelineObject.Target as PlayableAsset;
                RegisterPlayableAssetIfNeeded(assetPath, pooledAsset);
                PlayWithDirector(assetPath, pooledAsset, director);
                return handle;
            }

            EnqueuePendingPlayRequest(handle, assetPath, director);
            if (_loadingAssetNames.Add(assetPath))
                GameEntry.Resource.LoadAsset(assetPath, _loadAssetCallBack, null);

            return handle;
        }

        public void Stop(TimelinePlayRequestHandle handle, PlayableDirector director)
        {
            Cancel(handle);
            if (director != null)
                director.Stop();
        }

        internal void Cancel(TimelinePlayRequestHandle handle)
        {
            if (handle.Owner != this || handle.Generation != _generation)
                return;

            _pendingRequests.Remove(handle.RequestId);
        }

        private void OnLoadAssetSucc(string assetName, object asset, float duration, object userData)
        {
            var playableAsset = asset as PlayableAsset;
            if (playableAsset == null)
            {
                Tools.Logger.Warning("<color=yellow>Component_Timeline.OnLoadAssetSucc()--->playableAsset is null</color>");
                ClearLoadingState(assetName);
                return;
            }

            RegisterPlayableAssetIfNeeded(assetName, playableAsset);
            DrainPendingPlayRequests(assetName);
        }

        private void RegisterPlayableAssetIfNeeded(string assetName, PlayableAsset playableAsset)
        {
            if (playableAsset == null || _assetCache.ContainsKey(assetName))
                return;

            if (_registeredAssets.Add(playableAsset))
            {
                var obj = Object_PlayableAsset.Create(assetName, playableAsset);
                _assetPool.Register(obj, false);
            }

            _assetCache.Add(assetName, playableAsset);
        }

        private static void PlayWithDirector(string assetName, PlayableAsset playableAsset, PlayableDirector director)
        {
            director.playableAsset = playableAsset;
            if (director.playableAsset == null)
            {
                Tools.Logger.Warning($"<color=yellow>Component_Timeline.Play()--->playable asset is null, asset:{assetName}</color>");
                return;
            }

            director.Play();
        }

        private void OnLoadAssetFaild(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            Tools.Logger.Warning($"<color=yellow>Component_Timeline.OnLoadAssetFaild()--->asset:{assetName},status:{status},error:{errorMessage}</color>");
            ClearLoadingState(assetName);
        }

        protected override void Awake()
        {
            base.Awake();
            _generation++;
        }

        private void Start()
        {
            _assetPool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<Object_PlayableAsset>(nameof(Object_PlayableAsset));
            _assetPool.ExpireTime = 3600f;
            _loadAssetCallBack = new LoadAssetCallbacks(OnLoadAssetSucc, OnLoadAssetFaild);
        }

        private void OnDestroy()
        {
            _generation++;
            _pendingRequests.Clear();
            _pendingRequestIdsByAsset.Clear();
            _loadingAssetNames.Clear();
        }

        private void EnqueuePendingPlayRequest(
            TimelinePlayRequestHandle handle,
            string assetPath,
            PlayableDirector director)
        {
            _pendingRequests.Add(handle.RequestId, new PendingPlayRequest(assetPath, director));
            if (!_pendingRequestIdsByAsset.TryGetValue(assetPath, out var requestIds))
            {
                requestIds = new List<long>();
                _pendingRequestIdsByAsset.Add(assetPath, requestIds);
            }

            requestIds.Add(handle.RequestId);
        }

        private void DrainPendingPlayRequests(string assetPath)
        {
            _loadingAssetNames.Remove(assetPath);
            if (!_pendingRequestIdsByAsset.TryGetValue(assetPath, out var requestIds))
                return;

            _pendingRequestIdsByAsset.Remove(assetPath);
            for (var i = 0; i < requestIds.Count; i++)
            {
                var requestId = requestIds[i];
                if (!_pendingRequests.TryGetValue(requestId, out var request))
                    continue;

                _pendingRequests.Remove(requestId);
                if (_assetCache.TryGetValue(request.AssetPath, out var playableAsset))
                    PlayWithDirector(request.AssetPath, playableAsset, request.Director);
            }
        }

        private void ClearLoadingState(string assetPath)
        {
            _loadingAssetNames.Remove(assetPath);
            if (!_pendingRequestIdsByAsset.TryGetValue(assetPath, out var requestIds))
                return;

            _pendingRequestIdsByAsset.Remove(assetPath);
            for (var i = 0; i < requestIds.Count; i++)
                _pendingRequests.Remove(requestIds[i]);
        }

        private sealed class PendingPlayRequest
        {
            public PendingPlayRequest(string assetPath, PlayableDirector director)
            {
                AssetPath = assetPath;
                Director = director;
            }

            public string AssetPath { get; }
            public PlayableDirector Director { get; }
        }

        private readonly Dictionary<string, PlayableAsset> _assetCache = new Dictionary<string, PlayableAsset>();
        private readonly HashSet<string> _loadingAssetNames = new HashSet<string>();
        private readonly Dictionary<long, PendingPlayRequest> _pendingRequests = new Dictionary<long, PendingPlayRequest>();
        private readonly Dictionary<string, List<long>> _pendingRequestIdsByAsset = new Dictionary<string, List<long>>();
        private readonly HashSet<PlayableAsset> _registeredAssets = new HashSet<PlayableAsset>();
        private IObjectPool<Object_PlayableAsset> _assetPool;
        private LoadAssetCallbacks _loadAssetCallBack;
        private long _nextRequestId;
        private int _generation;
    }
}
