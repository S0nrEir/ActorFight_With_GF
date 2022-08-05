using Aquila.Config;
using Aquila.Extension;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// 战斗模块，管理战斗的主要流程，规则
    /// </summary>
    public class Module_Fight : GameFrameworkModuleBase, IUpdate
    {
        /// <summary>
        /// 开始战斗
        /// </summary>
        public void Start()
        {
            _fight_flag = true;
            _actor_module = GameEntry.Module.GetModule<Module_Actor>();
            _terrain_module = GameEntry.Module.GetModule<Module_Terrain>();
        }

        /// <summary>
        /// 结束战斗
        /// </summary>
        public void End()
        {
            _actor_module = null;
            _fight_flag = false;
        }

        public override void OnClose()
        {
            _fight_flag = false;
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            _fight_flag = false;
        }

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
        private void OnRaycastHit(RaycastHit hit)
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

        /// <summary>
        /// 地块模块
        /// </summary>
        private Module_Terrain _terrain_module = null;

        /// <summary>
        /// 开始标记
        /// </summary>
        private bool _fight_flag = false;
    }

}
