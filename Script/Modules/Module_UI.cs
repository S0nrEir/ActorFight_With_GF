using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Aquila.Module;
using Aquila;
using UnityGameFramework.Runtime;
using Aquila.Extension;

namespace Aquila.Module
{
    /// <summary>
    /// ui模块，作为UIComponent的扩展
    /// </summary>
    public class Module_UI : GameFrameworkModuleBase
    {
        #region public

        /// <summary>
        /// 关闭ui
        /// </summary>
        public void CloseUI( int form_id )
        {
            //#todo
        }

        /// <summary>
        /// 显示一个ui
        /// </summary>
        public void PushUI(int form_id,object user_data)
        {
            //#todo
        }

        #endregion

        public override void OnClose()
        {
        }

        public override void EnsureInit()
        {
            base.EnsureInit();
            if ( _cached_UI_Component == null )
                _cached_UI_Component = GameEntry.UI;
        }

        public override void End()
        {
            base.End();
        }

        /// <summary>
        /// ui组件缓存
        /// </summary>
        private UIComponent _cached_UI_Component = null;
    }
}
