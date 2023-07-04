using Aquila.Module;
using Aquila.Toolkit;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    //TActorBase的扩展类，专门写适用于具体项目需求，对应于TActorbase的扩展函数、属性和字段
    public abstract partial class Actor_Base
    {
        /// <summary>
        /// 扩展回收，专门用于Actor在调用OnRecycle时对于扩展的Recycle
        /// </summary>
        private void ExtensionRecycle()
        {
            _grid_x = 0;
            _grid_z = 0;
            _coord = (-1, -1);
        }

        /// <summary>
        /// 设置actor地块坐标
        /// </summary>
        public void SetCoord( int unique_key )
        {
            var coord = Tools.Fight.UniqueKey2Coord( unique_key );
            _grid_x = coord.x;
            _grid_z = coord.y;
            _coord = (_grid_x, _grid_z);
        }

        /// <summary>
        /// 设置actor的地块坐标和对应的世界坐标位置
        /// </summary>
        public bool SetCoordAndPosition( int grid_x, int grid_z )
        {
            SetCoord( grid_x, grid_z );
            SetWorldPosition( new Vector3( 0f, 0f, 0f ) );

            return true;
        }

        /// <summary>
        /// 设置actor地块坐标
        /// </summary>
        public void SetCoord( int grid_x, int grid_z )
        {
            _grid_x = grid_x;
            _grid_z = grid_z;
        }

        /// <summary>
        /// 获取所属的坐标
        /// </summary>
        public (int x, int z) Coord()
        {
            return _coord;
        }

        //xz坐标
        private int _grid_x = 0;
        private int _grid_z = 0;

        private (int x, int z) _coord = (-1, -1);
    }
}
