using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Toolkit
{
    public partial class Tools
    {
        /// <summary>
        /// 路径相关工具类
        /// </summary>
        public static class Path
        {
            /// <summary>
            /// 获取内部指定配置文件路径
            /// </summary>
            public static string ConfigPath(string tableFileName)
            {
                return @$"Assets/Res/Config/{tableFileName}.txt";
            }

            /// <summary>
            /// 脚本文件根目录
            /// </summary>
            public static string LuaScriptRootPath()
            {
                return @$"{Application.dataPath}/Script/Lua/";
            }
        }
    }
}