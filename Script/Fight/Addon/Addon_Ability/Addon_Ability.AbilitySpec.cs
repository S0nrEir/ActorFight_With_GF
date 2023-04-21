using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace  Aquila.Fight.Addon
{
    public partial class Addon_Ability : AddonBase
    {
        /// <summary>
        /// 技能逻辑类
        /// </summary>
        internal class  AbilitySpec : AbilitySpecBase
        {
            //CoolDown和Cost
            public AbilitySpec()
            {
                
            }

            public override void Clear()
            {
                base.Clear();
            }
        }
    }
}
