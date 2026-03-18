#if UNITY_EDITOR

using Aquila.Fight;
using UnityGameFramework.Runtime;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// 技能编辑器沙盒控制组件
    /// </summary>
    public class Component_AbilityEditorSandBox : GameFrameworkComponent
    {
        /// <summary>
        /// 设置沙盒技能数据
        /// </summary>
        public void Init(AbilityData abilityData)
        {
            _sandBoxAbility = abilityData;
            _hasSandBoxAbility = true;
        }

        /// <summary>
        /// 获取沙盒技能数据
        /// </summary>
        public bool TryGetSandBoxAbility(out AbilityData abilityData)
        {
            abilityData = _sandBoxAbility;
            return _hasSandBoxAbility;
        }

        private AbilityData _sandBoxAbility;
        private bool _hasSandBoxAbility;
    }
}

#endif

