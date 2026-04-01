using GameFramework;
using GameFramework.Event;

namespace Aquila.Event
{
    /// <summary>
    /// 预加载项目完成事件
    /// </summary>
    public class PreloadItemCompleteEventArgs : GameEventArgs
    {
        public static readonly int EventID = typeof(PreloadItemCompleteEventArgs).GetHashCode();

        public override int Id => EventID;

        /// <summary>
        /// 预加载项目类型
        /// </summary>
        public PreloadItemType ItemType { get; private set; }

        public static PreloadItemCompleteEventArgs Create(PreloadItemType itemType)
        {
            var args = ReferencePool.Acquire<PreloadItemCompleteEventArgs>();
            args.ItemType = itemType;
            return args;
        }

        public override void Clear()
        {
            ItemType = PreloadItemType.HPBar;
        }
    }

    /// <summary>
    /// 预加载项目类型枚举
    /// </summary>
    public enum PreloadItemType
    {
        /// <summary>
        /// HPBar预制体
        /// </summary>
        HPBar = 0,

        /// <summary>
        /// 伤害数字预制体
        /// </summary>
        DamageNumber = 1
    }
}
