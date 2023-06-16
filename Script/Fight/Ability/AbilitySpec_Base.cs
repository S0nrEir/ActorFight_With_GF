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
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 技能逻辑基类
    /// </summary>
    public /*abstract*/ class AbilitySpecBase : IReference
    {
        //CoolDown和Cost：拿CD和Cost类型的GE数据
        //根据GE数据创建对应GESpec
        //Spec持有数据
        //EffectScriptableObject翻译成配表，包含CD和Cost
        //#todo为tag添加移除和添加时的回调
        //子弹类技能怎么配置：技能表添加类型，比如召唤物，子弹类，蓄力，位移等，根据类型生成特殊的spec，使用技能时加载actor，路径就放在Numric字段----------暂时不用了，可以用effect来做

        /// <summary>
        /// 移除tag
        /// </summary>
        public void RemoveTag( ushort bit_to_remove )
        {
            _tagContainer.Remove( bit_to_remove );
        }

        /// <summary>
        /// 添加tag
        /// </summary>
        public void AddTag( ushort bit_to_add )
        {
            _tagContainer.Add( bit_to_add );
        }

        /// <summary>
        /// 包含某个tag
        /// </summary>
        public bool ContainsTag( ushort bit_tag )
        {
            return _tagContainer.Contains( bit_tag );
        }

        public virtual void Setup( Table_AbilityBase meta )
        {
            Meta = meta;
            if ( Meta is null || Meta.effects is null )
                return;

            _costEffect = ReferencePool.Acquire<EffectSpec_Cost>();
            _costEffect.Init( GameEntry.DataTable.Table<Effect>().Get( Meta.CostEffectID ) );
            _cdEffect = ReferencePool.Acquire<EffectSpec_CoolDown>();
            _cdEffect.Init( GameEntry.DataTable.Table<Effect>().Get( Meta.CoolDownEffectID ) );
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        public virtual bool UseAbility( Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            if ( !OnPreAbility( result ) )
                return false;

            //刷新CD
            if ( _cdEffect != null )
                _cdEffect._remain = _cdEffect._totalDuration;

            //扣除cost
            if (_costEffect != null)
                _costEffect.Apply( _owner, result );

            //#todo因为要处理持续性的effect，不需要effectList了（在释放技能的时候遍历实例化），cd和cost类型的effect在这个时候也会被遍历到，是否考虑把这两个类型的effect单独放到技能表单独的两个字段里？
            Table_Effect effectMeta = null;
            EffectSpec_Base tempEffect = null;
            foreach ( var effectID in Meta.effects )
            {
                effectMeta = GameEntry.DataTable.Table<Effect>().Get( effectID );
                tempEffect = Tools.Ability.CreateEffectSpec( effectMeta );
                
                tempEffect.Apply( effectMeta.Target == 1 ? target : _owner, result );
                ReferencePool.Release( tempEffect );
                //duration effect:
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
            Meta = null;
            //处理CD和Cost
            _costEffect?.Clear();
            _cdEffect?.Clear();
            _tagContainer = null;
            _cdEffect = null;
            _costEffect = null;
            _owner = null;
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

            //因为都独立开来了，拿不到技能所属的addon和actor，暂时想到的解决办法：通过proxy拿他们的代理实例
            var attr_addon = _owner.GetAddon<Addon_BaseAttrNumric>();
            if ( attr_addon is null )
                return false;

            var cur_mp = attr_addon.GetCurrMPCorrection();
            return _costEffect.Calc( cur_mp ) >= 0;
        }

        /// <summary>
        /// tag发生改变的回调
        /// </summary>
        private void OnTagChange( Int64 oldTag, Int64 newTag, ushort changedIndex )
        {
            Log.Info( $"tag changed,tag:{newTag}" );
        }

        /// <summary>
        /// 表数据
        /// </summary>
        public Table_AbilityBase Meta { get; private set; } = null;

        /// <summary>
        /// 该技能持有的tag
        /// </summary>
        private TagContainer _tagContainer = null;

        /// <summary>
        /// 技能CD
        /// </summary>
        private EffectSpec_CoolDown _cdEffect = null;

        /// <summary>
        /// 技能消耗
        /// </summary>
        private EffectSpec_Cost _costEffect = null;

        /// <summary>
        /// 持有的actor代理实例，技能的持有者
        /// </summary>
        public Module_ProxyActor.ActorInstance _owner = null;

        public AbilitySpecBase()
        {
            _tagContainer = new TagContainer( OnTagChange );
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

