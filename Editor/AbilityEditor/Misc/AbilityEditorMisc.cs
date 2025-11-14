using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    public static class Misc
    {
        // UI布局文件路径
        public static readonly string UXML_FILE_PATH = "Assets/Editor/AbilityEditor/EditorUILayout/AbilityEditorWindow.uxml";

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

        /// <summary>
        /// 预定义的轨道颜色数组（用于区分不同轨道）
        /// </summary>
        private static readonly Color[] TrackColors = new Color[]
        {
            new Color(0.4f, 0.6f, 0.8f),
            new Color(0.6f, 0.8f, 0.4f),
            new Color(0.8f, 0.6f, 0.4f),
            new Color(0.8f, 0.4f, 0.6f),
            new Color(0.6f, 0.4f, 0.8f),
        };

        /// <summary>
        /// 根据轨道索引获取颜色（循环使用颜色数组）
        /// </summary>
        public static Color GetTrackColor(int trackIndex)
        {
            if (trackIndex < 0)
                trackIndex = 0;

            return TrackColors[trackIndex % TrackColors.Length];
        }
    }
}