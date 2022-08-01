using Bright.Serialization;
using GameFramework;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// LuBan扩展组件
    /// </summary>
    public class Component_LuBan : GameFrameworkComponent
    {
        public void Test()
        {
            var meta = Tables.TB_RoleBaseAttr.GetOrDefault( 10001 );
            if ( meta is null )
                Debug.Log( "meta is nulll" );

            if ( meta.Class == Cfg.Enum.Role_Class.Warior )
            {
                //to xxx
            }

            return;
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
                throw new GameFrameworkException( "data table has been loaded!" );
             
            if ( string.IsNullOrEmpty( _bytesPath ) )
                throw new GameFrameworkException( "bytesPath is null or empty!" );

            var tableCtor = typeof( Cfg.Tables ).GetConstructors()[0];
            var loader = new System.Func<string, ByteBuf>( ( file ) =>
             {
                 return new ByteBuf( File.ReadAllBytes( $"{_bytesPath}{file}{_fileExtension}" ) );
             } );
            Tables = ( Cfg.Tables ) tableCtor.Invoke( new object[] { loader } );
            _loadFlag = true;
            return _loadFlag;
        }

        protected override void Awake()
        {
            base.Awake();
            _bytesPath = $"{Application.dataPath}/Res/DataTables/";
            LoadDataTable();
        }


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
