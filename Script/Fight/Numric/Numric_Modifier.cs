using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Numric
{
    /// <summary>
    /// 数值修饰器，考虑改成strcut的原因：会被实现了IReference的类型实例持有，大小最好不要超过16byte，描述单一状态
    /// </summary>
    public struct Numric_Modifier
    {
        public Numric_Modifier( NumricModifierType type, float fac )
        {
            _type = type;
            valueFac = fac;
            _valueAfterModifying = 0f;
            _modified = false;
        }

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
        private bool _modified;
#pragma warning restore IDE0052 // 未读的私有成员

        /// <summary>
        /// 数值修改方式
        /// </summary>
        private NumricModifierType _type;

        /// <summary>
        /// 修正值
        /// </summary>
        private float _valueAfterModifying;

        /// <summary>
        /// 修改系数
        /// </summary>
        private float valueFac;
    }
}
