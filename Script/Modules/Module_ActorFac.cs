using Aquila.Extension;
using Aquila.Fight.Actor;
using Aquila.ToolKit;
using System.Threading.Tasks;
using UGFExtensions.Await;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    /// <summary>
    /// Actor工厂类，创建和回收
    /// </summary>
    public class Module_Actor_Fac : GameFrameworkModuleBase
    {
        //--------------------public--------------------
        /// <summary>
        /// 获取一个actor，获取的地方移到了Proxy_Actor，这里不用了
        /// </summary>
        //public T GetActor<T>( int actor_id ) where T: TActorBase
        //{
        //    if ( !_open_flag )
        //    {
        //        Log.Error( "!_open_flag" );
        //        return null;
        //    }

        //    if ( _actor_cache_dic is null || _actor_cache_dic.Count == 0 )
        //        return null;

        //    _actor_cache_dic.TryGetValue( actor_id, out var actor );
        //    return actor as T;
        //}

        /// <summary>
        /// 异步显示一个actor，注意，调用该接口前使用await关键字进行等待
        /// </summary>
        public async Task<Entity> ShowActorAsync<T>
            (
                int role_meta_id,
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
            //#todo_根据actor类型决定传入函数的tag值，不要写死
            OnShowActorSucc( result.Logic as TActorBase, role_meta_id, Config.GameConfig.Entity.GROUP_HERO_ACTOR );
            OnShowActorSuccBasedOnTerrain( result.Logic as TActorBase, grid_x, grid_z );
            return result;
        }

        /// <summary>
        /// actor生成回调
        /// </summary>
        private void OnShowActorSucc( TActorBase actor, int role_meta_id, string tag )
        {
            //actor.Setup( role_meta_id, tag );
        }

        /// <summary>
        /// 基于地块的actor生成回调
        /// </summary>
        private void OnShowActorSuccBasedOnTerrain( TActorBase actor, int grid_x, int grid_z )
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
        private async void TestLoadActor()
        {
            var entity_id = ACTOR_ID_POOL.Gen();
            var actor = await ShowActorAsync<HeroActor>
                   (
                       1,
                       entity_id,
                       @"Assets/Res/Prefab/Aquila_001.prefab",
                       0,
                       0,
                       new HeroActorEntityData( entity_id ) { _role_meta_id = 1 }
                   );
            Log.Info( $"show actor succ,name:{actor.gameObject.name}" );
        }


        public override void Start( object param )
        {
            base.Start( param );
            //TestLoadActor();
        }

        public override void End()
        {
            base.End();
        }

        public override void OnClose()
        {
            //if ( _actor_cache_dic != null )
            //    _actor_cache_dic?.Clear();

            //_actor_cache_dic = null;
        }

        public override void EnsureInit()
        {
            //if ( _actor_cache_dic is null )
            //    _actor_cache_dic = new Dictionary<int, TActorBase>();
        }
    }
}
