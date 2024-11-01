using Aquila.Toolkit;
using System;

namespace Aquila.GameTag
{
    /// <summary>
    /// tag容器，持有一组tag
    /// </summary>
    public struct TagContainer
    {
        /// <summary>
        /// 移除tag
        /// </summary>
        public void Remove( int tagToRemove , Action<UInt32, int, bool> callBack)
        {
            Tag = Tools.SetBitValue_U32( Tag, tagToRemove, false );
            callBack?.Invoke( Tag, tagToRemove, false );
        }

        /// <summary>
        /// 添加一个Tag
        /// </summary>
        public void Add( int tagToAdd , Action<UInt32, int, bool> callBack )
        {
            Tag = Tools.SetBitValue_U32( Tag, tagToAdd, true );
            callBack?.Invoke( Tag, tagToAdd, false );
        }

        /// <summary>
        /// 是否包含某项tag
        /// </summary>
        public bool HasFlag( int bitTag )
        {
            return Tools.GetBitValue_U32( Tag, bitTag );
        }

        public TagContainer( Action<UInt32, int, bool> callback )
        {
            Tag = 0;
        }

        public void Reset()
        {
            Tag = 0;
        }


        /// <summary>
        /// tag位数据
        /// </summary>
        public UInt32 Tag
        {
            get;
            private set;
        }
    }
}