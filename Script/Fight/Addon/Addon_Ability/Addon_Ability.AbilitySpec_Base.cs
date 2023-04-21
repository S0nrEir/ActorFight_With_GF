using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight
{

    /// <summary>
    /// 技能逻辑基类
    /// </summary>
    public abstract class AbilitySpecBase : IReference
    {
        public virtual void Clear()
        {
            //处理CD和Cost
        }
    }
}

