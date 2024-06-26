using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Cfg.Enum;
using System;
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
            /// 获取actor对应的entity数据，拿不到返回false
            /// </summary>
            public static (bool genSucc, Type actorType, string group, int priority) GetActorEntityInfo( RoleType roleType )
            {
                var result = (genSucc: true, actorType: typeof( Actor_Base ), group: string.Empty, priority: -1);
                switch ( roleType )
                {
                    case RoleType.Hero:
                        result.actorType = typeof( Actor_Hero );
                        result.group = Config.GameConfig.Entity.GROUP_HERO_ACTOR;
                        result.priority = Config.GameConfig.Entity.PRIORITY_ACTOR;
                        break;

                    case RoleType.Orb:
                        result.actorType = typeof( Actor_Orb );
                        result.group = Config.GameConfig.Entity.GROUP_PROJECTILE;
                        result.priority = Config.GameConfig.Entity.PRIORITY_ACTOR;
                        break;

                    default:
                        result.genSucc = false;
                        break;
                }
                return result;
            }

            /// <summary>
            /// 枚举的roleType转system.type类型，找不到匹配类型会返回Actor_Base类型
            /// </summary>
            public static Type RoleTypeEnum2SystemType( RoleType type )
            {
                switch ( type )
                {
                    case RoleType.Hero:
                        return typeof( Actor_Hero );

                    case RoleType.Orb:
                        return typeof( Actor_Orb );
                }

                return typeof( Actor_Base ).GetType();
            }

            /// <summary>
            /// 获取法球类型actor的默认技能ID，取不到返回-1
            /// </summary>
            public static int DefaultOrbOnHitAbilityID( int[] abilityIdArr )
            {
                return abilityIdArr != null && abilityIdArr.Length > 0 ? abilityIdArr[0] : -1;
            }

            /// <summary>   
            /// 获取默认的法球资产路径
            /// </summary>
            public static string DefaultOrbAssetPath()
            {
                return $"Assets/Res/Prefab/Character/TestOrb_001.prefab";
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
            public static T FilterAddon<T>( Addon_Base[] addons ) where T : Addon_Base
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