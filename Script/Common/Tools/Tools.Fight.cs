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
        }
    }
}
