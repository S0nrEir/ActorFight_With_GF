using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Aquila.Editor
{
    public class GenFormIdEnum
    {
        /// <summary>
        /// 生成form枚举
        /// </summary>
        [MenuItem( "Aquila/GenFormIdEnum" )]
        public static void GenFormIdEnum_()
        {
            StringBuilder generator = new StringBuilder();
            generator.AppendLine( "/// <summary>" );
            generator.AppendLine( "/// generated by tool,do not modify" );
            generator.AppendLine( "/// <summary>" );
            generator.AppendLine( "namespace Aquial.UI" );
            generator.AppendLine( "{" );
            generator.AppendLine( "\tpublic enum FormIdEnum : byte " );
            generator.AppendLine( "\t{" );

            var fileName = @"Assets/Res/Config/UIForm.txt";
            var rows = File.ReadAllLines( fileName );
            var len = rows.Length;
            string row = string.Empty;
            string[] fields = null;
            for ( var i = 4; i < len; i++ )
            {
                row = rows[i];
                fields = row.Split( '\t' );
                var assetPath = fields[3];
                if ( string.IsNullOrEmpty( assetPath ) )
                {
                    Debug.LogWarning( $"<color=yellow>GenInternalTable.GenFormIdEnum()--->assetPath is null</color>" );
                    break;
                }
                //Assets / Res / UI / TestForm.prefab
                var comment = fields[2];
                var formName = Path.GetFileNameWithoutExtension( assetPath );
                generator.AppendLine( "\t\t/// <summary>" );
                generator.AppendLine( $"\t\t/// {comment}" );
                generator.AppendLine( "\t\t/// <summary>" );
                generator.AppendLine( $"\t\t{formName}," );
            }

            generator.AppendLine( "\t}" );
            generator.AppendLine( "}" );
            File.WriteAllText( @"Assets/Script/UI/FormIdEnum.cs", generator.ToString(), Encoding.UTF8 );
        }
    }

}