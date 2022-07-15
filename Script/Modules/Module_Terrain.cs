using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Module
{
    /// <summary>
    /// 战斗地形的模组类，负责处理地块的生成，管理，回收，逻辑
    /// </summary>
    public class Module_Terrain : GameFrameworkModuleBase
    {
        //#todo所有的地块获取，都要从对象池里拿TerrainObject

        public override void OnClose()
        {
            _root_go = null;
            _terrain_cache_dic?.Clear();
            _terrain_cache_dic = null;
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
        }

        /// <summary>
        /// 地块缓存
        /// </summary>
        private Dictionary<int, GameObject> _terrain_cache_dic = null;

        /// <summary>
        /// 所有地块的根节点
        /// </summary>
        private GameObject Root_GO
        {
            get
            {
                if ( _root_go == null )
                {
                    //拿不到就找
                    _root_go = GameObject.FindGameObjectWithTag( "TerrainRoot" );
                    //找不到就创建
                    if ( _root_go == null )
                    {
                        _root_go = new GameObject( "TerrainRoot" );
                        _root_go.transform.position = Vector3.zero;
                        _root_go.transform.localScale = Vector3.one;
                        _root_go.transform.eulerAngles = Vector3.zero;
                    }
                }
                return _root_go;
            }
        }
        private GameObject _root_go = null;
    }
}
