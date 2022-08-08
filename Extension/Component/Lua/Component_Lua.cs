using Aquila.Config;
using Cfg.Enum;
using GameFramework;
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
    public partial class Component_Lua : GameFrameworkComponent
    {
        /// <summary>
        /// 加载一个脚本实例//#todo__lua的执行结果返回参数怎么获得,尤其是在resourceModeLoad下
        /// </summary>
        public void Load( Cfg.common.Scripts meta )
        {
            if ( meta is null )
            {
                Log.Error( "Compoent_Lua--->Load faild! meta is null" );
                return;
            }

            if ( _script_running_dic.ContainsKey( meta.AssetPath ) )
            {
                Log.Info( $"_script_running_dic.ContainsKey:{meta.AssetPath}" );
                return;
            }

            var data = ReferencePool.Acquire<Script_Running_Data>();
            data.SetUp( meta, ObtainTable() );

            if ( GameEntry.Base.EditorResourceMode )
                EditorModeLoad( data );
            else
                ResourceModeLoad(data);
        }

        /// <summary>
        /// 移除一个脚本实例
        /// </summary>
        public bool Remove( string script_name )
        {
            if ( !_script_running_dic.TryGetValue( script_name, out var data ) )
                return false;

            data.ExecFinishFunc();
            ReferencePool.Release( data );
            return _script_running_dic.Remove( script_name );
        }

        private object[] DoString( byte[] bytes, Script_Running_Data data )
        {
            if ( _lua_env is null )
            {
                Log.Error( "lua_end is null!" );
                return null;
            }

            if ( bytes is null || bytes.Length == 0 )
            {
                Log.Error( "lua bytes is null!" );
                return null;
            }

            var result = _lua_env.DoString( bytes, data.Chunk_Name, data.Lua_Table );
            switch ( data._script_meta.Type )
            {
                //只有开始，不用进缓存，跑一遍就结束了
                case Script_Type.On_Start:
                    {
                        data.SetStartFunc();
                        data.ExecStartFunc();
                        ReferencePool.Release( data );
                        return result;
                    }

                //默认
                default:
                    RunningCache( data.Script_Name, data );
                    break;
            }
            return result;
        }

        /// <summary>
        /// 加载一个lua脚本
        /// </summary>
        //public void LoadScript( string script_name, string chunk_name, Script_Type script_type )
        //{
        //    if ( string.IsNullOrEmpty( script_name ) )
        //    {
        //        Log.Warning( $"Lua script name is invalid,name:{script_name}" );
        //        return;
        //    }
        //    var asset_name = $"{_lua_root_path}{script_name}.lua.txt";
        //    if ( string.IsNullOrEmpty( asset_name ) )
        //    {
        //        Log.Warning( $"Lua asset name is invalid,name:{asset_name}" );
        //        return;
        //    }

        //    //构建脚本信息类
        //    //#todo这里改成引用池
        //    var script_info = new ScriptInfo()
        //    {
        //        script_name = Tools.Lua.GetScriptName( asset_name ),
        //        chunk_name = chunk_name,
        //        _table = ObtainTable(),
        //        _type = script_type
        //    };

        //    if ( GameEntry.Base.EditorResourceMode )
        //        EditorModeLoad( asset_name, script_info );
        //    else
        //        ResourceModeLoad( asset_name, script_info );
        //}

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
        private void ResourceModeLoad( Script_Running_Data data )
        {
            if ( !ScriptIsCached( data.Asset_Path) )
                GameEntry.Resource.LoadAsset( data.Asset_Path, _load_asset_callbacks, data );
            else
                DoString( GetFromCache( data.Asset_Path ), data );
        }

        /// <summary>
        /// 编辑器模式加载
        /// </summary>
        private void EditorModeLoad( Script_Running_Data data )
        {
            if ( !File.Exists( @data.Asset_Path ) )
            {
                Log.Warning( $"lua asset doesnt exist,name:{data.Asset_Path}" );
                return;
            }

            if ( !ScriptIsCached( data.Asset_Path ) )
                Cache( data.Asset_Path, File.ReadAllBytes( data.Asset_Path ) );

            DoString( GetFromCache( data.Asset_Path ), data );
        }

        /// <summary>
        /// 从缓存中获取指定的脚本字节流
        /// </summary>
        public byte[] GetFromCache( string asset_path )
        {
            _script_cache_dic.TryGetValue( asset_path.GetHashCode(), out var bytes );
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
        /// 缓存运行脚本
        /// </summary>
        private bool RunningCache( string script_name, Script_Running_Data data )
        {
            if ( _script_running_dic.ContainsKey( script_name ) )
                return false;

            _script_running_dic.Add( script_name, data );
            return true;
        }

        /// <summary>
        /// 缓存脚本
        /// </summary>
        private void Cache( string script_name, byte[] bytes )
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
        private void OnScriptLoadSucc( string asset_name, object asset, float duration, object userData )
        {
            var data = userData as Script_Running_Data;
            if ( data is null )
            {
                Log.Warning( $"cast script info faild" );
                return;
            }

            var text_asset = asset as TextAsset;
            if ( text_asset is null )
            {
                Log.Warning( $"cast text asset {asset_name} faild" );
                return;
            }

            if ( ScriptIsCached( data.Asset_Path ) )
            {
                DoString( text_asset.bytes, data );
                return;
            }

            Cache( data.Asset_Path, text_asset.bytes );
            DoString( text_asset.bytes, data );
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
            _load_asset_callbacks = new LoadAssetCallbacks( OnScriptLoadSucc, OnScriptLoadFaild );
            _script_cache_dic = new Dictionary<int, byte[]>();
            _script_running_dic = new Dictionary<string, Script_Running_Data>();
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
        /// 脚本运行时实例集合
        /// </summary>
        private Dictionary<string, Script_Running_Data> _script_running_dic = null;

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
        /// <summary>
        /// 脚本路径
        /// </summary>
        public string script_name = string.Empty;

        /// <summary>
        /// chunkName
        /// </summary>
        public string chunk_name = string.Empty;

        /// <summary>
        /// 对应的luaTable
        /// </summary>
        public LuaTable _table = null;

        /// <summary>
        /// 脚本周期
        /// </summary>
        public Cfg.Enum.Script_Type _type = Cfg.Enum.Script_Type.On_Start;
    }
}
