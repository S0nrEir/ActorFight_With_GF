using Aquila.Fight.Actor;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace  Aquila.Module
{
    /// <summary>
    /// Module_Proxy_Actor的部分类，用于处理actor proxy instance的战斗逻辑
    /// </summary>
    public partial class Module_Proxy_Actor
    {
        /// <summary>
        /// 单对单释放技能
        /// </summary>
        /// <param name="castor_">施法者</param>
        /// <param name="target_">目标</param>
        /// <param name="ability_meta_">技能元数据</param>
        /// <returns></returns>
        public object AbilityToSingleTarget(TActorBase castor_,TActorBase target_,object ability_meta_)
        {
            //检查类型走不同的流程
            if (castor_ is null || target_ is null)
            {
                Log.Warning("<color=yellow>castor_ is null || target_ is null</color>");
                return null;
            }

            if (target_.Length == 0)
            {
                Debug.Log("<color=yellow>DoAbility--->target_.Length == 0</color>");
                return null;
            }
            
            //obtain ability result
            var result = default(AbilityResult);
            result.Init();
            
            return null;
        }
    }

    /// <summary>
    /// 技能结果
    /// </summary>
    public struct AbilityResult
    {
        public void Init()
        {
            _ability_result = AbilityResultTypeEnum.INVALID;
        }
        
        /// <summary>
        /// 是否成功
        /// </summary>
        public AbilityResultTypeEnum _ability_result;
    }

    /// <summary>
    /// 技能结果
    /// </summary>
    public enum AbilityResultTypeEnum
    {
        /// <summary>
        /// 无效
        /// </summary>
        INVALID = -1,
        
        /// <summary>
        /// 命中
        /// </summary>
        HIT = 0,
        
        /// <summary>
        /// miss
        /// </summary>
        MISS = 1,
    }
}