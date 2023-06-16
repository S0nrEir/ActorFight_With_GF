using Aquila.Fight.Actor;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Impact
{
    /// <summary>
    /// 角色的主动和被动效果组件
    /// </summary>
    public partial class Component_Impact : GameFrameworkComponent
    {

        //----------------------- priv -----------------------
        /// <summary>
        /// buff&debuff轮询
        /// </summary>
        private void Update()
        {
            
        }

        protected override void Awake()
        {
            base.Awake();
        }
        
        private void Start()
        {
            EnsureInit();
        }

        private void EnsureInit()
        {
            _effectDic = new Dictionary<int, EffectSpec_Base>();
        }

        //----------------------- fields -----------------------
        /// <summary>
        /// 存储的effect实例集合
        /// </summary>
        private Dictionary<int, EffectSpec_Base> _effectDic = null;
    }

}
