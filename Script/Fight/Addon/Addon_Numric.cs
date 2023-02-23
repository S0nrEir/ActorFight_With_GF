using Aquila.Fight.Actor;
using Aquila.Numric;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{

    public class Addon_Numric : AddonBase
    {
        #region priv

        /// <summary>
        /// ���û���ֵ
        /// </summary>
        public float SetBaseVal( Cfg.Enum.Numric_Type type, Numric_Modifier modifier )
        {
            var int_type = ( int ) type;
            if ( !OverLen( int_type ) )
                return 0f;

            return _numric_arr[int_type].Value;
        }

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


        #region override

        public override void Reset()
        {
            ResetNumricArr();
            base.Reset();
        }

        public override void Init( TActorBase actor, GameObject targetGameObject, Transform targetTransform )
        {
            base.Init( actor, targetGameObject, targetTransform );
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.NUMRIC;

        public override void OnAdd()
        {

        }

        public override void SetEnable( bool enable )
        {
            _enable = enable;
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
