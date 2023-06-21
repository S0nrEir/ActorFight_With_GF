using Aquila.Toolkit;
using System;

namespace Aquila.GameTag
{
    /// <summary>
    /// tag容器，持有一组tag
    /// </summary>
    public class TagContainer
    {
        /// <summary>
        /// 移除tag
        /// </summary>
        public void Remove( ushort tag_to_remove )
        {
            var old_tag = _tag;
            _tag = Tools.SetBitValue_i64( _tag, tag_to_remove, false );
            _tag_change_callback?.Invoke( old_tag, _tag, tag_to_remove );
        }

        /// <summary>
        /// 添加一个Tag
        /// </summary>
        public void Add( ushort tag_to_add )
        {
            var old_tag = _tag;
            _tag = Tools.SetBitValue_i64( _tag, tag_to_add, true );
            _tag_change_callback?.Invoke( old_tag, _tag, tag_to_add );
        }

        /// <summary>
        /// 是否包含某项tag
        /// </summary>
        public bool Contains( ushort bit_tag )
        {
            return Tools.GetBitValue_i64( _tag, bit_tag );
        }

        public TagContainer()
        {
        }

        public TagContainer( Action<Int64, Int64, ushort> callback )
        {
            _tag_change_callback = callback;
        }

        /// <summary>
        /// 保存的tag
        /// </summary>
        private Int64 _tag = 0;

        /// <summary>
        /// tag发生变化时的回调
        /// </summary>
        private Action<Int64, Int64, ushort> _tag_change_callback = null;
    }
}