using Aquila.Config;
using Aquila.Extension;
using Aquila.Fight.Actor;
using System;
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
        /// 异步显示一个actor
        /// </summary>
        public async Task<Entity> ShowActorAsync
            (
                Type actorType,
                int roleMetaID,
                int actorID,
                string assetPath,
                object userData
            )
        {
            if ( actorType == typeof( Actor_Base ).GetType() )
            {
                Log.Warning( $"<color=yellow>not correct actor type,actor type:{typeof( Actor_Base ).Name}</color>" );
                return null;
            }
            var result = await AwaitableExtensions.ShowEntityAsync
                (
                    GameEntry.Entity,
                    actorID,
                    actorType,
                    assetPath,
                    Config.GameConfig.Entity.GROUP_HERO_ACTOR,
                    Config.GameConfig.Entity.PRIORITY_ACTOR,
                    userData
                );
            switch ( result.Logic )
            {
                //英雄类actor
                case Actor_Hero:
                    //#todo_根据actor类型决定传入函数的tag值，不要写死
                    OnShowHeroActorSucc( result.Logic as Actor_Hero, roleMetaID, GameConfig.Tags.ACTOR );
                    break;

                //法球类actor
                case Actor_Orb:
                    OnShowOrbActorSucc( result.Logic as Actor_Orb, userData );
                    break;
            }

            return result;
        }



        /// <summary>
        /// 异步显示一个actor，注意，调用该接口前使用await关键字进行等待
        /// </summary>
        public async Task<Entity> ShowActorAsync<T>
            (
                int roleMetaID,
                int actorID,
                string assetPath,
                int grid_x,
                int grid_z,
                object userData
            ) where T : Actor_Base
        {
            var result = await AwaitableExtensions.ShowEntityAsync
                (
                    GameEntry.Entity,
                    actorID,
                    typeof( T ),
                    assetPath,
                    Config.GameConfig.Entity.GROUP_HERO_ACTOR,
                    Config.GameConfig.Entity.PRIORITY_ACTOR,
                    userData
                );
            OnShowHeroActorSucc( result.Logic as Actor_Base, roleMetaID, Config.GameConfig.Entity.GROUP_HERO_ACTOR );
            return result;
        }

        /// <summary>
        /// 法球类actor回调
        /// </summary>
        private void OnShowOrbActorSucc( Actor_Orb actor,object userData)
        {
            var orbData = userData as Actor_Orb_EntityData;
            if ( orbData is null )
            {
                Log.Warning( $"Module_ActorFac.OnShowTracingProjectileActorSucc()--->orbData is null" );
                return;
            }

            //查找目标actor
            var targetTransform = GameEntry.Module.GetModule<Module_ProxyActor>().AddRelevance( orbData._targetActorID, actor.ActorID );
            if ( targetTransform == null)
            {
                Log.Warning( $"Module_ActorFac.OnShowTracingProjectileActorSucc()--->add actor relevance faild" );
                return;
            }

            actor.SetTargetTransformAndReady( targetTransform );
        }

        /// <summary>
        /// HeroActor生成回调
        /// </summary>
        private void OnShowHeroActorSucc( Actor_Base actor, int roleMetaID, string tag )
        {
            //actor.Setup( role_meta_id, tag );

            actor.SetCoordAndPosition( 0, 0 );
        }

        public override void Open( object param )
        {
            base.Open( param );
        }
        public override void Close()
        {
            base.Close();
        }

        public override void EnsureInit()
        {
            //if ( _actor_cache_dic is null )
            //    _actor_cache_dic = new Dictionary<int, TActorBase>();
        }
    }
}
