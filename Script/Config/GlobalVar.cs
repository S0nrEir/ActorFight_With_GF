using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    /// <summary>
    /// 全局变量类
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
        public static Camera Main_Camera
        {
            get
            {
                if ( _main_camera == null )
                {
                    var cameraGO = GameObject.FindGameObjectWithTag( "MainCamera" );
                    if ( cameraGO == null )
                        throw new GameFrameworkException( "faild to find MainCamera GameObject!" );

                    _main_camera = cameraGO.GetComponent<Camera>();
                    if(_main_camera == null)
                        throw new GameFrameworkException( "faild to get MainCamera!" );
                }
                return _main_camera;
            }
        }

        /// <summary>
        /// 主相机
        /// </summary>
        private static Camera _main_camera = null;
    }

}
