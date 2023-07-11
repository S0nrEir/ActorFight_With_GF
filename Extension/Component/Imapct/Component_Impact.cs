using Aquila.Module;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Impact
{
    /// <summary>
    /// 角色的主动和被动效果组件
    /// </summary>
    public partial class Component_Impact : GameFrameworkComponent
    {
        //----------------------- pub -----------------------

        /// <summary>
        /// 获取附加在某actor上的某类型effect，拿不到返回空
        /// </summary>
        public EffectSpec_Base GetAttachedEffect<T>( int actorID ) where T : EffectSpec_Base
        {
            var effectArr = GetAttachedEffect( actorID );
            if ( effectArr is null || effectArr.Length == 0 )
                return null;

            foreach ( var effect in effectArr )
            {
                if ( effect is T )
                    return effect as T;
            }
            return null;
        }

        /// <summary>
        /// 获取附加在某actor上的effect实例集合，拿不到返回一个空集合
        /// </summary>
        public EffectSpec_Base[] GetAttachedEffect( int actorID )
        {
            var indexList = GetMapIndex( actorID );
            if ( indexList is null || indexList.Count == 0 )
            {
                //Log.Info( $"<color=white>Component_Impact.GetAttachedEffect--->idList is null || idList.Count == 0,id{actorID}</color>" );
                //#todo这里不要用数组
                return new EffectSpec_Base[0];
            }

            var result = new EffectSpec_Base[indexList.Count];
            var i = 0;
            foreach ( var index in indexList )
                result[i++] = GetEffect( index );

            return result;
        }

        /// <summary>
        /// 将一个effect添加为impact
        /// </summary>
        public void Attach( EffectSpec_Base newEffect, int castorActorID, int targetActorID )
        {
            var existEffect = GetEffectByID( targetActorID, newEffect.GetType() );
            //已经有了，叠加层数
            if ( existEffect != null )
            {
                //拿相应的impactData
                ref var impactData = ref _pool.Get( existEffect._impactEntityIndex );
                //叠加层数是否重制持续时间？
                if ( impactData._resetDurationWhenOverride )
                    impactData._elapsed = 0f;

                //叠加层数
                if ( impactData._stackCount < impactData._stackLimit )
                    impactData._stackCount++;

                //ReferencePool.Release( newEffect );
                GameEntry.Module.GetModule<Module_ProxyActor>().InvalidEffect( castorActorID, targetActorID, newEffect, false );
            }
            //没有，添加新的
            else
            {
                //new entity
                var entity = NewImpactEntity();
                var key = newEffect.GetHashCode();
                if ( _effectDic.ContainsKey( key ) )
                {
                    Log.Warning( $"<color=yellow>Component_Impact.Attach()--->already have key:{key}</color>" );
                    //ReferencePool.Release( newEffect );
                    GameEntry.Module.GetModule<Module_ProxyActor>().InvalidEffect( castorActorID, targetActorID, newEffect, false );
                    return;
                }

                ref var impactData = ref _pool.Add( entity );
                InitImpactData( ref impactData, newEffect, castorActorID, targetActorID, key );
                newEffect._impactEntityIndex = entity;
                _curr.Add( entity );
                AddEffect( key, newEffect );
                AddMapIndex( targetActorID, impactData._effectIndex );

                if ( impactData._effectOnAwake )
                    GameEntry.Module.GetModule<Module_ProxyActor>().ImplAwakeEffect( impactData._castorActorID, impactData._targetActorID, newEffect );

                //唤起一次性的effect
                if ( newEffect.Meta.AwakeEffects.Length != 0 )
                {
                    GameEntry.Module.GetModule<Module_ProxyActor>().ImplAwakeEffect( impactData._castorActorID, impactData._targetActorID, newEffect );
                }
            }
        }

        //----------------------- priv -----------------------

        /// <summary>
        /// 拿到指定actor身上指定的effect，拿不到返回null
        /// </summary>
        private EffectSpec_Base GetEffectByID( int targetID, Type type )
        {
            var effectArr = GetAttachedEffect( targetID );
            if ( effectArr is null || effectArr.Length == 0 )
                return null;

            foreach ( var effect in effectArr )
            {
                if ( effect.GetType() == type )
                    return effect;
            }
            return null;
        }

        /// <summary>
        /// 是否已经有相同effect
        /// </summary>
        private bool HasSame( int targetID, Type type )
        {
            var effectArr = GetAttachedEffect( targetID );
            if ( effectArr is null || effectArr.Length == 0 )
                return false;

            foreach ( var effect in effectArr )
            {
                if ( effect.GetType() == type )
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 轮询处理impact数据
        /// </summary>
        private void ImpactDataSystem()
        {
            EffectSpec_Base tempEffect = null;
            foreach ( var entity in _curr )
            {
                //get
                ref ImpactData impactData = ref _pool.Get( entity );
                impactData._elapsed += Time.deltaTime;
                impactData._interval += Time.deltaTime;
                //impact时间到了，而且不是永久性的imapct
                if ( impactData._elapsed >= impactData._duration && impactData._policy != Cfg.Enum.DurationPolicy.Infinite )
                {
                    _invalid.Add( entity );
                    continue;
                }

                //生效
                if ( impactData._interval >= impactData._period )
                {
                    tempEffect = GetEffect( impactData._effectIndex );
                    if ( tempEffect is null )
                    {
                        Log.Warning( $"<color=yellow>Component_Impact.Update()--->effectSpec is null ,index:{impactData._effectIndex}</color>" );
                        continue;
                    }
                    tempEffect.StackCount = impactData._stackCount;
                    GameEntry.Module.GetModule<Module_ProxyActor>().ImplEffect( impactData._castorActorID, impactData._targetActorID, tempEffect );

                    impactData._interval = 0f;
                }
                //仍然有效的impact添加进去
                //因为这里curr是无效的impact和有效的impact集合
                //不希望在curr里做removeAt这样的操作
                _next.Add( entity );
            }//end foreach

            //清掉无效或者已经过期的imapct
            foreach ( var entity in _invalid )
            {
                var impactData = _pool.Get( entity );
                tempEffect = GetEffect( impactData._effectIndex );
                GameEntry.Module.GetModule<Module_ProxyActor>().InvalidEffect( impactData._castorActorID, impactData._targetActorID, tempEffect, true );
                //tempEffect.OnEffectEnd();
                //ReferencePool.Release( tempEffect );
                RemoveEffect( impactData._effectIndex );
                RemoveMapIndex( impactData._targetActorID, impactData._effectIndex );
                RecycleImpactEntity( entity );
                _pool.Recycle( entity );
            }//end foreach
            _invalid.Clear();
            _curr.Clear();

            //交换entity实例缓存
            _tempBuffer = _curr;
            _curr = _next;
            _next = _tempBuffer;
        }

        /// <summary>
        /// 初始化一个impact数据
        /// </summary>
        private void InitImpactData( ref ImpactData impactData, EffectSpec_Base effect, int castorActorID, int targetActorID, int key )
        {
            impactData._castorActorID = castorActorID;
            impactData._targetActorID = targetActorID;
            impactData._duration = effect.Meta.Duration;
            impactData._effectIndex = key;
            impactData._effectOnAwake = effect.Meta.EffectOnAwake;
            impactData._period = effect.Meta.Period;
            impactData._policy = effect.Meta.Policy;
            impactData._elapsed = 0f;
            impactData._interval = 0f;
            impactData._stackCount = effect.StackCount;
            impactData._stackLimit = effect.StackLimit;
            impactData._resetDurationWhenOverride = effect.ResetWhenOverride;
        }

        /// <summary>
        /// 通过一个effect实例修改impact数据
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private void ModifyImpactDataByEffect( ref ImpactData data, EffectSpec_Base effect )
        {

        }

        /// <summary>
        /// 返回一个新的impact实体
        /// </summary>
        private int NewImpactEntity()
        {
            var entity = 0;
            //回收池里有，先从回收池拿
            if ( _recycleImpactEntityCount > 0 )
            {
                entity = _recycleImpactEntityArr[--_recycleImpactEntityCount];
                return entity;
            }

            if ( _impactEntityCount == _impactEntityArr.Length )
                Array.Resize( ref _impactEntityArr, _impactEntityArr.Length << 1 );

            entity = _impactEntityCount++;
            return entity;
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
        private bool RemoveEffect( int key )
        {
            return _effectDic.Remove( key );
        }

        /// <summary>
        /// 添加到effect存储集合
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private void AddEffect( int key, EffectSpec_Base effect )
        {
            _effectDic.Add( key, effect );
        }

        /// <summary>
        /// 获取一个effect实例
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private EffectSpec_Base GetEffect( int key )
        {
            if ( _effectDic.TryGetValue( key, out var effectSpec ) )
                return effectSpec;

            return null;
        }

        /// <summary>
        /// 获取映射索引
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
                list = new LinkedList<int>();

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
            _effectDic = new Dictionary<int, EffectSpec_Base>( _defaultCacheCapcity );
            _targetImpactDataMapDic = new Dictionary<int, LinkedList<int>>( _defaultCacheCapcity );
            _impactEntityArr = new int[_defaultEntityCount];
            _recycleImpactEntityArr = new int[_defaultEntityCount];
            _impactEntityCount = 0;
            _recycleImpactEntityCount = 0;
            _pool = new ImpactDataPool( _defaultEntityCount );
            _curr = new List<int>( _defaultEntityCount / 2 );
            _next = new List<int>( _defaultEntityCount / 2 );
            _invalid = new List<int>( _defaultEntityCount / 2 );
        }

        //----------------------- fields -----------------------
        private ImpactDataPool _pool = null;

        /// <summary>
        /// 存储的effect实例集合,k=impactIndex
        /// </summary>
        private Dictionary<int, EffectSpec_Base> _effectDic = null;

        /// <summary>
        /// impact数据和附加对象的映射集合,k=targetID,v=effectIndex
        /// </summary>
        private Dictionary<int, LinkedList<int>> _targetImpactDataMapDic = null;

        private List<int> _curr = null;
        private List<int> _next = null;
        private List<int> _invalid = null;
        private List<int> _tempBuffer = null;

        /// <summary>
        /// 默认的effect缓存容量
        /// </summary>
        [SerializeField] private int _defaultCacheCapcity = 0x10;

        /// <summary>
        /// impact实体数量
        /// </summary>
        private int _impactEntityCount = 0;

        /// <summary>
        /// impact实体默认容量
        /// </summary>
        [SerializeField] private int _defaultEntityCount = 0x40;

        /// <summary>
        /// impact实体集合
        /// </summary>
        private int[] _impactEntityArr = null;

        /// <summary>
        /// 回收池的impact实体对象
        /// </summary>
        private int _recycleImpactEntityCount = 0;

        /// <summary>
        /// impact实体回收池
        /// </summary>
        private int[] _recycleImpactEntityArr = null;
    }
}
