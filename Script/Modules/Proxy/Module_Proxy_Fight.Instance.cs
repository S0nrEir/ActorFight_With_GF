using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using GameFramework;

namespace Aquila.Module
{
    //Module_Proxy_Fight�Ĳ����࣬��������ActorInstance
    public partial class Module_Proxy_Fight
    {
        /// <summary>
        /// ս������Actor�࣬��ʾActor��Proxy�еı�ʾ����װ��Actor�Ͷ�Ӧ��Addon
        /// </summary>
        private class Proxy_Actor_Instance : IReference
        {
            #region pub

            public void Setup( TActorBase actor, AddonBase[] addons )
            {
                _actor = actor;
                _addon_arr = addons;
            }

            public Proxy_Actor_Instance() { }

            /// <summary>
            /// ���ظ�ʵ�����е�actor
            /// </summary>
            public TActorBase Actor
            {
                get => _actor;
            }

            /// <summary>
            /// ��ȡactor���е�ָ�����͵�addon��û�з��ؿ�
            /// </summary>
            public T GetAddon<T>() where T : AddonBase
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
            /// ���е�Actor
            /// </summary>
            private TActorBase _actor = null;

            /// <summary>
            /// actor���е�addon����
            /// </summary>
            private AddonBase[] _addon_arr = null;

            #endregion

            public void Clear()
            {
                _actor = null;
                _addon_arr = null;
            }
        }
    }
}