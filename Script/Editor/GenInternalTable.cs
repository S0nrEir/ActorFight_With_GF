//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using Aquila.Editor.DataTableTools;
using GameFramework;
using UnityEditor;
using UnityEngine;

namespace Aquila.Editor
{
    public class GenInternalTable
    {

        private static string[] _txtFiles = new string[]
            {
                "UIForm.txt"
            };

        [MenuItem( "Aquila/GenInternalTable" )]
        private static void GenInternalTable_()
        {
            //foreach ( string dataTableName in ProcedurePreload.DataTableNames )
            foreach ( string dataTableName in _txtFiles )
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

            AssetDatabase.Refresh();
        }

    }
}
