using Cfg.common;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Procedure
{
    public partial class Procedure_Test : ProcedureBase
    {
        private void LoadScript()
        {
            var meta = GameEntry.DataTable.Table<TB_Scripts>().Get( 10001 );
            GameEntry.Lua.Load( meta );
            //GameEntry.Lua.UnLoadAllRunningData();
        }
    }
}
