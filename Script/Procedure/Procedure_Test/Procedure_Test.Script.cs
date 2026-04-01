using Cfg.Common;
using GameFramework.Procedure;

namespace Aquila.Procedure
{
    public partial class Procedure_Test : ProcedureBase
    {
        private void LoadScript()
        {
            var meta = GameEntry.LuBan.Table<Scripts>().Get( 10001 );
            GameEntry.Lua.Load( meta );
            //GameEntry.Lua.UnLoadAllRunningData();
        }
    }
}
