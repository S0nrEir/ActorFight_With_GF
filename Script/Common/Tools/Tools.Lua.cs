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
        }
    }
}