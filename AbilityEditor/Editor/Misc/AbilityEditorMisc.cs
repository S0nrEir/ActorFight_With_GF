using UnityEngine;

namespace Aquila.AbilityEditor
{
    public static class Misc
    {
        /// <summary>
        /// 训练场木桩角色配置ID / sand box dummy meta role id
        /// </summary>
        public const int DUMMY_META_ROLE_ID = 999998;
        
        /// <summary>
        /// 训练场玩家角色ID / sand box player meta role id
        /// </summary>
        public const int PLYAER_META_ROLE_ID = 999999;

        /// <summary>
        /// 编辑器技能数据的存储目录 / ability data asset storage path
        /// </summary>
        public static readonly string ABILITY_ASSET_BASE_PATH = "Assets/AbilityEditor/Editor/Config/Ability";

        /// <summary>
        /// 二进制技能数据的存储目录
        /// </summary>
        public static readonly string ABILITY_BIN_ASSET_PATH = "Assets/Res/Config/Ability";
        
        /// <summary>
        /// 编辑器 Effect 数据的存储目录 / effect data asset storage path
        /// </summary>
        public static readonly string EFFECT_ASSET_BASE_PATH = "Assets/AbilityEditor/Editor/Config/Effects";
        
        /// <summary>
        /// 二进制 Effect 数据的存储目录 / binary effect data storage path
        /// </summary>
        public static readonly string EFFECT_BIN_ASSET_PATH = "Assets/Res/Config/Effect";
        
        /// <summary>
        /// effect数据的保存路径 / path for saving effect data
        /// </summary>
        public static readonly string NEW_EFFECT_DATA_PATH = "Assets/Editor/AbilityEditor/Config/Effects";
        
        /// <summary>
        /// UI布局文件路径 / UXML file path
        /// </summary>
        public static readonly string UXML_FILE_PATH = "Assets/AbilityEditor/Editor/EditorUILayout/AbilityEditorWindow.uxml";

        /// <summary>
        /// 技能配置生成路径 / ability config gen path
        /// </summary>
        //public const string ABILITY_CFG_GEN_PATH = "AbilityEditor/Editor/Config/Ability";
        
        /// <summary>
        /// 默认的clip ui的固定宽度
        /// </summary>
        public const float DEFAULT_INSTANT_CLIP_UI_WIDTH = 24f;
        
        /// <summary>
        /// effect clip ui的固定宽度
        /// </summary>
        public const float EFFECT_CLIP_UI_WIDTH = 50f;
        
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
            new Color(.8f, .4f,0f),
            new Color(.8f, .8f, 0f),
            new Color(.4f, .8f, 0f),
            new Color(0f, .4f, .8f),
            new Color(.8f, 0f, .4f),
            new Color(.6f, .4f, 1f),
            
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