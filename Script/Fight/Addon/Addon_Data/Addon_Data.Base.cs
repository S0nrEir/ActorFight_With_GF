using Aquila.Numric;
using Cfg.Enum;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    public partial class Addon_Data
    {
        /// <summary>
        /// 设置基础值
        /// </summary>
        public float SetBaseVal( Cfg.Enum.Numric_Type type, Numric_Modifier modifier)
        {
            var int_type = ( int ) type;
            if ( !OverLen( int_type ) )
                return 0f;

            return _numric_arr[int_type].Value;
        }

        #region priv

        /// <summary>
        /// 重置数值为基础值
        /// </summary>
        private void ResetNumricArr()
        {
            var len = ( int ) Numric_Type.Max - 1;
            _numric_arr = new Numric.Numric[len];
            Numric.Numric temp = null;
            for ( int i = 0; i < len; i++ )
            {
                temp = ReferencePool.Acquire<Numric.Numric>();
                _numric_arr[i] = temp;
                temp.Setup( 0f, 0f, 0f, 0f );
            }
        }

        /// <summary>
        /// 设置某一类型数值的基础值
        /// </summary>
        private void SetBaseVal( Cfg.Enum.Numric_Type type, float val )
        {
            var int_type = ( int ) type;
            if ( !OverLen( int_type ) )
                return;

            _numric_arr[int_type].SetBaseVal( val );
        }

        /// <summary>
        /// 设置某一类型数值的职业修正
        /// </summary>
        private void SetClassAdd( Numric_Type type, float val )
        {
            var int_type = ( int ) type;
            if ( !OverLen( int_type ) )
                return;

            _numric_arr[int_type].SetClassAdd( val );
        }

        /// <summary>
        /// 检查数值是否正确，正确返回true
        /// </summary>
        private bool OverLen( int int_type )
        {
            if ( _numric_arr is null || int_type >= _numric_arr.Length )
            {
                Log.Warning( "Addon_Data.Base.cs--->int_type >= _numric_arr.Length" );
                return false;
            }
            return true;
        }

        #endregion


        #region fields

        //#todo_数据分成四个部分，基础数值，装备，buff，总值
        /// <summary>
        /// 数值集合
        /// </summary>
        private Numric.Numric[] _numric_arr = null;

        #endregion
    }
}
