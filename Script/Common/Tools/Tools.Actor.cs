using Cfg.Enum;
using Aquila.Fight.Addon;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// Actor工具类
        /// </summary>
        public static class Actor
        {
            /// <summary>
            /// 尝试从一组addon中获取指定类型的addon，拿不到返回null
            /// </summary>
            public static T FilterAddon<T>(Addon_Base[] addons) where T : Addon_Base
            {
                if ( addons is null || addons.Length == 0 )
                    return null;

                foreach ( var addon in addons )
                {
                    if ( addon is T )
                        return addon as T;
                }

                return null;
            }
        }
    }
}