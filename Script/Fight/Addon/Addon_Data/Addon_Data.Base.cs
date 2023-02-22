using Aquila.Numric;
using Cfg.Enum;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    public partial class Addon_Data
    {
        /// <summary>
        /// ���û���ֵ
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
        /// ������ֵΪ����ֵ
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
        /// ����ĳһ������ֵ�Ļ���ֵ
        /// </summary>
        private void SetBaseVal( Cfg.Enum.Numric_Type type, float val )
        {
            var int_type = ( int ) type;
            if ( !OverLen( int_type ) )
                return;

            _numric_arr[int_type].SetBaseVal( val );
        }

        /// <summary>
        /// ����ĳһ������ֵ��ְҵ����
        /// </summary>
        private void SetClassAdd( Numric_Type type, float val )
        {
            var int_type = ( int ) type;
            if ( !OverLen( int_type ) )
                return;

            _numric_arr[int_type].SetClassAdd( val );
        }

        /// <summary>
        /// �����ֵ�Ƿ���ȷ����ȷ����true
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

        //#todo_���ݷֳ��ĸ����֣�������ֵ��װ����buff����ֵ
        /// <summary>
        /// ��ֵ����
        /// </summary>
        private Numric.Numric[] _numric_arr = null;

        #endregion
    }
}
