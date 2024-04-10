using Aquila.Module;
using Cfg.Common;

namespace Aquila.Fight
{
    /// <summary>
    /// 冷却类effect
    /// </summary>
    public class EffectSpec_Period_CoolDown : EffectSpec_Base
    {
        public override void Init( Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null )
        {
            base.Init( meta ,castor,target);
            _totalDuration = meta.ExtensionParam.FloatParam_1;
            _remain = 0f;
        }

        public EffectSpec_Period_CoolDown()
        {
            
        }

        /// <summary>
        /// 剩余时间
        /// </summary>
        public float _remain = 0f;
        
        /// <summary>
        /// cool down
        /// </summary>
        public float _totalDuration = 0f;
    }
   
}
