using Aquila.Module;
using Aquila.ToolKit;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    //TActorBase����չ�࣬ר��д�����ھ�����Ŀ���󣬶�Ӧ��TActorbase����չ���������Ժ��ֶ�
    public abstract partial class TActorBase
    {
        /// <summary>
        /// ��չ���գ�ר������Actor�ڵ���OnRecycleʱ������չ��Recycle
        /// </summary>
        private void ExtensionRecycle()
        {
            _grid_x = 0;
            _grid_z = 0;
            _coord = (-1, -1);
        }

        /// <summary>
        /// ����actor�ؿ�����
        /// </summary>
        public void SetCoord( int unique_key )
        {
            var coord = Tools.Fight.UniqueKey2Coord( unique_key );
            _grid_x = coord.x;
            _grid_z = coord.y;
            _coord = (_grid_x, _grid_z);
        }

        /// <summary>
        /// ����actor�ĵؿ�����Ͷ�Ӧ����������λ��
        /// </summary>
        public bool SetCoordAndPosition( int grid_x, int grid_z )
        {
            var terrain = GameEntry.Module.GetModule<Module_Terrain>().Get( Tools.Fight.Coord2UniqueKey( grid_x, grid_z ) );
            if ( terrain is null || terrain.State != ObjectPool.TerrainStateTypeEnum.NONE )
            {
                Log.Warning( "terrain is null || terrain.State != ObjectPool.TerrainStateTypeEnum.NONE", LogColorTypeEnum.Yellow );
                return false;
            }

            SetCoord( grid_x, grid_z );
            SetWorldPosition( terrain.WorldHangPosition );

            return true;
        }

        /// <summary>
        /// ����actor�ؿ�����
        /// </summary>
        public void SetCoord( int grid_x, int grid_z )
        {
            _grid_x = grid_x;
            _grid_z = grid_z;
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public (int x, int z) Coord()
        {
            return _coord;
        }

        //xz����
        private int _grid_x = 0;
        private int _grid_z = 0;

        private (int x, int z) _coord = (-1, -1);
    }
}