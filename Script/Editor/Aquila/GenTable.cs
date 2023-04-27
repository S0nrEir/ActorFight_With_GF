using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Codice.Utils;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace  Aquila.Editor
{
    /// <summary>
    /// 调用表格生成工具
    /// </summary>
    public class GenTable
    {
        /// <summary>
        /// 调用表格生成工具
        /// </summary>
        [MenuItem("Aquila/GenTable")]
        public static void GenerateTale()
        {
            try
            {
                Process pr = new Process();
                var _bat_path = new DirectoryInfo(@Application.dataPath).Parent.FullName + "/DataTable/";
                pr.StartInfo.CreateNoWindow = true;
                pr.StartInfo.WorkingDirectory = _bat_path;
                pr.StartInfo.FileName = _bat_path + "gen_code_bin.bat";
                pr.Start();
                pr.WaitForExit();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }
}
