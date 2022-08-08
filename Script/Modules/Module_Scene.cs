﻿using Aquila.Config;
using Aquila.Extension;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// 场景模块，场景入口，管理下面的actor，地块等
    /// </summary>
    public partial class Module_Scene : GameFrameworkModuleBase, IUpdate
    {
        public T GetSubModule<T>( System.Type type ) where T : class, IModule_Fighting_SubModule
        {
            if ( _sub_module_dic is null || _sub_module_dic.Count == 0 )
                return null;

            _sub_module_dic.TryGetValue( type, out var sub_module );
            return sub_module as T;
        }

        /// <summary>
        /// 场景初始化
        /// </summary>
        public bool Start( Module_Scene_Param param )
        {
            if ( param is null )
                return _fight_flag;

            if ( !param.FieldValid() )
                return _fight_flag;

            _param = param;

            //module
            Terrain_Module.Start( param.x_width,param.z_width );
            _actor_module = GameEntry.Module.GetModule<Module_Actor>();

            //script
            _fight_flag = true;
            return _fight_flag;
        }

        /// <summary>
        /// 结束战斗
        /// </summary>
        public void End()
        {
            Terrain_Module.End();
            //_actor_module = null;
            ReferencePool.Release( _param );
            _param = null;
            _fight_flag = false;
        }

        #region override
        public override void OnClose()
        {
            _fight_flag = false;

            Terrain_Module = null;

            if ( _sub_module_dic != null )
            {
                var iter = _sub_module_dic.GetEnumerator();
                while ( iter.MoveNext() )
                {
                    var value = iter.Current.Value;
                    value.OnClose();
                    ReferencePool.Release( value as IReference );
                }
                _sub_module_dic.Clear();
            }
            _sub_module_dic = null;
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            _fight_flag = false;
            if ( _sub_module_dic is null )
                _sub_module_dic = new Dictionary<System.Type, IModule_Fighting_SubModule>();

            //添加sub module
            Terrain_Module = ReferencePool.Acquire<Module_Scene_Terrain>();
            _sub_module_dic.Add( typeof( Module_Scene_Terrain ), Terrain_Module );

            var iter = _sub_module_dic.GetEnumerator();
            while ( iter.MoveNext() )
                iter.Current.Value.EnsureInit();
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
            var terrain = Terrain_Module.Get( _curr_hovered_go );
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

        /// <summary>
        /// 地块模块
        /// </summary>
        //private Module_Fighting_Terrain _terrain_module = null;
        public Module_Scene_Terrain Terrain_Module { get; private set; }

        /// <summary>
        /// 子模块集合
        /// </summary>
        private Dictionary<System.Type, IModule_Fighting_SubModule> _sub_module_dic = null;

        /// <summary>
        /// 开始标记
        /// </summary>
        private bool _fight_flag = false;

        /// <summary>
        /// 场景参数
        /// </summary>
        private Module_Scene_Param _param = null;
    }

    public interface IModule_Fighting_SubModule
    {
        public void EnsureInit();
        public void OnClose();
        public void End();
    }

    /// <summary>
    /// 场景参数
    /// </summary>
    public class Module_Scene_Param : IReference
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
