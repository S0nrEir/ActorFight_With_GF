using System.Diagnostics;
using GameFramework;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.TestTools;

namespace Aquila.Timeline
{
    public class PlayableAsset_Base<T> : PlayableAsset where T: PlayableBehaviour_Base,new()
    {
        public override Playable CreatePlayable( PlayableGraph graph, GameObject owner )
        {
            var errMsg = AssetValid();
            if (!string.IsNullOrEmpty(errMsg))
                throw new GameFrameworkException(errMsg);

            var bhvr = new T();
            bhvr._asset = this;
            return ScriptPlayable<T>.Create(graph, bhvr);
        }
        
        
        
        /// <summary>
        /// playable asset是否有效
        /// </summary>
        protected virtual string AssetValid()
        {
            return string.Empty;
        }
    }
}

