using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Aquila.Module;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Impact
{
    //            impact pool
    //----------------------------------
    //| entity_1 | entity_2 | entity_3 |
    //----------------------------------
    //| impact_1 | impact_2 | impact_3 |
    //----------------------------------
    
    //                             get by entity id
    //          entity pool <-------------------------------imapct pool
    //       /      |       \
    //      /       |         \
    //----------------------------------
    //| entity_1 | entity_2 | entity_3 |
    //----------------------------------
    //        \       |     /
    //         \      |    /
    //          _curr/invalid
    
    /// <summary>
    /// 角色的主动和被动效果组件
    /// </summary>
    public partial class Component_Impact : GameFrameworkComponent
    {
        //----------------------- pub -----------------------

        /// <summary>
        /// 在下一帧移除actor特定类型的effect
        /// </summary>
        public void PrepareRemoveEffect(EffectSpec_Base effect)
        {
            var entity = effect._impactEntityIndex;
            ref var impact = ref _pool.Get(entity);
            impact._elapsed = 9999f;
            impact._policy = DurationPolicy.Instant;
        }

        /// <summary>
        /// 筛选指定target上的特定类型的effect，有返回true
        /// </summary>
        public bool FilterSpecEffect(int targetID,Func<EffectSpec_Base,bool> filterFunc)
        {
            var effects = GetAttachedEffect(targetID);
            if (effects is null || effects.Count == 0)
                return false;

            foreach (var effect in effects)
            {
                if (filterFunc(effect))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取指定actor上的指定effect，有返回true
        /// </summary>
        public bool FilterSpecEffect(int actorID,EffectSpec_Base effect)
        {
            var effects = GetAttachedEffect(actorID);
            if (effects is null || effects.Count == 0)
                return false;

            foreach (var tempEffects in effects)
            {
                if (tempEffects == effect)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 在下一帧移除actor特定类型的effect
        /// </summary>
        public void PrepareRemoveEffectByType<T>(int actorID) where T : EffectSpec_Base
        {
            var effect = GetAttachedEffect<T>(actorID);
            if (effect is null)
            {
                Tools.Logger.Warning($"Component_Impact.PrepareRemoveEffectByType()--->effect is null,actorID:{actorID},type:{true.GetType()}");
                return;
            }
            
            PrepareRemoveEffect(effect);
        }

        /// <summary>
        /// 获取附加在某actor上的某类型effect，拿不到返回空
        /// </summary>
        public EffectSpec_Base GetAttachedEffect<T>( int actorID ) where T : EffectSpec_Base
        {
            var effectArr = GetAttachedEffect( actorID );
            if ( effectArr is null || effectArr.Count == 0 )
                return null;

            foreach ( var effect in effectArr )
            {
                if ( effect is T )
                    return effect as T;
            }
            return null;
        }
        
        /// <summary>
        /// 获取附加在一个actor上的所有effect
        /// </summary>
        public IReadOnlyCollection<EffectSpec_Base> GetAttachedEffect( int actorID )
        {
            var indexList = GetMapIndex( actorID );
            if ( indexList is null || indexList.Count == 0 )
            {
                Tools.Logger.Info( $"<color=white>Component_Impact.GetAttachedEffect--->idList is null || idList.Count == 0,id{actorID}</color>" );
                return null;
            }
            
            _cachedEffectResultList.Clear();
            foreach ( var index in indexList )
                _cachedEffectResultList.Add(GetEffect( index ));
        
            return _cachedEffectResultList.AsReadOnly();
        }

        /// <summary>
        /// 将一个effect添加为impact
        /// </summary>
        public void Attach( EffectSpec_Base newEffect, int castorActorID, int targetActorID )
        {
            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            var castor = actorMgr?.Get(castorActorID);
            var target = actorMgr?.Get(targetActorID);
            var allowSameType = newEffect is EffectSpec_Period_ActorTag;
            var existEffect = allowSameType ? null : GetEffectByID( targetActorID, newEffect.GetType() );
            //普通Effect保持原有叠层/覆盖行为；ActorTag Effect由各自实例独立持有来源。
            if ( existEffect != null )
            {
                ref var impactData = ref _pool.Get( existEffect._impactEntityIndex );
                if ( impactData._resetDurationWhenOverride )
                    impactData._elapsed = 0f;

                if ( impactData._stackCount < impactData._stackLimit )
                    impactData._stackCount++;

                ReferencePool.Release( newEffect );
                return;
            }

            var entity = NewImpactEntity();
            ref var newImpactData = ref _pool.Add( entity );
            InitImpactData( ref newImpactData, newEffect, castorActorID, targetActorID, entity );
            newEffect._impactEntityIndex = entity;
            _curr.Add( entity );
            AddEffect( entity, newEffect );
            AddMapIndex( targetActorID, entity );

            if ( newImpactData._effectOnAwake )
                newEffect.OnEffectAwake(castor, target);
        }


        //----------------------- priv -----------------------

        /// <summary>
        /// 拿到指定actor身上指定的effect，拿不到返回null
        /// </summary>
        private EffectSpec_Base GetEffectByID( int targetID, Type type )
        {
            var effectArr = GetAttachedEffect( targetID );
            if ( effectArr is null || effectArr.Count == 0 )
                return null;

            foreach ( var effect in effectArr )
            {
                if ( effect.GetType() == type )
                    return effect;
            }
            return null;
        }

        /// <summary>
        /// 轮询处理impact数据
        /// </summary>
        private void ImpactDataSystem()
        {
            EffectSpec_Base tempEffect = null;
            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            _isUpdating = true;
            foreach ( var entity in _curr )
            {
                // Actor隐藏或主动清理可能已经标记了该实体。
                if ( _invalidEntitySet.Contains( entity ) )
                    continue;

                ref ImpactData impactData = ref _pool.Get( entity );
                impactData._elapsed += Time.deltaTime;
                impactData._interval += Time.deltaTime;
                if ( impactData._elapsed >= impactData._duration && impactData._policy != DurationPolicy.Infinite )
                {
                    MarkInvalid( entity );
                    continue;
                }

                if ( impactData._interval >= impactData._period )
                {
                    tempEffect = GetEffect( entity );
                    if ( tempEffect is null )
                    {
                        Tools.Logger.Warning( $"<color=yellow>Component_Impact.Update()--->effectSpec is null ,index:{entity}</color>" );
                        continue;
                    }

                    tempEffect.StackCount = impactData._stackCount;
                    var castor = actorMgr.Get( impactData._castorActorID );
                    var target = actorMgr.Get( impactData._targetActorID );
                    tempEffect.Apply( castor, target );
                    impactData._interval = 0f;
                }

                _next.Add( entity );
            }

            _isUpdating = false;
            FlushInvalid();
            _curr.Clear();

            _tempBuffer = _curr;
            _curr = _next;
            _next = _tempBuffer;
        }

        /// <summary>
        /// 清理与指定Actor相关的全部Impact。
        /// </summary>
        public void ClearEffectsForActor( int actorID )
        {
            MarkActorEffectsInvalid( _curr, actorID );
            MarkActorEffectsInvalid( _next, actorID );

            // 已标记集合中的实体已经进入统一回收队列，不重复添加。
            if ( !_isUpdating )
                FlushInvalid();
        }

        private void MarkActorEffectsInvalid( List<int> entities, int actorID )
        {
            foreach ( var entity in entities )
            {
                if ( !_allEffectDic.ContainsKey( entity ) )
                    continue;

                ref var impactData = ref _pool.Get( entity );
                if ( impactData._castorActorID == actorID || impactData._targetActorID == actorID )
                    MarkInvalid( entity );
            }
        }

        private void MarkInvalid( int entity )
        {
            if ( _allEffectDic.ContainsKey( entity ) )
                _invalidEntitySet.Add( entity );
        }

        private void FlushInvalid()
        {
            while ( _invalidEntitySet.Count > 0 )
            {
                var entity = 0;
                foreach ( var invalidEntity in _invalidEntitySet )
                {
                    entity = invalidEntity;
                    break;
                }

                _invalidEntitySet.Remove( entity );
                RemoveEntityFromActiveLists( entity );
                RemoveImpactAndEffect( entity );
            }
        }

        private void RemoveEntityFromActiveLists( int entity )
        {
            _curr.Remove( entity );
            _next.Remove( entity );
        }

        /// <summary>
        /// 移除impact和effect数据
        /// </summary>
        private void RemoveImpactAndEffect( int entity )
        {
            if ( !_allEffectDic.TryGetValue( entity, out var effect ) )
                return;

            var impactData = _pool.Get( entity );
            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            var castor = actorMgr?.Get( impactData._castorActorID );
            var target = actorMgr?.Get( impactData._targetActorID );
            effect.OnEffectEnd( castor, target );
            ReferencePool.Release( effect );

            RemoveEffect( entity );
            RemoveMapIndex( impactData._targetActorID, entity );
            RecycleImpactEntity( entity );
            _pool.Recycle( entity );
        }

        /// <summary>
        /// 初始化一个impact数据
        /// </summary>
        private void InitImpactData( ref ImpactData impactData, EffectSpec_Base effect, int castorActorID, int targetActorID, int entityIndex )
        {
            impactData._castorActorID             = castorActorID;
            impactData._targetActorID             = targetActorID;
            impactData._duration                  = effect.Meta.GetDuration();
            impactData._entityIndex               = entityIndex;
            impactData._effectOnAwake             = effect.Meta.GetEffectOnAwake();
            impactData._period                    = effect.Meta.GetPeriod();
            impactData._policy                    = effect.Meta.GetPolicy();
            impactData._elapsed                   = 0f;
            impactData._interval                  = 0f;
            impactData._stackCount                = effect.StackCount;
            impactData._stackLimit                = effect.StackLimit;
            impactData._resetDurationWhenOverride = effect.ResetWhenOverride;
        }
        /// <summary>
        /// 返回一个新的impact实体
        /// </summary>
        private int NewImpactEntity()
        {
            if ( _recycleImpactEntityCount > 0 )
                return _recycleImpactEntityArr[--_recycleImpactEntityCount];

            if ( _impactEntityCount == _impactEntityArr.Length )
                Array.Resize( ref _impactEntityArr, _impactEntityArr.Length << 1 );

            return _impactEntityCount++;
        }

        /// <summary>
        /// 回收impact实体
        /// </summary>
        private void RecycleImpactEntity( int entity )
        {
            if ( _recycleImpactEntityCount == _recycleImpactEntityArr.Length )
                Array.Resize( ref _recycleImpactEntityArr, _recycleImpactEntityArr.Length << 1 );

            _recycleImpactEntityArr[_recycleImpactEntityCount++] = entity;
        }


        /// <summary>
        /// 移除出effect存储集合
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private bool RemoveEffect( int entityIndex )
        {
            return _allEffectDic.Remove( entityIndex );
        }

        /// <summary>
        /// 添加到effect存储集合
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private void AddEffect( int entityIndex, EffectSpec_Base effect )
        {
            _allEffectDic.Add( entityIndex, effect );
        }

        /// <summary>
        /// 获取一个effect实例
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private EffectSpec_Base GetEffect( int entityIndex )
        {
            if ( _allEffectDic.TryGetValue( entityIndex, out var effectSpec ) )
                return effectSpec;

            return null;
        }

        /// <summary>
        /// 获取actor持有的所有impact索引
        /// </summary>
        private LinkedList<int> GetMapIndex( int targetID )
        {
            if ( !_targetImpactDataMapDic.TryGetValue( targetID, out var list ) )
                return null;

            return list;
        }

        /// <summary>
        /// 添加映射索引
        /// </summary>
        private void AddMapIndex( int targetID, int effectIndex )
        {
            if ( !_targetImpactDataMapDic.TryGetValue( targetID, out var list ) )
            {
                list = new LinkedList<int>();
                _targetImpactDataMapDic.Add( targetID, list );
            }

            list.AddLast( effectIndex );
        }

        /// <summary>
        /// 移除映射索引
        /// </summary>
        private bool RemoveMapIndex( int targetID, int effectIndex )
        {
            if ( _targetImpactDataMapDic.TryGetValue( targetID, out var list ) )
                return list.Remove( effectIndex );

            return false;
        }

        /// <summary>
        /// buff&debuff轮询
        /// </summary>
        private void Update()
        {
            ImpactDataSystem();
        }
        
        private void Start()
        {
            EnsureInit();
        }
        
        private void EnsureInit()
        {
            _allEffectDic           = new Dictionary<int, EffectSpec_Base>( _defaultCacheCapcity );
            _targetImpactDataMapDic = new Dictionary<int, LinkedList<int>>( _defaultCacheCapcity );
            _impactEntityArr        = new int[_defaultEntityCount];
            _recycleImpactEntityArr = new int[_defaultEntityCount];
            _impactEntityCount = 0;
            _recycleImpactEntityCount = 0;
            _pool    = new ImpactDataPool( _defaultEntityCount );
            _curr    = new List<int>( _defaultEntityCount / 2 );
            _next    = new List<int>( _defaultEntityCount / 2 );
            _invalidEntitySet = new HashSet<int>();
            _cachedEffectResultList = new List<EffectSpec_Base>();
        }

        //----------------------- fields -----------------------
        private ImpactDataPool _pool;

        /// <summary>
        /// 存储的effect实例集合，k=impact实体索引。
        /// </summary>
        private Dictionary<int, EffectSpec_Base> _allEffectDic;

        /// <summary>
        /// impact数据和附加对象的映射集合,k=targetID,v=impact实体索引。
        /// </summary>
        private Dictionary<int, LinkedList<int>> _targetImpactDataMapDic;

        private List<int> _curr;
        private List<int> _next;
        private HashSet<int> _invalidEntitySet;
        private List<int> _tempBuffer;
        private bool _isUpdating;

        [SerializeField] private int _defaultCacheCapcity = 0x10;
        private int _impactEntityCount;
        [SerializeField] private int _defaultEntityCount = 0x40;
        private int[] _impactEntityArr;
        private int _recycleImpactEntityCount;
        private int[] _recycleImpactEntityArr;
        private List<EffectSpec_Base> _cachedEffectResultList;
    }
}
