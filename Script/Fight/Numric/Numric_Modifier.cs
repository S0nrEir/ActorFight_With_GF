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

        //#todo��ֵ�޸ķ�ʽ��������ֵ
        //�������������������buff֮��ģ����Ƕ����Գ�����ֵ�޸�����װ��ʲô��Ҳ��
        //��Ϊ�������������ֵ���Ǽ�����ֵ��
        //��ˣ�����˵һ��buff���ӻ���������(100)��25%����������װ����������25���޸ĺ���ӱ�Ϊ125��
        //��buffʱ�䵽���Ժ�Ҫ����25%��������Ϊ������25%���൱�ڳ���1.25(0.25)�����Ҫ�������buff
        //�൱�ڳ���1-(1-0.25)���ٰ����ֵ���¼�����������װ�������ϣ����¼�25���ֱ����0

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
    }
}
