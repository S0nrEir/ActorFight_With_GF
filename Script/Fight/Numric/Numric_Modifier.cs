using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Numric
{
    /// <summary>
    /// ��ֵ�޸���
    /// </summary>
    public class Numric_Modifier : IReference
    {
        /// <summary>
        /// �����޸���������
        /// </summary>
        public void Setup( float val_to_modify_,Numric_Modify_Type_Enum type_ )
        {
            _value_to_modify = val_to_modify_;
            _type = type_;
        }

        /// <summary>
        /// �޸ģ������޸ĺ��ʵ��ֵ
        /// </summary>
        public float Modify(float original_val_ )
        {
            float val_after_modify = 0f;
            switch ( _type )
            {
                case Numric_Modify_Type_Enum.Add:
                    val_after_modify = original_val_ + _value_to_modify;
                    break;

                //case Numric_Modify_Type_Enum.Multiple:
                //    val_after_modify = original_val_ * _value_to_modify;
                //    break;

                case Numric_Modify_Type_Enum.Percent:
                    val_after_modify =  _value_to_modify * original_val_;
                    break;

                default:
                    throw new GameFrameworkException("invalid modifier type!");
            }

            return val_after_modify;
        }

        /// <summary>
        /// �ָ��޸ģ������޸ĺ��ʵ��ֵ
        /// </summary>
        public void Recover(float original_val_)
        {
            //float val_after_modify = 0f;
            ////�������ͷ��ż���
            //switch ( _type )
            //{
            //    case Numric_Modify_Type_Enum.Add:
            //        val_after_modify = original_val_ - _value_to_modify;
            //        break;

            //    case Numric_Modify_Type_Enum.Percent:
            //        val_after_modify = _value_to_modify * original_val_;
            //        break;

            //    default:
            //        throw new GameFrameworkException( "invalid modifier type!" );
            //}

            //return val_after_modify;
        }

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
        public void Clear()
        {
        }

        /// <summary>
        /// ��ֵ�޸ķ�ʽ
        /// </summary>
        private Numric_Modify_Type_Enum _type = Numric_Modify_Type_Enum.None;

        /// <summary>
        /// Ҫ�޸ĵ�ֵ
        /// </summary>
        private float _value_to_modify = 0f;
    }
}
