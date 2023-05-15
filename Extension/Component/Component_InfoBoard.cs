using System;
using System.Collections;
using System.Collections.Generic;
using Aquila.ObjectPool;
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
        public Aquila_Object_Base GenObject<T>() where T : Aquila_Object_Base
        {
            var pool = GameEntry.ObjectPool.GetObjectPool<T>(nameof(T));
            if (pool == null)
            {
                Log.Warning("<color=yellow>Component_InfoBoard.GenObject--->pool == null</color>");
                return null;
            }
            return pool.Spawn() as T;
        }

        //-----------------------priv-----------------------
        public void Preload()
        {
            if(_init_flag)
                return;
            
            //创建对象池
            var pool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<Object_HPBar>(nameof(Object_HPBar),0xf).ExpireTime = 360f;
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
    }
}
