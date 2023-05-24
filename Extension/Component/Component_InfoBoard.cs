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
        /// 获取一个hpbar，获取不到返回null
        /// </summary>
        public Object_HPBar GenHPBar()
        {
            Object_HPBar obj = GenObject<Object_HPBar>(typeof(Object_HPBar).Name);
            if (obj is null)//对象池里没对象，先创建
            {
                if (_hp_bar_prefab == null)
                {
                    Log.Warning("<color=red>Component_InfoBoard.GenHPBar()--->obj is null</color>");
                    return null;
                }

                var go = Instantiate(_hp_bar_prefab) as GameObject;
                InitTransform(go.transform);
                //spanw obj
                var pool = GameEntry.ObjectPool.GetObjectPool<Object_HPBar>(nameof(Object_HPBar));
                pool.Register(Object_HPBar.Gen(go), false );
                obj = pool.Spawn();
            }
            obj.Setup(obj.Target as GameObject);
            return obj;
        }
        
        /// <summary>
        /// 初始化变换
        /// </summary>
        private void InitTransform(Transform tran)
        {
            tran.SetParent(_root);
            tran.localScale = Vector3.one;
        }
        
        /// <summary>
        /// 获取一个指定类型的对象池对象，拿不到返回null
        /// </summary>
        private T GenObject<T>(string pool_name) where T : Aquila_Object_Base
        {
            var type_name = nameof(T);
            var pool = GameEntry.ObjectPool.GetObjectPool<T>(pool_name);
            if (pool != null)
                return pool.Spawn() as T;
            
            Log.Warning("<color=yellow>Component_InfoBoard.GenObject--->pool == null</color>");
            return null;
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

        /// <summary>
        /// 世界坐标转信息板的矩形空间坐标
        /// </summary>
        public Vector2 WorldPos2BoardRectPos(Vector3 world_pos,Camera world_camera)
        {
            var screen_pos = RectTransformUtility.WorldToScreenPoint(world_camera, world_pos);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(Rect, screen_pos, Camera,
                out var board_pos);
            return board_pos;
        }

        //-----------------------priv-----------------------
        public void Preload()
        {
            if (_init_flag)
            {
                return;
            }

            //创建对象池
            var pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_HPBar>(typeof(Object_HPBar).Name, 0xf);
            pool.ExpireTime = 360f;
            
            GameEntry.Resource.LoadAsset
                (
                    @"Assets/Res/Prefab/UI/Item/HPBar.prefab",
                    new LoadAssetCallbacks((assetName, asset, duration, userData) =>
                        {
                            _hp_bar_prefab = asset as GameObject;
                            if (_hp_bar_prefab == null)
                            {
                                Log.Warning("<color=yellow>HPBar.prefab convert faild</color>");
                                return;
                            }

                            if (GameEntry.Procedure.GetProcedure<Procedure_Prelaod>() is not Procedure_Prelaod procedure)
                            {
                                Log.Warning("<color=yellow>procedure preload is null</color>");
                                return;
                            }
                            //#todo:主动通知流程加载完成，因为GF只有异步加载,暂时没时间加同步，先这样做了
                            procedure.NotifyFlag(Procedure_Prelaod._infoboard_load_finish);
                        },
                        (assetName, status, errorMessage, userData) =>
                        {
                            Log.Warning("<color=yellow>hpbar asset doesnt exit!</color>");
                        })
                );
            _init_flag = true;
        }

        protected override void Awake()
        {
            base.Awake();
            _init_flag = false;
        }
        
        //-----------------------fields-----------------------
        public RectTransform Rect
        {
            get
            {
                if (_rect == null)
                    _rect = _root.GetComponent<RectTransform>();

                return _rect;
            }
        }
        private RectTransform _rect = null;
        
        /// <summary>
        /// 渲染信息板的画布
        /// </summary>
        public Canvas Canvas => _canvas;
        
        /// <summary>
        /// 渲染信息板用的相机
        /// </summary>
        public Camera Camera => _camera;

        /// <summary>
        /// 初始化标记
        /// </summary>
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
