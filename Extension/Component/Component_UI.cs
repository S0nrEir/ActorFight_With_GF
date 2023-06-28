using GameFramework.DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// UI模块
    /// </summary>
    public class Component_UI : GameFrameworkComponent
    {
        //--------------------------- pub ---------------------------
        /// <summary>
        /// 打开
        /// </summary>
        public void Open(int formID)
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close(int formID)
        {
            
        }

        /// <summary>
        /// 关闭所有
        /// </summary>
        public void CloseAll()
        {
            
        }

        //--------------------------- priv ---------------------------

        private void Start()
        {
            _uiComp = GameEntry.BaseUI;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        public UIComponent _uiComp = null;
    }
}
