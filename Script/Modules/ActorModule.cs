using Aquila.Fight.Actor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Module
{
    /// <summary>
    /// Actor模块类型，管理actor
    /// </summary>
    public class ActorModule : GameFrameworkModuleBase
    {
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
            _actorCacheDic.TryGetValue( id, out var actor );
            return actor;
        }

        public override void OnClose()
        {
            if(_actorCacheDic != null)
                _actorCacheDic?.Clear();

            _actorCacheDic = null;
        }

        public override void EnsureInit()
        {
            if ( _actorCacheDic is null )
                _actorCacheDic = new Dictionary<int, TActorBase>();
        }

        /// <summary>
        /// actor缓存
        /// </summary>
        private Dictionary<int, TActorBase> _actorCacheDic = null;
    }
}
