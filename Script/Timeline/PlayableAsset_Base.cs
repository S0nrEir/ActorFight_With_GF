using UnityEngine;
using UnityEngine.Playables;

namespace Aquila.Timeline
{
    public class PlayableAsset_Base : PlayableAsset
    {
        public override Playable CreatePlayable( PlayableGraph graph, GameObject owner )
        {
            return new Playable();
        }
    }
}

