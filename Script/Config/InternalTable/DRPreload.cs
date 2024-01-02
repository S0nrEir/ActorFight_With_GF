//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-10-31 19:40:55.359
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UGFExtensions;
using UnityGameFramework.Runtime;

namespace Aquila.UI
{
	/// <summary>
	/// 界面配置表。
	/// </summary>
	public class DRPreload : DataRowBase
	{
		private int m_Id = 0;

		/// <summary>
		/// 获取ID。
		/// </summary>
		public override int Id
{
			get
{
				return m_Id;
			
}		
}        /// <summary>
        /// 获取备注。
        /// </summary>
        public string Comment
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要预加载的资源路径。
        /// </summary>
        public string PreloadAssetName
        {
            get;
            private set;
        }
        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            Comment = columnStrings[index++];
            PreloadAssetName = columnStrings[index++];

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    Comment = binaryReader.ReadString();
                    PreloadAssetName = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }
        private void GeneratePropertyArray()
        {

        }
}
}
