using GameFramework;
using System;
using UnityEngine;

namespace Aquila.Config
{
    /// <summary>
    /// 全局实例
    /// </summary>
    public static class GlobalVar
    {
        /// <summary>
        /// 无效GUID
        /// </summary>
        public const UInt64 INVALID_GUID = 0xFFFFFFFFFFFFFFFFul;

        /// <summary>
        /// 无效ID
        /// </summary>
        public const int INVALID_ID = -1;
        
        /// <summary>
        /// 场景主相机
        /// </summary>
        public static Camera Main_Camera => _main_camera ?? GetMainCamera();

        /// <summary>
        /// 获取主相机
        /// </summary>
        public static Camera GetMainCamera()
        {
            var cameraGO = GameObject.FindGameObjectWithTag( "MainCamera" );
            if ( cameraGO == null )
                throw new GameFrameworkException( "faild to find MainCamera GameObject!" );

            _main_camera = cameraGO.GetComponent<Camera>();
            if ( _main_camera == null )
                throw new GameFrameworkException( "faild to get MainCamera!" );

            UnityEngine.Object.DontDestroyOnLoad( cameraGO );
            return _main_camera;
        }

        /// <summary>
        /// 主相机
        /// </summary>
        private static Camera _main_camera = null;
    }

}
