using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// 时间轮组件
    /// </summary>
    [DisallowMultipleComponent]
    public class Component_TimeWheel : GameFrameworkComponent
    {

        /// <summary>
        /// 移除任务，对于一次性的任务，无需手动调用移除
        /// </summary>
        public bool RemoveTask( int task_id_ )
        {
            if ( _task_dic.TryGetValue( task_id_, out var task ) )
            {
                task.SetAsDestroy();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        public void AddTask( TimeWheel_Task task_ )
        {
            _current.AddTask( task_ );
        }

        protected override void Awake()
        {
            base.Awake();
            _wheel_size = DEFAULT_WHEEL_SIZE;
            _current = new TimeWheel( _wheel_size );
            _elpased = 0f;
            _task_dic = new Dictionary<int, TimeWheel_Task>( 0x10 );
        }

        private void Update()
        {
            if ( _elpased >= DEFAULT_TICK_SEC )
            {
                _current?.Exec();
                _elpased = 0f;
            }
            _elpased += Time.deltaTime;
        }

        /// <summary>
        /// 时间轮任务集合索引
        /// </summary>
        private Dictionary<int, TimeWheel_Task> _task_dic = null;

        /// <summary>
        /// 流逝时间
        /// </summary>
        private float _elpased = 0f;

        /// <summary>
        /// 时间轮尺度
        /// </summary>
        private int _wheel_size;

        /// <summary>
        /// 当前时间轮
        /// </summary>
        private TimeWheel _current = null;

        /// <summary>
        /// 时间轮默认尺度
        /// </summary>
        public const int DEFAULT_WHEEL_SIZE = 0xa;//10

        /// <summary>
        /// 时间跨度
        /// </summary>
        private const float DEFAULT_TICK_SEC = 0.1f;
    }

    /// <summary>
    /// 时间轮
    /// </summary>
    internal class TimeWheel
    {
        /// <summary>
        /// 调用该时间轮当前刻度的所有任务，并且让时间轮走向下一个刻度
        /// </summary>
        public void Exec()
        {
            _buckets[_curr_bucket].Exec();
            Move2Next();
        }

        /// <summary>
        /// 时间轮走向下一个刻度
        /// </summary>
        public void Move2Next()
        {
            //重置刻度
            if ( ++_curr_bucket >= _buckets.Length )
                _curr_bucket = 0;
        }

        /// <summary>
        /// 在某一刻度上添加一个task
        /// </summary>
        public void AddTask( TimeWheel_Task task_ )
        {
            if ( task_ is null )
            {
                Log.Warning( "TimeWheel.AddTask--->task_ is null." );
                return;
            }

            var idx_and_counter = CalcBucketIndex( task_.Interval );
            task_.Reset( idx_and_counter.counter );
            _buckets[idx_and_counter.idx].AddTask( task_ );
        }

        /// <summary>
        /// 计算下标，返回新的下标位置和计数器
        /// </summary>
        private (int idx, int counter) CalcBucketIndex( float interval_ )
        {
            var next_pos = interval_ * Component_TimeWheel.DEFAULT_WHEEL_SIZE + _curr_bucket;
            if ( next_pos >= _size )
            {
                //next_pos = next_pos / _size;
                return (( int ) next_pos % _size, ( int ) next_pos / _size);
            }
            else
            {
                //next_pos = interval_ * Component_TimeWheel.DEFAULT_WHEEL_SIZE;
                return (( int ) next_pos, 0);
            }
        }

        protected TimeWheel() { }
        public TimeWheel( int bucket_size_ )
        {
            Bucket temp = null;
            _size = bucket_size_;
            _buckets = new Bucket[_size];
            for ( var i = 0; i < _size; i++ )
            {
                temp = new Bucket( i, this );
                _buckets[i] = temp;
            }
            _curr_bucket = 0;
        }

        /// <summary>
        /// 时间轮长度
        /// </summary>
        private int _size = 0;

        /// <summary>
        /// 刻度槽集合，每一轮的所有刻度槽位
        /// </summary>
        private Bucket[] _buckets = null;

        /// <summary>
        /// 时间轮当前指向的刻度
        /// </summary>
        private int _curr_bucket = 0;

    }//end TimeWheel

    /// <summary>
    /// 时间轮刻度
    /// </summary>
    internal class Bucket
    {
        /// <summary>
        /// 在当前刻度上添加task
        /// </summary>
        public bool AddTask( TimeWheel_Task task )
        {
            var succ = _tasks.AddLast( task ) != null;
            if ( !succ )
                Log.Warning( "faild to add task to linklist!" );

            return succ;
        }

        /// <summary>
        /// 删除当前刻度上的指定task
        /// </summary>
        public bool RemoveTask( TimeWheel_Task task )
        {
            if ( task is null )
            {
                Log.Warning( "<color=yellow>task to remove is null</color>" );
                return false;
            }

            var succ = _tasks.Remove( task );
            ReferencePool.Release( task );
            GameEntry.TimeWheel.RemoveTask( task.ID );
            return succ;
        }

        /// <summary>
        /// 调用该刻度上的所有任务
        /// </summary>
        public void Exec()
        {
            if ( _tasks.Count >= 1 )
                ;

            var iter = _tasks.GetEnumerator();
            while ( iter.MoveNext() )
            {
                if ( iter.Current.DestroyFlag )
                {
                    _tasks.Remove( iter.Current );
                    continue;
                }

                iter.Current.Count();
                if ( iter.Current.Counter != 0 )
                    continue;

                iter.Current.CallBack();
                if ( iter.Current.Repeat )
                {
                    //移到别的bucket
                    //RemoveFromTasks( iter.Current );
                    //_owner.AddTask( iter.Current );
                    _move_queue.Enqueue( iter.Current );
                }
                else
                {
                    _remove_queue.Enqueue( iter.Current );
                }
            }
            //#todo要移除的task放到remove task中
            RemoveUnusedTask();
            HandleMoveTasks();
            #region
            //foreach ( var task in _tasks )
            //{
            //    if ( task.DestroyFlag )
            //    {
            //        RemoveTask( task );
            //        continue;
            //    }

            //    task.CallBack();
            //    //周期任务，放到下一个刻度槽
            //    if ( task.Repeat )
            //    {
            //        //todo:move task
            //        //计算时间和下一位置的时间刻度，放到别的bucket里
            //        _owner.AddTask( task );
            //        RemoveTask( task );
            //    }
            //    else
            //    {
            //        //task.SetAsDestroy();
            //        //GameEntry.TimeWheel.RemoveTask( task.ID );
            //        var temp_id = task.ID;
            //        if ( !RemoveTask( task ) )
            //            Log.Error( $"faild to remove task id={temp_id}" );
            //    }
            //}
            #endregion
        }

        private void HandleMoveTasks()
        {
            TimeWheel_Task task = null;
            while ( _move_queue.Count != 0 )
            {
                task = _move_queue.Dequeue();
                _tasks.Remove( task );
                _owner.AddTask( task );
            }
        }


        /// <summary>
        /// 彻底移除对应的task
        /// </summary>
        private void RemoveUnusedTask()
        {
            TimeWheel_Task task = null;
            while ( _remove_queue.Count != 0 )
            {
                task = _remove_queue.Dequeue();
                RemoveTask( task );
            }
        }

        public Bucket( int idx_, TimeWheel owner_ )
        {
            Index = idx_;
            _owner = owner_;
            _tasks = new GameFrameworkLinkedList<TimeWheel_Task>();
            _remove_queue = new Queue<TimeWheel_Task>();
            _move_queue = new Queue<TimeWheel_Task>();
        }

        /// <summary>
        /// 该bucket的持有者
        /// </summary>
        private TimeWheel _owner = null;

        /// <summary>
        /// 刻度槽下标位置
        /// </summary>
        public int Index { get; private set; } = -1;

        /// <summary>
        /// 持有的任务集合
        /// </summary>
        private GameFrameworkLinkedList<TimeWheel_Task> _tasks = null;

        /// <summary>
        /// 移除队列
        /// </summary>
        private Queue<TimeWheel_Task> _remove_queue = null;

        /// <summary>
        /// 待移动的task队列
        /// </summary>
        private Queue<TimeWheel_Task> _move_queue = null;
    }

    /// <summary>
    /// 时间轮任务
    /// </summary>
    public class TimeWheel_Task : IReference
    {
        public static TimeWheel_Task GenOnce( float interval_, Action call_back_ )
        {
            var task = ReferencePool.Acquire<TimeWheel_Task>();
            task.Setup( interval_, false, call_back_ );
            return task;
        }

        /// <summary>
        /// 返回一个TimeWheel_Task实例
        /// </summary>
        public static TimeWheel_Task GenRepeat( float interval_, Action call_back_ )
        {
            var task = ReferencePool.Acquire<TimeWheel_Task>();
            task.Setup( interval_, true, call_back_ );
            return task;
        }
        /// <summary>
        /// 重置触发轮次数
        /// </summary>
        public void Reset( int count_ )
        {
            Counter = count_;
        }

        public void Count()
        {
            Counter--;
        }

        /// <summary>
        /// 调用回调
        /// </summary>
        public void CallBack()
        {
            if(Counter == 0)
                _call_back?.Invoke();
        }

        /// <summary>
        /// 设置销毁标记
        /// </summary>
        public void SetAsDestroy()
        {
            DestroyFlag = true;
        }

        /// <summary>
        /// 设置task参数
        /// </summary>
        public void Setup( float interval_, bool repeat_, Action call_back_ )
        {
            Interval = interval_;
            Repeat = repeat_;
            _call_back = call_back_;
        }

        public void Clear()
        {
            Debug.Log("clear");
            Repeat = false;
            DestroyFlag = false;
            _call_back = null;
        }

        /// <summary>
        /// 销毁标记
        /// </summary>
        public bool DestroyFlag { get; private set; } = false;

        /// <summasry>
        /// taskID
        /// </summary>
        public int ID { get; private set; } = -1;

        /// <summary>
        /// 周期任务标记
        /// </summary>
        public bool Repeat { get; private set; } = false;

        /// <summary>
        /// 调用间隔
        /// </summary>
        public float Interval { get; private set; } = -1f;

        /// <summary>
        /// 计数器
        /// </summary>
        public int Counter { get; private set; } = 0;

        /// <summary>
        /// 回调
        /// </summary>
        private Action _call_back = null;
    }
}
