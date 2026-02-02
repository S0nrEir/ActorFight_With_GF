using UnityEngine;

namespace Aquila.Fight
{
    /// <summary>
    /// 特效数据
    /// </summary>
    public struct VFXData
    {
        private float _startTime;
        private float _endTime;
        private string _vfxPath;
        private string _attachPoint;
        private Vector3 _positionOffset;
        private Vector3 _rotationOffset;
        private Vector3 _scale;
        private bool _followAttachPoint;

        public VFXData(
            float startTime,
            float endTime,
            string vfxPath,
            string attachPoint,
            Vector3 positionOffset,
            Vector3 rotationOffset,
            Vector3 scale,
            bool followAttachPoint)
        {
            _startTime = startTime;
            _endTime = endTime;
            _vfxPath = vfxPath;
            _attachPoint = attachPoint;
            _positionOffset = positionOffset;
            _rotationOffset = rotationOffset;
            _scale = scale;
            _followAttachPoint = followAttachPoint;
        }

        public float GetStartTime() => _startTime;
        public float GetEndTime() => _endTime;
        public string GetVfxPath() => _vfxPath;
        public string GetAttachPoint() => _attachPoint;
        public Vector3 GetPositionOffset() => _positionOffset;
        public Vector3 GetRotationOffset() => _rotationOffset;
        public Vector3 GetScale() => _scale;
        public bool GetFollowAttachPoint() => _followAttachPoint;

        public void SetStartTime(float value) => _startTime = value;
        public void SetEndTime(float value) => _endTime = value;
        public void SetVfxPath(string value) => _vfxPath = value;
        public void SetAttachPoint(string value) => _attachPoint = value;
        public void SetPositionOffset(Vector3 value) => _positionOffset = value;
        public void SetRotationOffset(Vector3 value) => _rotationOffset = value;
        public void SetScale(Vector3 value) => _scale = value;
        public void SetFollowAttachPoint(bool value) => _followAttachPoint = value;
    }
}
