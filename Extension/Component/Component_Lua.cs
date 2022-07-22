using Aquila.Config;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

namespace Aquila.Extension
{
    /// <summary>
    /// lua环境组件
    /// </summary>
    public class Component_Lua : GameFrameworkComponent
    {
        /// <summary>
        /// 加载一个lua脚本
        /// </summary>
        public void LoadScript( string script_name, string chunk_name )
        {
            if ( string.IsNullOrEmpty( script_name ) )
            {
                Log.Warning( $"Lua script name is invalid,name:{script_name}" );
                return;
            }
            var asset_name = $"{_lua_root_path}{script_name}.lua.txt";
            if ( string.IsNullOrEmpty( asset_name ) )
            {
                Log.Warning( $"Lua asset name is invalid,name:{asset_name}" );
                return;
            }

            //构建脚本信息类
            var script_info = new ScriptInfo()
            {
                script_name = Tools.Lua.GetScriptName( asset_name ),
                chunk_name = chunk_name,
                _table = ObtainTable()
            };

            if ( GameEntry.Base.EditorResourceMode )
                EditorModeLoad( asset_name, script_info );
            else
                ResourceModeLoad( asset_name, script_info );
        }

        /// <summary>
        /// 以bytes的形式读入一段脚本
        /// </summary>
        private object[] DoString( byte[] bytes, ScriptInfo script_info )
        {
            if ( _lua_env is null )
            {
                Log.Error( "lua_end is null!" );
                return null;
            }

            if ( bytes is null )
            {
                Log.Error( "lua bytes is null!" );
                return null;
            }

            var obj_arr = _lua_env.DoString( bytes, script_info.chunk_name, script_info._table );
            if ( script_info._table != null )
            {
                var table = script_info._table;
                table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_START,  out _lua_on_start );
                table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_UPDATE, out _lua_on_update );
                table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_TICK,   out _lua_on_timer_tick );
                table.Get( GameConfig.Lua.LUA_FUNCTION_NAME_ON_FINISH, out _lua_on_finish );
            }
            _lua_on_start?.Invoke();
            return obj_arr;
        }

        /// <summary>
        /// 启动一个lua虚拟机
        /// </summary>
        public void StartVM()
        {
            //reset
            if ( _lua_env != null )
            {
                _lua_env.Dispose();
                _lua_env = null;
            }

            _lua_env = new LuaEnv();
            _lua_env.AddLoader( ScriptLoader );

            _meta_table = _lua_env.NewTable();
            _meta_table.Set( "__index", _lua_env.Global );
        }

        /// <summary>
        /// 提供一个luaTable
        /// </summary>
        private LuaTable ObtainTable()
        {
            if ( _lua_env is null )
            {
                Log.Error( "lua env is null" );
                return null;
            }

            if ( _meta_table is null )
            {
                Log.Error( "lua meta table is null" );
                return null;
            }

            var table = _lua_env.NewTable();
            table.SetMetaTable( _meta_table );
            return table;
        }

        /// <summary>
        /// 资源模式加载
        /// </summary>
        private void ResourceModeLoad( string asset_name, ScriptInfo script_info )
        {
            if ( !ScriptIsCached( script_info.script_name ) )
            {
                GameEntry.Resource.LoadAsset( asset_name, _load_asset_callbacks, script_info );
            }
            else
                DoString( GetFromCache( script_info.script_name ), script_info );
        }

        /// <summary>
        /// 编辑器模式加载
        /// </summary>
        private void EditorModeLoad( string asset_name, ScriptInfo script_info )
        {
            if ( !File.Exists( @asset_name ) )
            {
                Log.Warning( $"lua asset doesnt exist,name:{asset_name}" );
                return;
            }

            if ( !ScriptIsCached( script_info.script_name ) )
                Cache( script_info.script_name, asset_name, File.ReadAllBytes( asset_name ) );

            DoString( GetFromCache( script_info.script_name ), script_info );
        }

        /// <summary>
        /// 从缓存中获取指定的脚本字节流
        /// </summary>
        public byte[] GetFromCache( string script_name )
        {
            _script_cache_dic.TryGetValue( script_name.GetHashCode(), out var bytes );
            return bytes;
        }

        /// <summary>
        /// 一个脚本是否已被缓存，是返回true
        /// </summary>
        private bool ScriptIsCached( string script_name )
        {
            return _script_cache_dic.ContainsKey( script_name.GetHashCode() );
        }

        /// <summary>
        /// 缓存脚本
        /// </summary>
        private void Cache( string script_name, string asset_name, byte[] bytes )
        {
            var hashCode = script_name.GetHashCode();
            if ( _script_cache_dic.ContainsKey( hashCode ) )
                return;

            //#??处理UTF-8bom头
            if ( bytes[0] == 239 && bytes[1] == 187 && bytes[2] == 191 )
                bytes[0] = bytes[1] = bytes[2] = 32;

            _script_cache_dic.Add( script_name.GetHashCode(), bytes );
        }

        /// <summary>
        /// 脚本加载完成
        /// </summary>
        private void OnScriptLoadSucc( string assetName, object asset, float duration, object userData )
        {
            var script_info = userData as ScriptInfo;
            if ( script_info is null )
            {
                Log.Warning( $"cast script info faild" );
                return;
            }

            var text_asset = asset as TextAsset;
            if ( text_asset is null )
            {
                Log.Warning( $"cast text asset {assetName} faild" );
                return;
            }

            if ( ScriptIsCached( script_info.script_name ) )
                return;

            Cache( script_info.script_name, assetName, text_asset.bytes );
        }

        /// <summary>
        /// 脚本加载失败 
        /// </summary>
        private void OnScriptLoadFaild( string assetName, LoadResourceStatus status, string errorMessage, object userData )
        {
            Log.Error( errorMessage );
        }

        private byte[] ScriptLoader( ref string script_name )
        {
            if ( !_script_cache_dic.TryGetValue( script_name.GetHashCode(), out var bytes ) )
            {
                Log.Warning( "Can not find lua script '{0}'.", script_name );
                return null;
            }

            return bytes;
        }

        private void Update()
        {

        }

        protected override void Awake()
        {
            base.Awake();
            _lua_root_path = $"{Application.dataPath}/Script/Lua/";
            //#todo给个初始capcity
            _script_cache_dic = new Dictionary<int, byte[]>();
            _load_asset_callbacks = new LoadAssetCallbacks( OnScriptLoadSucc, OnScriptLoadFaild );
            StartVM();
        }

        /// <summary>
        /// 脚本加载回调
        /// </summary>
        private LoadAssetCallbacks _load_asset_callbacks = null;

        /// <summary>
        /// 脚本缓存
        /// </summary>
        private Dictionary<int, byte[]> _script_cache_dic = null;

        /// <summary>
        /// lua环境
        /// </summary>
        private static LuaEnv _lua_env = null;

        /// <summary>
        /// 全局元表
        /// </summary>
        private static LuaTable _meta_table = null;

        /// <summary>
        /// 脚本路径根节点
        /// </summary>
        private static string _lua_root_path = string.Empty;

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

        /// <summary>
        /// luaGC时长
        /// </summary>
        private const float LUA_INTERNAL_GC = 1f;
    }

    /// <summary>
    /// 脚本信息类，包含了脚本的一些基本信息
    /// </summary>
    internal class ScriptInfo
    {
        public string script_name = string.Empty;
        public string chunk_name = string.Empty;
        public LuaTable _table = null;
    }
}
