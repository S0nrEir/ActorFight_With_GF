using GameFramework;

namespace Aquila.Numric
{

    /// <summary>
    /// �̳���Numric����������actor����ֵ����
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

                //#todo-���ڸ��������Ǵ���ķ�����̫�ã���������������߼�Ҫ�����ڸ��࣬��ʱ���Ż�һ��
                _total += base.Value;
                return _total;
            }
        }
        /// <summary>
        /// ���һ��װ������
        /// </summary>
        public void AddEquipModifier( Numric_Modifier modifier_ )
        {
            _equip_correction.AddLast( modifier_ );
        }

        /// <summary>
        /// �Ƴ�һ��װ������
        /// </summary>
        public bool RemoveEquipModifier( Numric_Modifier modifier_ )
        {
            return _equip_correction.Remove( modifier_ );
        }

        /// <summary>
        /// ���һ��ְҵ����
        /// </summary>
        public void AddClassModifier( Numric_Modifier modifier_ )
        {
            _class_correction.AddLast( modifier_ );
        }

        /// <summary>
        /// �Ƴ�һ��ְҵ����
        /// </summary>
        public bool RemoveClassModifier( Numric_Modifier modifier_ )
        {
            return _class_correction.Remove( modifier_ );
        }

        /// <summary>
        /// ���һ��buff����
        /// </summary>
        public void AddBuffModifier( Numric_Modifier modifier_ )
        {
            _buff_correction.AddLast( modifier_ );
        }

        /// <summary>
        /// �Ƴ�һ��buff����
        /// </summary>
        public bool RemoveBuffModifier( Numric_Modifier modifier_ )
        {
            return _buff_correction.Remove( modifier_ );
        }

        public override void Clear()
        {
            //#todo-�����ߵĶ������óأ���ʼ��û�й̶�����ֻ��д�����Ҫ��Ҫ��ȡ������
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
        /// ����һ�����������ϲ������������ֵ
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

        //#todo�ĳ�LinkRange
        /// <summary>
        /// װ���ӳ�����
        /// </summary>
        private GameFrameworkLinkedList<Numric_Modifier> _equip_correction;

        /// <summary>
        /// ְҵ����
        /// </summary>
        private GameFrameworkLinkedList<Numric_Modifier> _class_correction;

        /// <summary>
        /// buff�ӳ�������buff�����б��ʱ
        /// </summary>
        private GameFrameworkLinkedList<Numric_Modifier> _buff_correction;

        /// <summary>
        /// �������������ֵ
        /// </summary>
        private float _total = 0f;
    }
}
