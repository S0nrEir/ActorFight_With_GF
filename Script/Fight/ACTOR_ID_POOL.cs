using System;
namespace Aquila.Fight.Actor
{
    /// <summary>
    /// ActorID池
    /// </summary>
    public class ACTOR_ID_POOL
    {
        public const int Invalid = -1;

        /// <summary>
        /// id起始值
        /// </summary>
        private static int initID = int.MaxValue;

        /// <summary>
        /// 生成一个ActorID
        /// </summary>
        public static int Gen () => initID--;
    }
}
