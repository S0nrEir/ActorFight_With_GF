using Aquila.Config;
using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using Cfg.Common;
using UnityEngine;
using UnityGameFramework.Runtime;

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
            //_input_module       = GameEntry.Module.GetModule<Module_Input>();
            _terrain_module     = GameEntry.Module.GetModule<Module_Terrain>();
            //_actor_fac_module   = GameEntry.Module.GetModule<Module_Actor_Fac>();
        }

        protected override async void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            _procedure_owner = procedureOwner;
            if ( !InitializeData( procedureOwner ) )
            {
                Log.Error( "procedure data initialize failed!" );
                return;
            }

            var scene_config = Tools.Table.GetSceneConfig();
            MainCameraInitializeSetting();

            var param = ReferencePool.Acquire<Fight_Param>();
            param.x_width = scene_config.Fight_Scene_Default_X_Width;
            param.z_width = scene_config.Fight_Scene_Default_Y_Width;
            param._scene_script_meta = _data._scene_script_meta;

            //if ( !_scene_module.Start( param ) )
            //    Log.Error( "scene_module start failed" );

            //_input_module.Start( param );
            _terrain_module.Start( param );
            var entity_id = ACTOR_ID_POOL.Gen();
            var entity = await GameEntry.Module.GetModule<Module_Actor_Fac>().ShowActorAsync<HeroActor>
                (
                    1,
                    entity_id,
                    @"Assets/Res/Prefab/Character/TestCharacter_001.prefab",
                    0,
                    0,
                    new HeroActorEntityData( entity_id ) { _roleMetaID = 1 }
                );

            //_actor_fac_module.Start( param );
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            //_terrain_module.End();
            //_input_module.End();
            _terrain_module.End();
            //_actor_fac_module.End();
            _procedure_owner = null;
            if ( !procedureOwner.RemoveData( typeof( Procedure_Fight_Variable ).Name ) )
                Log.Error( "Failed to remove procedure data Procedure_Fight_Variable " );

            base.OnLeave( procedureOwner, isShutdown );
        }

        /// <summary>
        /// 初始化流程数据，失败返回false
        /// </summary>
        private bool InitializeData( IFsm<IProcedureManager> owner )
        {
            var name = typeof( Procedure_Fight_Variable ).Name;
            if ( !owner.HasData( name ) )
                return false;

            var variable = owner.GetData( name );
            _data = null;
            _data = variable.GetValue() as Procedure_Fight_Data;
            if ( _data is null )
            {
                Log.Error( "_data is null!" );
                return false;
            }
            return true;
        }

        /// <summary>
        /// 主相机初始化设置
        /// </summary>
        private void MainCameraInitializeSetting()
        {
            _mainCamera = GameEntry.GlobalVar.MainCamera;
            var sceneConfig = GameEntry.DataTable.Tables.SceneConfig;
            _mainCamera.transform.eulerAngles = sceneConfig.Main_Camera_Default_Euler;
            _mainCamera.transform.position = sceneConfig.MainCameraDefaultPosition;

        }

        /// <summary>
        /// 场景主相机
        /// </summary>
        private Camera _mainCamera = null;

        /// <summary>
        /// 模块
        /// </summary>
        //private Module_Input _input_module = null;

        /// <summary>
        /// 地块模块
        /// </summary>
        private Module_Terrain _terrain_module = null;

        /// <summary>
        /// actor工厂模块
        /// </summary>
        //private Module_Actor_Fac _actor_fac_module = null;

        /// <summary>
        /// 地块模块
        /// </summary>
        //private Module_Terrain _terrain_module = null;

        /// <summary>
        /// 战斗阶段状态数据
        /// </summary>
        private Procedure_Fight_Data _data = null;

        /// <summary>
        /// 流程持有者
        /// </summary>
        private IFsm<IProcedureManager> _procedure_owner = null;
    }

    internal class Procedure_Fight_Data : IReference
    {
        public void Clear()
        {
            _chunk_name = string.Empty;
            _scene_script_meta = null;
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string _chunk_name = string.Empty;

        /// <summary>
        /// 场景脚本表数据
        /// </summary>
        public Table_Scripts _scene_script_meta = null;
    }

    /// <summary>
    /// 战斗流程的数据携带类，它的实例持有Procedure_Fight_Data#todo这里重新设计
    /// </summary>
    internal class Procedure_Fight_Variable : Variable<Procedure_Fight_Data>
    {
        public override Type Type => typeof( Procedure_Fight_Variable );
        public override void SetValue( object value )
        {
            base.SetValue( value );
        }

        //orginal code:
        //public override void SetValue( object value )
        //{
        //    m_Value = ( T ) value;
        //}

        public override object GetValue()
        {
            return base.GetValue();
        }

        //orginal code:
        //public override void Clear()
        //{
        //    m_Value = default( T );
        //}

        public override string ToString()
        {
            return base.ToString();
        }

        //orginal code:
        //public override string ToString()
        //{
        //    return ( m_Value != null ) ? m_Value.ToString() : "<Null>";
        //}

        public override void Clear()
        {
            if ( Value is IReference )
                ReferencePool.Release( ( Value as IReference ) );

            Value = null;
            //base.Clear();
        }

        //orginal code:
        //public override void Clear()
        //{
        //    m_Value = default( T );
        //}
    }
}
