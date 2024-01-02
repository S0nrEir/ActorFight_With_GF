using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Toolkit;
using GameFramework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Module
{
    //Module_Proxy_Actor的部分类，用于描述Actor代理的instance
    public partial class Module_ProxyActor
    {
        /// <summary>
        /// 战斗代理Actor类，表示Actor在Proxy中的表示，封装了Actor和对应的Addon
        /// </summary>
        public class ActorInstance : IReference
        {
            //-----------------pub-----------------

            /// <summary>
            /// 为actor实例添加一个addon
            /// </summary>
            public bool AddAddon( Addon_Base addon )
            {
                var code = addon.GetType().GetHashCode();
                if (_addons.ContainsKey(code.GetHashCode()))
                {
                    Log.Warning( $"<color=yellow>Module_ProxyActor.AddAddon()--->actor {Actor.ActorID} has same addon,type:{addon.AddonType}</color>" );
                    return false;
                }

                _addons.Add(code,addon);
                return true;
            }

            public void Setup( Actor_Base actor )
            {
                _actor = actor;
                _addons = new Dictionary<int, Addon_Base>();
            }

            public ActorInstance()
            {
            }

            /// <summary>
            /// 返回该实例持有的actor
            /// </summary>
            public Actor_Base Actor => _actor;

            /// <summary>
            /// 获取actor持有的指定类型的addon，没有返回空
            /// </summary>
            public T GetAddon<T>() where T : Addon_Base
            {
                var code = typeof(T).GetHashCode();
                if (!_addons.TryGetValue(code, out var addon))
                    return null;

                return addon as T;
                // return Tools.Actor.FilterAddon<T>( _addonList );                
            }

            /// <summary>
            /// 获取该实例的所有addon
            /// </summary>
            public Addon_Base[] AllAddons()
            {
                Addon_Base[] arr = new Addon_Base[_addons.Count ];
                var i = 0;
                var iter = _addons.GetEnumerator();
                while (iter.MoveNext())
                    arr[i++] = iter.Current.Value;
                
                iter.Dispose();
                return arr;
            }

            //-----------------fields-----------------

            /// <summary>
            /// 持有的Actor
            /// </summary>
            private Actor_Base _actor = null;

            /// <summary>
            /// actor持有的addon集合
            /// </summary>
            private Dictionary<int, Addon_Base> _addons = null;

            public void Clear()
            {
                //attention:现在actor的组件释放，放在了addonSystem的下一帧中进行，这里只释放instance持有的其他资源
                //换言之，其他资源在本帧释放，addon在下一帧处理
                
                // foreach (var kv in _addons)
                //     kv.Value.Dispose();
                
                _addons.Clear();
                _addons = null;
                _actor  = null;
            }
        }
    }
}