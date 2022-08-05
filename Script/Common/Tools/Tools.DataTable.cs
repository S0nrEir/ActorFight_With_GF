using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila
{
    public static partial class Tools
    {
        /// <summary>
        /// 数据表工具类
        /// </summary>
        public static class Table
        {
            /// <summary>
            /// 获取SceneConfig表实例
            /// </summary>
            public static Cfg.single.TB_SceneConfig GetSceneConfig()
            {
                return GameEntry.DataTable.Tables.TB_SceneConfig;
            }
        }
    }
}
