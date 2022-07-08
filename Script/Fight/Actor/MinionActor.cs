using GameFramework;
using GameFramework.Event;
using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using MRG.Fight.FSM;
using MRG.Fight.Addon;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 随从类actor
    /// </summary>
    public class MinionActor :
        DynamicActor,
        INavMoveBehavior,
        ISwitchStateBehavior,
        IDoAbilityBehavior,
        ITakeDamageBehavior
    {
        #region public methods

        //#todo抽离setup
        public void Setup (int roleBaseID)
        {
        }

        #endregion

        #region impl
        public void DoAbilityAction ( )
        {
        }

        public void MoveTo ( float targetX, float targetZ )
        {
            throw new System.NotImplementedException();
        }

        public void SwitchTo ( ActorStateTypeEnum stateType, object[] enterParam, object[] existParam )
        {
            throw new System.NotImplementedException();
        }

        public void TakeDamage ( int dmg )
        {
            throw new System.NotImplementedException();
        }


        #endregion

        /// <summary>
        /// 到了目标处
        /// </summary>
        private void OnNavArriveTarget ( int eventID, object[] param )
        {
            if (_FsmAddon is null)
                return;

            switch (_FsmAddon.CurrState)
            {
                //索敌到了目标点
                //case ActorStateTypeEnum.SEARCHING_STATE:
                //    SwitchTo( ActorStateTypeEnum.ABILITY_STATE, null, null );
                //    break;

                case ActorStateTypeEnum.ABILITY_STATE:
                    break;
            }
        }

        #region override

        protected override void OnShow ( object userData )
        {
            base.OnShow( userData );
            RegisterActorEvent( ActorEventEnum.NAV_ARRIVE_TARGET, OnNavArriveTarget );
        }

        protected override void OnHide ( bool isShutdown, object userData )
        {
            base.OnHide( isShutdown, userData );
        }

        protected override void Register ()
        {
            base.Register();
        }

        protected override void UnRegister ()
        {
            base.UnRegister();
        }

        protected override void ResetData ()
        {
            base.ResetData();
            //if (_dataAddon is null)
            //    throw new GameFrameworkException( "MINION (_DataAddon is null)" );

            ////#todo添加数据
            //var roleBaseData = _dataAddon.GetObjectDataValue<Tab_RoleBaseAttr>(DataAddonFieldTypeEnum.OBJ_META_ROLEBASE);
            //if (roleBaseData is null)
            //    throw new GameFrameworkException( "roleBaseData is null" );

            //// 血量上限
            //_dataAddon.SetIntDataValue( DataAddonFieldTypeEnum.INT_MAX_HP, roleBaseData.MaxHP );
            ////当前血量
            //_dataAddon.SetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, roleBaseData.MaxHP );
            ////移速
            //_dataAddon.SetIntDataValue( DataAddonFieldTypeEnum.INT_MOVE_SPEED, roleBaseData.MoveSpeed );
            ////攻
            //_dataAddon.SetIntDataValue( DataAddonFieldTypeEnum.INT_ACK, roleBaseData.Attack );
            ////防
            //_dataAddon.SetIntDataValue( DataAddonFieldTypeEnum.INT_DEF, roleBaseData.Defense );
            ////警戒范围
            //_dataAddon.SetFloatDataValue( DataAddonFieldTypeEnum.FLOAT_ALERT_RADIUS, roleBaseData.AlertRadius );
        }

        public override ActorTypeEnum ActorType => ActorTypeEnum.MINION;

        protected override void OnRecycle ()
        {
            base.OnRecycle();
            UnRegisterActorEvent( ActorEventEnum.NAV_ARRIVE_TARGET );
            Reset();
        }

        protected override void OnUpdate ( float elapseSeconds, float realElapseSeconds )
        {
            base.OnUpdate( elapseSeconds, realElapseSeconds );
            _FsmAddon?.OnUpdateDate( elapseSeconds, realElapseSeconds );
        }

        protected override void InitAddons ()
        {
            base.InitAddons();
            _FsmAddon       = AddAddon<MinionStateAddon>();
            _ProcessorAddon = AddAddon<ProcessorAddon>();
            _AnimAddon      = AddAddon<AnimAddon>();
            _MoveAddon      = AddAddon<MoveAddon>();
            _HPAddon        = AddAddon<InfoBoardAddon>();
            _NavAddon       = AddAddon<NavAddon>();
        }

        #endregion

        private FSMAddon _FsmAddon { get; set; } = null;
        private ProcessorAddon _ProcessorAddon { get; set; } = null;
        private AnimAddon _AnimAddon { get; set; } = null;
        private MoveAddon _MoveAddon { get; set; } = null;
        private InfoBoardAddon _HPAddon { get; set; } = null;
        private NavAddon _NavAddon { get; set; } = null;
    }

}