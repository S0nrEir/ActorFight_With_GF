using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Toolkit;
using GameFramework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            /// 给一个actor添加关联actor
            /// </summary>
            [MethodImpl( MethodImplOptions.AggressiveInlining )]
            public bool AddRevelence( int actorID )
            {
                return _actor.AddRelevance( actorID );
            }

            /// <summary>
            /// 为actor实例添加一个addon
            /// </summary>
            public bool AddAddon( Addon_Base addon )
            {
                foreach ( var temp in _addonList )
                {
                    if ( temp.AddonType == addon.AddonType )
                    {
                        Log.Warning( $"<color=yellow>Module_ProxyActor.AddAddon()--->actor {Actor.ActorID} has same addon,type:{addon.AddonType}</color>" );
                        return false;
                    }
                }
                _addonList.Add( addon );
                return true;
            }

            public void Setup( Actor_Base actor, Addon_Base[] addons )
            {
                _actor = actor;
                //_addon_arr = addons;
                _addonList = new List<Addon_Base>( addons.Length * 2 );
                _addonList.AddRange( addons );
            }

            public void Setup( Actor_Base actor )
            {
                _actor = actor;
                _addonList = new List<Addon_Base>();
            }

            public ActorInstance()
            {
            }

            /// <summary>
            /// 返回该实例持有的actor
            /// </summary>
            public Actor_Base Actor
            {
                get => _actor;
            }

            /// <summary>
            /// 获取actor持有的指定类型的addon，没有返回空
            /// </summary>
            public T GetAddon<T>() where T : Addon_Base
            {
                //#todo优化：别用遍历查找的方式检查然后获取addon
                return Tools.Actor.FilterAddon<T>( _addonList );
            }

            /// <summary>
            /// 获取该实例的所有addon
            /// </summary>
            public Addon_Base[] AllAddons()
            {
                return _addonList.ToArray();
            }

            public IReadOnlyCollection<Addon_Base> AllAddonsAsReadOnly()
            {
                return _addonList.AsReadOnly();
            }

            //-----------------fields-----------------

            /// <summary>
            /// 持有的Actor
            /// </summary>
            private Actor_Base _actor = null;

            /// <summary>
            /// actor持有的addon集合
            /// </summary>
            private List<Addon_Base> _addonList = null;

            public void Clear()
            {
                var cnt = _addonList.Count;
                for ( var i = 0; i < cnt; i++ )
                    _addonList[i] = null;

                _addonList.Clear();
                _addonList = null;
                _actor = null;
            }
        }
    }
}