using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Module
{
    /// <summary>
    /// 场景管理模块
    /// </summary>
    public class Module_Scene : GameFrameworkModuleBase
    {
        public override void Start( object param )
        {
            base.Start( param );
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            _terrain_module = GameEntry.Module.GetModule<Module_Terrain>();
        }

        public override void End()
        {
            base.End();
        }

        public override void OnClose()
        {
            _terrain_module = null;
            base.OnClose();
        }

        /// <summary>
        /// 地块模块
        /// </summary>
        private Module_Terrain _terrain_module = null;
    }
}
