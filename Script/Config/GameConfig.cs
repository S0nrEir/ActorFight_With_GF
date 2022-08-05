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
        /// Lua脚本常量相关
        /// </summary>
        public static class Lua
        {
            /// <summary>
            /// lua函数结束脚本回调
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_FINISH = "on_finish_lua";

            /// <summary>
            /// 脚本计时器回调函数名称
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_TICK = "on_timer_tick_lua";

            /// <summary>
            /// 脚本刷帧函数名称
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_UPDATE = "on_update_lua";

            /// <summary>
            /// 脚本启动函数名
            /// </summary>
            public const string LUA_FUNCTION_NAME_ON_START = "on_start_lua";
        }

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
            /// 地块层级
            /// </summary>
            public const string LAYER_TERRAIN_BLOCK = "TerrainBlock";

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
            //public const float TERRAIN_BLOCK_OFFSET_DISTANCE = .9f;

            /// <summary>
            /// 主相机默认旋转角度
            //public static Vector3 MAIN_CAMERA_DEFAULT_EULER { get; } = new Vector3( 36f, 45f, 0 );

            /// <summary>
            /// 主相机默认世界空间坐标位置//#todo解决excel中数据首字符不能为'-'的情况
            /// </summary>
            /// </summary>
            public static Vector3 MAIN_CAMERA_DEFAULT_POSITION { get; } = new Vector3( -2.75f,5.95f,-3.91f );

            /// <summary>
            /// 战斗场景地块默认x方向长度
            /// </summary>
            //public const int FIGHT_SCENE_DEFAULT_X_WIDTH = 10;

            ///// <summary>
            ///// 战斗场景地块默认z方向长度
            ///// </summary>
            //public const int FIGHT_SCENE_DEFAULT_Y_WIDTH = 10;

            ///// <summary>
            ///// 场景地块两位坐标精度总范围
            ///// </summary>
            //public const int FIGHT_SCENE_TERRAIN_COORDINATE_RANGE = 9999;

            /// <summary>
            /// 场景地块两位坐标精度系数
            /// </summary>
            //public const int FIGHT_SCENE_TERRAIN_COORDINATE_PRECISION = 100;
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

            /// <summary>
            /// 地块节点
            /// </summary>
            public static string TERRAIN_BLOCK = "TerrainBlock";
        }
    }

}
