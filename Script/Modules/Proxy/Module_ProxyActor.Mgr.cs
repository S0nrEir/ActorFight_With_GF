using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using GameFramework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ICSharpCode.SharpZipLib.Core;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    //Module_Proxy_Actor的部分类，用于管理各类Actor实例
    public partial class Module_ProxyActor
    {
        //----------------pub----------------

        /// <summary>
        /// 获取actor的根Transform组件
        /// </summary>
        public Transform GetActorTransform(int actorID)
        {
            return Get(actorID).Actor.CachedTransform;
        }

        /// <summary>
        /// 移除一个actor的关联actor
        /// </summary>
        public bool RemoveRelevance( int actorID, int toRemoveActorID )
        {
            var instance = Get( actorID );
            if ( instance is null || !instance.RemoveRevelence( toRemoveActorID ) )
                return false;

            return true;
        }

        /// <summary>
        /// 为一个actor添加关联actor
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public bool AddRelevance(int actorID, int relevanceActorID)
        {
            var instance = Get(actorID);
            return instance != null && instance.AddRelevance(relevanceActorID);
        }

        /// <summary>
        /// 将actor和addon关联，为一个actor实例添加一个addon
        /// </summary>
        public (bool succ, ActorInstance instance) AddAddon( Actor_Base actor, Addon_Base addon )
        {
            if ( actor is null )
            {
                Log.Warning( "<color=yellow>actor is null.</color>" );
                return (false, null);
            }

            var res = TryGet( actor.ActorID );
            if ( !res.has )
                return (false, null);

            var ins = res.instance;
            ins.AddAddon( addon );
            return (true, ins);
        }

        /// <summary>
        /// 将一个actor添加到instance管理模块中
        /// </summary>
        public (bool regSucc, ActorInstance instance) Register( Actor_Base actor )
        {
            if ( actor is null )
            {
                Log.Warning( "<color=yellow>Module_ProxyActor.Register()--->actor is null.</color>" );
                return (false, null);
            }

            if ( Contains( actor.ActorID ) )
            {
                Log.Warning( $"<color=yellow>Module_ProxyActor.Register()--->proxy has contains actor,id={actor.ActorID}.</color>" );
                return (false, null); ;
            }

            var actorCase = ReferencePool.Acquire<ActorInstance>();
            actorCase.Setup( actor );
            _proxyActorDic.Add( actor.ActorID, actorCase );
            _registered_id_set.Add( actor.ActorID );

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
                //当前的问题是，当前这帧先调用了dispose清掉了组件数据，然后才走到了MonoBehaviour的Update，这可能是由引擎层决定的调用顺序，导致组件系统访问了已经被清理的组件，在下一帧的时候组件系统才会清掉要移除的组件
                //解决办法是要么在这帧update调用前就把组件系统的对应组件清掉，要么保证脏标记在这帧update调用前被设置
                //当前选择了第一种办法，直接在组件系统里调用了addon.dispose，参见Module_ProxyActor.System.cs的issue
                
                //也有可能是当前这帧组件系统在跑update，然后调用了dispose，导致修改了正在update访问中的组件数据
                //设置releaseFlag为true的时候当前帧已经开始了，换句话说已经开始update了
                // addon.ReleasFlag = true;
                RemoveFromAddonSystem( addon );
                // addon.Dispose();
            }

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
#pragma warning disable CS0162 // 检测到无法访问的代码
            if ( _proxyActorDic is null || _proxyActorDic.Count == 0 )
#pragma warning restore CS0162 // 检测到无法访问的代码
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