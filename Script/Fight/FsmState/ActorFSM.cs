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
        /// <returns></returns>
        public ActorInstance GetActorInstance()
        {
            return _instance;
        }

        /// <summary>
        /// 状态切换
        /// </summary>
        public void SwitchTo ( int target_state_id, object enter_param, object exit_param )
        {
            if (_currState != null)
                _currState.OnLeave(exit_param);

            var next = GetState( target_state_id );
            if (next is null)
            {
                Debug.LogError( $"<color=red>cant find state with id:{target_state_id}/color>" );
                return;
            }

            _currState = next;
            _currState.OnEnter( enter_param );
        }

        /// <summary>
        /// 添加状态
        /// </summary>
        public bool AddState ( ActorStateBase state )
        {
            if (state is null)
                return false;

            if (GetState( state._stateID ) != null)
            {
                Debug.LogError("has same state! id:"+ state);
                return false;
            }

            state.Init( this, _actor );
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
        public ActorStateBase[] AllState ()
        {
            var stateArr = new ActorStateBase[_stateDic.Count];
            var idx = 0;
            var iter = _stateDic.GetEnumerator();

            while (iter.MoveNext())
                stateArr[idx++] = iter.Current.Value;

            return stateArr;
        }

        private ActorStateBase GetState ( int stateID )
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

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="actor"></param>
        //public void Setup ( TActorBase actor )
        //{
        //    _stateDic = new Dictionary<int, ActorStateBase>();
        //    _actor = actor;
        //}

        public void Setup( ActorInstance actorIns )
        {
            _stateDic = new Dictionary<int, ActorStateBase>();
            _instance = actorIns;
        }

        public void Dispose ()
        {
            if (_stateDic != null)
            {
                ActorStateBase state = null;
                foreach (var item in _stateDic)
                {
                    state = item.Value;
                    if (state != null)
                        state.Dispose();
                }

                _stateDic.Clear();
                _stateDic = null;
                _actor = null;
                _currState = null;
                _instance = null;
            }
        }

        /// <summary>
        /// 当前运行中状态
        /// </summary>
        public ActorStateBase CurrState
        {
            get { return _currState; }
        }

        /// <summary>
        /// 当前的指向状态
        /// </summary>
        protected ActorStateBase _currState;

        /// <summary>
        /// FSM持有的状态集合
        /// </summary>
        private Dictionary<int, ActorStateBase> _stateDic;

        private TActorBase _actor;

        private ActorInstance _instance = null;
    }

}

