using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    //Module_Proxy_Actor的部分类，用于管理各类Actor实例
    public partial class Module_ProxyActor
    {
        //----------------pub----------------
        /// <summary>
        /// 将actor注册到代理中，成功返回true
        /// </summary>
        public (bool succ,ActorInstance instance) Register( TActorBase actor, Addon_Base[] addons )
        {
            if ( actor is null )
            {
                Log.Warning( "<color=yellow>actor is null.</color>" );
                return (false,null);
            }

            if ( Contains( actor.ActorID ) )
            {
                Log.Warning( $"<color=yellow>proxy has contains actor,id={actor.ActorID}.</color>" );
                return (false,null);;
            }

            var actor_case = ReferencePool.Acquire<ActorInstance>();
            actor_case.Setup( actor, addons );
            _proxy_actor_dic.Add( actor.ActorID, actor_case );

            return (true,actor_case);
        }

        /// <summary>
        /// 注销单个实例
        /// </summary>
        public bool UnRegister( int id )
        {
            if ( !Contains( id ) )
            {
                Log.Warning( $"proxy doesnt have actor wich id = {id}" );
                return false;
            }

            _proxy_actor_dic.TryGetValue( id, out var actor_case );
            if (actor_case != null)
            {
                actor_case.Clear();
                return _proxy_actor_dic.Remove( id ) && _registered_id_set.Remove( id );
            }
            return false;
        }

        //---------------- private 

        /// <summary>
        /// </summary>
        ///  /// <summary>
        private ActorInstance Get( int id )
        {
            if ( !_proxy_actor_dic.TryGetValue( id, out var actor_instance ) )
                Log.Warning( $"faild to get actor id={id}" );

            return actor_instance;
        }

        /// <summary>
        /// 根据ID获取一个ActorInstan
        /// </summary>
        private (bool has, ActorInstance instance) TryGet( int id )
        {
            (bool, ActorInstance) result = (false, null);
            if ( !_proxy_actor_dic.TryGetValue( id, out result.Item2 ) )
                return result;
            else
                result.Item1 = true;

            return result;
        }

        /// <summary>
        /// 是否包含指定ActorInstance，包含返回true
        /// </summary>
        private bool Contains( int actor_id )
        {
            return _registered_id_set.Contains( actor_id );
        }

        /// <summary>
        /// 释放所有
        /// </summary>
        private bool ReleaseAll()
        {
            if ( _proxy_actor_dic is null || _proxy_actor_dic.Count == 0 )
            {
                Log.Warning( "<color=yellow>_proxy_actor_dic is null || _proxy_actor_dic.Count == 0</color>" );
                return false;
            }

            var iter = _proxy_actor_dic.GetEnumerator();
            ActorInstance actor_case = null;
            while ( iter.MoveNext() )
            {
                actor_case = iter.Current.Value;
                actor_case.Clear();
            }
            _proxy_actor_dic.Clear();
            _registered_id_set.Clear();

            return true;
        }

        /// <summary>
        /// 管理器部分模块初始化
        /// </summary>
        private void MgrEnsureInit()
        {
            _proxy_actor_dic = new Dictionary<int, ActorInstance>();
            _registered_id_set = new HashSet<int>();
        }

        /// <summary>
        /// 管理器部分的模块开启处理
        /// </summary>
        private void MgrStart()
        {
        }

        /// <summary>
        /// 管理器部分的模块关闭处理
        /// </summary>
        private void MgrEnd()
        {
            ReleaseAll();
        }

        //----------------fields

        /// <summary>
        /// actor索引集合，保存了战斗中所有的ActorProxy
        /// </summary>
        private Dictionary<int, ActorInstance> _proxy_actor_dic;

        /// <summary>
        /// 注册的ID集合
        /// </summary>
        private HashSet<int> _registered_id_set = null;
    }
}