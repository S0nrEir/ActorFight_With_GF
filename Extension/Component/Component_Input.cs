using Aquila.Input;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace  Aquila.Extension
{
    /// <summary>
    /// 输入组件
    /// </summary>
    public class Component_Input : GameFrameworkComponent
    {
        
        //----------------override----------------
        
        
        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            _inputProxy.Update(Time.deltaTime);
        }

        /// <summary>
        /// 输入代理
        /// </summary>
        private InputProxy_Base _inputProxy = null;
    }
}
