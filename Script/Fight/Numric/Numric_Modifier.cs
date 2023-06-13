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
            valueFac = fac;
        }

        /// <summary>
        /// 计算，传入初始值，设置并返回修改后的实际值
        /// </summary>
        public float Calc( float originalVal )
        {
            // if ( _modified )
            // {
            //     Log.Warning( "this modifier has modified" );
            //     return 0f;
            // }

            switch ( _type )
            {
                case NumricModifierType.Sum:
                    _valueAfterModifying = originalVal + valueFac;
                    break;

                case NumricModifierType.Mult:
                    _valueAfterModifying = originalVal * valueFac;
                    break;
                
                case NumricModifierType.Dive:
                    _valueAfterModifying = originalVal / valueFac;
                    break;
                
                default:
                    Log.Warning("none modifier type.");
                    // throw new GameFrameworkException( "invalid modifier type!" );
                    break;
            }
            _modified = true;
            return _valueAfterModifying;
        }

        public void Clear()
        {
            _valueAfterModifying = 0;
            _type = NumricModifierType.None;
            _modified = false;
        }

        /// <summary>
        /// 修改标记
        /// </summary>
#pragma warning disable IDE0052 // 未读的私有成员
        private bool _modified = false;
#pragma warning restore IDE0052 // 未读的私有成员

        /// <summary>
        /// 数值修改方式
        /// </summary>
        private NumricModifierType _type = NumricModifierType.None;

        /// <summary>
        /// 修正值
        /// </summary>
        private float _valueAfterModifying = 0f;

        /// <summary>
        /// 修改系数
        /// </summary>
        private float valueFac = 0f;
    }
}
