using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using GameFramework.Resource;
using System.Collections.Generic;
using System.IO;
using Cfg.Common;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

namespace Aquila.Extension
{
    /// <summary>
    /// lua组件
    /// </summary>
    public partial class Component_Lua : GameFrameworkComponent
    {
        //-----------------lua call cs-----------------
        /// <summary>
        /// 由脚本开启一个定时回调
        /// </summary>
        [XLua.LuaCallCSharp]
        public void Tick( int id, float internal_ )
        {
            if ( !_script_running_dic.TryGetValue( id, out var data ) )
            {
                Log.Warning( $"!_script_running_dic.TryGetValue( {id} )" );
                return;
            }
            //关联timer
            var timer = GameEntry.Timer.StartTick( internal_, data.ExecTickFunc );
            data.SetupTimer( timer );
        }

        //-----------------public-----------------
        public void Load( Table_Scripts meta_ )
        {
            Load( meta_, meta_.id );
        }

        /// <summary>
        /// 加载一个脚本实例//#todo__lua的执行结果返回参数怎么获得,尤其是在resourceModeLoad下
        /// </summary>
        public void Load( Table_Scripts meta, int id )
        {
            if ( meta is null )
            {
                Log.Error( "Compoent_Lua--->Load faild! meta is null" );
                return;
            }

            //已经正在运行的实例
            if ( _script_running_dic.ContainsKey( id ) )
            {
                Log.Warning( $"_script_running_dic.ContainsKey:{id}" );
                return;
            }

            var data = ReferencePool.Acquire<Script_Running_Data>();
            data.SetUp( meta, ObtainTable() );

            if ( GameEntry.Base.EditorResourceMode )
                EditorModeLoad( data );
            else
                ResourceModeLoad( data );
        }

        /// <summary>
        /// 卸载所有脚本实例
        /// </summary>
        public bool UnLoadAllRunningData()
        {
            _update_script_set.Clear();
            if ( _script_running_dic.Count == 0 )
                return true;

            var ids = _script_running_dic.Keys;
            foreach ( var id in ids )
            {
                if ( !UnLoad( id ) )
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 移除一个脚本实例
        /// </summary>
        private bool UnLoad( int id )
        {
            if ( !_script_running_dic.TryGetValue( id, out var data ) )
                return false;

            //clear timer
            if ( data.Timer != null )
                GameEntry.Timer.UnRegisterUpdate( data.Timer );

            //如果有finish回调，调用
            if ( Tools.GetBitValue( ( int ) data._script_meta.Type, 1 ) )
                data.ExecFinishFunc();

            ReferencePool.Release( data );
            return true;
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
        /// 从缓存中获取指定的脚本字节流
        /// </summary>
        public byte[] GetFromCache( string asset_path )
        {
            _script_cache_dic.TryGetValue( asset_path.GetHashCode(), out var bytes );
            return bytes;
        }

        //-----------------private-----------------
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
            Debug.Log( $"<color=white>DoString:{data.Script_Name}</color>" );

            //do start
            //包含start，先设置start
            //只有start，不用进缓存，跑一遍就结束了
            if ( data._script_meta.Type == Script_Type.Start )
            {
                data.SetStartFunc();
                data.ExecStartFunc();
                ReferencePool.Release( data );
                return result;
            }

            RunningCache( data );
            return result;
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
            if ( !ScriptIsCached( data.Asset_Path ) )
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
        /// 一个脚本是否已被缓存，是返回true
        /// </summary>
        private bool ScriptIsCached( string script_name )
        {
            return _script_cache_dic.ContainsKey( script_name.GetHashCode() );
        }

        /// <summary>
        /// 缓存运行脚本
        /// </summary>
        private bool RunningCache( Script_Running_Data data )
        {
            if ( data is null || data._script_meta is null )
            {
                Log.Warning( "Running Cache--->Data is null || meta is null" );
                return false;
            }

            if ( _script_running_dic.ContainsKey( data._script_meta.id ) )
                return false;

            //#todo对脚本周期类型进行更细致的划分
            //关于finish是否要主动通知lua组件进行回调关闭：目前的办法是在流程或某一阶段结束后，lua组件统一全部卸载
            data.SetOnFinishFunc();
            data.SetOnTickFunc();
            data.SetOnUpdateFunc();

            //如果是update，放到update里
            if ( Tools.GetBitValue( (int)data._script_meta.Type, 2 ) )
                _update_script_set.Add( data._script_meta.id );

            _script_running_dic.Add( data._script_meta.id, data );

            //包含start，跑一遍start
            if ( Tools.GetBitValue( ( int ) data._script_meta.Type, 0 ) )
            {
                data.SetStartFunc();
                data.ExecStartFunc();
            }

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
            {
                bytes[0] = 32;
                bytes[1] = 32;
                bytes[2] = 32;
            }

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

            if ( !ScriptIsCached( data.Asset_Path ) )
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

        private void LuaUpdate(float delta_time)
        {
            if ( _update_script_set.Count == 0 )
                return;

            foreach ( var script_id in _update_script_set )
            {
                if ( !_script_running_dic.TryGetValue( script_id, out var data ) )
                {
                    Log.Warning($"faild to get script data with id:{script_id}");
                    continue;
                }

                data.ExecUpdateFunc( delta_time );
            }
        }

        private void LuaGC()
        {
            if ( _gc_timer >= LUA_GC_INTERNAL )
            {
                _gc_timer = 0f;
                _lua_env?.Tick();
            }
            _gc_timer++;
        }

        private void Update()
        {
            LuaUpdate(Time.deltaTime);
            LuaGC();
        }

        protected override void Awake()
        {
            base.Awake();
            _lua_root_path = $"{Application.dataPath}/Script/Lua/";
            //#todo给个初始capcity
            _load_asset_callbacks = new LoadAssetCallbacks( OnScriptLoadSucc, OnScriptLoadFaild );
            _script_cache_dic = new Dictionary<int, byte[]>();
            _script_running_dic = new Dictionary<int, Script_Running_Data>( 128 );
            _update_script_set = new HashSet<int>( 128 );
            _gc_timer = 0f;
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
        /// 脚本运行时实例集合,key = 脚本配置表id
        /// </summary>
        private Dictionary<int, Script_Running_Data> _script_running_dic = null;

        /// <summary>
        /// 需要刷帧的脚本集合
        /// </summary>
        private HashSet<int> _update_script_set = null;

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

        private static float _gc_timer = 0f;

        /// <summary>
        /// luaGC时长
        /// </summary>
        private const float LUA_GC_INTERNAL = 1f;
    }
}