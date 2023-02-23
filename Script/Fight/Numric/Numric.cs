using GameFramework;
using UnityGameFramework.Runtime;

namespace Aquila.Numric
{

    /// <summary>
    /// ����һ����ֵ����
    /// </summary>
    public class Numric : IReference
    {
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        public virtual float Value
        {
            get
            {
                if ( _change_flag )
                {
                    _change_flag = false;
                    return ReCalc();
                }
                return _correction_value;
            }
        }

        /// <summary>
        /// �Ƴ�һ������ֵ������
        /// </summary>
        public bool RemoveBaseModifier(Numric_Modifier to_remove_)
        {
            var succ = _correction.Remove( to_remove_ );
            if ( !succ )
                Log.Error("remove numric modifier faild!");

            return succ;
        }

        #region override

        /// <summary>
        /// ���¼�������ֵ
        /// </summary>
        protected virtual float ReCalc()
        {
            _correction_value = 0f;
            var iter = _correction.GetEnumerator();
            while ( iter.MoveNext() )
                _correction_value += iter.Current.Calc( _value );

            return _correction_value;
        }

        /// <summary>
        /// ���û�����ֵ
        /// </summary>
        public virtual void Setup( float base_val_ )
        {
            _change_flag = true;
            _value = base_val_;
            if ( _correction is null )
                _correction = new GameFrameworkLinkedList<Numric_Modifier>();
        }

        /// <summary>
        /// �������
        /// </summary>
        public virtual void Clear()
        {
            _value = 0f;
            _correction_value = 0f;
            _correction.Clear();
            _change_flag = false;
        }
        #endregion

        #region fields

        /// <summary>
        /// ������
        /// </summary>
        protected bool _change_flag = false;

        /// <summary>
        /// ����ֵ���͵Ļ���ֵ
        /// </summary>
        protected float _value = 0f;

        /// <summary>
        /// �����ӳ�ֵ�������������������Ľ��
        /// </summary>
        protected float _correction_value;

        /// <summary>
        /// ����ֵ����
        /// </summary>
        protected GameFrameworkLinkedList<Numric_Modifier> _correction;

        #endregion
    }
}