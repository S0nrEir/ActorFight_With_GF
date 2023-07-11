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
        public void Remove( ushort tagToRemove )
        {
            //var oldTag = _tag;
            //_tag = Tools.SetBitValue_i64( _tag, tagToRemove, false );
            //_tagChangeCallBack?.Invoke( oldTag, _tag, false);

            _tag = Tools.SetBitValue_i64( _tag, tagToRemove, false );
            _tagChangeCallBack?.Invoke( _tag, tagToRemove, false );
        }

        /// <summary>
        /// 添加一个Tag
        /// </summary>
        public void Add( ushort tagToAdd )
        {
            //var oldTag = _tag;
            //_tag = Tools.SetBitValue_i64( _tag, tagToAdd, true );
            //_tagChangeCallBack?.Invoke( oldTag, _tag, true );

            _tag = Tools.SetBitValue_i64( _tag, tagToAdd, true );
            _tagChangeCallBack?.Invoke( _tag, tagToAdd, true );
        }

        /// <summary>
        /// 是否包含某项tag
        /// </summary>
        public bool Contains( ushort bitTag )
        {
            return Tools.GetBitValue_i64( _tag, bitTag );
        }

        public TagContainer()
        {
            Reset();
        }

        public TagContainer( Action<Int64, Int64, bool> callback )
        {
            _tagChangeCallBack = callback;
            Reset();
        }

        public void Reset()
        {
            _tag = 0;
        }

        /// <summary>
        /// 当前tag
        /// </summary>
        public Int64 Tag => _tag;

        /// <summary>
        /// 保存的tag
        /// </summary>
        private Int64 _tag = 0;

        /// <summary>
        /// tag变化回调
        /// </summary>
        private Action<Int64, Int64, bool> _tagChangeCallBack = null;
    }
}