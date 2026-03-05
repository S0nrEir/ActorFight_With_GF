// using UnityEngine;
//
// namespace Aquila.Fight
// {
//     /// <summary>
//     /// 特效数据（不可变结构体）
//     /// </summary>
//     public readonly struct VFXData
//     {
//         private readonly float _startTime;
//         private readonly float _endTime;
//         private readonly string _vfxPath;
//         private readonly string _attachPoint;
//         private readonly Vector3 _positionOffset;
//         private readonly Vector3 _rotationOffset;
//         private readonly Vector3 _scale;
//         private readonly bool _followAttachPoint;
//
//         public VFXData(
//             float startTime,
//             float endTime,
//             string vfxPath,
//             string attachPoint,
//             Vector3 positionOffset,
//             Vector3 rotationOffset,
//             Vector3 scale,
//             bool followAttachPoint)
//         {
//             _startTime = startTime;
//             _endTime = endTime;
//             _vfxPath = vfxPath;
//             _attachPoint = attachPoint;
//             _positionOffset = positionOffset;
//             _rotationOffset = rotationOffset;
//             _scale = scale;
//             _followAttachPoint = followAttachPoint;
//         }
//
//         // Getter 方法
//         public float GetStartTime() => _startTime;
//         public float GetEndTime() => _endTime;
//         public string GetVfxPath() => _vfxPath;
//         public string GetAttachPoint() => _attachPoint;
//         public Vector3 GetPositionOffset() => _positionOffset;
//         public Vector3 GetRotationOffset() => _rotationOffset;
//         public Vector3 GetScale() => _scale;
//         public bool GetFollowAttachPoint() => _followAttachPoint;
//     }
// }
