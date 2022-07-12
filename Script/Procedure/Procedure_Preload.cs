using Bright.Serialization;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 预加载流程
    /// </summary>
    public class Procedure_Prelaod : ProcedureBase
    {
        protected override void OnInit( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnInit( procedureOwner );
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            //加载数据表
            LoadDataTables();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            base.OnLeave( procedureOwner, isShutdown );
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        private void LoadDataTables()
        {
            var table_ctor = typeof( cfg.Tables ).GetConstructors()[0];
            var loader = new System.Func<string, ByteBuf>( (file)=>
            {
                return new ByteBuf( File.ReadAllBytes( $"{Application.dataPath}/Res/DataTables/{file}.bytes" ) );
            } );
            var tables = ( cfg.Tables ) table_ctor.Invoke( new object[] { loader } );
            foreach ( var meta in tables.TbItem.DataList )
            {
                Log.Info( $"meta info,desc:{meta.Desc},id:{meta.Id}" ,LogColorTypeEnum.White);
            }
        }
    }

}
