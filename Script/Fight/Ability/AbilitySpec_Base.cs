using Aquila.Event;
using Aquila.Fight.Addon;
using Aquila.GameTag;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Common;
using Cfg.Enum;
using Cfg.Fight;
using GameFramework;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

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
            //刷新CD
            if ( _cdEffect != null )
                _cdEffect._remain = _cdEffect._totalDuration;

            //扣除cost
            if ( _costEffect != null )
                _costEffect.Apply( _owner, _owner, null );
        }

        /// <summary>
        /// cd effect
        /// </summary>
        public EffectSpec_Period_CoolDown CoolDown => _cdEffect;

        /// <summary>
        /// 移除tag
        /// </summary>
        public void RemoveTag( ushort bitToRemove ,Action<UInt32, int , bool> callBack = null)
        {
            _tagContainer.Remove( bitToRemove ,callBack);
        }

        /// <summary>
        /// 添加tag
        /// </summary>
        public void AddTag( ushort bitToAdd,Action<UInt32, int , bool> callBack = null)
        {
            _tagContainer.Add( bitToAdd,callBack);
        }

        /// <summary>
        /// 包含某个tag
        /// </summary>
        public bool ContainsTag( ushort bitTag )
        {
            // return _tagContainer.Contains( bitTag );
            return _tagContainer.HasFlag(bitTag);
        }
        
        /// <summary>
        /// 设置该技能的meta信息
        /// </summary>
        public virtual void Setup( Table_AbilityBase meta )
        {
            Meta = meta;
            if ( Meta is null)
                return;

            if (meta.Triggers is null || meta.Triggers.Length == 0)
                Log.Warning($"<color=yellow>ability id {meta.id},trigger is null || trigger.lenth equlas 0</color>");

            _costEffect = ReferencePool.Acquire<EffectSpec_Instant_Cost>();
            _costEffect.Init( GameEntry.LuBan.Table<Effect>().Get( Meta.CostEffectID ) );
            _cdEffect = ReferencePool.Acquire<EffectSpec_Period_CoolDown>();
            _cdEffect.Init( GameEntry.LuBan.Table<Effect>().Get( Meta.CoolDownEffectID ) );
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        public virtual bool UseAbility(int triggerIndex, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            if ( !OnPreAbility( result ) )
                return false;

            Table_Effect effectMeta = null;
            EffectSpec_Base tempEffect = null;
            if (triggerIndex >= Meta.Triggers.Length)
            {
                Log.Warning($"<color=yellow>AbilitySpec.UseAbility--->triggerIndex >= Meta.Triggers.Length,index:{triggerIndex},abilityID:{Meta.id}</color>");
            }
            
            var trigger = Meta.Triggers[triggerIndex];
            foreach (var effectID in trigger.CarrayedEffects)
            // foreach ( var effectID in Meta.effects )
            {
                effectMeta = GameEntry.LuBan.Table<Effect>().Get( effectID );
                if ( effectMeta is null )
                {
                    Log.Warning( $"AbilitySpec_Base.UseAbility()--->effectMeta is null,id:{effectID}" );
                    break;
                }
                tempEffect = Tools.Ability.CreateEffectSpecByReferencePool( effectMeta,_owner,target);
                if ( tempEffect is null )
                {
                    Log.Warning( $"AbilitySpec_Base.UseAbility()--->tempEffect is null,effectMeta:{effectMeta.ToString()}" );
                    break;
                }

                if ( tempEffect.Meta.Policy != DurationPolicy.Instant )
                {
                    GameEntry.Impact.Attach( tempEffect, _owner.Actor.ActorID, target.Actor.ActorID );
                }
                else
                {
                    tempEffect.Apply( _owner, target, result );
                    //tempEffect.OnEffectEnd(_owner,target);
                    GameEntry.Module.GetModule<Module_ProxyActor>().InvalidEffect( _owner, target, tempEffect );
                    //ReferencePool.Release( tempEffect );
                }
            }

            if ( !OnAfterAbility( result ) )
                return false;

            result._stateDescription = Tools.SetBitValue( result._stateDescription, ( int ) AbilityHitResultTypeEnum.HIT, true );

            return true;
        }

        /// <summary>
        /// 使用技能前置函数
        /// </summary>
        public virtual bool OnPreAbility( AbilityResult_Hit result )
        {
            return true;
        }

        /// <summary>
        /// 使用技能后置函数
        /// </summary>
        public virtual bool OnAfterAbility( AbilityResult_Hit result )
        {
            return true;
        }

        /// <summary>
        /// 是否可以使用技能，
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
            //处理CD和Cost
            _costEffect?.Clear();
            _cdEffect?.Clear();
            // _tagContainer = null;
            _tagContainer.Reset();
            _cdEffect     = null;
            _costEffect   = null;
            _owner        = null;
        }

        /// <summary>
        /// 刷帧，处理CD
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
        /// tag发生改变的回调
        /// </summary>
        private void OnTagChange( Int64 tagAfterChange, Int64 changedTag, bool isAdd )
        {
            Log.Info( $"tag changed,tag:{changedTag}" );
        }

        /// <summary>
        /// 表数据
        /// </summary>
        public Table_AbilityBase Meta { get; private set; } = null;

        /// <summary>
        /// 该技能持有的tag
        /// </summary>
        // private TagContainer _tagContainer = null;
        private TagContainer _tagContainer;
        
        /// <summary>
        /// 技能CD
        /// </summary>
        private EffectSpec_Period_CoolDown _cdEffect = null;

        /// <summary>
        /// 技能消耗
        /// </summary>
        private EffectSpec_Instant_Cost _costEffect = null;

        /// <summary>
        /// 持有的actor代理实例，技能的持有者
        /// </summary>
        public Module_ProxyActor.ActorInstance _owner = null;

        public AbilitySpecBase()
        {
            // _tagContainer = new TagContainer( OnTagChange );
            _tagContainer = new TagContainer();
        }

        /// <summary>
        /// 根据表格配置生成一个spec实例
        /// </summary>
        /// <param name="meta">技能元数据</param>
        /// <param name="instance">携带的各个组件</param>
        public static AbilitySpecBase Gen( Table_AbilityBase meta, Module_ProxyActor.ActorInstance instance )
        {
            var spec = ReferencePool.Acquire<AbilitySpecBase>();
            spec.Setup( meta );
            spec._owner = instance;
            return spec;
        }
    }
}

