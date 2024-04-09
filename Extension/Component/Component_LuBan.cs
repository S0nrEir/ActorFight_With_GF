using Bright.Serialization;
using Cfg.Role;
using GameFramework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// LuBan扩展组件
    /// </summary>
    public class Component_LuBan : GameFrameworkComponent
    {
        [XLua.DoNotGen]
        public void Test()
        {
            var meta = Table<RoleBaseAttr>().Get(10001);
        }

        /// <summary>
        /// 获取指定类型的表实例
        /// </summary>
        public T Table<T>() where T : class
        {
            if ( !_is_custom_cache_tables )
                return null;

            if ( !_loadFlag || Tables is null )
                return null;

            if ( _custom_table_cache is null || _custom_table_cache.Count == 0 )
                return null;

            var code = typeof( T ).GetHashCode();
            if(!_custom_table_cache.TryGetValue( code, out var field_value ))
                return null;

            return field_value as T;
        }

        /// <summary>
        /// 表数据
        /// </summary>
        public Cfg.Tables Tables
        {
            get;
            private set;
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        public bool LoadDataTable()
        {
            if ( _loadFlag )
            {
                Log.Warning( $"<color=yellow>data table has been loaded!</color>" );
                return false;
            }

            if ( string.IsNullOrEmpty( _bytesPath ) )
                throw new GameFrameworkException( "bytesPath is null or empty!" );

            var tableCtor = typeof( Cfg.Tables ).GetConstructors()[0];
            var loader = new System.Func<string, ByteBuf>( ( file ) =>
             {
                 return new ByteBuf( File.ReadAllBytes( $"{_bytesPath}{file}{_fileExtension}" ) );
             } );
            Tables = ( Cfg.Tables ) tableCtor.Invoke( new object[] { loader } );

            if ( _is_custom_cache_tables )
            {
                if ( _custom_table_cache is null )
                    _custom_table_cache = new Dictionary<int, object>();

                _custom_table_cache.Clear();
                var properties = Tables.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );
                int hashCode;
                object temp;
                foreach ( var property in properties )
                {
                    hashCode = property.PropertyType.GetHashCode();
                    temp = property.GetValue( Tables );
                    _custom_table_cache.Add( hashCode, temp );
                }
            }

            _loadFlag = true;
            return _loadFlag;
        }

        protected override void Awake()
        {
            base.Awake();
            _bytesPath = Utility.Path.GetRegularPath($"{Application.dataPath}/Res/DataTables/");
            //_bytesPath = $"{Application.dataPath}/Res/DataTables/";
        }

        /// <summary>
        /// 使用自定义缓存表数据
        /// </summary>
        [SerializeField] private bool _is_custom_cache_tables = false;

        /// <summary>
        /// 自定义缓存表集合
        /// </summary>
        private Dictionary<int, object> _custom_table_cache = null;

        /// <summary>
        /// bytes文件路径
        /// </summary>
        private string _bytesPath = string.Empty;

        /// <summary>
        /// 扩展名
        /// </summary>
        private string _fileExtension = ".bytes";

        /// <summary>
        /// 加载标记
        /// </summary>
        private bool _loadFlag = false;
    }
}
