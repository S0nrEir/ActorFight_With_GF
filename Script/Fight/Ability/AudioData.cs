// namespace Aquila.Fight
// {
//     /// <summary>
//     /// 音频数据（不可变结构体）
//     /// </summary>
//     public readonly struct AudioData
//     {
//         private readonly float _startTime;
//         private readonly float _endTime;
//         private readonly string _audioPath;
//         private readonly float _volume;
//         private readonly bool _loop;
//         private readonly float _fadeInDuration;
//         private readonly float _fadeOutDuration;
//
//         public AudioData(
//             float startTime,
//             float endTime,
//             string audioPath,
//             float volume,
//             bool loop,
//             float fadeInDuration,
//             float fadeOutDuration)
//         {
//             _startTime = startTime;
//             _endTime = endTime;
//             _audioPath = audioPath;
//             _volume = volume;
//             _loop = loop;
//             _fadeInDuration = fadeInDuration;
//             _fadeOutDuration = fadeOutDuration;
//         }
//
//         // Getter 方法
//         public float GetStartTime() => _startTime;
//         public float GetEndTime() => _endTime;
//         public string GetAudioPath() => _audioPath;
//         public float GetVolume() => _volume;
//         public bool GetLoop() => _loop;
//         public float GetFadeInDuration() => _fadeInDuration;
//         public float GetFadeOutDuration() => _fadeOutDuration;
//     }
// }
