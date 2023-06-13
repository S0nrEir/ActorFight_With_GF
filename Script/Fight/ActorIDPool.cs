using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// ActorID池
    /// </summary>
    public class ActorIDPool
    {
        public const int Invalid = -1;

        /// <summary>
        /// id起始值
        /// </summary>
        private static int initID = int.MaxValue;

        /// <summary>
        /// 生成一个ActorID
        /// </summary>
        public static int Gen()
        {
            if ( initID < 0 )
            {
                Log.Error( "Init ID < 0!" );
                return -1;
            }

            return initID--;
        }
    }
}
