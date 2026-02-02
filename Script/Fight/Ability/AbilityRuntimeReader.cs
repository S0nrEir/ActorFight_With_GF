// using System;
// using System.IO;
// using Aquila.Tools;
// using Cfg.Enum;
// using UnityEngine;
//
// namespace Aquila.Fight
// {
//     /// <summary>
//     /// 运行时技能数据
//     /// </summary>
//     public class RuntimeAbilityData
//     {
//         public int AbilityId;
//         public int CostEffectId;
//         public int CoolDownEffectId;
//         public AbilityTargetType TargetType;
//         public int TimelineId;
//         public float TimelineDuration;
//         public RuntimeTrackData[] Tracks;
//     }
//
//     /// <summary>
//     /// 运行时轨道数据
//     /// </summary>
//     public class RuntimeTrackData
//     {
//         public RuntimeClipData[] Clips;
//     }
//
//     /// <summary>
//     /// 运行时 Clip 基类
//     /// </summary>
//     public abstract class RuntimeClipData
//     {
//         public int ClipType;
//         public float StartTime;
//         public float EndTime;
//     }
//
//     /// <summary>
//     /// Effect Clip 数据
//     /// </summary>
//     public class RuntimeEffectClipData : RuntimeClipData
//     {
//         public int EffectId;
//     }
//
//     /// <summary>
//     /// Audio Clip 数据
//     /// </summary>
//     public class RuntimeAudioClipData : RuntimeClipData
//     {
//         public string AudioPath;
//         public float Volume;
//         public bool Loop;
//         public float FadeInDuration;
//         public float FadeOutDuration;
//     }
//
//     /// <summary>
//     /// VFX Clip 数据
//     /// </summary>
//     public class RuntimeVFXClipData : RuntimeClipData
//     {
//         public string VfxPath;
//         public string AttachPoint;
//         public Vector3 PositionOffset;
//         public Vector3 RotationOffset;
//         public Vector3 Scale;
//         public bool FollowAttachPoint;
//     }
//
//     /// <summary>
//     /// 技能二进制数据运行时读取器
//     /// </summary>
//     public static class AbilityRuntimeReader
//     {
//         private const string MAGIC = "ABLT";
//         private const byte SUPPORTED_VERSION = 0x01;
//
//         public static RuntimeAbilityData Read(byte[] data)
//         {
//             using (var reader = new ByteReader(data))
//             {
//                 return ReadInternal(reader);
//             }
//         }
//
//         public static RuntimeAbilityData Read(Stream stream)
//         {
//             using (var reader = new ByteReader(stream))
//             {
//                 return ReadInternal(reader);
//             }
//         }
//
//         private static RuntimeAbilityData ReadInternal(ByteReader reader)
//         {
//             // Header
//             string magic = reader.ReadFixedString(4);
//             if (magic != MAGIC)
//                 throw new InvalidDataException($"Invalid magic: {magic}, expected: {MAGIC}");
//
//             byte version = reader.ReadByte();
//             if (version != SUPPORTED_VERSION)
//                 throw new InvalidDataException($"Unsupported version: {version}, expected: {SUPPORTED_VERSION}");
//
//             // Basic Info
//             var abilityData = new RuntimeAbilityData
//             {
//                 AbilityId = reader.ReadInt32(),
//                 CostEffectId = reader.ReadInt32(),
//                 CoolDownEffectId = reader.ReadInt32(),
//                 TargetType = (AbilityTargetType)reader.ReadInt32(),
//                 TimelineId = reader.ReadInt32(),
//                 TimelineDuration = reader.ReadSingle()
//             };
//
//             // Tracks
//             int trackCount = reader.ReadInt32();
//             abilityData.Tracks = new RuntimeTrackData[trackCount];
//
//             for (int t = 0; t < trackCount; t++)
//             {
//                 abilityData.Tracks[t] = ReadTrack(reader);
//             }
//
//             return abilityData;
//         }
//
//         private static RuntimeTrackData ReadTrack(ByteReader reader)
//         {
//             var track = new RuntimeTrackData();
//
//             int clipCount = reader.ReadInt32();
//             track.Clips = new RuntimeClipData[clipCount];
//
//             for (int c = 0; c < clipCount; c++)
//             {
//                 track.Clips[c] = ReadClip(reader);
//             }
//
//             return track;
//         }
//
//         private static RuntimeClipData ReadClip(ByteReader reader)
//         {
//             int clipType = reader.ReadInt32();
//             float startTime = reader.ReadSingle();
//             float endTime = reader.ReadSingle();
//
//             RuntimeClipData clip;
//
//             switch (clipType)
//             {
//                 case 1: // Effect
//                     clip = ReadEffectClip(reader);
//                     break;
//                 case 2: // Audio
//                     clip = ReadAudioClip(reader);
//                     break;
//                 case 3: // VFX
//                     clip = ReadVFXClip(reader);
//                     break;
//                 default:
//                     Debug.LogWarning($"[AbilityRuntimeReader] Unknown clip type: {clipType}");
//                     clip = new RuntimeEffectClipData();
//                     break;
//             }
//
//             clip.ClipType = clipType;
//             clip.StartTime = startTime;
//             clip.EndTime = endTime;
//
//             return clip;
//         }
//
//         private static RuntimeEffectClipData ReadEffectClip(ByteReader reader)
//         {
//             return new RuntimeEffectClipData
//             {
//                 EffectId = reader.ReadInt32()
//             };
//         }
//
//         private static RuntimeAudioClipData ReadAudioClip(ByteReader reader)
//         {
//             return new RuntimeAudioClipData
//             {
//                 AudioPath = reader.ReadString(),
//                 Volume = reader.ReadSingle(),
//                 Loop = reader.ReadBoolean(),
//                 FadeInDuration = reader.ReadSingle(),
//                 FadeOutDuration = reader.ReadSingle()
//             };
//         }
//
//         private static RuntimeVFXClipData ReadVFXClip(ByteReader reader)
//         {
//             return new RuntimeVFXClipData
//             {
//                 VfxPath = reader.ReadString(),
//                 AttachPoint = reader.ReadString(),
//                 PositionOffset = reader.ReadVector3(),
//                 RotationOffset = reader.ReadVector3(),
//                 Scale = reader.ReadVector3(),
//                 FollowAttachPoint = reader.ReadBoolean()
//             };
//         }
//     }
// }
