using Aquila.Fight.Actor;
using Aquila.ObjectPool;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// timeline组件
    /// </summary>
    public class Component_Timeline : GameFrameworkComponent
    {
        public void Play( string assetPath ,PlayableDirector director)
        {
            if ( string.IsNullOrEmpty( assetPath ) )
            {
                Log.Warning( "<color=yellow>Compoent_Timeline.Play()--->string.IsNullOrEmpty( path )</color>" );
                return;
            }

            var timelineObject = _assetPool.Spawn(assetPath);
            if ( timelineObject == null )
            {
                //池里没有就加载
                GameEntry.Resource.LoadAsset
                    ( 
                        assetPath, 
                        _loadAssetCallBack,
                        new LoadPlayableAssetData() { _director = director ,_playNow = true} 
                    );
            }
            else
            {
                director.playableAsset = timelineObject.Target as PlayableAsset;
                if ( director.playableAsset is null )
                {

                    Log.Warning( "<color=yellow>Component_Timeline.Play()--->director.playableAsset is null </color>" );
                    return;
                }
                director.Play();
            }
        }

        //---------------priv---------------
        /// <summary>
        /// 加载成功
        /// </summary>
        private void OnLoadAssetSucc( string assetName, object asset, float duration, object userData )
        {
            var playableAsset= asset as PlayableAsset;
            if ( playableAsset is null )
            {
                Log.Warning( "<color=yellow>Component_Timeline.OnLoadAssetSucc()--->playableAsset is null </color>" );
                return;
            }

            var obj = Object_PlayableAsset.Create( assetName, playableAsset );
            //#todo考虑持有director的角色已经死亡的情况
            _assetPool.Register( obj, false );
            var playData = userData as LoadPlayableAssetData;
            if ( playData is null )
            {
                //Log.Warning( "<color=yellow>Component_Timeline.OnLoadAssetSucc()--->playData is null </color>" );
                return;
            }
            if ( playData._playNow && playData._director != null)
                Play( assetName, playData._director );
        }

        /// <summary>
        /// 加载失败
        /// </summary>
        private void OnLoadAssetFaild( string assetName, LoadResourceStatus status, string errorMessage, object userData )
        {
            
        }

        //---------------override---------------
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            _assetPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_PlayableAsset>( nameof( Object_PlayableAsset ) );
            _assetPool.ExpireTime = 3600f;
            _loadAssetCallBack = new LoadAssetCallbacks( OnLoadAssetSucc, OnLoadAssetFaild );
        }

        /// <summary>
        /// playableAsset缓存
        /// </summary>
        private Dictionary<string, PlayableAsset> _assetCache = null;

        /// <summary>
        /// 对象池
        /// </summary>
        private IObjectPool<Object_PlayableAsset> _assetPool = null;

        /// <summary>
        /// 资产加载回调
        /// </summary>
        private LoadAssetCallbacks _loadAssetCallBack = null;


        /// <summary>
        /// timeline asset的加载参数
        /// </summary>
        private class LoadPlayableAssetData
        {
            public PlayableDirector _director = null;
            public bool _playNow = false;
        }
    }

}