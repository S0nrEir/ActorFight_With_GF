using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    /// <summary>
    /// ʱ�������
    /// </summary>
    [DisallowMultipleComponent]
    public class Component_TimeWheel : GameFrameworkComponent
    {

        /// <summary>
        /// �Ƴ����񣬶���һ���Ե����������ֶ������Ƴ�
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
        /// ����һ������
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
        /// ʱ�������񼯺�����
        /// </summary>
        private Dictionary<int, TimeWheel_Task> _task_dic = null;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        private float _elpased = 0f;

        /// <summary>
        /// ʱ���ֳ߶�
        /// </summary>
        private int _wheel_size;

        /// <summary>
        /// ��ǰʱ����
        /// </summary>
        private TimeWheel _current = null;

        /// <summary>
        /// ʱ����Ĭ�ϳ߶�
        /// </summary>
        public const int DEFAULT_WHEEL_SIZE = 0xa;//10

        /// <summary>
        /// ʱ����
        /// </summary>
        private const float DEFAULT_TICK_SEC = 0.1f;
    }

    /// <summary>
    /// ʱ����
    /// </summary>
    internal class TimeWheel
    {
        /// <summary>
        /// ���ø�ʱ���ֵ�ǰ�̶ȵ��������񣬲�����ʱ����������һ���̶�
        /// </summary>
        public void Exec()
        {
            _buckets[_curr_bucket].Exec();
            Move2Next();
        }

        /// <summary>
        /// ʱ����������һ���̶�
        /// </summary>
        public void Move2Next()
        {
            //���ÿ̶�
            if ( ++_curr_bucket >= _buckets.Length )
                _curr_bucket = 0;
        }

        /// <summary>
        /// ��ĳһ�̶�������һ��task
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
        /// �����±꣬�����µ��±�λ�úͼ�����
        /// </summary>
        private (int idx, int counter) CalcBucketIndex( float interval_ )
        {
            var next_pos = interval_ * Component_TimeWheel.DEFAULT_WHEEL_SIZE + _curr_bucket;
            if ( next_pos >= _size )
            {
                //next_pos = next_pos / _size;
                return (( int ) next_pos / _size, ( int ) next_pos % _size);
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
        /// ʱ���ֳ���
        /// </summary>
        private int _size = 0;

        /// <summary>
        /// �̶Ȳۼ��ϣ�ÿһ�ֵ����п̶Ȳ�λ
        /// </summary>
        private Bucket[] _buckets = null;

        /// <summary>
        /// ʱ���ֵ�ǰָ��Ŀ̶�
        /// </summary>
        private int _curr_bucket = 0;

    }//end TimeWheel

    /// <summary>
    /// ʱ���̶ֿ�
    /// </summary>
    internal class Bucket
    {
        /// <summary>
        /// �ڵ�ǰ�̶�������task
        /// </summary>
        public bool AddTask( TimeWheel_Task task )
        {
            return _tasks.AddLast( task ) != null;
        }

        /// <summary>
        /// ɾ����ǰ�̶��ϵ�ָ��task
        /// </summary>
        public bool RemoveTask( TimeWheel_Task task )
        {
            if ( task is null )
            {
                Log.Warning( "<color=yellow>task to remove is null</color>" );
                return false;
            }

            GameEntry.TimeWheel.RemoveTask( task.ID );
            ReferencePool.Release( task );
            return _tasks.Remove( task );
        }

        /// <summary>
        /// ���øÿ̶��ϵ���������
        /// </summary>
        public void Exec()
        {
            foreach ( var task in _tasks )
            {
                if ( task.DestroyFlag )
                {
                    RemoveTask( task );
                    continue;
                }

                task.CallBack();
                //�������񣬷ŵ���һ���̶Ȳ�
                if ( task.Repeat )
                {
                    //todo:move task
                    //����ʱ�����һλ�õ�ʱ��̶ȣ��ŵ����bucket��
                    _owner.AddTask( task );
                    RemoveTask( task );
                }
                else
                {
                    //task.SetAsDestroy();
                    //GameEntry.TimeWheel.RemoveTask( task.ID );
                    var temp_id = task.ID;
                    if ( !RemoveTask( task ) )
                        Log.Error( $"faild to remove task id={temp_id}" );
                }
            }
        }

        public Bucket( int idx_, TimeWheel owner_ )
        {
            Index = idx_;
            _tasks = new GameFrameworkLinkedList<TimeWheel_Task>();
            _owner = owner_;
        }

        /// <summary>
        /// ��bucket�ĳ�����
        /// </summary>
        private TimeWheel _owner = null;

        /// <summary>
        /// �̶Ȳ��±�λ��
        /// </summary>
        public int Index { get; private set; } = -1;

        /// <summary>
        /// ���е����񼯺�
        /// </summary>
        private GameFrameworkLinkedList<TimeWheel_Task> _tasks = null;
    }

    /// <summary>
    /// ʱ��������
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
        /// ����һ��TimeWheel_Taskʵ��
        /// </summary>
        public static TimeWheel_Task GenRepeat( float interval_, Action call_back_ )
        {
            var task = ReferencePool.Acquire<TimeWheel_Task>();
            task.Setup( interval_, true, call_back_ );
            return task;
        }

        /// <summary>
        /// ���ô����ִ���
        /// </summary>
        public void Reset( int count_ )
        {
            _counter = count_;
        }

        /// <summary>
        /// ���ûص�
        /// </summary>
        public void CallBack()
        {
            if ( _counter < 0 )
                return;

            if ( --_counter == 0 )
                _call_back?.Invoke();
        }

        /// <summary>
        /// �������ٱ��
        /// </summary>
        public void SetAsDestroy()
        {
            DestroyFlag = true;
        }

        /// <summary>
        /// ����task����
        /// </summary>
        public void Setup( float interval_, bool repeat_, Action call_back_ )
        {
            Interval = interval_;
            Repeat = repeat_;
            _call_back = call_back_;
        }

        public void Clear()
        {
            Repeat = false;
            DestroyFlag = false;
            _call_back = null;
        }

        /// <summary>
        /// ���ٱ��
        /// </summary>
        public bool DestroyFlag { get; private set; } = false;

        /// <summasry>
        /// taskID
        /// </summary>
        public int ID { get; private set; } = -1;

        /// <summary>
        /// ����������
        /// </summary>
        public bool Repeat { get; private set; } = false;

        /// <summary>
        /// ���ü��
        /// </summary>
        public float Interval { get; private set; } = -1f;

        /// <summary>
        /// ������
        /// </summary>
        private int _counter = 0;

        /// <summary>
        /// �ص�
        /// </summary>
        private Action _call_back = null;
    }
}