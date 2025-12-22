using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// Clip Inspector代理的抽象基类，clip和inspector的桥接类
    /// </summary>
    public abstract class ClipInspectorProxyBase<TClipData> : ScriptableObject
        where TClipData : TimelineClipData
    {
        /// <summary>
        /// 绑定Clip数据到代理
        /// </summary>
        public void BindClipData(TClipData clipData, TimelineClipUI clipUI, float duration)
        {
            TargetClipData = clipData;
            TargetClipUI = clipUI;
            TimelineDuration = duration;
            SyncFromClipData();
        }

        /// <summary>
        /// 从ClipData同步数据到代理对象
        /// 子类必须实现此方法来同步具体的字段
        /// </summary>
        public abstract void SyncFromClipData();

        /// <summary>
        /// 将代理对象的数据同步到ClipData
        /// 子类必须实现此方法来同步具体的字段
        /// </summary>
        public abstract void SyncToClipData();

        /// <summary>
        /// 刷新UI显示
        /// </summary>
        protected void RefreshUI()
        {
            if (TargetClipUI != null)
                TargetClipUI.Refresh();
        }

        /// <summary>
        /// 验证数据是否有效
        /// </summary>
        protected bool IsDataValid()
        {
            return TargetClipData != null && TargetClipUI != null;
        }

        /// <summary>
        /// 目标Clip数据
        /// </summary>
        [HideInInspector]
        public TClipData TargetClipData;

        /// <summary>
        /// 目标Clip的UI表示
        /// </summary>
        [HideInInspector]
        public TimelineClipUI TargetClipUI;

        /// <summary>
        /// Timeline的总时长
        /// </summary>
        [HideInInspector]
        public float TimelineDuration = 0f;
    }
}
