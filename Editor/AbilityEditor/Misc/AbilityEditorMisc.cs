using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    public static class Misc
    {
        // UI布局文件路径
        public static readonly string UXML_FILE_PATH = "Assets/Editor/AbilityEditor/EditorUILayout/AbilityEditorWindow.uxml";

        #region Timeline Clip 常量

        /// <summary>
        /// 最小clip宽度（像素）
        /// </summary>
        public const float MIN_CLIP_WIDTH = 20f;

        /// <summary>
        /// 最小clip时长（秒）
        /// </summary>
        public const float MIN_CLIP_DURATION = 0.1f;

        /// <summary>
        /// 拖动手柄宽度（像素）
        /// </summary>
        public const float HANDLE_WIDTH = 8f;

        /// <summary>
        /// Clip高度（像素）
        /// </summary>
        public const float CLIP_HEIGHT = 30f;

        /// <summary>
        /// 时间对齐间隔（秒）
        /// </summary>
        public const float TIME_SNAP_INTERVAL = 0.1f;

        #endregion
    }
}