using Aquila.Fight;
using Aquila.Fight.Addon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila
{
    public static partial class Tools
    {
        /// <summary>
        /// 战斗相关工具类
        /// </summary>
        public static class Fight
        {
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
            /// 设置一个fight effect的transform信息到actor
            /// </summary>
            public static void SetEffectActorTran( ActorEffectEntityData effectEntityData, ActorEffect effect )
            {
                var actor = effect.Actor;
                if ( actor == null || effectEntityData is null )
                    return;

                if ( string.IsNullOrEmpty( effectEntityData._effectPointName ) )
                    return;

                //#todo还是改用find方式了，因为你不知道又有别的什么go会被放进来
                var effectPoint = actor.CachedTransform.Find( effectEntityData._effectPointName );
                if ( effectPoint == null )
                {
                    effectPoint = Tools.AddChild( actor.CachedTransform );
                    effectPoint.gameObject.name = effectEntityData._effectPointName;

                    effectPoint.localScale       = UnityEngine.Vector3.one;
                    effectPoint.localEulerAngles = UnityEngine.Vector3.zero;
                    effectPoint.localPosition    = UnityEngine.Vector3.zero;
                    effectPoint.SetAsFirstSibling();
                }

                //EFFECT Point放到actor下，effect放到effect point下
                effectPoint.SetParent( actor.CachedTransform );

                effect.CachedTransform.SetParent( effectPoint );
                effect.CachedTransform.localPosition = UnityEngine.Vector3.zero;
                effect.CachedTransform.localScale = UnityEngine.Vector3.one;

                effect.CachedTransform.localPosition = effectEntityData._localPositionOffset;
                effect.CachedTransform.eulerAngles = UnityEngine.Vector3.zero;
                effect.CachedTransform.localEulerAngles = UnityEngine.Vector3.zero;
            }
        }
    }
}
