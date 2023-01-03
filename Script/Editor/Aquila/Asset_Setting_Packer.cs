using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Aquila.Editor
{
    /// <summary>
    /// �༭��������
    /// </summary>
    public partial class AquilaEditor
    {
        /// <summary>
        /// ����ļ���
        /// </summary>
        private static string[] _include_dic = new string[]
        {
            "DataTable",
            "Note",
            "Packages",
            "ProjectSettings",
            "UserSettings",
        };

        /// <summary>
        /// ������zip�ļ���
        /// </summary>
        private static string _file_name = @"/AssetSetting_Test.zip";

        /// <summary>
        /// ����·��
        /// </summary>
        private static string _create_path = Application.dataPath + _file_name;

        /// <summary>
        /// Ĭ�ϵ�ѹ������
        /// </summary>
        private const int _default_compress_level = 5;

        /// <summary>
        /// ���AssetSettingΪZip�����ҷŵ�applicationĿ¼��
        /// </summary>
        [MenuItem( "Aquila/PackAssetSetting" )]
        public static void PackAssetSetting()
        {
            PrevOp();

            using ( ZipOutputStream stream = new ZipOutputStream( File.Create( _create_path ) ) )
            {
                var size = 0l;
                stream.SetLevel( _default_compress_level );
                foreach ( var dic in _include_dic )
                {
                    size += ZipDict( dic, stream );
                }
                stream.Flush();
            }
        }

        /// <summary>
        /// ǰ�ò������������ļ��Ѿ����ڣ���ɾ��
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
        /// ѹ�������ļ�
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
            byte[] buffer = new byte[file.Length];//�ļ����̫�󣬴��ڻ�������С���ᱨ������������������Ҫѭ����ȡ
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
