using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Aquila.Timeline
{
    //TrackClipType指定可放置的PlayableAsset类型
    //TrackColor指定轨道的颜色
    /// <summary>
    /// 动画轨道
    /// </summary>
    [TrackColor(0.6f,1,0.6f)]
    [TrackClipType(typeof(PlayableAsset_Anim))]
    public class Track_Anim : PlayableTrack
    {
        
    }
}
