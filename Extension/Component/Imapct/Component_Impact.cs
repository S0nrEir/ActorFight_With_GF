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

        //public void Recycle(int entity)
        //{
        //    RecycleImpactEntity( entity );
        //}

        //public int NewEntity()
        //{
        //    return NewImpactEntity();
        //}

        /// <summary>
        /// 将一个effect添加为impact
        /// </summary>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public void Attach( EffectSpec_Base effect )
        {
            //new entity
            var entity = NewImpactEntity();
            var key = effect.GetHashCode();
            if ( _effectDic.ContainsKey( key ) )
            {
                Log.Warning( $"<color=yellow>Component_Impact.Attach()--->already have key:{key}</color>" );
                return;
            }

            if ( _attachedEntitySet.Contains( entity ) )
            {
                Log.Warning( $"<color=yellow>Component_Impact.Attach()--->already have key:{key}</color>" );
                return;
            }

            ImpactData impactData = default;


            _attachedEntitySet.Add( entity );
            AddEffect( key, effect );
        }
        //----------------------- priv -----------------------

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

            //entity = _impactEntityArr[_impactEntityCount++];
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
        private bool Remove( int key )
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
        /// buff&debuff轮询
        /// </summary>
        private void Update()
        {
            foreach ( var entity in _attachedEntitySet )
            {
                
            }
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
            _attachedEntitySet        = new HashSet<int>();
        }

        //----------------------- fields -----------------------

        private HashSet<int> _attachedEntitySet = null;

        private ImpactDataPool _pool = null;

        /// <summary>
        /// 存储的effect实例集合
        /// </summary>
        private Dictionary<int, EffectSpec_Base> _effectDic = null;

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
