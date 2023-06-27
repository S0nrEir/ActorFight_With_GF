using Aquila.Event;
using Aquila.Module;
using Aquila.Numric;
using Cfg.Common;
using GameFramework;

namespace Aquila.Fight
{
    /// <summary>
    /// effect逻辑基类
    /// </summary>
    public abstract class EffectSpec_Base : IReference
    {
        public virtual void Init( Table_Effect meta )
        {
            Meta = meta;
            _modifier = default;
        }

        /// <summary>
        /// 当effect销毁
        /// </summary>
        public virtual void OnEffectEnd()
        {
            
        }

        /// <summary>
        /// 将effect施加到actor上
        /// </summary>
        public virtual void Apply( Module_ProxyActor.ActorInstance instance, AbilityResult_Hit result )
        {

        }

        public virtual void Clear()
        {
            _modifier = default;
            Meta = null;
        }

        protected EffectSpec_Base()
        {

        }

        protected EffectSpec_Base( Table_Effect meta )
        {
            Init( meta );
        }

        /// <summary>
        /// 元数据
        /// </summary>
        public Table_Effect Meta { get; private set; } = null;

        /// <summary>
        /// 对应的数值修改器
        /// </summary>
        protected Numric_Modifier _modifier;
    }
}
