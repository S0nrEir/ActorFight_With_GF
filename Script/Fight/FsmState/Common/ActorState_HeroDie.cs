using Aquila.Toolkit;
using UnityEngine.Playables;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 死亡状态
    /// </summary>
    public class ActorState_HeroDie : ActorState_Base
    {
        public ActorState_HeroDie( int stateID ) : base( stateID )
        {
        }

        public override void OnEnter( object param )
        {
            GameEntry.Timeline.Play( Tools.Actor.CommonDieTimelineAssetPath(), Tools.GetComponent<PlayableDirector>( _actor.transform ) );
            Log.Info($"actor die!actor:{_actor.ActorID}");
        }

        public override void OnUpdate( float deltaTime )
        {
            base.OnUpdate( deltaTime );
        }
    }
}