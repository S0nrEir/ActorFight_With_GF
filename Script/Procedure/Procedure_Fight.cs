using Aquila.Config;
using Aquila.Module;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
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
            _terrain_module = GameFrameworkModule.GetModule<Module_Terrain>();
            _fight_module = GameFrameworkModule.GetModule<Module_Fight>();
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            if ( !InitializeData( procedureOwner ) )
            {
                Log.Error( "procedure data initialize faild!" );
                return;
            }
            _terrain_module.Start( GameConfig.Scene.FIGHT_SCENE_DEFAULT_X_WIDTH, GameConfig.Scene.FIGHT_SCENE_DEFAULT_Y_WIDTH );
            MainCameraInitializeSetting();
            GameEntry.Lua.LoadScript( @"SceneModifier/Modifier_01", "Modifier_01" );
            _fight_module.Start();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            _terrain_module.End();
            _fight_module.End();
            base.OnLeave( procedureOwner, isShutdown );
        }

        /// <summary>
        /// 初始化流程数据
        /// </summary>
        private bool InitializeData(IFsm<IProcedureManager> owner)
        {
            var name = typeof( Procedure_Fight_Variable ).Name;
            if ( !owner.HasData( name ) )
                return false;

            var data = owner.GetData( name );
            _data = null;
            _data = data.GetValue() as Procedure_Fight_Data;
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
            _main_camera = GlobalVar.Main_Camera;
            _main_camera.transform.eulerAngles = GameConfig.Scene.MAIN_CAMERA_DEFAULT_EULER;
            _main_camera.transform.position = GameConfig.Scene.MAIN_CAMERA_DEFAULT_POSITION;
        }

        /// <summary>
        /// 场景主相机
        /// </summary>
        private Camera _main_camera = null;

        /// <summary>
        /// 战斗模块
        /// </summary>
        private Module_Fight _fight_module = null;

        /// <summary>
        /// 地块模块
        /// </summary>
        private Module_Terrain _terrain_module = null;

        /// <summary>
        /// 战斗阶段状态数据
        /// </summary>
        private Procedure_Fight_Data _data = null;
    }

    internal class Procedure_Fight_Data : IReference
    {
        public void Clear()
        {
        }
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
        //public override void SetValue( object value )
        //{
        //    m_Value = ( T ) value;
        //}

        public override object GetValue()
        {
            return base.GetValue();
        }

        //public override void Clear()
        //{
        //    m_Value = default( T );
        //}

        public override string ToString()
        {
            return base.ToString();
        }

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
        //public override void Clear()
        //{
        //    m_Value = default( T );
        //}
    }
}
