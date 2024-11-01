using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Aquila.Procedure
{
    /// <summary>
    /// 测试流程
    /// </summary>
    public partial class Procedure_Test : ProcedureBase
    {
        private async void TestAsync()
        {
            _counter = 0;
            _counter = await GameEntry.Async.StartAsyncTask(TestAsyncTask).Task;
            Debug.Log( $"TestAsync End,result:{_counter}" );
        }

        private async Task<int> TestAsyncTask()
        {
            //Debug.Log( $"TestAsyncTask Start,_count:{_counter}" );
            //await Task.Delay( 5000 );
            //_counter++;
            //Debug.Log( $"TestAsyncTask End,_count:{_counter}" );
            //return _counter;

            await Task.Delay( 5000 );
            return 1;
        }

        private int _counter = 0;
    }
}
