using Cfg.Common;

namespace Aquila.Fight
{
    /// <summary>
    /// 冷却类effect
    /// </summary>
    public class EffectSpec_CoolDown : EffectSpec_Base
    {
        public override void Init( Table_Effect meta )
        {
            base.Init( meta );
            _totalDuration = meta.ExtensionParam.FloatParam_1;
            _remain = 0f;
        }

        public EffectSpec_CoolDown(Table_Effect meta) : base(meta)
        {
            _totalDuration = meta.ExtensionParam.FloatParam_1;
            _remain = 0f;
        }

        public EffectSpec_CoolDown()
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
