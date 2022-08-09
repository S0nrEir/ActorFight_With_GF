using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila
{
    public static partial class Tools
    {
        /// <summary>
        /// Lua工具类
        /// </summary>
        public static class Lua
        {
            /// <summary>
            /// 根据asset路径返回对应的脚本名称，失败返回string.empty
            /// </summary>
            public static string GetScriptName( string asset_path )
            {
                if ( string.IsNullOrEmpty( asset_path ) )
                {
                    Log.Warning( "asset path is null" );
                    return string.Empty;
                }
                var temp = asset_path.Split( '/' );
                if ( temp is null || temp.Length == 0 )
                    return string.Empty;

                return temp[temp.Length - 1];
            }

            /// <summary>
            /// 获取一段lua脚本的chunk name
            /// </summary>
            public static string GetChunkName(string asset_path)
            {
                if ( string.IsNullOrEmpty( asset_path ) )
                    return $"empty_chunk_name";

                var arr = asset_path.Split('/' );
                return arr[arr.Length - 1];
            }

            /// <summary>
            /// 获取脚本的资源路径
            /// </summary>
            public static string GetScriptAssetPath(string meta_asset_path)
            {
                return $"{SCRIPT_PATH}{meta_asset_path}{SCRIPT_SUFFIX}";
            }

            /// <summary>
            /// 脚本资产路径
            /// </summary>
            private static string SCRIPT_PATH = $"{Application.dataPath}/Script/Lua/";

            /// <summary>
            /// 脚本资产后缀名
            /// </summary>
            private static string SCRIPT_SUFFIX = ".lua.txt";
        }
    }
}