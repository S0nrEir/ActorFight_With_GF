using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace  Aquila.Timeline
{
    [TrackColor(.3f, .5f, .7f)]
    [TrackClipType(typeof(PlayableAsset_Audio))]
    public class Track_Audio : PlayableTrack
    {
    }
}
