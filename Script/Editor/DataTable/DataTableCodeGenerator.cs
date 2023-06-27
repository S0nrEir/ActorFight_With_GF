//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Text;

namespace Aquila.Editor.DataTableTools
{
    public delegate void DataTableCodeGenerator(DataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData);
}
