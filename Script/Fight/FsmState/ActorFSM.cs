using Aquila.Fight.Actor;
using System.Collections.Generic;
using UnityEngine;
using static Aquila.Module.Module_ProxyActor;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// Actor的FSM，为什么没用GF的FSM，因为Actor想要通过自身来对状态进行控制，而不是像GF的FSM那样以状态为主导，连Actor自身的逻辑都要包含在GFFSM中 
    /// </summary>
    public class ActorFSM
    {
        /// <summary>
        /// 获取actorInstance
        /// </summary>
        public ActorInstance GetActorInstance()
        {
            return _instance;
        }

        /// <summary>
        /// 状态切换
        /// </summary>
        public void SwitchTo ( int targetStateID, object enterParam, object exitParam )
        {
            if (_currState != null)
                _currState.OnLeave(exitParam);

            var next = GetState( targetStateID );
            if (next is null)
            {
                Debug.LogError( $"<color=red>cant find state with id:{targetStateID}/color>" );
                return;
            }

            _currState = next;
            _currState.OnEnter( enterParam );
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        public bool AddState ( ActorState_Base state )
        {
            if (state is null)
                return false;

            if (GetState( state._stateID ) != null)
            {
                Debug.LogError("has same state! id:"+ state);
                return false;
            }

            state.Init( this, _instance.Actor );
            _stateDic.Add( state._stateID, state );
            return true;
        }

        /// <summary>
        /// 是否有某个state
        /// </summary>
        public bool HasState ( int state_id )
        {
            if (_stateDic.Count == 0)
                return false;

            return _stateDic.TryGetValue( state_id, out var _ );
        }

        /// <summary>
        /// 获取该状态机持有的所有状态
        /// </summary>
        public ActorState_Base[] AllState ()
        {
            var stateArr = new ActorState_Base[_stateDic.Count];
            var idx = 0;
            var iter = _stateDic.GetEnumerator();

            while (iter.MoveNext())
                stateArr[idx++] = iter.Current.Value;

            return stateArr;
        }

        private ActorState_Base GetState ( int stateID )
        {
            if (_stateDic is null)
            {
                Debug.LogError( "state dic is nulll!" );
                return null;
            }

            _stateDic.TryGetValue( stateID, out var state );
            return state;
        }

        public void Update ( float deltaTime )
        {
            _currState?.OnUpdate( deltaTime );
        }

        public void Setup( ActorInstance actorIns )
        {
            _stateDic = new Dictionary<int, ActorState_Base>();
            _instance = actorIns;
        }

        public void Dispose ()
        {
            if (_stateDic != null)
            {
                ActorState_Base state = null;
                foreach (var item in _stateDic)
                {
                    state = item.Value;
                    if (state != null)
                        state.Dispose();
                }

                _stateDic.Clear();
                _stateDic = null;
                _currState = null;
                _instance = null;
            }
        }

        /// <summary>
        /// 当前运行中状态
        /// </summary>
        public ActorState_Base CurrState
        {
            get { return _currState; }
        }

        /// <summary>
        /// 当前的指向状态
        /// </summary>
        protected ActorState_Base _currState;

        /// <summary>
        /// FSM持有的状态集合
        /// </summary>
        private Dictionary<int, ActorState_Base> _stateDic;

        /// <summary>
        /// actor实例
        /// </summary>
        private ActorInstance _instance = null;
    }

}

