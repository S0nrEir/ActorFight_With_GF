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
        /// <summary>
        /// 设置修改器的类型
        /// </summary>
        public void Setup( float val_to_modify_,Numric_Modify_Type_Enum type_ )
        {
            _value_to_modify = val_to_modify_;
            _type = type_;
        }

        /// <summary>
        /// 修改，返回修改后的实际值
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
        /// 恢复修改，返回修改后的实际值
        /// </summary>
        public void Recover(float original_val_)
        {
            //float val_after_modify = 0f;
            ////各种类型反着计算
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

        //#todo数值修改方式，具体数值
        //关联到数据组件，还有buff之类的，他们都可以持有数值修改器，装备什么的也行
        //#todo_修改数值后如何改回去？
        //因为数据组件的修正值，是计算后的值，
        //因此，比如说一个buff增加基础攻击力(100)的25%，攻击力的装备修正就是25，修改后相加变为125，
        //等buff时间到了以后，要把这25%消掉，因为是增加25%，相当于乘以1.25(0.25)，因此要消掉这个buff
        //相当于乘以1-(1-0.25)，再把这个值重新减到攻击力的装备补正上，重新减25，又变成了0
        //修改改为每个类型数值持有一个链表类的modifier

        //Modifier如何应用于Numric:
        //addonBase的接口

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

        /// <summary>
        /// 数值修改方式
        /// </summary>
        private Numric_Modify_Type_Enum _type = Numric_Modify_Type_Enum.None;

        /// <summary>
        /// 要修改的值
        /// </summary>
        private float _value_to_modify = 0f;
    }
}
