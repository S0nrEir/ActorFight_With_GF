using Aquila.Extension;
using Aquila.Fight.Addon;
using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using static Aquila.Fight.Addon.Addon_Base;

namespace Aquila.Module
{
    /// <summary>
    /// actor代理的逻辑类，轮询处理所有actor的update
    /// </summary>
    public partial class Module_ProxyActor : GameFrameworkModuleBase
    {
        /// <summary>
        /// 添加组件到轮询系统中
        /// </summary>
        public void AddToAddonSystem( Addon_Base addon )
        {
            if ( _existAddon.Contains( addon.GetHashCode() ) )
            {
                Log.Warning( $"Module_ProxyActor.System.Add()--->_existAddon.Contains( hashCode )" );
                return;
            }

            //待添加列表
            _readyToAdd.Enqueue( addon );
        }

        /// <summary>
        /// 从轮询系统中移除组件
        /// </summary>
        public void RemoveFromAddonSystem( Addon_Base addon )
        {
            if ( !_existAddon.Contains( addon.GetHashCode() ) )
                return;

            //待移除列表
            _readyToRemove.Enqueue( addon );
        }

        public void SystemEnsureInit()
        {
            _readyToAdd    = new Queue<Addon_Base>();
            _readyToRemove = new Queue<Addon_Base>();
            _existAddon    = new HashSet<int>();
            _containerList = new AddonContainer[( int ) AddonTypeEnum.Max];
            var len    = _containerList.Length;
            for ( var i = 0; i < len; i++ )
                _containerList[i] = ReferencePool.Acquire<AddonContainer>();
        }

        private void SystemUpdate( float elapsed, float realElapsed )
        {
            ProcessRemoveAddon();
            ProcessAddAddon();
            for ( var i = 0; i < _containerList.Length; i++ )
                _containerList[i].Update( elapsed, realElapsed );
        }

        /// <summary>
        /// 处理要添加的addon
        /// </summary>
        private void ProcessAddAddon()
        {
            Addon_Base curr = null;
            while ( _readyToAdd.TryDequeue( out curr ) )
            {
                _containerList[( int ) curr.AddonType].Add( curr );
                _existAddon.Add( curr.GetHashCode() );
            }
        }

        /// <summary>
        /// 处理要移除的
        /// </summary>
        private void ProcessRemoveAddon()
        {
            Addon_Base curr = null;
            while ( _readyToRemove.TryDequeue( out curr ) )
            {
                _containerList[( int ) curr.AddonType].Remove( curr );
                _existAddon.Remove( curr.GetHashCode() );
            }
        }

        private void SystemOpen()
        {
            //_containerList = new List<AddonContainer>( ( int ) AddonTypeEnum.Max - 1 );
            //var cnt = _containerList.Count;
            //for ( var i = 0; i < cnt; i++ )
            //    _containerList[i] = ReferencePool.Acquire<AddonContainer>();

            //_readyToAdd    = new Queue<Addon_Base>();
            //_readyToRemove = new Queue<Addon_Base>();
            //_existAddon    = new HashSet<int>();
            //_containerList = new AddonContainer[( int ) AddonTypeEnum.Max - 1];
            //var len = _containerList.Length;
            //for ( var i = 0; i < len; i++ )
            //    _containerList[i] = ReferencePool.Acquire<AddonContainer>();
        }

        private void OnSystemClose()
        {
            return;

#pragma warning disable CS0162 // 检测到无法访问的代码
            foreach ( var pool in _containerList )
                ReferencePool.Release( pool );

            //_containerList.Clear();
            _containerList = null;

            _readyToAdd.Clear();
            _readyToAdd = null;

            _readyToRemove.Clear();
            _readyToRemove = null;

            _existAddon.Clear();
            _existAddon = null;
#pragma warning restore CS0162 // 检测到无法访问的代码
        }

        //------------------- fields -------------------
        
        /// <summary>
        /// 组件容器列表
        /// </summary>
        private AddonContainer[] _containerList;

        /// <summary>
        /// 待添加
        /// </summary>
        private Queue<Addon_Base> _readyToAdd;

        /// <summary>
        /// 待移除
        /// </summary>
        private Queue<Addon_Base> _readyToRemove;

        /// <summary>
        /// 保存已添加的addon hashcode
        /// </summary>
        private HashSet<int> _existAddon;

        /// <summary>
        /// Addon池，保存不同类型的addon
        /// </summary>
        private class AddonContainer : IReference
        {
            public void Add( Addon_Base addon )
            {
                _curr.Add( addon );
            }

            public void Remove( Addon_Base addon )
            {
                _toRemove.Add( addon );
            }

            public void Update( float elapsed, float realElapsed )
            {
                //attention:这里的调用是发生在下一帧的
                //question:考虑给addon加上释放标记？
                var cnt = _curr.Count;
                for (var i = 0; i < cnt; i++)
                {
                    if (_toRemove.Contains(_curr[i]))
                    {
                        _curr[i].Dispose();
                        _toRemove.Remove(_curr[i]);
                        continue;
                    }
                    
                    _curr[i].OnUpdate(elapsed,realElapsed);
                    _next.Add(_curr[i]);
                }

                SwapCurrAndNext();
            }

            public AddonContainer()
            {
                _curr = new List<Addon_Base>( 64 );
                _next = new List<Addon_Base>( 64 );

                _toRemove = new HashSet<Addon_Base>();
            }

            public void Clear()
            {
                _curr.Clear();
                _curr = null;
                _next.Clear();
                _next = null;
                _temp = null;

                _toRemove.Clear();
                _toRemove = null;
            }

            /// <summary>
            /// 交换curr和next
            /// </summary>
            private void SwapCurrAndNext()
            {
                _temp = _curr;
                _temp.Clear();
                _curr = _next;
                _next = _temp;
            }

            /// <summary>
            /// addon列表
            /// </summary>
            private List<Addon_Base> _curr = null;
            private List<Addon_Base> _next = null;
            private List<Addon_Base> _temp = null;

            //todo:考虑是否不用hashset保存，是否有更好的剔除思路，如果不用的话，考虑使用addon的释放标记
            /// <summary>
            /// 要移除的组件
            /// </summary>
            private HashSet<Addon_Base> _toRemove = null;
        }
    }
}
