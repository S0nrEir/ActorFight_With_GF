using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Config
{
    public partial class GameConfig
    {
        /// <summary>
        /// Lua�ű��������
        /// </summary>
        public static class Lua
        {
            /// <summary>
            /// lua���������ű��ص�
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_FINISH = "on_finish_lua";

            /// <summary>
            /// �ű���ʱ���ص���������
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_TICK = "on_timer_tick_lua";

            /// <summary>
            /// �ű�ˢ֡��������
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_UPDATE = "on_update_lua";

            /// <summary>
            /// �ű�����������
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_START = "on_start_lua";
        }
    }
}
