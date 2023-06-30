using Aquila.Event;
using Aquila.Fight;
using Aquila.Fight.Addon;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// 战斗相关工具类
        /// </summary>
        public static class Fight
        {
            /// <summary>
            /// 获取技能使用失败的状态描述
            /// </summary>
            public static string AbilityUseFaildDescription(int stateDescription)
            {
                if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.NO_TARGET ) )
                    return "技能无目标";
                else if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.NO_CASTOR ) )
                    return "无施法者";
                else if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.COST_NOT_ENOUGH ) )
                    return "法力值不够";
                else if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.CD_NOT_OK ) )
                    return "技能还未准备就绪";
                else if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.NONE_PARAM ) )
                    return "无技能参数";
                else if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.NO_CASTOR ) )
                    return "无施法者";
                else if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.NO_CASTOR ) )
                    return "无技能配置参数";
                else if ( GetBitValue( stateDescription, ( int ) AbilityUseResultTypeEnum.NO_CASTOR ) )
                    return "没有timeline配置参数";

                return string.Empty;
            }

            /// <summary>
            /// 累加伤害
            /// </summary
            public static int AddDealedDamage(int value,int toAdd)
            {
                return value + toAdd;
            }

            /// <summary>
            /// 获取一个xz坐标的坐标key，失败返回-1
            /// </summary>
            /// <param name="x">x坐标</param>
            /// <param name="z">z坐标</param>
            public static int Coord2UniqueKey( int x, int z )
            {
                var result = x * Table.GetSceneConfig().Fight_Scene_Terrain_Coordinate_Precision + z;
                if ( result > Table.GetSceneConfig().Fight_Scene_Terrain_Coordinate_Range )
                {
                    Log.Error( $"terrain range wrong!,value is :{result}" );
                    return -1;
                }
                return result;
            }

            /// <summary>
            /// 坐标key转xz坐标值
            /// </summary>
            public static (int x, int z) UniqueKey2CoordTuple( int key )
            {
                var vec = UniqueKey2Coord( key );
                return (vec.x, vec.y);
            }

            /// <summary>
            /// 获取一个坐标key对应的xz坐标值，失败返回vector2Int.zero
            /// </summary>
            /// <param name="key">key</param>
            /// <returns>失败返回-1</returns>
            public static Vector2Int UniqueKey2Coord( int key )
            {
                if ( key <= 0 )
                    return Vector2Int.zero;

                var scene_config = Table.GetSceneConfig();
                var x = key / scene_config.Fight_Scene_Terrain_Coordinate_Precision;
                var y = key - ( x * scene_config.Fight_Scene_Terrain_Coordinate_Precision );
                return new Vector2Int( x, y );
            }

            /// <summary>
            /// 获取指定layer的y值，拿不到返回默认值
            /// </summary>
            public static float TerrainPositionY( string layer, float x, float z, float defaultValue = 0f )
            {
                Vector3 rayPos = new Vector3( x, 100, z );
                if ( Physics.Raycast( new Ray( rayPos, Vector3.down ), out var rayResult, 500, NameToLayer( layer ) ) )
                {
                    return rayResult.point.y;
                }

                return defaultValue;
            }

            /// <summary>
            /// 绑定一个特效到actor上
            /// </summary>
            /// <param name="effectEntityData">特效实体数据</param>
            /// <param name="effect">特效实体</param>
            public static void BindEffect( ActorEffectEntityData effect_entity_data, ActorFX effect )
            {
                var actor = effect.Actor;
                if ( actor == null || effect_entity_data is null )
                    return;

                if ( string.IsNullOrEmpty( effect_entity_data._effectPointName ) )
                    return;

                //还是改用find方式了，因为你不知道又有别的什么go会被放进来
                var effectPoint = actor.CachedTransform.Find( effect_entity_data._effectPointName );
                if ( effectPoint == null )
                {
                    effectPoint = Tools.AddChild( actor.CachedTransform );
                    effectPoint.gameObject.name = effect_entity_data._effectPointName;

                    effectPoint.localScale = UnityEngine.Vector3.one;
                    effectPoint.localEulerAngles = UnityEngine.Vector3.zero;
                    effectPoint.localPosition = UnityEngine.Vector3.zero;
                    effectPoint.SetAsFirstSibling();
                }

                //EFFECT Point放到actor下，effect放到effect point下
                effectPoint.SetParent( actor.CachedTransform );

                effect.CachedTransform.SetParent( effectPoint );
                effect.CachedTransform.localScale = UnityEngine.Vector3.one;

                effect.CachedTransform.localPosition = effect_entity_data._localPositionOffset;
                effect.CachedTransform.eulerAngles = UnityEngine.Vector3.zero;
                effect.CachedTransform.localEulerAngles = UnityEngine.Vector3.zero;
            }
        }
    }
}
