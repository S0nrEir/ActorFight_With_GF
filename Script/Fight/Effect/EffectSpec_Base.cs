using Aquila.Event;
using Aquila.Module;
using Aquila.Numric;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;

namespace Aquila.Fight
{
    /// <summary>
    /// effect逻辑基类
    /// </summary>
    public abstract class EffectSpec_Base : IReference
    {
        public DurationPolicy Policy => _effectData.GetPolicy();
        
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
            get;
            set;
        }

        /// <summary>
        /// 当前叠加层数
        /// </summary>
        public virtual ushort StackCount
        {
            get => _stackCount;
            set => _stackCount = value;
        }

        /// <summary>
        /// 从 EffectData 初始化（新数据源）
        /// </summary>
        public virtual void Init(EffectData meta, Module_ProxyActor.ActorInstance castor = null,
            Module_ProxyActor.ActorInstance target = null)
        {
            _effectData = meta;
            _modifier = default;
            // EffectId = data.GetEffectId();
            // EffectType = data.GetEffectType();
            // Policy = data.GetPolicy();
            // Duration = data.GetDuration();
            // Period = data.GetPeriod();
            // Target = data.GetTarget();
            // AffectedAttribute = data.GetAffectedAttribute();
            // ModifierType = data.GetModifierType();
            // EffectOnAwake = data.GetEffectOnAwake();
            //
            // FloatParam1 = data.GetFloatParam1();
            // FloatParam2 = data.GetFloatParam2();
            // FloatParam3 = data.GetFloatParam3();
            // FloatParam4 = data.GetFloatParam4();
            //
            // IntParam1 = data.GetIntParam1();
            // IntParam2 = data.GetIntParam2();
            // IntParam3 = data.GetIntParam3();
            // IntParam4 = data.GetIntParam4();
            
            // var derives = data.GetDeriveEffects();
            // DeriveEffects = new int[derives.Count];
            // for (int i = 0; i < derives.Count; i++)
            //     DeriveEffects[i] = derives[i];
            //
            // var awakes = data.GetAwakeEffects();
            // AwakeEffects = new int[awakes.Count];
            // for (int i = 0; i < awakes.Count; i++)
            //     AwakeEffects[i] = awakes[i];

        }

        /// <summary>
        /// 从 Table_Effect 初始化（LuBan 配置，用于派生 effect 回退）
        /// </summary>
        // public virtual void Init(Table_Effect meta, Module_ProxyActor.ActorInstance castor = null,
        //     Module_ProxyActor.ActorInstance target = null)
        // {
        //     // EffectId = meta.id;
        //     // EffectType = meta.Type;
        //     // Policy = meta.Policy;
        //     // Duration = meta.Duration;
        //     // Period = meta.Period;
        //     // Target = meta.Target;
        //     // AffectedAttribute = meta.EffectType;
        //     // ModifierType = meta.ModifierType;
        //     // EffectOnAwake = meta.EffectOnAwake;
        //     //
        //     // FloatParam1 = meta.ExtensionParam.FloatParam_1;
        //     // FloatParam2 = meta.ExtensionParam.FloatParam_2;
        //     // FloatParam3 = meta.ExtensionParam.FloatParam_3;
        //     // FloatParam4 = meta.ExtensionParam.FloatParam_4;
        //     //
        //     // IntParam1 = meta.ExtensionParam.IntParam_1;
        //     // IntParam2 = meta.ExtensionParam.IntParam_2;
        //     // IntParam3 = meta.ExtensionParam.IntParam_3;
        //     // IntParam4 = meta.ExtensionParam.IntParam_4;
        //     //
        //     // DeriveEffects = meta.DeriveEffects;
        //     // AwakeEffects = meta.AwakeEffects;
        //     //
        //      // _modifier = default;
        // }

        /// <summary>
        /// effect被唤起时
        /// </summary>
        public virtual void OnEffectAwake( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            //派发子effect
            EffectSpec_Base newEffect = null;
            foreach ( var effectID in _effectData.GetAwakeEffects() )
            {
                if (GameEntry.AbilityPool.TryGetEffect(effectID, out var effectData))
                {
                    newEffect = Tools.Ability.CreateEffectSpecByReferencePool(effectData, castor, target);
                }
                else
                {
                    Tools.Logger.Warning($"<color=yellow>EffectSpec_Base.OnEffectAwake --> No effect found with id: {effectID}</color>");
                    // 回退到 LuBan 查询
                    // var meta = GameEntry.LuBan.Tables.Effect.Get(effectID);
                    // if (meta == null)
                    // {
                    //     Aquila.Toolkit.Tools.Logger.Warning($"<color=yellow>EffectSpec_Base.OnEffectAwake()--->effect not found in pool or LuBan, id:{effectID}</color>");
                    //     continue;
                    // }
                    // newEffect = Tools.Ability.CreateEffectSpecByReferencePool(meta, castor, target);
                }
                
                if ( newEffect is null )
                {
                    Tools.Logger.Warning( $"EffectSpec_Base.OnEffectAwake()--->newEffect is null, effectID:{effectID}" );
                    continue;
                }

                if ( effectData.GetPolicy() != DurationPolicy.Instant )
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
        public virtual void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
        }

        public virtual void Clear()
        {
            _modifier          = default;
            // ModifierType       = default;
            StackCount         = 1;
            // StackLimit         = 0;
            // _impactEntityIndex = 0;
            ResetWhenOverride  = false;
            _effectData = default;
        }

        /// <summary>
        /// 叠加层数
        /// </summary>
        // private int _stackCount = 1;

        /// <summary>
        /// 叠加层数上限
        /// </summary>
        private int _stackLimit = 0;

        /// <summary>
        /// impact数据的实体索引
        /// </summary>
        public int _impactEntityIndex = 0;

        // /// <summary>
        // /// Effect ID
        // /// </summary>
        // public int EffectId;
        //
        // /// <summary>
        // /// Effect 类型
        // /// </summary>
        // public EffectType EffectType;
        //
        // /// <summary>
        // /// 持续策略
        // /// </summary>
        // public DurationPolicy Policy;
        //
        // /// <summary>
        // /// 持续时间
        // /// </summary>
        // public float Duration;
        //
        // /// <summary>
        // /// 周期
        // /// </summary>
        // public float Period;
        //
        // /// <summary>
        // /// 目标
        // /// </summary>
        // public int Target;
        //
        // /// <summary>
        // /// 影响的属性
        // /// </summary>
        // public actor_attribute AffectedAttribute;
        
        /// <summary>
        /// 修改器类型
        /// </summary>
        //public NumricModifierType ModifierType;
        
        /// <summary>
        /// 唤醒时生效
        /// </summary>
        // public bool EffectOnAwake;
        //
        // /// <summary>
        // /// 浮点参数
        // /// </summary>
        // public float FloatParam1, FloatParam2, FloatParam3, FloatParam4;
        //
        // /// <summary>
        // /// 整数参数
        // /// </summary>
        // public int IntParam1, IntParam2, IntParam3, IntParam4;
        //
        // /// <summary>
        // /// 派生效果 ID 列表
        // /// </summary>
        // public int[] DeriveEffects;
        //
        // /// <summary>
        // /// 唤醒效果 ID 列表
        // /// </summary>
        // public int[] AwakeEffects;

        /// <summary>
        /// 对应的数值修改器
        /// </summary>
        protected Numric_Modifier _modifier;
        protected EffectData _effectData;
        protected ushort _stackCount;
        public EffectData Meta => _effectData;
    }
}

