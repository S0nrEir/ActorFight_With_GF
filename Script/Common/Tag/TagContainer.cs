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
        public void Remove(UInt32 tag_to_remove_)
        {
            _tag = Tools.SetBitValue_U32(_tag,tag_to_remove_,false);
        }

        /// <summary>
        /// 添加一个Tag
        /// </summary>
        public void Add(UInt32 bit_to_add_)
        {
            _tag = Tools.SetBitValue_U32(_tag, bit_to_add_, true); 
        }

        /// <summary>
        /// 是否包含某项tag
        /// </summary>
        /// <param name="bit_tag_">要检查的位tag</param>
        public bool Contains(UInt32 bit_tag_)
        {
            return Tools.GetBitValue_U32(_tag, bit_tag_ - 1);
        }

        public TagContainer()
        {
        }

        // public void Clear()
        // {
        //     _tag = 0;
        // }
        
        /// <summary>
        /// 保存的tag
        /// </summary>
        private UInt32 _tag = 0;
    }    
}