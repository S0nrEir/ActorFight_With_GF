#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using Aquila.Module;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// 技能沙盒界面类
    /// </summary>
    public class Form_AbilitySandBox : UIFormLogic
    {
        
        
        
        //override:
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
        }

        /// <summary>
        /// 沙盒界面参数类
        /// </summary>
        public class AbilitySandBoxForm_Param
        {
        }
    }

}

#endif