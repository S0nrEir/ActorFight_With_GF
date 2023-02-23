using GameFramework;

namespace Aquila.Numric
{

    /// <summary>
    /// 继承自Numric，用于描述actor的数值类型
    /// </summary>
    public class Numric_Actor : Numric
    {
        public override float Value
        {
            get
            {
                if ( !_change_flag )
                    return _total;

                _total = 0f;
                _total += Enumrate( _total, _equip_correction );
                _total += Enumrate( _total, _class_correction );
                _total += Enumrate( _total, _buff_correction );

                //#todo-基于父类做脏标记处理的方法不太好，这样子类的脏标记逻辑要依赖于父类，找时间优化一下
                _total += base.Value;
                return _total;
            }
        }
        /// <summary>
        /// 添加一个装备修正
        /// </summary>
        public void AddEquipModifier( Numric_Modifier modifier_ )
        {
            _equip_correction.AddLast( modifier_ );
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
        public void AddClassModifier( Numric_Modifier modifier_ )
        {
            _class_correction.AddLast( modifier_ );
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
        public void AddBuffModifier( Numric_Modifier modifier_ )
        {
            _buff_correction.AddLast( modifier_ );
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

        //#todo改成LinkRange
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
