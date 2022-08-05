using Aquila.Extension;
using Aquila.Fight.Actor;
using System.Collections;
using System.Collections.Generic;
using UGFExtensions.Await;
using UnityEngine;

namespace Aquila.Module
{
    /// <summary>
    /// Actor模块类型，管理actor
    /// </summary>
    public class Module_Actor : GameFrameworkModuleBase
    {
        #region show

        /// <summary>
        /// 显示一个英雄actor
        /// </summary>
        //public async Task<bool> ShowHeroActor()
        //{
        //    var entityID = ACTOR_ID_POOL.Gen();
        //    var task = await AwaitableExtensions.ShowEntity
        //        (

        //        )

        //    return true;
        //}

        #endregion

        public void HideActor(int id)
        {
            if ( _actor_cache_dic is null || _actor_cache_dic.Count == 0 )
                return;


        }

        /// <summary>
        /// 隐藏所有actor，并且清空缓存
        /// </summary>
        public void HideAllActor()
        {
            if ( _actor_cache_dic is null || _actor_cache_dic.Count == 0 )
                return;

            var iter = _actor_cache_dic.GetEnumerator();
            TActorBase actor = null;
            while ( iter.MoveNext() )
            {
                actor = iter.Current.Value;
                if(actor is null)
                    continue;

                GameEntry.Entity.HideEntity( actor.ActorID );
            }
            _actor_cache_dic.Clear();
        }

        /// <summary>
        /// 返回指定类型的actor，转换失败或拿不到返回null
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="id">actorID</param>
        /// <returns>返回指定类型的actor</returns>
        public T GetActor<T>( int id ) where T : TActorBase
        {
            return GetActor( id ) as T;
        }

        /// <summary>
        /// 根据一个ActorID返回一个Actor，拿不到返回空
        /// </summary>
        public TActorBase GetActor( int id )
        {
            _actor_cache_dic.TryGetValue( id, out var actor );
            return actor;
        }

        public override void OnClose()
        {
            if( _actor_cache_dic != null)
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
