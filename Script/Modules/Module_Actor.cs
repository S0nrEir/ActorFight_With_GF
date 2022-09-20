using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.ToolKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGFExtensions.Await;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// Actor模块类型，管理actor
    /// </summary>
    public class Module_Actor : GameFrameworkModuleBase
    {
        #region public

        /// <summary>
        /// 获取一个actor
        /// </summary>
        public T GetActor<T>( int actor_id ) where T: TActorBase
        {
            if ( !_open_flag )
            {
                Log.Error( "!_open_flag" );
                return null;
            }

            if ( _actor_cache_dic is null || _actor_cache_dic.Count == 0 )
                return null;

            _actor_cache_dic.TryGetValue( actor_id, out var actor );
            return actor as T;
        }

        /// <summary>
        /// 异步显示一个actor
        /// </summary>
        public async Task<Entity> ShowActorAsync<T>
            (
                int actor_id,
                string asset_path,
                int grid_x,
                int grid_z,
                object user_data
            ) where T : TActorBase
        {
            var result = await AwaitableExtensions.ShowEntityAsync
                (
                    GameEntry.Entity,
                    actor_id,
                    typeof( T ),
                    asset_path,
                    Config.GameConfig.Entity.GROUP_HERO_ACTOR,
                    Config.GameConfig.Entity.PRIORITY_ACTOR,
                    user_data
                );

            OnShowActorSuccBasedTerrain( result.Logic as TActorBase, grid_x, grid_z );
            return result;
        }

        #endregion

        #region 

        /// <summary>
        /// 基于地块的actor生成后处理
        /// </summary>
        private void OnShowActorSuccBasedTerrain( TActorBase actor, int grid_x, int grid_z )
        {
            var terrain_module = GameEntry.Module.GetModule<Module_Terrain>();
            var terrain = terrain_module.Get( Tools.Fight.Coord2UniqueKey( grid_x, grid_z ) );
            //拿不到地块
            if ( terrain is null )
            {
                Log.Info( "terrain is null", LogColorTypeEnum.Red );
                return;
            }

            if ( terrain.State != ObjectPool.TerrainStateTypeEnum.NONE )
            {
                Log.Info( "terrain.State != ObjectPool.TerrainStateTypeEnum.NONE", LogColorTypeEnum.Red );
                return;
            }
            actor.SetCoordAndPosition( grid_x, grid_z );
        }

        /// <summary>
        /// 加载对应的actor
        /// </summary>
        private async void LoadActor()
        {
            var entity_id = ACTOR_ID_POOL.Gen();
            var actor = await ShowActorAsync<HeroActor>
                   (
                       entity_id,
                       @"Assets/Res/Prefab/Aquila_001.prefab",
                       0,
                       0,
                       new HeroActorEntityData( entity_id )
                   );
            Log.Info( $"show actor succ,name:{actor.gameObject.name}" );
        }

        #endregion

        public override void Start( object param )
        {
            base.Start( param );
            LoadActor();
        }

        public override void End()
        {
            base.End();
        }

        public override void OnClose()
        {
            if ( _actor_cache_dic != null )
                _actor_cache_dic?.Clear();

            _actor_cache_dic = null;
        }

        public override void EnsureInit()
        {
            if ( _actor_cache_dic is null )
                _actor_cache_dic = new Dictionary<int, TActorBase>();
        }
        
        /// <summary>
        /// actor缓存
        /// </summary>
        private Dictionary<int, TActorBase> _actor_cache_dic = null;
    }
}
