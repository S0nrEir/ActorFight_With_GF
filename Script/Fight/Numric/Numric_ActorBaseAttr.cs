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
                if ( !_changeFlag )
                    return _total;

                _total = 0f;

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
        public bool AddEquipModifier( Numric_Modifier modifier )
        {
            if ( _equip_correction.AddLast( modifier ) != null )
            {
                _changeFlag = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除一个装备修正
        /// </summary>
        public bool RemoveEquipModifier( Numric_Modifier modifier )
        {
            if ( _equip_correction.Remove( modifier ) )
            {
                _changeFlag = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加一个职业修正
        /// </summary>
        public bool AddClassModifier( Numric_Modifier modifier )
        {
            if ( _class_correction.AddLast( modifier ) != null )
            {
                _changeFlag = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除一个职业修正
        /// </summary>
        public bool RemoveClassModifier( Numric_Modifier modifier )
        {
            if ( _class_correction.Remove( modifier ) )
            {
                _changeFlag = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 添加一个buff修正
        /// </summary>
        public bool AddBuffModifier( Numric_Modifier modifier )
        {
            if ( _buff_correction.AddLast( modifier ) != null )
            {
                _changeFlag = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除一个buff修正
        /// </summary>
        public bool RemoveBuffModifier( Numric_Modifier modifier )
        {
            if ( _buff_correction.Remove( modifier ) )
            {
                _changeFlag = true;
                return true;
            }

            return false;
        }

        public override void Clear()
        {
            //todo:现在走的都是引用池，初始化没有固定函数只能写在这里，要不要抽取出来？

            //reset
            _equip_correction.Clear();
            _class_correction.Clear();
            _buff_correction.Clear();

            _equip_correction = null;
            _class_correction = null;
            _buff_correction  = null;

            _total = 0f;
            base.Clear();
        }

        
        /// <summary>
        /// 遍历一个修饰器集合并返回修正后的值（变化后的值）
        /// </summary>
        private float Enumrate( float val, GameFrameworkLinkedList<Numric_Modifier> linked_list )
        {
            var iter = linked_list.GetEnumerator();
            var changedVal = 0f;
            while ( iter.MoveNext() )
                changedVal += iter.Current.Calc( _value );

            iter.Dispose();
            return changedVal;
        }

        public override void EnsureInit( )
        {
            base.EnsureInit();
            _equip_correction ??= new GameFrameworkLinkedList<Numric_Modifier>();
            _class_correction ??= new GameFrameworkLinkedList<Numric_Modifier>();
            _buff_correction  ??= new GameFrameworkLinkedList<Numric_Modifier>();
        }

        public Numric_ActorBaseAttr() : base()
        {
        }

        //todo:改成LinkedRange
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
