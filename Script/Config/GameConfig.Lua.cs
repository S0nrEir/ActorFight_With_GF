using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// Lua脚本常量相关
        /// </summary>
        public static class Lua
        {
            /// <summary>
            /// lua函数结束脚本回调
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_FINISH = "on_finish";

            /// <summary>
            /// 脚本计时器回调函数名称
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_TICK = "on_timer_tick";

            /// <summary>
            /// 脚本刷帧函数名称
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_UPDATE = "on_update";

            /// <summary>
            /// 脚本启动函数名
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_START = "on_start";
        }
    }
}
