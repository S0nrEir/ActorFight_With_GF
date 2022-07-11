using System;

namespace Aquila.Config
{
    /// <summary>
    /// 游戏业务相关配置类
    /// </summary>
    public class GameConfig
    {
        /// <summary>
        /// 实体相关
        /// </summary>
        public static class Entity
        {
            /// <summary>
            /// 英雄类actor组
            /// </summary>
            public const string GROUP_HeroActor = "HeroActor";

            /// <summary>
            /// 防御塔类actor组
            /// </summary>
            public const string GROUP_TowerActor = "TowerActor";

            /// <summary>
            /// 随从类actor组
            /// </summary>
            public const string GROUP_MinionActor = "MinionActor";

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
        /// 层级相关
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
            /// 卡牌放置层级
            /// </summary>
            public const string LAYER_NAME_CARD_AREA = "CardArea";

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
            //下面这些都走静态表

            /// <summary>
            /// 每行区域地块数量
            /// </summary>
            public const int PARCEL_COUNT = 4;

            /// <summary>
            /// 区域数量上限范围
            /// </summary>
            public const int AREA_COUNT = 4;

            /// <summary>
            /// 阶段3到达时间,五分钟三百秒
            /// </summary>
            public const float STAGE_3_ARRIVE_TIME = 300f;

            /// <summary>
            /// 阶段2到达时间,三分钟一百八十秒
            /// </summary>
            public const float STAGE_2_ARRIVE_TIME = 180f;

            /// <summary>
            /// 阶段3费用每秒上涨速率
            /// </summary>
            public const float STAGE_3_COST_INCREASE_RATE = .8f;

            /// <summary>
            /// 阶段2费用每秒上涨速率
            /// </summary>
            public const float STAGE_2_COST_INCREASE_RATE = .8f;

            /// <summary>
            /// 阶段1费用每秒上涨速率
            /// </summary>
            public const float STAGE_1_COST_INCREASE_RATE = .5f;

            /// <summary>
            /// 固定点的周围8方向点的偏移，itme1=x,item2=y
            /// </summary>
            public static Tuple<int, int>[] _8WaysOffset = new Tuple<int, int>[]
            {
                Tuple.Create(1,0),//右
                Tuple.Create(1,1),//右上
                Tuple.Create(0,1),//上
                Tuple.Create(-1,1),//左上
                Tuple.Create(-1,0),//左
                Tuple.Create(-1,-1),//左下
                Tuple.Create(0,-1),//下
                Tuple.Create(1,-1),//右下
            };  

            /// <summary>
            /// 默认坐标点的寻路代价
            /// </summary>
            public const int DEFAULT_COST = 1;

            /// <summary>
            /// 不可寻寻路的寻路代价值
            /// </summary>
            public const int UN_TRAVELABLE_COST = -1;

            /// <summary>
            /// 最大场景单位范围，最多支持5位数坐标，即最大(999,999)
            /// </summary>
            public const int MAX_SCENE_RANGE = 1000;
        }

    }

}
