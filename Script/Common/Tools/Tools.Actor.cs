using Cfg.Enum;
using Aquila.Fight.Addon;
using System.Collections.Generic;

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
            /// 获取法球类型actor的默认技能ID，取不到返回-1
            /// </summary>
            public static int DefaultOrbOnHitAbilityID( int[] abilityIdArr )
            {
                return abilityIdArr != null && abilityIdArr.Length > 0 ? abilityIdArr[0] : -1;
            }

            /// <summary>
            /// 返回通用的待机timeline资源路径
            /// </summary>
            public static string CommonIdleTimelineAssetPath()
            {
                return @"Assets/Res/Timeline/Common/Common_Idle_1000.playable";
            }

            /// <summary>
            /// 返回通用的死亡timeline资源路径
            /// </summary>
            public static string CommonDieTimelineAssetPath()
            {
                return @"Assets/Res/Timeline/Common/Common_Die_1001.playable";
            }

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

            /// <summary>
            /// 尝试从一组addon中获取指定类型的addon，拿不到返回null
            /// </summary>
            public static T FilterAddon<T>( List<Addon_Base> addons ) where T : Addon_Base
            {
                if ( addons is null || addons.Count == 0 )
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