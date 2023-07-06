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
        /// <summary>
        /// 当覆盖时重置
        /// </summary>
        public bool ResetWhenOverride
        {
            get;
            protected set;
        }

        /// <summary>
        /// 叠加层数上限
        /// </summary>
        public int StackLimit
        {
            get => _stackLimit;
            set => _stackLimit = value;
        }

        /// <summary>
        /// 当前叠加层数
        /// </summary>
        public virtual int StackCount
        {
            get => _stackCount;
            set => _stackCount = value;
        }

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
        public virtual void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {

        }

        public virtual void Clear()
        {
            _modifier          = default;
            Meta               = null;
            StackCount         = 0;
            StackLimit         = 0;
            _impactEntityIndex = 0;
            ResetWhenOverride  = false;
        }

        protected EffectSpec_Base()
        {

        }

        protected EffectSpec_Base( Table_Effect meta )
        {
            Init( meta );
        }

        /// <summary>
        /// 叠加层数
        /// </summary>
        private int _stackCount = 0;

        /// <summary>
        /// 叠加层数上限
        /// </summary>
        private int _stackLimit = 0;

        /// <summary>
        /// impact数据的实体索引
        /// </summary>
        public int _impactEntityIndex = 0;

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
