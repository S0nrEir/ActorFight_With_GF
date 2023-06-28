//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-06-28 18:27:33.512
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
	/// 鐣岄潰閰嶇疆琛?。
	/// </summary>
	public class DRUIForm : DataRowBase
	{
		private int m_Id = 0;

		/// <summary>
		/// 获取鐣岄潰缂栧彿。
		/// </summary>
		public override int Id
{
			get
{
				return m_Id;
			
}		
}        /// <summary>
        /// 获取绛栧垝澶囨敞。
        /// </summary>
        public string Comment
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取璧勬簮鍚嶇О。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鐣岄潰缁勫悕绉?鏄?惁鍏佽?澶氫釜鐣岄潰瀹炰緥。
        /// </summary>
        public string UIGroupName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鏄?惁鏆傚仠琚?叾瑕嗙洊鐨勭晫闈?鎵撳紑闊虫晥。
        /// </summary>
        public bool AllowMultiInstance
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取鍏抽棴闊虫晥。
        /// </summary>
        public bool PauseCoveredUIForm
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public int OpenSound
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取。
        /// </summary>
        public int CloseSound
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
            AssetName = columnStrings[index++];
            UIGroupName = columnStrings[index++];
            AllowMultiInstance = bool.Parse(columnStrings[index++]);
            PauseCoveredUIForm = bool.Parse(columnStrings[index++]);
            OpenSound = int.Parse(columnStrings[index++]);
            CloseSound = int.Parse(columnStrings[index++]);

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
                    AssetName = binaryReader.ReadString();
                    UIGroupName = binaryReader.ReadString();
                    AllowMultiInstance = binaryReader.ReadBoolean();
                    PauseCoveredUIForm = binaryReader.ReadBoolean();
                    OpenSound = binaryReader.Read7BitEncodedInt32();
                    CloseSound = binaryReader.Read7BitEncodedInt32();
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
