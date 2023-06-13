using Aquila.Fight.Actor;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 描述actor的状态
    /// </summary>
    public abstract class ActorStateBase
    {
        public ActorStateBase( int state_id )
        {
            _stateID = state_id;
        }

        public virtual void Dispose()
        {
            _fsm = null;
            _actor = null;
            _stateID = -1;
        }

        public virtual void OnUpdate( float delta_time )
        {

        }

        public virtual void OnLeave( params object[] param )
        {

        }

        public virtual void OnEnter( params object[] param )
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init( ActorFSM fsm, TActorBase actor )
        {
            _fsm = fsm;
            _actor = actor;
        }

        /// <summary>
        /// 状态ID
        /// </summary>
        public int _stateID = -1;

        /// <summary>
        /// 自己master的引用
        /// </summary>
        protected ActorFSM _fsm;

        /// <summary>
        /// acotr
        /// </summary>
        protected TActorBase _actor;
    }

    //-----------------------sub state-----------------------
    #region abilityState
    //
    // /// <summary>
    // /// 技能state
    // /// </summary>
    // public abstract class ActorAbilityStateBase : ActorStateBase
    // {
    //     public ActorAbilityStateBase ( int state_id ) : base ( state_id ) { }
    //
    //     public override void Init ( ActorFSM fsm, TActorBase actor )
    //     {
    //         base.Init ( fsm, actor );
    //     }
    //
    //     public override void OnEnter ( params object[] param )
    //     {
    //         base.OnEnter ( param );
    //     }
    //
    //     public override void OnLeave ( params object[] param )
    //     {
    //         base.OnLeave ( param );
    //     }
    //
    //     public override void OnUpdate( float deltaTime )
    //     {
    //         base.OnUpdate( deltaTime );
    //     }
    //
    //     /// <summary>
    //     /// 完成标记
    //     /// </summary>
    //     protected bool _finishFlag = false;
    //
    //     /// <summary>
    //     /// 技能释放时间
    //     /// </summary>
    //     protected float _timer = 0f;
    //
    // }

    #endregion

    #region IdleState

    // public abstract class ActorIdleStateBase : ActorStateBase
    // {
    //     public ActorIdleStateBase ( int stateID ) : base ( stateID )
    //     { }
    //
    //     public override void Init ( ActorFSM fsm, TActorBase actor )
    //     {
    //         base.Init ( fsm, actor );
    //     }
    //
    //     public override void OnEnter ( params object[] param )
    //     {
    //         base.OnEnter ( param );
    //         //if ( _actor.TryGetAddon<Addon_Anim> ( out _animAddon ) )
    //         //    _animAddon.PlayStandAnim ();
    //
    //         //if ( _actor.TryGetAddon<Addon_Nav> ( out var navAddon ) )
    //         //    navAddon.StopInmidiate ();
    //     }
    //
    //     public override void OnLeave ( params object[] param )
    //     {
    //         base.OnLeave ( param );
    //         _animAddon = null;
    //     }
    //
    //     public override void OnUpdate ( float deltaTime )
    //     {
    //         base.OnUpdate ( deltaTime );
    //     }
    //
    //     private Addon_Anim _animAddon = null;
    // }

    #endregion

    #region MoveState

    // public class ActorMoveStateBase : ActorStateBase
    // {
    //     public ActorMoveStateBase ( int stateID ) : base ( stateID )
    //     {
    //     }
    //
    //     public override void Init ( ActorFSM fsm, TActorBase actor )
    //     {
    //         base.Init ( fsm, actor );
    //     }
    //
    //     public override void OnEnter ( params object[] param )
    //     {
    //         base.OnEnter ( param );
    //         if ( param is null || param.Length < 1 )
    //         {
    //             _actor.Trigger ( ActorEventEnum.MOVE_TO_FINAL_POINT, null );
    //             return;
    //         }
    //
    //         //#todo有的obj没有动画，先这样处理
    //         //if ( _actor.TryGetAddon<Addon_Anim> ( out var addon ) && addon.CurrClipName != "Run" )
    //         //    addon.PlayRunAnim ();
    //
    //         //if ( !_actor.TryGetAddon ( out _moveAddon ) )
    //         //    throw new GameFrameworkException ( "!_actor.TryGetAddon<MoveAddon>( out _moveAddon )" );
    //
    //         //if ( !_actor.TryGetAddon ( out _navAddon ) )
    //         //    throw new GameFrameworkException ( "!_actor.TryGetAddon<NavAddon>( out _moveAddon )" );
    //
    //         //var listArr = param[0] as object[];
    //         //IList<float> xList = listArr[0] as IList<float>;
    //         //IList<float> zList = listArr[1] as IList<float>;
    //
    //         //OnNavAddonEnter ( xList, zList );
    //         //return;
    //
    //         //OnMoveAddonEnter( xList, zList );
    //     }
    //
    //     /// <summary>
    //     /// navAddon的处理
    //     /// </summary>
    //     private void OnNavAddonEnter ( IList<float> xList, IList<float> zList )
    //     {
    //         //_actor.SetWorldPosition( new Vector3( xList[0], Utils.FightScene.TerrainPositionY( xList[0], zList[0], 0f ), zList[0] ) );
    //         //_navAddon.SamplePosition(_actor.CachedTransform.position);
    //         _navAddon.SetDestination ( new Vector3 ( xList[xList.Count - 1], Tools.Fight.TerrainPositionY (string.Empty, xList[xList.Count - 1], zList[zList.Count - 1], 0f ), zList[zList.Count - 1] ) );//#todo修改layer
    //     }
    //
    //     /// <summary>
    //     /// moveAddon的处理
    //     /// </summary>
    //     private void OnMoveAddonEnter ( IList<float> xList, IList<float> zList )
    //     {
    //         _actor.SetWorldPosition ( new Vector3 ( xList[0], Tools.Fight.TerrainPositionY (string.Empty,  xList[0], zList[0], 0f ), zList[0] ) );
    //
    //         var xCnt = xList.Count;
    //         var zCnt = zList.Count;
    //
    //         var pos = _actor.transform.position;
    //         var pathList = new List<Vector2> ();
    //         for ( var i = 0; i < xCnt && i < zCnt; i++ )
    //             pathList.Add ( new Vector2 ( xList[i], zList[i] ) );
    //
    //         _moveAddon.SetTargetPahtList ( pathList );
    //     }
    //
    //     public override void OnLeave ( params object[] param )
    //     {
    //         base.OnLeave ( param );
    //         OnNavAddonLeave ();
    //         _moveAddon = null;
    //         _navAddon = null;
    //     }
    //
    //     public override void OnUpdate ( float deltaTime )
    //     {
    //         base.OnUpdate ( deltaTime );
    //
    //         //OnMoveAddonUpdate(deltaTime);
    //         //return;
    //
    //         OnNavAddonUpdate ( deltaTime );
    //     }
    //
    //     /// <summary>
    //     /// 导航组件离开
    //     /// </summary>
    //     private void OnNavAddonLeave ()
    //     {
    //         if ( _navAddon is null )
    //             return;
    //
    //         //可能会导致位置不同步的问题
    //         //_navAddon.StopInmidiate();
    //     }
    //
    //     /// <summary>
    //     /// 导航组件update
    //     /// </summary>
    //     private void OnNavAddonUpdate ( float deltaTime )
    //     {
    //         if ( _navAddon.IsReachDestination () )
    //             _actor.Trigger ( ActorEventEnum.MOVE_TO_FINAL_POINT, null );
    //     }
    //
    //     /// <summary>
    //     /// 移动组件update
    //     /// </summary>
    //     private void OnMoveAddonUpdate ( float deltaTime )
    //     {
    //         if ( _moveAddon.IsReachedFinalTarget () )
    //         {
    //             _actor.Trigger ( ActorEventEnum.MOVE_TO_FINAL_POINT, null );
    //             return;
    //         }
    //
    //         _moveAddon.TargetNext ( deltaTime );
    //     }
    //
    //     private Addon_Move _moveAddon = null;
    //     private Addon_Nav _navAddon = null;
    // }

    #endregion

    #region DieState

    // public class ActorDieStateBase : ActorStateBase
    // {
    //     public ActorDieStateBase ( int state_id ) : base ( state_id )
    //     { }
    //
    //     public override void Init ( ActorFSM fsm, TActorBase actor )
    //     {
    //         base.Init ( fsm, actor );
    //     }
    //
    //     public override void OnEnter ( params object[] param )
    //     {
    //         base.OnEnter ( param );
    //         //if ( _actor.TryGetAddon<Addon_Anim> ( out _animAddon ) )
    //         //    _animAddon.PlayDieAnim();
    //
    //         //if ( _actor.TryGetAddon<Addon_InfoBoard> ( out var sliderAddon ) )
    //         //{
    //         //    sliderAddon.ChangeHPValue( 0 );
    //         //    sliderAddon.HideAll();
    //         //}
    //
    //         // GameEntry.Event.Fire ( this, ReferencePool.Acquire<ActorDieEventArgs> ().Fill ( _actor ) );
    //     }
    //
    //     public override void OnLeave ( params object[] param )
    //     {
    //         base.OnLeave ( param );
    //         _animAddon = null;
    //     }
    //
    //     public override void OnUpdate ( float deltaTime )
    //     {
    //         base.OnUpdate ( deltaTime );
    //     }
    //
    //     private Addon_Anim _animAddon = null;
    // }

    #endregion
}
