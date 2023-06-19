using Aquila.Event;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Module.Module_ProxyActor;

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
        public void Attach( EffectSpec_Base effect )
        {
            //if ( effect is null )
            //{
            //    Log.Warning( "Component_Impact.Attach()--->effect is null" );
            //    return;
            //}
            var key = effect.GetHashCode();
            if ( _effectDic.ContainsKey( key ) )
            {
                Log.Warning( $"Component_Impact.Attach()--->already have key:{key}" );
                return;
            }

            Add( key, effect );
        }

        //----------------------- priv -----------------------

        //private void Apply( int index, ActorInstance instance, AbilityResult_Hit result )
        //{
        //    _effectDic[index].Apply( instance, result );
        //}

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
        private void Add( int key, EffectSpec_Base effect )
        {
            _effectDic.Add( key, effect );
        }

        /// <summary>
        /// buff&debuff轮询
        /// </summary>
        private void Update()
        {

        }

        private void Start()
        {
            EnsureInit();
        }

        private void EnsureInit()
        {
            _effectDic = new Dictionary<int, EffectSpec_Base>( _defaultCacheCapcity );
        }

        //----------------------- fields -----------------------
        /// <summary>
        /// 存储的effect实例集合
        /// </summary>
        private Dictionary<int, EffectSpec_Base> _effectDic = null;

        /// <summary>
        /// 默认的effect缓存容量
        /// </summary>
        [SerializeField] private int _defaultCacheCapcity = 16;
    }

}
