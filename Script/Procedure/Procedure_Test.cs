using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Procedure
{
    /// <summary>
    /// ≤‚ ‘¡˜≥Ã
    /// </summary>
    public class Procedure_Test : ProcedureBase
    {
        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            GameEntry.TimeWheel.AddTask( Extension.TimeWheel_Task.GenRepeat
                ( 
                    1f, 
                    () => 
                    { 
                        Debug.Log( $"testing...{counter++}" ); 
                    } 
                ));
        }
        private int counter = 0;
    }

}
