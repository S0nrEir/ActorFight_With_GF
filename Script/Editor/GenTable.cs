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
        /// 调用luban表生成工具，生成luban表配置对应的代码文件和bytes资源文件
        /// </summary>
        [MenuItem("Aquila/GenTable")]
        public static void GenTable_()
        {
            try
            {
                Process pr = new Process();
                var batPath = new DirectoryInfo(@Application.dataPath).Parent.FullName + "/DataTable/";
                pr.StartInfo.CreateNoWindow = true;
                pr.StartInfo.WorkingDirectory = batPath;
                pr.StartInfo.FileName = batPath + "gen_code_bin.bat";
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
