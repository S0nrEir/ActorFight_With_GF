using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Numric
{
    /// <summary>
    /// 数值修饰器
    /// </summary>
    public class Numric_Modifier : IReference
    {
        /// <summary>
        /// 获取修正值
        /// </summary>
        // public float ValueAfterModify
        // {
        //     get
        //     {
        //         if ( !_modified )
        //         {
        //             Log.Warning( "修饰器还未被修改" );
        //             return 0f;
        //         }
        //         return _value_after_modify;
        //     }
        // }

        /// <summary>
        /// 设置修改器的类型
        /// </summary>
        public void Setup( NumricModifierType type ,float fac)
        {
            _type = type;
            _value_fac = fac;
        }

        /// <summary>
        /// 计算，传入初始值，设置并返回修改后的实际值
        /// </summary>
        public float Calc( float original_val )
        {
            if ( _modified )
            {
                Log.Warning( "this modifier has modified" );
                return 0f;
            }

            switch ( _type )
            {
                case NumricModifierType.Sum:
                    _value_after_modify = original_val + _value_fac;
                    break;

                case NumricModifierType.Mult:
                    _value_after_modify = original_val * _value_fac;
                    break;
                
                case NumricModifierType.Dive:
                    _value_after_modify = original_val / _value_fac;
                    break;
                
                default:
                    Log.Warning("none modifier type.");
                    // throw new GameFrameworkException( "invalid modifier type!" );
                    break;
            }
            _modified = true;
            return _value_after_modify;
        }

        public void Clear()
        {
            _value_after_modify = 0;
            _type = NumricModifierType.None;
            _modified = false;
        }

        /// <summary>
        /// 修改标记
        /// </summary>
        private bool _modified = false;

        /// <summary>
        /// 数值修改方式
        /// </summary>
        private NumricModifierType _type = NumricModifierType.None;

        /// <summary>
        /// 修正值
        /// </summary>
        private float _value_after_modify = 0f;

        /// <summary>
        /// 修改系数
        /// </summary>
        private float _value_fac = 0f;

        //#todo数值修改方式，具体数值
        //关联到数据组件，还有buff之类的，他们都可以持有数值修改器，装备什么的也行
        //#todo_修改数值后如何改回去？
        //因为数据组件的修正值，是计算后的值，

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
    }
}
