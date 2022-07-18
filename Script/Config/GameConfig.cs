using System;
using UnityEngine;

namespace Aquila.Config
{
    /// <summary>
    /// 游戏业务相关配置类
    /// </summary>
    public class GameConfig
    {
        /// <summary>
        /// 对象池配置相关
        /// </summary>
        public static class ObjectPool
        {
            /// <summary>
            /// 地块对象池名称
            /// </summary>
            public const string OBJECT_POOL_TERRAIN_NAME = "Object_Terrain";
        }

        /// <summary>
        /// 实体配置相关
        /// </summary>
        public static class Entity
        {
            /// <summary>
            /// 英雄类actor组
            /// </summary>
            public const string GROUP_HeroActor = "HeroActor";

            /// <summary>
            /// 特效类actor组
            /// </summary>
            public const string GROUP_ActorEffect = "ActorEffect";

            /// <summary>
            /// 投射物
            /// </summary>
            public const string GROUP_Projectile = "ProjectileActor";

            /// <summary>
            /// Trigger类actor组
            /// </summary>
            public const string GROUP_Trigger = "TriggerActor";

            /// <summary>
            /// 其他
            /// </summary>
            public const string GROUP_Other = "Other";

            /// <summary>
            /// actor
            /// </summary>
            public const int Priority_Actor = 1;

            /// <summary>
            /// 特效
            /// </summary>
            public const int Priority_Effect = 2;
        }

        /// <summary>
        /// 层级配置相关
        /// </summary>
        public static class Layer
        {
            /// <summary>
            /// floor层级
            /// </summary>
            public const string LAYER_NAME_FLOOR = "Floor";

            /// <summary>
            /// 默认层级
            /// </summary>
            public const string LAYER_NAME_DEFAULT = "Default";

            /// <summary>
            /// 后处理层级
            /// </summary>
            public const string LAYER_NAME_POST_PROCESSING = "PostProcessing";
        }

        /// <summary>
        /// 场景相关
        /// </summary>
        public class Scene
        {
            /// <summary>
            /// 单个地块的默认偏移距离
            /// </summary>
            public float TERRAIN_BLOCK_OFFSET_DISTANCE = .9f;

            /// <summary>
            /// 主相机默认旋转角度
            /// </summary>
            public static Vector3 MAIN_CAMERA_DEFAULT_ROTATION { get; } = new Vector3( 36f, 45f, 0 );

            /// <summary>
            /// 主相机默认世界空间坐标位置
            /// </summary>
            public static Vector3 MAIN_CAMERA_DEFAULT_POSITION { get; } = new Vector3( -2.75f,5.95f,-3.91f );

        }


        /// <summary>
        /// 全局配置Tags类
        /// </summary>
        public static class Tags
        {
            /// <summary>
            /// 战斗地块根节点
            /// </summary>
            public static string TERRAIN_ROOT = "TerrainRoot";
        }
    }

}
