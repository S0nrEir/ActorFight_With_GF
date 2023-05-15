using System;
using System.Collections;
using System.Collections.Generic;
using Aquila.ObjectPool;
using Aquila.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace  Aquila.Extension
{
    
    public class Component_InfoBoard : GameFrameworkComponent
    {
        /// <summary>
        /// 获取一个指定类型的对象池对象，拿不到返回null
        /// </summary>
        public T GenObject<T>() where T : Aquila_Object_Base
        {
            return null;
            // var pool = GameEntry.ObjectPool.GetObjectPool<T>(nameof(T));
            // if (pool is null)
            // {
            //     Log.Warning("<color=yellow>Component_InfoBoard.GenObject--->pool == null</color>");
            //     return null;
            // }
            //
            // var obj = pool.Spawn() as T;
            // GameEntry.Resource.LoadAsset();
            //
            // return false;
        }

        /// <summary>
        /// 回收
        /// </summary>
        public bool UnSpawn<T>(object obj) where T : Aquila_Object_Base
        {
            var pool = GameEntry.ObjectPool.GetObjectPool<T>(nameof(T));
            if (pool is null)
            {
                Log.Warning("<color=yellow>Component_InfoBoard.UnSpawn--->pool == null</color>");
                return false;
            }
            pool.Unspawn(obj);
            return true;
        }

        //-----------------------priv-----------------------
        public void Preload()
        {
            if(_init_flag)
                return;
            
            //创建对象池
            var pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_HPBar>(nameof(Object_HPBar),0xf).ExpireTime = 360f;
            
            GameEntry.Resource.LoadAsset
                (
                    @"Assets/Res/Prefab/Common/HPBar.prefab",
                    new LoadAssetCallbacks((assetName, asset, duration, userData) =>
                        {
                            _hp_bar_prefab = asset as GameObject;
                            if (_hp_bar_prefab == null)
                            {
                                Log.Warning("<color=yellow>HPBar.prefab convert faild</color>");
                                return;
                            }

                            var procedure = GameEntry.Procedure.GetProcedure<Procedure_Prelaod>() as Procedure_Prelaod;
                            if (procedure is null)
                            {
                                Log.Warning("<color=yellow>procedure preload is null</color>");
                                return;
                            }
                            //#todo:主动通知流程加载完成，因为GF只有异步加载，所以先这样做了
                            procedure.NotifyFlag(Procedure_Prelaod._infoboard_load_finish);
                        },
                        (assetName, status, errorMessage, userData) =>
                        {
                            Log.Warning("<color=yellow>hpbar asset doesnt exit!</color>");
                        })
                );
            
            _init_flag = true;
        }

        private void Start()
        {
            // _init_flag = true;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        // private void Update()
        // {
        //     throw new NotImplementedException();
        // }
        
        //-----------------------fields-----------------------
        private bool _init_flag = false;

        /// <summary>
        /// 根节点
        /// </summary>
        [SerializeField] private Transform _root = null;

        /// <summary>
        /// 画布
        /// </summary>
        private Canvas _canvas = null;

        /// <summary>
        /// hpbar预设
        /// </summary>
        private GameObject _hp_bar_prefab = null;

        /// <summary>
        /// 显示用的相机
        /// </summary>
        [SerializeField] private Camera _camera = null;
    }
}
