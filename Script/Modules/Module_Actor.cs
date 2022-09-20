using Aquila.Extension;
using Aquila.Fight.Actor;
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
        public TActorBase GetActor( int actor_id )
        {
            if ( !_open_flag )
            {
                Log.Error( "!_open_flag" );
                return null;
            }

            return null;
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

            OnActorShowSucc( result.Logic as TActorBase, grid_x, grid_z );
            return result;
        }

        #endregion

        #region 

        private void OnActorShowSucc( TActorBase actor, int grid_x, int grid_z )
        {

        }

        #endregion

        public override async void Start( object param )
        {
            base.Start( param );
            LoadActor();
        }

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
