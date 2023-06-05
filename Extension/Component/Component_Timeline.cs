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
    /// timeline���
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
                //����û�оͼ���
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
        /// ���سɹ�
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
            //#todo���ǳ���director�Ľ�ɫ�Ѿ����������
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
        /// ����ʧ��
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
        /// playableAsset����
        /// </summary>
        private Dictionary<string, PlayableAsset> _assetCache = null;

        /// <summary>
        /// �����
        /// </summary>
        private IObjectPool<Object_PlayableAsset> _assetPool = null;

        /// <summary>
        /// �ʲ����ػص�
        /// </summary>
        private LoadAssetCallbacks _loadAssetCallBack = null;


        /// <summary>
        /// timeline asset�ļ��ز���
        /// </summary>
        private class LoadPlayableAssetData
        {
            public PlayableDirector _director = null;
            public bool _playNow = false;
        }
    }

}