using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Aquila.Editor
{
    public class PackAssetSetting
    {
        /// <summary>
        /// 要处理的目录
        /// </summary>
        private static string[] _includeDic = new string[]
        {
            //".vs",
            "DataTable",
            "Note",
            "obj",
            "Packages",
            "ProjectSettings",
            "UserSettings",
            "UIElementsSchema"
        };
        
        /// <summary>
        /// 要创建的压缩文件名
        /// </summary>
        private static string _fileName = @"/AssetSetting.zip";
        
        /// <summary>
        /// 压缩文件meta文件
        /// </summary>
        private static string _metaFileName = @"/AssetSetting.zip.meta";
        
        /// <summary>
        /// 要创建压缩文件的路径
        /// </summary>
        private static string _createPath = Application.dataPath + _fileName;
        
        private const int _default_compress_level = 5;
        
        /// <summary>
        /// 压缩目录并输出，这是因为根目录没有上传至git，所以压缩一下然后放到子目录内，以便让git提交
        /// </summary>
        [MenuItem( "Aquila/PackAssetSetting" )]
        public static void PackAssetSetting_()
        {
            DeleteLast();

            using ( ZipOutputStream stream = new ZipOutputStream( File.Create( _createPath ) ) )
            { 
                var size = 0L;
                stream.SetLevel( _default_compress_level );
                FileInfo file_info = null;
                var files = Directory.GetFiles( Application.dataPath + @"/.." );
                foreach ( var file in files )
                {
                    file_info = new FileInfo( file );
                    if ( file_info.Name == "ActorFight_With_GF.zip" )
                        continue;
                    
                    size += ZipFile( stream, file_info.Name );
                }

                foreach ( var dic in _includeDic )
                    size += ZipDict( dic, stream );

                stream.Flush();
            }
            Debug.Log( "<color=white>zip finished.</color>" );
        }
        
        /// <summary>
        /// 删除上次的压缩文件
        /// </summary>
        private static void DeleteLast()
        {
            var original_file = Application.dataPath + @"/" + _fileName;
            if ( File.Exists( original_file ) )
                File.Delete( original_file );
            
            original_file = Application.dataPath + @"/" + _metaFileName;
            if ( File.Exists( original_file ) )
                File.Delete( original_file );
        }
        
        private static long ZipDict( string dict, ZipOutputStream stream )
        {
            var size = 0L;
            var files_in_dict = Directory.GetFiles( dict );
            foreach ( var file in files_in_dict )
                size += ZipFile( stream, file );

            var dicts_in_dict = Directory.GetDirectories( dict );
            foreach ( var temp_dict in dicts_in_dict )
                size += ZipDict( temp_dict, stream );

            return size;
        }
        
        private static long ZipFile( ZipOutputStream stream, string file )
        {
            try
            {
                if ( !File.Exists( file ) )
                {
                    Debug.Log( "<color=red>file doesnt exists--->{file}</color>" );
                    return 0;
                }
                //512000000 bytes
                FileInfo fileInfo = new FileInfo( file );
                byte[] buffer = new byte[file.Length];
                var size = 0L;
                //size = fs.Read( buffer, 0, buffer.Length );
                size = fileInfo.Length;
                ZipEntry entry = new ZipEntry( file );
                entry.DateTime = DateTime.Now;
                entry.Size = size;
                stream.PutNextEntry( entry );
                using ( FileStream fs = File.OpenRead( file ) )
                {
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy( fs, stream, new byte[4096] );
                }
                stream.CloseEntry();

                //有的文件没有内容，所以是0字节
                // if ( size == 0 )
                // {
                //     Debug.Log( $"<color=red>file size is 0,file:{file}</color>" );
                //     return 0;
                // }

                return size;
            }
            catch(Exception err)
            {
                Debug.LogError( $"ZipFile Exception:{err.Message} , file:{file}" );
                return 0;
            }
            
        }
    }
}