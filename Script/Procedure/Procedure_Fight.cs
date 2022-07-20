﻿using Aquila.Config;
using Aquila.Module;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Procedure
{
    /// <summary>
    /// 战斗流程
    /// </summary>
    public class Procedure_Fight : ProcedureBase
    {
        protected override void OnInit( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnInit( procedureOwner );
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            GameFrameworkModule.GetModule<Module_Terrain>().GenerateFightSceneTerrain( GameConfig.Scene.FIGHT_SCENE_DEFAULT_X_WIDTH, GameConfig.Scene.FIGHT_SCENE_DEFAULT_Y_WIDTH );
            MainCameraInitializeSetting();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            base.OnLeave( procedureOwner, isShutdown );
            GameFrameworkModule.GetModule<Module_Terrain>().RemoveAll();
        }

        /// <summary>
        /// 主相机初始化设置
        /// </summary>
        private void MainCameraInitializeSetting()
        {
            _main_camera = GlobalVar.Main_Camera;
            _main_camera.transform.eulerAngles = GameConfig.Scene.MAIN_CAMERA_DEFAULT_EULER;
            _main_camera.transform.position = GameConfig.Scene.MAIN_CAMERA_DEFAULT_POSITION;
        }

        /// <summary>
        /// 场景主相机
        /// </summary>
        private Camera _main_camera = null;
    }

}
