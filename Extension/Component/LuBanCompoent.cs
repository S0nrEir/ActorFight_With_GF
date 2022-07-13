﻿using Bright.Serialization;
using GameFramework;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension.Component
{
    /// <summary>
    /// LuBan扩展组件
    /// </summary>
    public class LuBanCompoent : GameFrameworkComponent
    {
        /// <summary>
        /// 表数据
        /// </summary>
        public cfg.Tables Tables
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

            var tableCtor = typeof( cfg.Tables ).GetConstructors()[0];
            var loader = new System.Func<string, ByteBuf>( ( file ) =>
             {
                 return new ByteBuf( File.ReadAllBytes( $"{_bytesPath}{file}{_fileExtension}" ) );
             } );

            Tables = ( cfg.Tables ) tableCtor.Invoke( new object[] { loader } );
            _loadFlag = true;
            return _loadFlag;
        }

        protected override void Awake()
        {
            base.Awake();
            _bytesPath = $"{Application.dataPath}/Res/DataTables/";
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
