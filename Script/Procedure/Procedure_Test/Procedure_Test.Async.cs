using System.Threading.Tasks;
using Aquila.Toolkit;
using GameFramework.Procedure;

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
            Tools.Logger.Info( $"TestAsync End,result:{_counter}" );
        }

        private async Task<int> TestAsyncTask()
        {
            //Aquila.Toolkit.Tools.Logger.Info( $"TestAsyncTask Start,_count:{_counter}" );
            //await Task.Delay( 5000 );
            //_counter++;
            //Aquila.Toolkit.Tools.Logger.Info( $"TestAsyncTask End,_count:{_counter}" );
            //return _counter;

            await Task.Delay( 5000 );
            return 1;
        }

        private int _counter;
    }
}
