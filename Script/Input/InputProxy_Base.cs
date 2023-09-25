using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Input
{
    public abstract class InputProxy_Base
    {
        
        public InputProxy_Base()
        {
        }

        /// <summary>
        /// 释放当前代理持有的相关资源
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 输入类型
        /// </summary>
        public abstract InputDeviceTypeEnum InputType { get; }

        /// <summary>
        /// 刷帧处理输入
        /// </summary>
        public abstract void Update(float elpased);
    }
}
