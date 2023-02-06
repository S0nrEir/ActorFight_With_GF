using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Numric
{
    /// <summary>
    /// 数值修改器
    /// </summary>
    public class Numric_Modifier : IReference
    {

        //#todo数值修改方式，具体数值
        //关联到数据组件，还有buff之类的，他们都可以持有数值修改器，装备什么的也行
        //因为数据组件的修正值，是计算后的值，
        //因此，比如说一个buff增加基础攻击力(100)的25%，攻击力的装备修正就是25，修改后相加变为125，
        //等buff时间到了以后，要把这25%消掉，因为是增加25%，相当于乘以1.25(0.25)，因此要消掉这个buff
        //相当于乘以1-(1-0.25)，再把这个值重新减到攻击力的装备补正上，重新减25，又变成了0

        //Base = 100,fac=0.25,add=0,final=100
        //1.添加
        //add=base*fac=25
        //final = base+add=125
        //2.移除
        //temp=base-base*(1-fac)=25
        //temp*=-1
        //add+=temp=0
        //final=base+add=100
        public void Clear()
        {
        }
    }
}
