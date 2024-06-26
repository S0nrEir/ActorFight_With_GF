using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using Aquila.Module;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Addon
{
    /// <summary>
    ///为什么没用GF的FSM，因为Actor想要通过自身来对状态进行控制，而不是像GF的FSM那样以状态为主导，连Actor自身的逻辑都要包含在GFFSM中
    /// </summary>
    public class Addon_FSM : Addon_Base
    {
        public ActorStateTypeEnum CurrState => ( ActorStateTypeEnum ) ActorFsm.CurrState._stateID;

        /// <summary>
        /// 转换状态
        /// </summary>
        public bool SwitchTo( int targetStateID, object enterParam, object exitParam )
        {
            //没有持有该状态
            if ( !ActorFsm.HasState( targetStateID ) )
            {
                Debug.Log( "dosent has state:" + targetStateID );
                return false;
            }

            ActorFsm.SwitchTo( targetStateID, enterParam, exitParam );

//#if UNITY_EDITOR
//            var inspector = Actor.gameObject.GetComponent<ActorInspector>();
//            if ( inspector != null )
//                inspector.SetState( ( ( ActorStateTypeEnum ) targetStateID ).ToString() );
//#endif
            return true;
        }

        /// <summary>
        /// 转换状态
        /// </summary>
        public bool SwitchTo( ActorStateTypeEnum type, object enterParam, object exitParam )
        {
            return SwitchTo( ( int ) type, enterParam, exitParam );
        }

        public override void Init( Module_ProxyActor.ActorInstance instance )
        {
            base.Init( instance );

            ActorFsm = new ActorFSM();
            ActorFsm.Setup( instance );

            foreach ( var state in StateList )
                ActorFsm.AddState( state );
        }

        /// <summary>
        /// addon type
        /// </summary>
        public override AddonTypeEnum AddonType => AddonTypeEnum.STATE_MATCHINE;

        /// <summary>
        /// on add addon
        /// </summary>
        public override void OnAdd()
        {
        }

        public virtual T GetTypedActor<T>() where T : Actor_Base
        {
            return Actor as T;
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            ActorFsm.Update( elapseSeconds );
        }

        public override void Dispose()
        {
            base.Dispose();
            //GameEntry.Timer.UnRegisterFrameLateUpdate( this );
            StateList.Clear();
            StateList = null;
        }

        public override void Reset()
        {
            base.Reset();
            // if ( StateList is null )
            // {
            //     Debug.LogError( "StateList is null" );
            //     return;
            // }

            //切换到自身第一个状态
            //SwitchTo( StateList[0]._stateID, null, null );
        }

        /// <summary>
        /// fsm
        /// </summary>
        public ActorFSM ActorFsm { get; private set; }
        
        /// <summary>
        /// state list,放在第一个位置的state将成为默认进入的state
        /// </summary>
        public virtual List<ActorState_Base> StateList { get; private set; } = new List<ActorState_Base>();

    }
}