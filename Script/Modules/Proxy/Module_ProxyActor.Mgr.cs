//using Aquila.Fight.Actor;
//using Aquila.Fight.Addon;
//using GameFramework;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using ICSharpCode.SharpZipLib.Core;
//using UnityEngine;
//using UnityGameFramework.Runtime;

//namespace Aquila.Module
//{
//    //Module_Proxy_Actor的部分类，用于管理各类Actor实例
//    public partial class Module_ProxyActor
//    {
//        //----------------pub----------------

//        //---------------- private 

//        /// <summary>
//        /// 根据ID获取一个ActorInstan
//        /// </summary>
//        //private (bool has, ActorInstance instance) TryGet( int id )
//        //{
//        //    (bool, ActorInstance) result = (false, null);
//        //    if ( !_proxyActorDic.TryGetValue( id, out result.Item2 ) )
//        //        return result;
//        //    else
//        //        result.Item1 = true;

//        //    return result;
//        //}

//        /// <summary>
//        /// 释放所有
//        /// </summary>
//        private bool ReleaseAll()
//        {
//            return true;
//#pragma warning disable CS0162 // 检测到无法访问的代码
//            if ( _proxyActorDic is null || _proxyActorDic.Count == 0 )
//#pragma warning restore CS0162 // 检测到无法访问的代码
//            {
//                Aquila.Toolkit.Tools.Logger.Warning( "<color=yellow>_proxy_actor_dic is null || _proxy_actor_dic.Count == 0</color>" );
//                return false;
//            }

//            var iter = _proxyActorDic.GetEnumerator();
//            ActorInstance actor_case = null;
//            while ( iter.MoveNext() )
//            {
//                actor_case = iter.Current.Value;
//                //actor_case.Clear();
//                ReferencePool.Release( actor_case );
//            }
//            _proxyActorDic.Clear();
//            _registered_id_set.Clear();

//            return true;
//        }

//        /// <summary>
//        /// 管理器部分模块初始化
//        /// </summary>
//        private void MgrEnsureInit()
//        {
//            _proxyActorDic = new Dictionary<int, ActorInstance>();
//            _registered_id_set = new HashSet<int>();
//        }

//        /// <summary>
//        /// 管理器部分的模块开启处理
//        /// </summary>
//        private void MgrOpen()
//        {
//        }

//        /// <summary>
//        /// 管理器部分的模块关闭处理
//        /// </summary>
//        private void MgrClose()
//        {
//            ReleaseAll();
//        }

//        //----------------fields

//        /// <summary>
//        /// actor索引集合，保存了战斗中所有的ActorProxy
//        /// </summary>
//        private Dictionary<int, ActorInstance> _proxyActorDic;

//        /// <summary>
//        /// 注册的Actor ID集合
//        /// </summary>
//        private HashSet<int> _registered_id_set = null;
//    }
//}