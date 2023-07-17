using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Toolkit;
using GameFramework;

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
            public void Setup( Actor_Base actor, Addon_Base[] addons )
            {
                _actor = actor;
                _addon_arr = addons;
            }
            
            public ActorInstance() { }

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
                return Tools.Actor.FilterAddon<T>(_addon_arr);
            }

            /// <summary>
            /// 获取该实例的所有addon
            /// </summary>
            public Addon_Base[] AllAddons()
            {
                return _addon_arr;
            }

            //-----------------fields-----------------

            /// <summary>
            /// 持有的Actor
            /// </summary>
            private Actor_Base _actor = null;

            /// <summary>
            /// actor持有的addon集合
            /// </summary>
            private Addon_Base[] _addon_arr = null;

            public void Clear()
            {
                _actor = null;
                _addon_arr = null;
            }
        }
    }
}