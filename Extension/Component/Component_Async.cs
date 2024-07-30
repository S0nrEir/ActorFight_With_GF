using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityGameFramework.Runtime;

namespace Aquila.Extension
{
    public class Component_Async : GameFrameworkComponent
    {
        public AsyncPromise<T> StartAsyncTask<T>(Func<Task<T>> taskFunc)
        {
            var promise = new AsyncPromise<T>(taskFunc());
            StartCoroutine( WaitforTask( promise ) );
            return promise;
        }

        private IEnumerator WaitforTask<T>(AsyncPromise<T> promise)
        {
            while ( !promise.Completed )
                yield return null;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        //private void Update()
        //{
        //    UnityEngine.Debug.Log( $"result:{_result}" );
        //}

        //private async void Start()
        //{
        //    _result = "none";
        //    //for test
        //    var promise = StartAsyncTask( TestMethod );
        //    _result = await promise.Task;
        //}

        //private async Task<string> TestMethod()
        //{
        //    await Task.Delay( 1500 );
        //    return "test";
        //}


        private string _result = string.Empty;
    }
    /// <summary>
    /// 异步结果
    /// </summary>
    public sealed class AsyncPromise<T>
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AsyncPromise( Task<T> task )
        {
            Task = task;
            Completed = false;
            _result = default;

            task.ContinueWith(paramTask => 
            {
                if ( paramTask.IsFaulted )
                {
                    throw task.Exception;
                }

                if ( paramTask.IsCompleted )
                {
                    _result = task.Result;
                    Completed = true;
                }
            } );
        }
        private AsyncPromise()
        {
        }

        /// <summary>
        /// task
        /// </summary>
        public Task<T> Task
        {
            get;
            private set;
        }

        /// <summary>
        /// 完成标记
        /// </summary>
        public bool Completed
        {
            get;
            private set;
        }

        /// <summary>
        /// 异步结果
        /// </summary>
        private T _result = default;

        /// <summary>
        /// 异步结果
        /// </summary>
        public T Result
        {
            get => Completed ? _result : default( T );
        }
    }
}
