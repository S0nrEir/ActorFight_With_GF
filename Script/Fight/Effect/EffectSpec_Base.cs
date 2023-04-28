using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Cfg.common;

namespace Aquila.Fight
{
    /// <summary>
    /// effect逻辑基类
    /// </summary>
    public abstract class EffectSpec_Base
    {
        protected EffectSpec_Base(Effect meta)
        {
            Meta = meta;
        }

        /// <summary>
        /// 将effect施加到actor上
        /// </summary>
        public virtual void Apply(TActorBase actor, Addon_Base[] addon)
        {
            
        }

        /// <summary>
        /// 元数据
        /// </summary>
        public Effect Meta { get; private set; } = null;

        public abstract void Clear();
    }
   
}
