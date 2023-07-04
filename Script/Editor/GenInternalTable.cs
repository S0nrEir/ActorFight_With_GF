//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Aquila.Editor.DataTableTools;
using Aquila.Procedure;
using GameFramework;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Aquila.Editor
{
    public class GenInternalTable
    {
        [MenuItem( "Aquila/GenInternalTable" )]
        private static void GenInternalTable_()
        {
            //foreach ( string dataTableName in ProcedurePreload.DataTableNames )
            foreach ( string dataTableName in Procedure_Prelaod.Configs )
            {
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor( dataTableName );
                if ( !DataTableGenerator.CheckRawData( dataTableProcessor, dataTableName ) )
                {
                    Debug.LogError( Utility.Text.Format( "Check raw data failure. DataTableName='{0}'", dataTableName ) );
                    break;
                }

                DataTableGenerator.GenerateDataFile( dataTableProcessor, dataTableName );
                DataTableGenerator.GenerateCodeFile( dataTableProcessor, dataTableName );
            }
            GenFormIdEnum.GenFormIdEnum_();
            AssetDatabase.Refresh();
        }

    }
}
