using GameFramework;
using UnityGameFramework.Runtime;

namespace Aquila.Numric
{

    /// <summary>
    /// 描述一个数值类型
    /// </summary>
    public class Numric : IReference
    {
        /// <summary>
        /// 获取值
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
        /// 移除一个基础值修饰器
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
        /// 重新计算修正值
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
        /// 设置基础数值
        /// </summary>
        public virtual void Setup( float base_val_ )
        {
            _change_flag = true;
            _value = base_val_;
            if ( _correction is null )
                _correction = new GameFrameworkLinkedList<Numric_Modifier>();
        }

        /// <summary>
        /// 清除数据
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
        /// 变更标记
        /// </summary>
        protected bool _change_flag = false;

        /// <summary>
        /// 该数值类型的基础值
        /// </summary>
        protected float _value = 0f;

        /// <summary>
        /// 修正加成值，保存所有修正运算后的结果
        /// </summary>
        protected float _correction_value;

        /// <summary>
        /// 基础值修正
        /// </summary>
        protected GameFrameworkLinkedList<Numric_Modifier> _correction;

        #endregion
    }
}