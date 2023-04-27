using System;
using System.Collections;
using System.Collections.Generic;
using Aquila.ToolKit;
using UnityEngine;
using GameFramework;

namespace Aquila.GameTag
{
    /// <summary>
    /// 表示一组Tag
    /// </summary>
    public class TagContainer 
    {
        /// <summary>
        /// 移除tag
        /// </summary>
        public void Remove(UInt32 tag_to_remove)
        {
            _tag = Tools.SetBitValue_U32(_tag,tag_to_remove,false);
        }

        /// <summary>
        /// 添加一个Tag
        /// </summary>
        public void Add(UInt32 bit_to_add)
        {
            _tag = Tools.SetBitValue_U32(_tag, bit_to_add, true); 
        }

        /// <summary>
        /// 是否包含某项tag
        /// </summary>
        /// <param name="bit_tag_">要检查的位tag</param>
        public bool Contains(UInt32 bit_tag)
        {
            return Tools.GetBitValue_U32(_tag, bit_tag - 1);
        }

        public TagContainer()
        {
        }
        
        /// <summary>
        /// 保存的tag
        /// </summary>
        private UInt32 _tag = 0;
    }    
}