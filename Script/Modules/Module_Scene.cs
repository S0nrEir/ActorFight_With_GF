using Aquila.Config;
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
        /// 开始战斗
        /// </summary>
        public void Start( int x_width, int z_width )
        {
            _fight_flag = true;
            Terrain_Module.Start( x_width, z_width );
            _actor_module = GameEntry.Module.GetModule<Module_Actor>();
        }

        /// <summary>
        /// 结束战斗
        /// </summary>
        public void End()
        {
            Terrain_Module.End();
            //_actor_module = null;
            _fight_flag = false;
        }

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
            Terrain_Module = ReferencePool.Acquire<Module_Fighting_Terrain>();
            _sub_module_dic.Add( typeof( Module_Fighting_Terrain ), Terrain_Module );

            var iter = _sub_module_dic.GetEnumerator();
            while ( iter.MoveNext() )
                iter.Current.Value.EnsureInit();
        }

        /// <summary>
        /// 刷帧处理选定逻辑
        /// </summary>
        public void OnUpdate( float deltaTime )
        {
            return;
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
        public Module_Fighting_Terrain Terrain_Module { get; private set; }

        /// <summary>
        /// 子模块集合
        /// </summary>
        private Dictionary<System.Type, IModule_Fighting_SubModule> _sub_module_dic = null;

        /// <summary>
        /// 开始标记
        /// </summary>
        private bool _fight_flag = false;
    }

    public interface IModule_Fighting_SubModule
    {
        public void EnsureInit();
        public void OnClose();
        public void End();
    }

}
