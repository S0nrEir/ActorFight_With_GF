
using Aquila.Module;
using GameFramework;

namespace Aquila.Fight
{
    /// <summary>
    /// 受击（普攻）触发类effect
    /// </summary>
    public interface IHitted_Trigger_Effect
    {
        /// <summary>
        /// 是否允许生效effect
        /// </summary>
        bool CanApplyEffect(HittedTriggerEffectParam param);
    }
    
    /// <summary>
    /// 用于检查的参数
    /// </summary>
    public class HittedTriggerEffectParam : IReference
    {
        public HittedTriggerEffectParam
            (
                Module_ProxyActor.ActorInstance castor, 
                Module_ProxyActor.ActorInstance target,
                float effectedValue
            )
        {
            _castor = castor;
            _target = target;
            _effectedValue = effectedValue;
        }
        
        public void Clear()
        {
            _castor = null;
            _target = null;
        }
        
        public Module_ProxyActor.ActorInstance _castor = null;
        public Module_ProxyActor.ActorInstance _target = null;
        
        /// <summary>
        /// 影响的值
        /// </summary>
        public float _effectedValue = 0f;
    }
}