using System;
using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.GameTag;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using Cfg.Fight;
using GameFramework;

namespace Aquila.Fight
{
    /// <summary>
    /// 技能逻辑基类
    /// </summary>
    public /*abstract*/ class AbilitySpecBase : IReference
    {
        /// <summary>
        /// 扣除技能消耗
        /// </summary>
        public void Deduct()
        {
            // 刷新 CD
            if ( _cdEffect != null )
                _cdEffect._remain = _cdEffect._totalDuration;

            // 扣除 Cost
            if ( _costEffect != null )
                _costEffect.Apply( _owner, _owner, null );
        }

        /// <summary>
        /// 冷却效果
        /// </summary>
        public EffectSpec_Period_CoolDown CoolDown => _cdEffect;

        /// <summary>
        /// 移除 Tag
        /// </summary>
        public void RemoveTag( ushort bitToRemove ,Action<UInt32, int , bool> callBack = null)
        {
            _tagContainer.Remove( bitToRemove ,callBack);
        }

        /// <summary>
        /// 添加 Tag
        /// </summary>
        public void AddTag( ushort bitToAdd,Action<UInt32, int , bool> callBack = null)
        {
            _tagContainer.Add( bitToAdd,callBack);
        }

        /// <summary>
        /// 是否包含指定 Tag
        /// </summary>
        public bool ContainsTag( ushort bitTag )
        {
            // return _tagContainer.Contains( bitTag );
            return _tagContainer.HasFlag(bitTag);
        }
        
        /// <summary>
        /// 使用 AbilityData 设置技能信息（新数据源）
        /// </summary>
        public virtual void Setup(AbilityData data)
        {
            _data = data;
            
            // 初始化 Cost 与 CoolDown 效果
            var costEffectId = data.GetCostEffectID();
            var cdEffectId = data.GetCoolDownEffectID();
            
            if (GameEntry.AbilityPool.TryGetEffect(costEffectId, out var costData))
            {
                var costEffect = Tools.Ability.CreateEffectSpecByReferencePool(costData, null, null);
                if (costEffect is EffectSpec_Instant_Cost costSpec)
                {
                    _costEffect = costSpec;
                }
                else
                {
                    if (costEffect != null)
                    {
                        Tools.Logger.Error($"<color=yellow>AbilitySpecBase.Setup: Cost effect type mismatch, effectId={costEffectId}, expected={nameof(EffectSpec_Instant_Cost)}, actual={costEffect.GetType().Name}</color>");
                        ReferencePool.Release(costEffect);
                    }

                    Tools.Logger.Warning($"<color=yellow>AbilitySpecBase.Setup: Failed to create Cost effect {costEffectId}</color>");
                }
            }
            else
            {
                Tools.Logger.Warning($"<color=yellow>AbilitySpecBase.Setup: Cost effect {costEffectId} not found in pool</color>");
            }
            
            if (GameEntry.AbilityPool.TryGetEffect(cdEffectId, out var cdData))
            {
                var cdEffect = Tools.Ability.CreateEffectSpecByReferencePool(cdData, null, null);
                if (cdEffect is EffectSpec_Period_CoolDown cdSpec)
                {
                    _cdEffect = cdSpec;
                }
                else
                {
                    if (cdEffect != null)
                    {
                        Tools.Logger.Error($"<color=yellow>AbilitySpecBase.Setup: CoolDown effect type mismatch, effectId={cdEffectId}, expected={nameof(EffectSpec_Period_CoolDown)}, actual={cdEffect.GetType().Name}</color>");
                        ReferencePool.Release(cdEffect);
                    }

                    Tools.Logger.Warning($"<color=yellow>AbilitySpecBase.Setup: Failed to create CoolDown effect {cdEffectId}</color>");
                }
            }
            else
            {
                Tools.Logger.Warning($"<color=yellow>AbilitySpecBase.Setup: CoolDown effect {cdEffectId} not found in pool</color>");
            }
        }
        
        /// <summary>
        /// 使用 Table_AbilityBase 设置技能信息（LuBan 配置，保留兼容）
        /// </summary>
        // public virtual void Setup( Table_AbilityBase meta )
        // {
        //     Meta = meta;
        //     if ( Meta is null)
        //         return;
        //
        //     if (meta.Triggers is null || meta.Triggers.Length == 0)
        //         Aquila.Toolkit.Tools.Logger.Warning($"<color=yellow>ability id {meta.id},trigger is null || trigger.lenth equlas 0</color>");
        //
        //     _costEffect = ReferencePool.Acquire<EffectSpec_Instant_Cost>();
        //     _costEffect.Init( GameEntry.LuBan.Table<Effect>().Get( Meta.CostEffectID ) );
        //     _cdEffect = ReferencePool.Acquire<EffectSpec_Period_CoolDown>();
        //     _cdEffect.Init( GameEntry.LuBan.Table<Effect>().Get( Meta.CoolDownEffectID ) );
        // }

        /// <summary>
        /// 使用技能
        /// </summary>
        public virtual bool UseAbility(int triggerIndex, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            if ( !OnPreAbility( result ) )
                return false;

            // 使用 AbilityData 时，根据 triggerIndex 仅执行对应 Effect
            if ( _data.GetId() > 0 )
            {
                var effects = _data.GetEffects();
                if ( effects is null || effects.Count == 0 )
                {
                    Tools.Logger.Warning( $"AbilitySpec_Base.UseAbility()--->ability {_data.GetId()} has no effects" );
                    return false;
                }

                if ( triggerIndex < 0 || triggerIndex >= effects.Count )
                {
                    Tools.Logger.Warning( $"AbilitySpec_Base.UseAbility()--->invalid triggerIndex:{triggerIndex}, abilityID:{_data.GetId()}, effectCount:{effects.Count}" );
                    return false;
                }

                var effectData = effects[triggerIndex];
                var tempEffect = Tools.Ability.CreateEffectSpecByReferencePool( effectData, _owner, target );
                if ( tempEffect == null )
                {
                    Tools.Logger.Warning( $"AbilitySpec_Base.UseAbility()--->Failed to create effect {effectData.GetEffectId()}" );
                    return false;
                }

                if ( tempEffect.Policy != DurationPolicy.Instant )
                {
                    if ( target == null )
                    {
                        Tools.Logger.Warning( $"AbilitySpec_Base.UseAbility()--->target is null for non-instant effect, effectID:{effectData.GetEffectId()}" );
                        return false;
                    }

                    GameEntry.Impact.Attach( tempEffect, _owner.Actor.ActorID, target.Actor.ActorID );
                }
                else
                {
                    tempEffect.Apply( _owner, target, result );
                    GameEntry.Module.GetModule<Module_ProxyActor>().InvalidEffect( _owner, target, tempEffect );
                }
            }
            // 否则使用 LuBan 配置（保留兼容）
            else
            {
                Tools.Logger.Warning($"<color=yellow>AbilitySpec_Base.UseAbility --> Invalid Ability ID , {_data.GetId()} </color>");
            }

            if ( !OnAfterAbility( result ) )
                return false;

            result._stateDescription = Tools.SetBitValue( result._stateDescription, ( int ) AbilityHitResultTypeEnum.HIT, true );

            return true;
        }
        /// <summary>
        /// 使用技能前置逻辑
        /// </summary>
        public virtual bool OnPreAbility( AbilityResult_Hit result )
        {
            return true;
        }

        /// <summary>
        /// 使用技能后置逻辑
        /// </summary>
        public virtual bool OnAfterAbility( AbilityResult_Hit result )
        {
            return true;
        }

        /// <summary>
        /// 是否可以使用技能
        /// </summary>
        public virtual int CanUseAbility()
        {
            if ( !CostOK() )
                return ( int ) AbilityUseResultTypeEnum.COST_NOT_ENOUGH;

            if ( !CDOK() )
                return ( int ) AbilityUseResultTypeEnum.CD_NOT_OK;

            return 0;
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public virtual void Clear()
        {
            Meta          = null;
            _data         = default;
            // 清理 CD 和 Cost
            _costEffect?.Clear();
            _cdEffect?.Clear();
            // _tagContainer = null;
            _tagContainer.Reset();
            _cdEffect     = null;
            _costEffect   = null;
            _owner        = null;
        }

        /// <summary>
        /// 刷帧处理 CD
        /// </summary>
        public virtual void OnUpdate( float delta_time )
        {
            _cdEffect._remain -= delta_time;
        }

        //-------------------priv-------------------
        /// <summary>
        /// 检查技能冷却
        /// </summary>
        private bool CDOK()
        {
            return _cdEffect._remain <= 0f;
        }

        /// <summary>
        /// 检查技能消耗
        /// </summary>
        private bool CostOK()
        {
            if ( _costEffect is null )
                return true;

            var attr_addon = _owner.GetAddon<Addon_BaseAttrNumric>();
            if ( attr_addon is null )
                return false;

            var cur_mp = attr_addon.GetCurrMPCorrection();
            return _costEffect.Calc( cur_mp ) >= 0;
        }

        /// <summary>
        /// Tag 变化回调
        /// </summary>
        private void OnTagChange( Int64 tagAfterChange, Int64 changedTag, bool isAdd )
        {
            Tools.Logger.Info( $"tag changed,tag:{changedTag}" );
        }

        /// <summary>
        /// 表数据（LuBan 配置，保留兼容）
        /// </summary>
        public Table_AbilityBase Meta { get; private set; }

        /// <summary>
        /// 技能数据（新数据源）
        /// </summary>
        private AbilityData _data;
        
        /// <summary>
        /// 技能 ID（优先从 AbilityData 获取，否则从 Meta 获取）
        /// </summary>
        public int AbilityId => _data.GetId() != 0 ? _data.GetId() : (Meta?.id ?? 0);

        /// <summary>
        /// 该技能持有的 Tag
        /// </summary>
        // private TagContainer _tagContainer = null;
        private TagContainer _tagContainer;
        
        /// <summary>
        /// 技能 CD
        /// </summary>
        private EffectSpec_Period_CoolDown _cdEffect;

        /// <summary>
        /// 技能消耗
        /// </summary>
        private EffectSpec_Instant_Cost _costEffect;

        /// <summary>
        /// 持有该技能的 Actor 实例
        /// </summary>
        public Module_ProxyActor.ActorInstance _owner;

        public AbilitySpecBase()
        {
            // _tagContainer = new TagContainer( OnTagChange );
            _tagContainer = new TagContainer();
        }

        /// <summary>
        /// 根据 AbilityData 生成一个 Spec 实例
        /// </summary>
        public static AbilitySpecBase Gen(AbilityData data, Module_ProxyActor.ActorInstance instance)
        {
            var spec = ReferencePool.Acquire<AbilitySpecBase>();
            spec.Setup(data);
            spec._owner = instance;
            return spec;
        }
        
        /// <summary>
        /// 根据表格配置生成一个 Spec 实例
        /// </summary>
        /// <param name="meta">技能元数据</param>
        /// <param name="instance">携带的各个组件</param>
        // public static AbilitySpecBase Gen( Table_AbilityBase meta, Module_ProxyActor.ActorInstance instance )
        // {
        //     var spec = ReferencePool.Acquire<AbilitySpecBase>();
        //     spec.Setup( meta );
        //     spec._owner = instance;
        //     return spec;
        // }
    }
}

