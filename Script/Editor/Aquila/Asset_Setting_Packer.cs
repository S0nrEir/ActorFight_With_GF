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
        private static string[] _include_dic = new string[]
        {
            ".vs",
            "DataTable",
            "Note",
            "obj",
            "Packages",
            "ProjectSettings",
            "UserSettings",
        };

        /// <summary>
        /// 创建的zip文件名
        /// </summary>
        private static string _file_name = @"/AssetSetting.zip";

        /// <summary>
        /// 对应的meta文件
        /// </summary>
        private static string _meta_file_name = @"/AssetSetting.zip.meta";

        /// <summary>
        /// 完整路径
        /// </summary>
        private static string _create_path = Application.dataPath + _file_name;

        /// <summary>
        /// 默认的压缩级别
        /// </summary>
        private const int _default_compress_level = 5;

        /// <summary>
        /// 打包AssetSetting为Zip，并且放到application目录下
        /// </summary>
        [MenuItem( "Aquila/PackAssetSetting" )]
        public static void PackAssetSetting()
        {
            PrevOp();

            using ( ZipOutputStream stream = new ZipOutputStream( File.Create( _create_path ) ) )
            {
                var size = 0l;
                stream.SetLevel( _default_compress_level );
                //先压缩当前目录的所有文件，然后压缩文件夹

                FileInfo file_info = null;
                var files = Directory.GetFiles( Application.dataPath + @"/.." );
                foreach ( var file in files )
                {
                    file_info = new FileInfo( file );
                    //跳过临时文件
                    if ( file_info.Name == "ActorFight_With_GF.zip" )
                        continue;

                    //这里的文件不带目录
                    size += ZipFile( stream, file_info.Name );
                }

                foreach ( var dic in _include_dic )
                    size += ZipDict( dic, stream );

                stream.Flush();
            }
            Debug.Log( "<color=white>zip finished.</color>" );
        }

        /// <summary>
        /// 前置操作，如果打包文件已经存在，则删除
        /// </summary>
        private static void PrevOp()
        {
            //先删除原文件
            var original_file = Application.dataPath + @"/" + _file_name;
            if ( File.Exists( original_file ) )
                File.Delete( original_file );

            //删除meta文件
            original_file = Application.dataPath + @"/" + _meta_file_name;
            if ( File.Exists( original_file ) )
                File.Delete( original_file );
        }

        /// <summary>
        /// 压缩目录
        /// </summary>
        private static long ZipDict( string dict, ZipOutputStream stream )
        {
            var size = 0l;
            //遍历每一项，如果是目录就继续，如果不是，直接压缩
            var files_in_dict = Directory.GetFiles( dict );
            foreach ( var file in files_in_dict )
                size += ZipFile( stream, file );

            var dicts_in_dict = Directory.GetDirectories( dict );
            foreach ( var temp_dict in dicts_in_dict )
                ZipDict( temp_dict, stream );

            return size;
            //var dicts = Directory.GetDirectories( @System.Environment.CurrentDirectory + "\\" + dict );

            //if ( dicts.Length != 0 )
            //{
            //    foreach ( var t in dicts )
            //        ZipDict( t, stream );
            //}

            //var files = Directory.GetFiles( @System.Environment.CurrentDirectory + "/" + dict );
            //string file_name;
            //int size = 0;
            //foreach ( var file in files )
            //{
            //    file_name = @System.Environment.CurrentDirectory + "/" + dict + "/" + file;
            //}
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        private static long ZipFile( ZipOutputStream stream, string file )
        {
            if ( !File.Exists( file ) )
            {
                Debug.Log( "<color=red>file doesnt exists--->{file}</color>" );
                return 0;
            }
            //512000000 bytes
            FileInfo file_info = new FileInfo( file );
            byte[] buffer = new byte[file.Length];//文件如果太大，大于缓冲区大小，会报错，如果又这种情况则需要循环读取
            var size = 0l;
            //size = fs.Read( buffer, 0, buffer.Length );
            size = file_info.Length;
            if ( size == 3488 || size == 21 )
                ;
            ZipEntry entry = new ZipEntry( file );
            entry.DateTime = DateTime.Now;
            entry.Size = size;
            stream.PutNextEntry( entry );
            using ( FileStream fs = File.OpenRead( file ) )
            {
                //size = file_info.Length;
                //if ( size == 3488 || size == 21 )
                //    ;
                //ZipEntry entry = new ZipEntry( file );
                //entry.DateTime = DateTime.Now;
                //entry.Size = size;
                //stream.PutNextEntry( entry );

                ICSharpCode.SharpZipLib.Core.StreamUtils.Copy( fs, stream, new byte[4096] );

                //stream.CloseEntry();
            }
            stream.CloseEntry();

            if ( size == 0 )
            {
                Debug.Log( "<color=red>file size is 0</color>" );
                return 0;
            }

            return size;
        }
    }
}
