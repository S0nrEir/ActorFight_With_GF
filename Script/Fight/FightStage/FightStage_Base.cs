using System.Collections;
using System.Collections.Generic;
using Aquila.Module;
using UnityEngine;

namespace  Aquila.Fight
{
    /// <summary>
    /// 战斗阶段的基类
    /// </summary>
    public abstract class FightStage_Base
    {
        /// <summary>
        /// 阶段开始
        /// </summary>
        public virtual bool Start(AbilityHitResult result)
        {
            return true;
        }
        
        /// <summary>
        /// 阶段结束
        /// </summary>
        public virtual bool Finish(AbilityHitResult result)
        {
            return true;
        }
    }
    
    /// <summary>
    /// 战斗流程
    /// </summary>
    // public interface IFightProcedure
    // {
    //     public void Enter();
    //
    //     public void HaveSubState();
    //     
    //     public void Next();
    // }
}