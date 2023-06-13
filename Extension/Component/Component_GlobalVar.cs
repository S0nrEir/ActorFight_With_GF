using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace  Aquila.Extension
{
    /// <summary>
    /// 全局组件，存储一些全局可访问的实例和变量
    /// </summary>
    public class Component_GlobalVar : GameFrameworkComponent
    {
        //--------------fields--------------
        /// <summary>
        /// 无效的GUID
        /// </summary>
        public const UInt64 InvalidGUID = 0xFFFFFFFFFFFFFFFFul;

        /// <summary>
        /// 无效ID
        /// </summary>
        public const int InvalidID = -1;

        /// <summary>
        /// 主相机
        /// </summary>
        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                    _mainCamera = GetMainCamera();

                return _mainCamera;
            }
        }
        
        //--------------priv--------------
        /// <summary>
        /// 主相机
        /// </summary>
        private Camera _mainCamera = null;

        private Camera GetMainCamera()
        {
            var camera_go = GameObject.FindWithTag("MainCamera");
            if (camera_go == null)
            {
                Log.Warning("<color=yellow>faild to get main camera</color>");
                return null;
            }
            DontDestroyOnLoad(camera_go);
            return camera_go.GetComponent<Camera>();
        }

        //--------------override--------------
        protected override void Awake()
        {
            base.Awake();
            _mainCamera = GetMainCamera();
        }
    }

}
