using Aquila.Module;
using Aquila.Numric;
using Cfg.Common;
using GameFramework;

namespace Aquila.Fight
{
    /// <summary>
    /// effect逻辑基类
    /// </summary>
    public abstract class EffectSpec_Base
    {
        protected EffectSpec_Base(Table_Effect meta)
        {
            Meta = meta;
            _modifier = ReferencePool.Acquire<Numric_Modifier>();
            _modifier.Setup(Meta.ModifierType,Meta.ExtensionParam.FloatParam_1);
        }

        /// <summary>
        /// 将effect施加到actor上
        /// </summary>
        public virtual void Apply( Module_ProxyActor.ActorInstance instance,ref AbilityHitResult result)
        {
            
        }

        public virtual void Clear()
        {
            ReferencePool.Release(_modifier);
            _modifier = null;
        }
        
        /// <summary>
        /// 元数据
        /// </summary>
        public Table_Effect Meta { get; private set; } = null;

        /// <summary>
        /// 对应的数值修改器
        /// </summary>
        protected Numric_Modifier _modifier = null;

    }
   
}
