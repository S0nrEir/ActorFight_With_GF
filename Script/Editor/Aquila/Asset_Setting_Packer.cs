using GameFramework;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Aquila.Editor
{
    /// <summary>
    /// 编辑器工具类
    /// </summary>
    public partial class AquilaEditor
    {
        /// <summary>
        /// 打包文件夹
        /// </summary>
        private static string[] _dicts = new string[]
        {
            "DataTable",
            "Note",
            "Packages",
            "ProjectSettings",
            "UserSettings",
        };

        /// <summary>
        /// 打包AssetSetting为Zip，并且放到application目录下
        /// </summary>
        [MenuItem( "Aquila/PackAssetSetting" )]
        public static void PackAssetSetting()
        {
            PrevOp();

            var create_path = Application.dataPath + "AssetSetting.zip";
            using ( ZipOutputStream stream = new ZipOutputStream( File.Create( create_path ) ) )
            {
                foreach ( var dic in _dicts )
                {
                    ZipDict( dic, stream );
                }
            }
        }

        /// <summary>
        /// 前置操作，如果打包文件已经存在，则删除
        /// </summary>
        private static void PrevOp()
        {
            var path = Application.dataPath + "AssetSetting.zip";
            if ( File.Exists( path ) )
            {
                Debug.Log( "<color=white>File.Exists--->AssetSetting.zip,deletting...</color>" );
                File.Delete( path );
            }
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        //private static void ZipDicts(string[] dicts)
        //{
        //    var create_path = Application.dataPath + "AssetSetting.zip";
        //    using ( ZipOutputStream stream = new ZipOutputStream( File.Create( create_path ) ) )
        //    {
        //        //压缩级别
        //        stream.SetLevel( 0 );
        //        byte[] buffer = new byte[4096];
        //        string[] files_in_dict = null;
        //        string dict_path;
        //        foreach ( var dict in dicts )
        //        {
        //            dict_path = @System.Environment.CurrentDirectory + "/" + dict;
        //            if ( !Directory.Exists( dict_path ) )
        //            {
        //                Debug.Log( $"<color=white>doesnt exsits dict {dict},skip</color>" );
        //                continue;
        //            }

        //            files_in_dict = Directory.GetFileSystemEntries( dict_path );
        //            ZipDicts( files_in_dict, stream );
        //        }
        //    }
        //}

        /// <summary>
        /// 压缩目录
        /// </summary>
        private static void ZipDict( string dict, ZipOutputStream stream )
        {
            var dicts = Directory.GetDirectories( @System.Environment.CurrentDirectory + "/" + dict );
            if ( dicts.Length != 0 )
            {
                foreach ( var t in dicts )
                    ZipDict( t, stream );
            }

            var files = Directory.GetFiles( @System.Environment.CurrentDirectory + "/" + dict );
            string file_name;
            foreach ( var file in files )
            {
                file_name = @System.Environment.CurrentDirectory + "/" + dict + "/" + file;
                byte[] buffer = new byte[4096];
                ZipEntry entry = new ZipEntry( file );
                entry.DateTime = DateTime.Now;
                stream.PutNextEntry( entry );
                //写入文件到zip内
                using ( FileStream fs = File.OpenRead( file_name ) )
                {
                    
                }
            }
        }
    }
}
