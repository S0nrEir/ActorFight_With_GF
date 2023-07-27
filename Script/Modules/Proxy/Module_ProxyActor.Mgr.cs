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
        /// 为一个actor实例添加一个addon
        /// </summary>
        public (bool succ, ActorInstance instance) AddAddon(Actor_Base actor,Addon_Base addon)
        {
            if ( actor is null )
            {
                Log.Warning( "<color=yellow>actor is null.</color>" );
                return (false, null);
            }

            var res = TryGet( actor.ActorID );
            if ( !res.has )
                return Register( actor, new Addon_Base[] { addon } );
            else
            {
                var ins = res.instance;
                ins.AddAddon( addon );
                //AddToAddonSystem( addon );
                return (true, ins);
            }
        }

        public (bool succ, ActorInstance instance) Register( Actor_Base actor )
        {
            if ( actor is null )
            {
                Log.Warning( "<color=yellow>actor is null.</color>" );
                return (false, null);
            }

            if ( Contains( actor.ActorID ) )
            {
                Log.Warning( $"<color=yellow>proxy has contains actor,id={actor.ActorID}.</color>" );
                return (false, null); ;
            }

            var actorCase = ReferencePool.Acquire<ActorInstance>();
            actorCase.Setup( actor );
            _proxyActorDic.Add( actor.ActorID, actorCase );
            return (true, actorCase);
        }

        /// <summary>
        /// 将actor注册到代理中，成功返回true
        /// </summary>
        public (bool succ, ActorInstance instance) Register( Actor_Base actor, Addon_Base[] addons )
        {
            if ( actor is null )
            {
                Log.Warning( "<color=yellow>actor is null.</color>" );
                return (false, null);
            }

            if ( Contains( actor.ActorID ) )
            {
                Log.Warning( $"<color=yellow>proxy has contains actor,id={actor.ActorID}.</color>" );
                return (false, null); ;
            }

            var actorCase = ReferencePool.Acquire<ActorInstance>();
            //actorCase.Setup( actor, addons );
            actorCase.Setup( actor );
            _proxyActorDic.Add( actor.ActorID, actorCase );

            //将addon加入组件系统
            //foreach ( var addon in addons )
            //    AddToAddonSystem( addon );

            return (true, actorCase);
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

            if ( !_proxyActorDic.TryGetValue( id, out var actorCase ) )
            {
                Log.Warning( $"Module_ProxyActor.Mgr.UnRegister()--->faild to get actor instance,id:{id}" );
                return false;
            }

            //从组件系统中移除
            var addons = actorCase.AllAddons();
            foreach ( var addon in addons )
            {
                RemoveFromAddonSystem( addon );
                addon.Dispose();
            }

            //actorCase.Clear();
            ReferencePool.Release( actorCase );
            return _proxyActorDic.Remove( id ) && _registered_id_set.Remove( id );
        }

        //---------------- private 

        /// <summary>
        /// </summary>
        ///  /// <summary>
        private ActorInstance Get( int id )
        {
            if ( !_proxyActorDic.TryGetValue( id, out var actor_instance ) )
                Log.Warning( $"faild to get actor id={id}" );

            return actor_instance;
        }

        /// <summary>
        /// 根据ID获取一个ActorInstan
        /// </summary>
        private (bool has, ActorInstance instance) TryGet( int id )
        {
            (bool, ActorInstance) result = (false, null);
            if ( !_proxyActorDic.TryGetValue( id, out result.Item2 ) )
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
            return true;
            if ( _proxyActorDic is null || _proxyActorDic.Count == 0 )
            {
                Log.Warning( "<color=yellow>_proxy_actor_dic is null || _proxy_actor_dic.Count == 0</color>" );
                return false;
            }

            var iter = _proxyActorDic.GetEnumerator();
            ActorInstance actor_case = null;
            while ( iter.MoveNext() )
            {
                actor_case = iter.Current.Value;
                //actor_case.Clear();
                ReferencePool.Release( actor_case );
            }
            _proxyActorDic.Clear();
            _registered_id_set.Clear();

            return true;
        }

        /// <summary>
        /// 管理器部分模块初始化
        /// </summary>
        private void MgrEnsureInit()
        {
            _proxyActorDic = new Dictionary<int, ActorInstance>();
            _registered_id_set = new HashSet<int>();
        }

        /// <summary>
        /// 管理器部分的模块开启处理
        /// </summary>
        private void MgrOpen()
        {
        }

        /// <summary>
        /// 管理器部分的模块关闭处理
        /// </summary>
        private void MgrClose()
        {
            ReleaseAll();
        }

        //----------------fields

        /// <summary>
        /// actor索引集合，保存了战斗中所有的ActorProxy
        /// </summary>
        private Dictionary<int, ActorInstance> _proxyActorDic;

        /// <summary>
        /// 注册的Actor ID集合
        /// </summary>
        private HashSet<int> _registered_id_set = null;
    }
}