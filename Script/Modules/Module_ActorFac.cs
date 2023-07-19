using Aquila.Extension;
using Aquila.Fight.Actor;
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
            //#todo_根据actor类型决定传入函数的tag值，不要写死
            OnShowActorSucc( result.Logic as Actor_Base, roleMetaID, Config.GameConfig.Entity.GROUP_HERO_ACTOR );
            return result;
        }

        /// <summary>
        /// actor生成回调
        /// </summary>
        private void OnShowActorSucc( Actor_Base actor, int roleMetaID, string tag )
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
