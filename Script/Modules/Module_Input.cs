using Aquila.Config;
using Aquila.Extension;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// 输入模块，管理输入
    /// </summary>
    public partial class Module_Input : GameFrameworkModuleBase, IUpdate
    {
        /// <summary>
        /// 场景初始化
        /// </summary>
        public override void Start( object param )
        {
            base.Start( param );
            var temp = param as Fight_Param;
            if ( temp is null || !temp.FieldValid() )
                throw new GameFrameworkException( "start scene module faild" );

            _param = temp;
        }

        /// <summary>
        /// 结束战斗
        /// </summary>
        public override void End()
        {
            ReferencePool.Release( _param );
            _param = null;
            base.End();
        }

        #region override
        public override void OnClose()
        {
            _fight_flag = false;
            _terrain_module = null;
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            _fight_flag = false;

            //添加sub module
            _terrain_module = GameEntry.Module.GetModule<Module_Terrain>();
        }
        #endregion

        /// <summary>
        /// 刷帧处理选定逻辑
        /// </summary>
        public void OnUpdate( float deltaTime )
        {
            if ( !_fight_flag )
                return;

            var ray = GlobalVar.Main_Camera.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
            if ( !Physics.Raycast( ray, out hit, 10000f, 256 ) )
                return;

            //Log.Info( "raycast hitted!", LogColorTypeEnum.White );
            OnRaycastHit( hit );
        }

        /// <summary>
        /// 当射线击中terrain
        /// </summary>
        private void OnRaycastHit( RaycastHit hit )
        {
            _curr_hovered_go = hit.collider.gameObject;
            var terrain = _terrain_module.Get( _curr_hovered_go );
            if ( terrain is null )
            {
                Log.Error( "terrain is null" );
                return;
            }

            Log.Info( $"unique key :{terrain.UniqueKey}" );
        }

        /// <summary>
        /// 输入射线方向
        /// </summary>
        //private Vector3 _raycast_dir = Vector3.zero; 

        /// <summary>
        /// 当前滑动到的gameObject
        /// </summary>
        private GameObject _curr_hovered_go = null;

        /// <summary>
        /// actor模块
        /// </summary>
        private Module_Actor _actor_module = null;

        public Module_Terrain _terrain_module { get; private set; }

        /// <summary>
        /// 开始标记
        /// </summary>
        private bool _fight_flag = false;

        /// <summary>
        /// 场景参数
        /// </summary>
        private Fight_Param _param = null;
    }

    /// <summary>
    /// 场景参数
    /// </summary>
    public class Fight_Param : IReference
    {
        public int x_width = 0;
        public int z_width = 0;
        public Cfg.common.Scripts _scene_script_meta = null;

        /// <summary>
        /// 检查字段有效性
        /// </summary>
        public bool FieldValid()
        {
            return z_width > 0 &&
                   x_width > 0;
        }

        public void Clear()
        {
            x_width = 0;
            z_width = 0;
            _scene_script_meta = null;
        }
    }

}
