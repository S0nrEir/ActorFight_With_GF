using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.UI
{
    /// <summary>
    /// Form参数类
    /// </summary>
    public class FormParam : IReference
    {
        public void Clear()
        {
            _formTable = null;
            _userData = null;
        }

        /// <summary>
        /// 配置数据
        /// </summary>
        public DRUIForm _formTable;

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object _userData = null;
    }

}
