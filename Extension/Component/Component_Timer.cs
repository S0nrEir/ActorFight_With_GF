using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// 一个简易的计时器
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Component_Timer : GameFrameworkComponent
    {
        public Timer StartTick( float n, Action<float> callBack )
        {
            var timer = Timer.GenRepeat( n, Timer.MaxCountLmt, callBack );
            timer.ReStart();
            Regiseter( timer, _updateRegDic );
            return timer;
        }

        public Timer StartCounting( float n, Action<float> callBack )
        {
            var timer = Timer.Gen( n, callBack );
            timer.ReStart();
            Regiseter( timer, _updateRegDic );
            return timer;
        }

        protected override void Awake()
        {
            base.Awake();

            //update time fixed
            _updateCallDic = new Dictionary<int, Timer>( MAX_TIMER_COUNT );
            _updateUnRegList = new List<int>( MAX_TIMER_COUNT );
            _updateRegDic = new Dictionary<int, Timer>( MAX_TIMER_COUNT );
        }

        public void Regiseter( Timer timer, Dictionary<int, Timer> dicToRegister )
        {
            if ( timer is null )
                return;

            if ( !timer.IsActive )
                return;

            if ( timer.ReadyToDestroy )
                return;

            if ( dicToRegister.ContainsKey( timer.ID ) )
                return;

            dicToRegister.Add( timer.ID, timer );
        }

        public void UnRegisterUpdate( Timer timer )
        {
            if ( timer is null )
                return;

            _updateUnRegList.Add( timer.ID );
        }

        private void FixedUpdate()
        {
            DoUpdateCallDic( Time.fixedDeltaTime );
            FinishUpdateCallList();
        }

        private void DoUpdateCallDic( float deltaTime )
        {
            foreach ( var kv in _updateCallDic )
            {
                if ( kv.Value is null )
                    continue;

                if ( kv.Value.ReadyToDestroy )
                    _updateUnRegList.Add( kv.Key );
                else
                    kv.Value.Do( deltaTime );
            }
        }

        private void FinishUpdateCallList()
        {
            //注册
            if ( _updateRegDic.Count != 0 )
            {
                foreach ( var kv in _updateRegDic )
                {
                    if ( _updateCallDic.TryGetValue( kv.Key, out var _ ) )
                        continue;

                    _updateCallDic.Add( kv.Key, kv.Value );
                }
                _updateRegDic.Clear();
            }

            //注销
            if ( _updateUnRegList.Count != 0 )
            {
                Timer timer = null;
                foreach ( var t in _updateUnRegList )
                {
                    if ( !_updateCallDic.TryGetValue( t, out timer ) )
                        continue;

                    ReferencePool.Release( timer );
                    _updateCallDic.Remove( t );
                }

                _updateUnRegList.Clear();
            }
        }
        private Dictionary<int, Timer> _updateCallDic = null;

        private List<int> _updateUnRegList = null;

        private Dictionary<int, Timer> _updateRegDic = null;

        public const int MAX_TIMER_COUNT = 0x32;//50

        public class Timer : IReference
        {
            public static Timer Gen( float interval, Action<float> callBack )
            {
                Timer timer = ReferencePool.Acquire<Timer>();
                timer.Set( IDPool++, interval, 0, callBack );
                return timer;
            }

            public static Timer GenRepeat( float interval, int countLimit, Action<float> callBack )
            {
                Timer timer = ReferencePool.Acquire<Timer>();
                timer.Set( IDPool++, interval, countLimit, callBack );
                return timer;
            }

            /// <summary>
            /// 立刻调用该timer的回调，无视时间,elapsed返回0f
            /// </summary>
            public void Doimmediately( bool destroyAfterCallBack = false )
            {
                _callBack?.Invoke( 0f );
                if ( destroyAfterCallBack )
                {
                    _timePassed = 999f;
                    Counter = int.MaxValue;
                }
            }

            public void Do( float deltaTime )
            {
                if ( _callBack == null )
                    return;

                if ( !IsActive )
                    return;

                _timePassed += deltaTime;
                if ( IsRepeat )
                {
                    if ( _timePassed >= Interval )
                    {
                        _timePassed = 0f;
                        Counter++;
                        if ( Counter > CountLimit )
                        {
                            Close();
                            return;
                        }
                        _callBack?.Invoke( deltaTime );
                        _timePassed = 0f;
                    }
                }
                else
                {

                    if ( _timePassed >= Interval )
                    {
                        _callBack?.Invoke( deltaTime );
                        Close();
                        return;
                    }
                }
            }

            public void ReStart()
            {
                Counter = 0;
                _timePassed = 0f;
                IsActive = true;
                ReadyToDestroy = false;
            }

            public void Pause()
            {
                IsActive = false;
            }

            private void Close()
            {
                IsActive = false;
                ReadyToDestroy = true;
            }

            public string DetailString()
            {
                return $"Timer,ID:{ID}\nCallBack:{( _callBack.Method != null ? _callBack.Method.Name : "None" )}\nIsRepeat:{IsRepeat}\nInterval:{Interval}\nCounter:{Counter}";
            }

            public override string ToString()
            {
                return $"Timer,ID:{ID}";
            }

            public Timer()
            {

            }

            /// <summary>
            /// 不重复回调
            /// </summary>
            public bool Set( int id, float interval, Action<float> callBack )
            {
                return Set( id, interval, 0, callBack );
            }

            public bool Set( int id, float interval, int countLimit, Action<float> callBack )
            {
                //初始化过，不管
                //if (id >= 0)
                //    return false;

                ID = id;
                Interval = interval;
                CountLimit = countLimit;
                _callBack = callBack;
                return true;
            }

            public void Dispose()
            {
                Pause();
                Close();
                _callBack = null;
            }

            public void Clear()
            {
                Dispose();
            }

            #region

            /// <summary>
            /// 开关
            /// </summary>
            public bool IsActive { get; private set; } = true;

            private float _timePassed = 0f;

            /// <summary>
            /// 回调间隔（重复为回调间隔，不重复为指定时间后回调）
            /// </summary>
            public float Interval { get; private set; } = 0f;

            /// <summary>
            /// 销毁标记
            /// </summary>
            public bool ReadyToDestroy { get; private set; } = false;

            /// <summary>
            /// 计数回调上限
            /// </summary>
            public int CountLimit { get; private set; } = 0;

            /// <summary>
            /// 计数器
            /// </summary>
            public int Counter { get; private set; } = 0;

            /// <summary>
            /// 是否重复调用，是返回true
            /// </summary>
            public bool IsRepeat => CountLimit > 0;

            /// <summary>
            /// 回调
            /// </summary>
            public Action<float> _callBack { get; private set; } = null;

            /// <summary>
            /// timerID
            /// </summary>
            public int ID { get; private set; } = -1;

            public const int MaxCountLmt = int.MaxValue;

            /// <summary>
            /// TimerID池
            /// </summary>
            private static int IDPool = 0;
            #endregion
        }
    }
}


