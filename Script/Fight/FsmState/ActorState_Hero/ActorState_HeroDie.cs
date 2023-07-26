using Aquila.Fight.Addon;
using Aquila.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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
        }

        public override void OnUpdate( float deltaTime )
        {
            base.OnUpdate( deltaTime );
        }
    }
}