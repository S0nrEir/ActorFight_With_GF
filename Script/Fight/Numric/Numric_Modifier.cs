using GameFramework;
using UnityGameFramework.Runtime;

namespace Aquila.Numric
{
    /// <summary>
    /// ��ֵ������
    /// </summary>
    public class Numric_Modifier : IReference
    {
        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        public float ValueAfterModify
        {
            get
            {
                if ( !_modified )
                {
                    Log.Warning( "��������δ���޸�" );
                    return 0f;
                }
                return _value_after_modify;
            }
        }

        /// <summary>
        /// �����޸���������
        /// </summary>
        public void Setup( Numric_Modify_Type_Enum type_ )
        {
            _type = type_;
        }

        /// <summary>
        /// ���㣬���ò������޸ĺ��ʵ��ֵ
        /// </summary>
        public float Calc( float original_val_ )
        {
            if ( _modified )
            {
                Log.Warning( "this modifier has modified" );
                return 0f;
            }

            switch ( _type )
            {
                case Numric_Modify_Type_Enum.Add:
                    _value_after_modify = original_val_ + _value_after_modify;
                    break;

                case Numric_Modify_Type_Enum.Percent:
                    _value_after_modify = _value_after_modify * original_val_;
                    break;

                default:
                    throw new GameFrameworkException( "invalid modifier type!" );
            }
            _modified = true;
            return _value_after_modify;
        }

        public void Clear()
        {
            _value_after_modify = 0;
            _type = Numric_Modify_Type_Enum.None;
            _modified = false;
        }

        /// <summary>
        /// �޸ı��
        /// </summary>
        private bool _modified = false;

        /// <summary>
        /// ��ֵ�޸ķ�ʽ
        /// </summary>
        private Numric_Modify_Type_Enum _type = Numric_Modify_Type_Enum.None;

        /// <summary>
        /// ����ֵ
        /// </summary>
        private float _value_after_modify = 0f;

        //#todo��ֵ�޸ķ�ʽ��������ֵ
        //�������������������buff֮��ģ����Ƕ����Գ�����ֵ�޸�����װ��ʲô��Ҳ��
        //#todo_�޸���ֵ����θĻ�ȥ��
        //��Ϊ�������������ֵ���Ǽ�����ֵ��
        //��ˣ�����˵һ��buff���ӻ���������(100)��25%����������װ����������25���޸ĺ���ӱ�Ϊ125��
        //��buffʱ�䵽���Ժ�Ҫ����25%��������Ϊ������25%���൱�ڳ���1.25(0.25)�����Ҫ�������buff
        //�൱�ڳ���1-(1-0.25)���ٰ����ֵ���¼�����������װ�������ϣ����¼�25���ֱ����0
        //�޸ĸ�Ϊÿ��������ֵ����һ���������modifier

        //Modifier���Ӧ����Numric:
        //addonBase�Ľӿ�

        //Base = 100,fac=0.25,add=0,final=100
        //1.���
        //add=base*fac=25
        //final = base+add=125
        //2.�Ƴ�
        //temp=base-base*(1-fac)=25
        //temp*=-1
        //add+=temp=0
        //final=base+add=100
    }
}
