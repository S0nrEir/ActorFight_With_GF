using Aquila.Module;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// actor的Event组件部分
    /// </summary>
    public class Addon_Event : Addon_Base
    {
        /// <summary>
        /// 通知
        /// </summary>
        public void Notify( int eventType, object param )
        {
            if ( !readyFlag )
            {
                Log.Error( "<color=red>Addon_Event.Notify--->!readyFlag </>" );
                return;
            }

            if ( !_eventDic.TryGetValue( eventType, out var callList ) )
            {
                //可能没有注册的addon，取消报错log
                //Log.Error( $"<color=red>Addon_Event.Notify()--->!_eventDic.TryGetValue( eventType, out var callList ),eventType:{eventType},enum:{( ( AddonEventTypeEnum ) eventType ).ToString()}</color>" );
                return;
            }

            //这里已经排好了
            foreach ( var item in callList )
                item.call( item.addonType, param );
        }

        /// <summary>
        /// 注册
        /// </summary>
        public Addon_Event Register( int eventType, int addonType, Action<int, object> call )
        {
            if ( eventType == ( int ) AddonTypeEnum.EVENT )
            {
                Log.Error( "<color=red>could not add event addon to event addon.</color>" );
                return this;
            }

            if ( !_eventDic.TryGetValue( eventType, out var list ) )
            {
                list = new List<(int addonType, Action<int, object> call)>( 0x10 );
                _eventDic.Add( eventType, list );
            }

            list.Add( (addonType, call) );
            //根据addon排个序，避免addon的执行顺序先后出错的问题
            //list.Sort();
            return this;
        }

        /// <summary>
        /// 注销
        /// </summary>
        public bool UnRegister( int eventType, Action<int, object> call )
        {
            if ( !_eventDic.TryGetValue( eventType, out var list ) )
                return false;

            var cnt = list.Count;
            for ( var i = 0; i < cnt; i++ )
            {
                if ( list[i].call == call )
                {
                    list.RemoveAt( i );
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 注销所有
        /// </summary>
        public void UnRegisterAll()
        {
            foreach ( var iter in _eventDic )
                iter.Value.Clear();

            readyFlag = false;
        }

        /// <summary>
        /// 就绪
        /// </summary>
        public void Ready()
        {
            List<(int, Action<int, object>)> list = null;
            foreach ( var iter in _eventDic )
            {
                list = iter.Value;
                list.Sort( ( x, y ) => x.Item1 <= y.Item1 ? -1 : 1 );
            }
            list = null;
            readyFlag = true;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private int Sort( int x, int y )
        {
            return x <= y ? -1 : 1;
        }

        public override void Init( Module_ProxyActor.ActorInstance instance )
        {
            base.Init( instance );
        }

        public override AddonTypeEnum AddonType => AddonTypeEnum.EVENT;

        public override void OnAdd()
        {
            _eventDic = new Dictionary<int, List<(int addonType, Action<int, object>)>>();
        }

        public override void Reset()
        {
            base.Reset();
        }

        public override void Dispose()
        {
            _eventDic?.Clear();
            _eventDic = null;
            base.Dispose();
        }

        /// <summary>
        /// 准备完成标记
        /// </summary>
        private bool readyFlag = false;

        /// <summary>
        /// 事件集 k=事件类型，action<int,object>:int=组件类型，object=参数
        /// </summary>
        private Dictionary<int, List<(int addonType, Action<int, object> call)>> _eventDic = null;

        #region nouse
        //----------------------- public-----------------------

        /// <summary>
        /// 发送一个event
        /// </summary>
        //public void Trigger( ActorEventEnum type, object[] param )
        //{
        //    var int_type = ( int ) type;
        //    if ( !_eventDic.TryGetValue( int_type, out var action ) )
        //        return;

        //    action?.Invoke( int_type, param );
        //}

        ///// <summary>
        ///// 添加
        ///// </summary>
        //public bool Register( int intType, Action<int, object[]> action )
        //{
        //    //不需要listener，因为listener就是actor自己
        //    if ( _eventDic.ContainsKey( intType ) )
        //        return false;

        //    _eventDic.Add( intType, action );
        //    return true;
        //}

        ///// <summary>
        ///// 添加
        ///// </summary>
        //public bool Register( ActorEventEnum eventType, Action<int, object[]> action )
        //{
        //    return Register( ( int ) eventType, action );
        //}

        ///// <summary>
        ///// 移除
        ///// </summary>
        //public bool UnRegister( ActorEventEnum eventType )
        //{
        //    return UnRegister( ( int ) eventType );
        //}

        ///// <summary>
        ///// 移除
        ///// </summary>
        //public bool UnRegister( int intType )
        //{
        //    return _eventDic.Remove( intType );
        //}

        ////-----------------------override-----------------------
        //public override AddonTypeEnum AddonType => AddonTypeEnum.EVENT;

        //public override void OnAdd()
        //{
        //    _eventDic = new Dictionary<int, Action<int, object[]>>();
        //}
        //public override void Reset()
        //{
        //    base.Reset();
        //}

        //public override void Dispose()
        //{
        //    base.Dispose();
        //    _eventDic?.Clear();
        //    _eventDic = null;
        //}

        //public override void Init( Module_ProxyActor.ActorInstance instance )
        //{
        //    base.Init( instance );
        //}

        ///// <summary>
        ///// 事件集,K=eventID,V=(eventID,param)
        ///// </summary>
        //private Dictionary<int, Action<int, object[]>> _eventDic;
    }
    #endregion
}