using Aquila.Config;
using GameFramework;
using System;
using UnityEngine;
using XLua;

namespace Aquila.Extension
{
    public partial class Component_Lua
    {
        /// <summary>
        /// lua脚本运行时数据，每一个实例描述一个独立的lua table
        /// </summary>
        private class Script_Running_Data : IReference
        {

            /// <summary>
            /// 设置开始函数
            /// </summary>
            public void SetStartFunc()
            {
                Lua_Table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_START, out _lua_on_start );
            }

            /// <summary>
            /// 执行开始函数
            /// </summary>
            public void ExecStartFunc()
            {
                _lua_on_start?.Invoke();
            }

            /// <summary>
            /// 设置结束函数
            /// </summary>
            public void SetOnFinishFunc()
            {
                Lua_Table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_FINISH, out _lua_on_finish );
            }

            /// <summary>
            /// 执行结束函数
            /// </summary>
            public void ExecFinishFunc()
            {
                _lua_on_finish?.Invoke();
            }

            /// <summary>
            /// 设置计时回调函数
            /// </summary>
            public void SetOnTickFunc()
            {
                Lua_Table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_TICK, out _lua_on_timer_tick );
            }

            public void ExecTickFunc( float delta_time )
            {
                _lua_on_timer_tick?.Invoke( delta_time );
            }

            /// <summary>
            /// 设置刷帧函数
            /// </summary>
            public void SetOnUpdateFunc()
            {
                Lua_Table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_UPDATE, out _lua_on_update );
            }

            /// <summary>
            /// 执行刷帧函数
            /// </summary>
            public void ExecUpdateFunc( float delta_time )
            {
                _lua_on_update?.Invoke( delta_time );
            }

            public bool SetUp( Cfg.common.Scripts meta, LuaTable table )
            {
                _script_meta = meta;
                Script_Name = Tools.Lua.GetScriptName( _script_meta.AssetPath );
                Asset_Path = Tools.Lua.GetScriptAssetPath(_script_meta.AssetPath);
                Chunk_Name = Tools.Lua.GetChunkName( Script_Name );
                Lua_Table = table;


                return true;
            }

            /// <summary>
            /// 脚本表格数据
            /// </summary>
            public Cfg.common.Scripts _script_meta = null;

            /// <summary>
            /// 资源路径
            /// </summary>
            public string Asset_Path { get; private set; } = string.Empty;

            /// <summary>
            /// 脚本名称，key
            /// </summary>
            public string Script_Name { get; private set; } = string.Empty;

            /// <summary>
            /// chunk name
            /// </summary>
            public string Chunk_Name { get; private set; } = string.Empty;

            public LuaTable Lua_Table { get; private set; } = null;

            #region 周期函数
            /// <summary>
            /// 脚本开始
            /// </summary>
            private Action _lua_on_start = null;

            /// <summary>
            /// 刷帧
            /// </summary>
            private Action<float> _lua_on_update = null;

            /// <summary>
            /// 时间回调
            /// </summary>
            private Action<float> _lua_on_timer_tick = null;

            /// <summary>
            /// 脚本结束
            /// </summary>
            private Action _lua_on_finish = null;

            #endregion


            public void Clear()
            {
                Script_Name        = string.Empty;
                Chunk_Name         = string.Empty;
                Asset_Path         = string.Empty;
                Lua_Table.Dispose();
                Lua_Table          = null;
                _lua_on_start      = null;
                _lua_on_finish     = null;
                _lua_on_timer_tick = null;
                _lua_on_update     = null;
                _script_meta       = null;
            }

        }//end class

    }
}
