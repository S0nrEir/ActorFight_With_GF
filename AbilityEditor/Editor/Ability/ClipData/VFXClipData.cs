using System;
using UnityEngine;

namespace Aquila.AbilityEditor
{
    /// <summary>
    /// 特效Clip数据类
    /// 用于在timeline中表示特效/粒子效果的播放
    /// </summary>
    [Serializable]
    public class VFXClipData : TimelineClipData
    {
        /// <summary>
        /// 特效资源路径或ID
        /// </summary>
        [SerializeField]
        private string _vfxPath;

        /// <summary>
        /// 特效挂载点（骨骼名称或挂载点名称）
        /// </summary>
        [SerializeField]
        private string _attachPoint;

        /// <summary>
        /// 位置偏移
        /// </summary>
        [SerializeField]
        private Vector3 _positionOffset;

        /// <summary>
        /// 旋转偏移
        /// </summary>
        [SerializeField]
        private Vector3 _rotationOffset;

        /// <summary>
        /// 缩放
        /// </summary>
        [SerializeField]
        private Vector3 _scale;

        /// <summary>
        /// 是否跟随挂载点
        /// </summary>
        [SerializeField]
        private bool _followAttachPoint;

        public override TimelineClipType ClipType => TimelineClipType.VFX;

        public VFXClipData()
        {
            _vfxPath = string.Empty;
            _attachPoint = string.Empty;
            _positionOffset = Vector3.zero;
            _rotationOffset = Vector3.zero;
            _scale = Vector3.one;
            _followAttachPoint = true;
            ClipColor = new Color(1f, 0.6f, 0.2f); // 橙色
        }

        public VFXClipData(string clipName, float startTime, float endTime, string vfxPath)
            : base(clipName, startTime, endTime, new Color(1f, 0.6f, 0.2f))
        {
            _vfxPath = vfxPath;
            _attachPoint = string.Empty;
            _positionOffset = Vector3.zero;
            _rotationOffset = Vector3.zero;
            _scale = Vector3.one;
            _followAttachPoint = true;
        }

        #region Properties

        public string VfxPath
        {
            get => _vfxPath;
            set => _vfxPath = value;
        }

        public string AttachPoint
        {
            get => _attachPoint;
            set => _attachPoint = value;
        }

        public Vector3 PositionOffset
        {
            get => _positionOffset;
            set => _positionOffset = value;
        }

        public Vector3 RotationOffset
        {
            get => _rotationOffset;
            set => _rotationOffset = value;
        }

        public Vector3 Scale
        {
            get => _scale;
            set => _scale = value;
        }

        public bool FollowAttachPoint
        {
            get => _followAttachPoint;
            set => _followAttachPoint = value;
        }

        #endregion

        #region Override Methods

        public override TimelineClipData Clone()
        {
            var clone = new VFXClipData();
            CopyBaseTo(clone);
            clone._vfxPath = _vfxPath;
            clone._attachPoint = _attachPoint;
            clone._positionOffset = _positionOffset;
            clone._rotationOffset = _rotationOffset;
            clone._scale = _scale;
            clone._followAttachPoint = _followAttachPoint;
            return clone;
        }

        public override bool Validate(out string errorMessage)
        {
            if (!base.Validate(out errorMessage))
                return false;

            if (string.IsNullOrEmpty(_vfxPath))
            {
                errorMessage = "VFX path cannot be empty";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public override string GetDisplayInfo()
        {
            string attachInfo = !string.IsNullOrEmpty(_attachPoint) ? $" @ {_attachPoint}" : "";
            return $"VFX: {ClipName}{attachInfo} [{StartTime:F2}s - {EndTime:F2}s]";
        }

        #endregion
    }
}
