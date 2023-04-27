using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using GameFramework;

namespace Aquila.Module
{
    //Module_Proxy_Actor的部分类，用于描述Actor代理的instance
    public partial class Module_Proxy_Actor
    {
        /// <summary>
        /// 战斗代理Actor类，表示Actor在Proxy中的表示，封装了Actor和对应的Addon
        /// </summary>
        public class ActorInstance : IReference
        {
            #region pub

            public void Setup( TActorBase actor, Addon_Base[] addons )
            {
                _actor = actor;
                _addon_arr = addons;
            }

            public ActorInstance() { }

            /// <summary>
            /// 返回该实例持有的actor
            /// </summary>
            public TActorBase Actor
            {
                get => _actor;
            }

            /// <summary>
            /// 获取actor持有的指定类型的addon，没有返回空
            /// </summary>
            public T GetAddon<T>() where T : Addon_Base
            {
                if ( _addon_arr is null || _addon_arr.Length == 0 )
                    return null;

                foreach ( var addon in _addon_arr )
                {
                    if ( addon is T )
                        return addon as T;
                }

                return null;
            }

            #endregion

            #region fields

            /// <summary>
            /// 持有的Actor
            /// </summary>
            private TActorBase _actor = null;

            /// <summary>
            /// actor持有的addon集合
            /// </summary>
            private Addon_Base[] _addon_arr = null;

            #endregion

            public void Clear()
            {
                _actor = null;
                _addon_arr = null;
            }
        }
    }
}