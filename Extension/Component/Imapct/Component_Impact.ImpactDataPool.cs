using System;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Impact
{
    public partial class Component_Impact : GameFrameworkComponent
    {
        /// <summary>
        /// Impact数据池
        /// </summary>
        private class ImpactDataPool
        {
            /// <summary>
            /// 获取一个entity持有的impact数据
            /// </summary>
            public ref ImpactData Get( int entity )
            {
                return ref _impactDataArr[_attachedEntityArr[entity]];
            }

            /// <summary>
            /// 移除一个entity持有的ImpactData组件
            /// </summary>
            public void Remove( int entity )
            {
                //移除，放到回收池里
                if ( _recycleImpactDataCount == _recycleImpactDataArr.Length )
                    Array.Resize( ref _recycleImpactDataArr, _recycleImpactDataArr.Length << 1 );

                _recycleImpactDataArr[_recycleImpactDataCount++] = entity;
            }

            /// <summary>
            /// 添加一个entity到数据池内
            /// </summary>
            public ref ImpactData Add( int entity )
            {
                //回收池有，先从回收池拿
                if ( _recycleImpactDataCount > 0 )
                    return ref _impactDataArr[_recycleImpactDataArr[--_recycleImpactDataCount]];

                if ( _impactDataCount == _impactDataArr.Length )
                    Array.Resize( ref _impactDataArr, _impactDataArr.Length << 1 );

                _attachedEntityArr[entity] = _impactDataCount++;
                return ref _impactDataArr[_attachedEntityArr[entity]];
            }

            public ImpactDataPool( int defaultCapcity )
            {
                _impactDataArr = new ImpactData[defaultCapcity];
                _recycleImpactDataArr = new int[defaultCapcity];
                _attachedEntityArr = new int[defaultCapcity];
                _recycleImpactDataCount = 0;
                _impactDataCount = 0;
            }

            /// <summary>
            /// 已经分配了entity的impact组件位置索引集合
            /// </summary>
            private int[] _attachedEntityArr = null;

            /// <summary> 
            /// impact组件回收池数量
            /// </summary>
            private int _recycleImpactDataCount;

            /// <summary>
            /// impact组件回收池
            /// </summary>
            private int[] _recycleImpactDataArr = null;

            /// <summary>
            /// impact组件数量l
            /// </summary>
            private int _impactDataCount;

            /// <summary>
            /// Impact组件集合
            /// </summary>
            private ImpactData[] _impactDataArr = null;
        }
    }
}
