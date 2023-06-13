using Aquila.Fight.Actor;
using System.Collections.Generic;

namespace Aquila.Fight
{
    /// <summary>
    /// 一次战斗流程
    /// </summary>
    public struct FightPrcd
    {
        public FightPrcd( Actor_Base source, FightPrcdTypeEnum[] type, float[] value )
        {
            Source = source;
            _attachValueDic = new Dictionary<int, float>( 10 );
            if ( type == null || value == null )
                return;

            var len = type.Length;
            var valueLen = value.Length;
            for ( int i = 0; i < len && i < valueLen; i++ )
                SetValue( type[i], value[i] );
        }

        /// <summary>
        /// 从流程类型中增加一个值
        /// </summary>
        public bool SetValue( FightPrcdTypeEnum type, float value )
        {
            if ( _attachValueDic is null )
                return false;

            var intType = ( int ) type;
            if ( _attachValueDic.ContainsKey( intType ) )
                return false;

            _attachValueDic.Add( intType, value );
            return true;
        }

        /// <summary>
        /// 从流程类型中获取一个值
        /// </summary>
        public float GetValue( FightPrcdTypeEnum type, float default_value = 0f )
        {
            if ( _attachValueDic is null )
                return default_value;

            _attachValueDic.TryGetValue( ( int ) type, out default_value );
            return default_value;
        }

        /// <summary>
        /// 附加值的KV数量
        /// </summary>
        public int AttachCount => _attachValueDic is null ? 0 : _attachValueDic.Count;

        /// <summary>
        /// 本次prcd的附加值
        /// </summary>
        private Dictionary<int, float> _attachValueDic;

        /// <summary>
        /// 本次prcd的发起方
        /// </summary>
        public Actor_Hero Source { get; private set; }
    }

    /// <summary>
    /// 攻击流程类型枚举
    /// </summary>
    public enum FightPrcdTypeEnum
    {
        /// <summary>
        /// 伤害
        /// </summary>
        Damage = 0,
    }
}