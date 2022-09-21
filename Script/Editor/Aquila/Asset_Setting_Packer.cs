using GameFramework;
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
        private static string[] _dicts = new string[]
        {
            "DataTable",
            "Note",
            "Packages",
            "ProjectSettings",
            "UserSettings",
        };

        /// <summary>
        /// ���AssetSettingΪZip�����ҷŵ�applicationĿ¼��
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
        /// ѹ���ļ���
        /// </summary>
        //private static void ZipDicts(string[] dicts)
        //{
        //    var create_path = Application.dataPath + "AssetSetting.zip";
        //    using ( ZipOutputStream stream = new ZipOutputStream( File.Create( create_path ) ) )
        //    {
        //        //ѹ������
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
        /// ѹ��Ŀ¼
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
                //д���ļ���zip��
                using ( FileStream fs = File.OpenRead( file_name ) )
                {
                    
                }
            }
        }
    }
}
