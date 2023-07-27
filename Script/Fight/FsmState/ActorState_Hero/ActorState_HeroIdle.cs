using Aquila.Toolkit;
using UnityEngine.Playables;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 待机状态
    /// </summary>
    public class ActorState_HeroIdle : ActorState_Base
    {
        public override void OnEnter( object param )
        {
            GameEntry.Timeline.Play( Tools.Actor.CommonIdleTimelineAssetPath(), Tools.GetComponent<PlayableDirector>( _actor.transform ) );
        }

        public ActorState_HeroIdle( int stateID ) : base( stateID )
        { }
    }
}
