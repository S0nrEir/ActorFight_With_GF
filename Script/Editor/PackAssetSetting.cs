using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Aquila.Editor
{
    public class PackAssetSetting
    {
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
        
        private static string _fileName = @"/AssetSetting.zip";
        
        private static string _metaFileName = @"/AssetSetting.zip.meta";
        
        private static string _create_path = Application.dataPath + _fileName;
        
        private const int _default_compress_level = 5;
        
        [MenuItem( "Aquila/PackAssetSetting" )]
        public static void PackAssetSetting_()
        {
            PrevOp();

            using ( ZipOutputStream stream = new ZipOutputStream( File.Create( _create_path ) ) )
            {
                var size = 0l;
                stream.SetLevel( _default_compress_level );
                //��ѹ����ǰĿ¼�������ļ���Ȼ��ѹ���ļ���

                FileInfo file_info = null;
                var files = Directory.GetFiles( Application.dataPath + @"/.." );
                foreach ( var file in files )
                {
                    file_info = new FileInfo( file );
                    //������ʱ�ļ�
                    if ( file_info.Name == "ActorFight_With_GF.zip" )
                        continue;

                    //������ļ�����Ŀ¼
                    size += ZipFile( stream, file_info.Name );
                }

                foreach ( var dic in _include_dic )
                    size += ZipDict( dic, stream );

                stream.Flush();
            }
            Debug.Log( "<color=white>zip finished.</color>" );
        }
        
        private static void PrevOp()
        {
            //��ɾ��ԭ�ļ�
            var original_file = Application.dataPath + @"/" + _fileName;
            if ( File.Exists( original_file ) )
                File.Delete( original_file );

            //ɾ��meta�ļ�
            original_file = Application.dataPath + @"/" + _metaFileName;
            if ( File.Exists( original_file ) )
                File.Delete( original_file );
        }

        /// <summary>
        /// ѹ��Ŀ¼
        /// </summary>
        private static long ZipDict( string dict, ZipOutputStream stream )
        {
            var size = 0l;
            //����ÿһ������Ŀ¼�ͼ�����������ǣ�ֱ��ѹ��
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

            if ( size == 0 )
            {
                Debug.Log( "<color=red>file size is 0</color>" );
                return 0;
            }

            return size;
        }
    }
}