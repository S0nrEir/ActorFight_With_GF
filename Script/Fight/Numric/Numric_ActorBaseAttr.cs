using GameFramework;

namespace Aquila.Numric
{
    /// <summary>
    /// 继承自Numric，用于描述actor基础属性的数值类型
    /// baseValue=该属性或类型的基础值，该类扩展=所有加成最后修正的数值
    /// 目前包含：buff，装备，职业修正
    /// </summary>
    public class Numric_ActorBaseAttr : Numric
    {
        public override float CorrectionValue
        {
            get
            {
                if ( !_change_flag )
                    return _total;

                _total = 0f;

                //#todo-基于父类做脏标记处理的方法不太好，这样子类的脏标记逻辑要依赖于父类，找时间优化一下
                _total += base.CorrectionValue;
                _total += Enumrate( _total, _equip_correction );
                _total += Enumrate( _total, _class_correction );
                _total += Enumrate( _total, _buff_correction );
                return _total;
            }
        }
        /// <summary>
        /// 添加一个装备修正
        /// </summary>
        public bool AddEquipModifier( Numric_Modifier modifier_ )
        {
            return _equip_correction.AddLast( modifier_ ) != null;
        }

        /// <summary>
        /// 移除一个装备修正
        /// </summary>
        public bool RemoveEquipModifier( Numric_Modifier modifier_ )
        {
            return _equip_correction.Remove( modifier_ );
        }

        /// <summary>
        /// 添加一个职业修正
        /// </summary>
        public bool AddClassModifier( Numric_Modifier modifier_ )
        {
            return _class_correction.AddLast( modifier_ ) != null;
        }

        /// <summary>
        /// 移除一个职业修正
        /// </summary>
        public bool RemoveClassModifier( Numric_Modifier modifier_ )
        {
            return _class_correction.Remove( modifier_ );
        }

        /// <summary>
        /// 添加一个buff修正
        /// </summary>
        public bool AddBuffModifier( Numric_Modifier modifier_ )
        {
            return _buff_correction.AddLast( modifier_ ) != null;
        }

        /// <summary>
        /// 移除一个buff修正
        /// </summary>
        public bool RemoveBuffModifier( Numric_Modifier modifier_ )
        {
            return _buff_correction.Remove( modifier_ );
        }

        public override void Clear()
        {
            //#todo-现在走的都是引用池，初始化没有固定函数只能写在这里，要不要抽取出来？
            //init
            EnsureInit( _equip_correction );
            EnsureInit( _class_correction );
            EnsureInit( _buff_correction );

            //reset
            _equip_correction.Clear();
            _class_correction.Clear();
            _buff_correction.Clear();

            _total = 0f;
            base.Clear();
        }

        /// <summary>
        /// 遍历一个修饰器集合并返回修正后的值
        /// </summary>
        private float Enumrate( float val, GameFrameworkLinkedList<Numric_Modifier> linked_list_ )
        {
            var iter = linked_list_.GetEnumerator();
            while ( iter.MoveNext() )
                val += iter.Current.Calc( _value );

            return val;
        }

        private void EnsureInit( GameFrameworkLinkedList<Numric_Modifier> correction_ )
        {
            if ( correction_ is null )
                correction_ = new GameFrameworkLinkedList<Numric_Modifier>();
        }

        //#todo改成LinkedRange
        /// <summary>
        /// 装备加成修正
        /// </summary>
        private GameFrameworkLinkedList<Numric_Modifier> _equip_correction;

        /// <summary>
        /// 职业修正
        /// </summary>
        private GameFrameworkLinkedList<Numric_Modifier> _class_correction;

        /// <summary>
        /// buff加成修正，buff修正有变更时
        /// </summary>
        private GameFrameworkLinkedList<Numric_Modifier> _buff_correction;

        /// <summary>
        /// 修正计算的最终值
        /// </summary>
        private float _total = 0f;
    }
}
