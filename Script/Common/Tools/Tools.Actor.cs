using Cfg.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Toolkit
{
    public static partial class Tools
    {
        /// <summary>
        /// Actor工具类
        /// </summary>
        public static class Actor
        {
            //#todo应该为Actor_Attr属性配置一个invalid枚举，并且在匹配失败时返回invalid，目前返回的是max
            /// <summary>
            /// Actor_Base_Attr到Actor_Attr枚举的映射，没有返回Max，并且不会匹配HP和MP
            /// </summary>
            public static Actor_Attr BaseAttr2NormalAttrEnum(Actor_Base_Attr type)
            {
                return Actor_Attr.Max;
            }
        }
    }
}