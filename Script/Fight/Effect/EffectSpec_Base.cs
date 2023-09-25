using Aquila.Event;
using Aquila.Module;
using Aquila.Numric;
using Aquila.Toolkit;
using Cfg.Common;
using Cfg.Enum;
using GameFramework;
using UnityGameFramework.Runtime;

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
        /// effect被唤起时
        /// </summary>
        public virtual void OnEffectAwake( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            //派发子effe
            Cfg.Common.Table_Effect meta = null;
            EffectSpec_Base newEffect = null;
            //这里并不能复用AbilitySpec的逻辑
            foreach ( var effectID in Meta.AwakeEffects )
            {
                meta = GameEntry.LuBan.Tables.Effect.Get( effectID );
                if ( meta is null )
                {
                    Log.Warning( $"<color=yellow>EffectSpec_Period_Deriging.Apply()--->meta is null,id:{effectID}</color>" );
                    continue;
                }
                newEffect = Tools.Ability.CreateEffectSpecByReferencePool( meta );
                if ( newEffect is null )
                {
                    Log.Warning( $"EffectSpec_Period_Deriving.Apply()--->newEffect is null,effectMeta:{meta.ToString()}" );
                    break;
                }

                if ( newEffect.Meta.Policy != DurationPolicy.Instant )
                {
                    GameEntry.Impact.Attach( newEffect, castor.Actor.ActorID, target.Actor.ActorID );
                }
                else
                {
                    GameEntry.Module.GetModule<Module_ProxyActor>().ApplyEffect( castor, target, newEffect );
                    GameEntry.Module.GetModule<Module_ProxyActor>().InvalidEffect( castor, target, newEffect );
                }
            }
        }

        /// <summary>
        /// 当effect销毁
        /// </summary>
        public virtual void OnEffectEnd( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
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
            StackCount         = 1;
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
        private int _stackCount = 1;

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
