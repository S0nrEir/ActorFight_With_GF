using Aquila.Config;
using Aquila.ToolKit;
using GameFramework;
using System;
using UnityEngine;
using XLua;

namespace Aquila.Extension
{
    public partial class Component_Lua
    {
        /// <summary>
        /// lua�ű�����ʱ���ݣ�ÿһ��ʵ������һ��������lua table
        /// </summary>
        private class Script_Running_Data : IReference
        {

            /// <summary>
            /// ���ÿ�ʼ����
            /// </summary>
            public void SetStartFunc()
            {
                Lua_Table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_START, out _lua_on_start );
            }

            /// <summary>
            /// ִ�п�ʼ����
            /// </summary>
            public void ExecStartFunc()
            {
                _lua_on_start?.Invoke();
            }

            /// <summary>
            /// ���ý�������
            /// </summary>
            public void SetOnFinishFunc()
            {
                Lua_Table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_FINISH, out _lua_on_finish );
            }

            /// <summary>
            /// ִ�н�������
            /// </summary>
            public void ExecFinishFunc()
            {
                _lua_on_finish?.Invoke();
            }

            /// <summary>
            /// ���ü�ʱ�ص�����
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
            /// ����ˢ֡����
            /// </summary>
            public void SetOnUpdateFunc()
            {
                Lua_Table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_UPDATE, out _lua_on_update );
            }

            /// <summary>
            /// ִ��ˢ֡����
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
            /// �ű��������
            /// </summary>
            public Cfg.common.Scripts _script_meta = null;

            /// <summary>
            /// ��Դ·��
            /// </summary>
            public string Asset_Path { get; private set; } = string.Empty;

            /// <summary>
            /// �ű����ƣ�key
            /// </summary>
            public string Script_Name { get; private set; } = string.Empty;

            /// <summary>
            /// chunk name
            /// </summary>
            public string Chunk_Name { get; private set; } = string.Empty;

            public LuaTable Lua_Table { get; private set; } = null;

            #region ���ں���
            /// <summary>
            /// �ű���ʼ
            /// </summary>
            private Action _lua_on_start = null;

            /// <summary>
            /// ˢ֡
            /// </summary>
            private Action<float> _lua_on_update = null;

            /// <summary>
            /// ʱ��ص�
            /// </summary>
            private Action<float> _lua_on_timer_tick = null;

            /// <summary>
            /// �ű�����
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
