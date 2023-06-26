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
        /// 将一个effect添加为impact
        /// </summary>
        public void Attach( EffectSpec_Base effect, int castorActorID, int targetActorID )
        {
            //new entity
            var entity = NewImpactEntity();
            var key = effect.GetHashCode();
            if ( _effectDic.ContainsKey( key ) )
            {
                Log.Warning( $"<color=yellow>Component_Impact.Attach()--->already have key:{key}</color>" );
                return;
            }

            ref var impactData = ref _pool.Add( entity );
            InitImpactData( ref impactData, effect, castorActorID, targetActorID, key );
            _curr.Add( entity );
            AddEffect( key, effect );

            if ( impactData._effectOnAwake )
                GameEntry.Module.GetModule<Module_ProxyActor>().AffectImpact( impactData._castorActorID, impactData._targetActorID, effect );
        }

        //----------------------- priv -----------------------
        
        /// <summary>
        /// 轮询处理impact数据
        /// </summary>
        private void ImpactDataSystem()
        {
            EffectSpec_Base tempEffect = null;
            foreach ( var entity in _curr )
            {
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
                    GameEntry.Module.GetModule<Module_ProxyActor>().AffectImpact( impactData._castorActorID, impactData._targetActorID, tempEffect );

                    impactData._interval = 0f;
                }
                _next.Add( entity );
            }//end foreach

            //清掉无效或者已经过期的imapct
            foreach ( var entity in _invalid )
            {
                var impactData = _pool.Get( entity );
                tempEffect = GetEffect( impactData._effectIndex );
                tempEffect.OnDestroy();
                RemoveEffect( impactData._effectIndex );
                RecycleImpactEntity( entity );
                _pool.Remove( entity );
            }//end foreach
            _invalid.Clear();
            _curr.Clear();

            //交换entity实例缓存
            _tempBuffer = _curr;
            _curr       = _next;
            _next       = _tempBuffer;
        }

        /// <summary>
        /// 初始化一个impact数据
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
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
            _effectDic                = new Dictionary<int, EffectSpec_Base>( _defaultCacheCapcity );
            _impactEntityArr          = new int[_defaultEntityCount];
            _recycleImpactEntityArr   = new int[_defaultEntityCount];
            _impactEntityCount        = 0;
            _recycleImpactEntityCount = 0;
            _pool                     = new ImpactDataPool( _defaultEntityCount );
            _curr                     = new List<int>( _defaultEntityCount / 2 );
            _next                     = new List<int>( _defaultEntityCount / 2 );
            _invalid                  = new List<int>( _defaultEntityCount / 2 );
        }

        //----------------------- fields -----------------------
        private ImpactDataPool _pool = null;

        /// <summary>
        /// 存储的effect实例集合
        /// </summary>
        private Dictionary<int, EffectSpec_Base> _effectDic = null;

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
