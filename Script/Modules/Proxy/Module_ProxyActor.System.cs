using Aquila.Extension;
using Aquila.Fight.Addon;
using GameFramework;
using System.Collections.Generic;
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
        private void AddToAddonSystem( Addon_Base addon )
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
        private void RemoveFromAddonSystem( Addon_Base addon )
        {
            if ( !_existAddon.Contains( addon.GetHashCode() ) )
                return;

            //待移除列表
            _readyToRemove.Enqueue( addon );
        }

        private void SystemUpdate( float elapsed, float realElapsed )
        {
            ProcessAdd();
            ProcessRemove();
            foreach ( var pool in _containerList )
                pool.Update( elapsed, realElapsed );
        }

        /// <summary>
        /// 处理要添加的addon
        /// </summary>
        private void ProcessAdd()
        {
            var curr = _readyToAdd.Dequeue();
            var intType = 0;
            while ( curr != null )
            {
                intType = ( int ) curr.AddonType;
                //add
                _containerList[intType].Add( curr );
                _existAddon.Add( curr.GetHashCode() );

                curr = _readyToAdd.Dequeue();
            }
        }

        /// <summary>
        /// 处理要移除的
        /// </summary>
        private void ProcessRemove()
        {
            var curr = _readyToRemove.Dequeue();
            var intType = 0;
            while ( curr != null )
            {
                intType = ( int ) curr.AddonType;
                //add
                _containerList[intType].Remove( curr );
                _existAddon.Remove( curr.GetHashCode() );

                curr = _readyToAdd.Dequeue();
            }
        }

        private void OnSystemOpen()
        {
            //_containerList = new List<AddonContainer>( ( int ) AddonTypeEnum.Max - 1 );
            //var cnt = _containerList.Count;
            //for ( var i = 0; i < cnt; i++ )
            //    _containerList[i] = ReferencePool.Acquire<AddonContainer>();

            _readyToAdd    = new Queue<Addon_Base>();
            _readyToRemove = new Queue<Addon_Base>();
            _existAddon    = new HashSet<int>();
            _containerList = new AddonContainer[( int ) AddonTypeEnum.Max - 1];
            var len = _containerList.Length;
            for ( var i = 0; i < len; i++ )
                _containerList[i] = ReferencePool.Acquire<AddonContainer>();
        }

        private void OnSystemClose()
        {
            foreach ( var pool in _containerList )
                ReferencePool.Release( pool );

            //_containerList.Clear();
            _containerList = null;

            _readyToAdd.Clear();
            _readyToAdd = null;

            _readyToRemove.Clear();
            _readyToRemove = null;
        }

        //------------------- fields -------------------
        /// <summary>
        /// 组件容器列表
        /// </summary>
        private AddonContainer[] _containerList = null;

        /// <summary>
        /// 待添加
        /// </summary>
        private Queue<Addon_Base> _readyToAdd = null;

        /// <summary>
        /// 待移除
        /// </summary>
        private Queue<Addon_Base> _readyToRemove = null;

        /// <summary>
        /// 保存已添加的addon hashcode
        /// </summary>
        private HashSet<int> _existAddon = null;

        /// <summary>
        /// Addon池，保存不同类型的addon
        /// </summary>
        private class AddonContainer : IReference
        {
            public void Add( Addon_Base addon )
            {
                _curr.Add( addon );
                //_currStack.Push( addon );
            }

            public void Remove( Addon_Base addon )
            {
                _toRemove.Add( addon );
            }

            public void Update( float elapsed, float realElapsed )
            {
                //_nextStack.Clear();
                //var currAddon = _currStack.Pop();
                //while ( currAddon != null )
                //{
                //    if ( _toRemove.Contains( currAddon ) )
                //        continue;
                //}
                //_nextStack.Push( currAddon );
                //currAddon.OnUpdate( elapsed, realElapsed );

                //_currStack.Clear();
                //_currStack = _currStack == _tempStack_1 ? _tempStack_2 : _tempStack_1;
                //_nextStack = _nextStack == _tempStack_1 ? _tempStack_2 : _tempStack_1;

                foreach ( var addon in _curr )
                {
                    if ( _toRemove.Contains( addon ) )
                        continue;

                    addon.OnUpdate( elapsed, realElapsed );
                    _next.Add( addon );
                }

                _temp = _curr;
                _temp.Clear();
                _curr = _next;
                _next = _temp;

                _toRemove.Clear();
            }

            public AddonContainer()
            {
                ////_curr = new List<Addon_Base>( 64 );
                ////_next = new List<Addon_Base>();
                ////_temp = new List<Addon_Base>();

                //_tempStack_1 = new Stack<Addon_Base>();
                //_tempStack_2 = new Stack<Addon_Base>();
                //_currStack = _tempStack_1;
                //_nextStack = _tempStack_2;

                _toRemove = new HashSet<Addon_Base>();
            }

            public void Clear()
            {
                _curr.Clear();
                _curr = null;
                _next.Clear();
                _next = null;
                _temp = null;

                //_currStack = null;
                //_nextStack = null;
                //_tempStack_1.Clear();
                //_tempStack_2.Clear();
                //_tempStack_1 = null;
                //_tempStack_2 = null;

                _toRemove.Clear();
                _toRemove = null;
            }

            /// <summary>
            /// addon列表
            /// </summary>
            private List<Addon_Base> _curr = null;
            private List<Addon_Base> _next = null;
            private List<Addon_Base> _temp = null;

            //private Stack<Addon_Base> _currStack = null;
            //private Stack<Addon_Base> _nextStack = null;
            //private Stack<Addon_Base> _tempStack_1 = null;
            //private Stack<Addon_Base> _tempStack_2 = null;

            //#todo考虑是否不用hashset保存，是否有更好的剔除思路
            /// <summary>
            /// 要移除的组件
            /// </summary>
            private HashSet<Addon_Base> _toRemove = null;
        }
    }
}
