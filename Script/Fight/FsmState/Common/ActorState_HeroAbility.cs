using Aquila.Combat;
using Aquila.Fight.Addon;
using Aquila.Module;
using GameFramework;

namespace Aquila.Fight.FSM
{
    public class ActorState_HeroAbility : ActorState_Base
    {
        public override void OnEnter(object param)
        {
            base.OnEnter(param);

            if (!(param is int abilityId))
                throw new GameFrameworkException("ActorState_HeroAbility.OnEnter param must be an ability id.");

            _abilityId = abilityId;
            _castCompleted = false;
            _abilityAddon = GameEntry.Module.GetModule<Module_ActorMgr>().Get(_actor.ActorID).GetAddon<Addon_Ability>();
            _abilityAddon.OnCastComplete += OnCastComplete;
        }

        public override void OnLeave(object param)
        {
            base.OnLeave(param);

            _abilityAddon.OnCastComplete -= OnCastComplete;
            _abilityAddon = null;

            if (!_castCompleted)
            {
                var combat = GameEntry.Module.GetModule<Module_Combat>();
                combat?.InterruptCast(_actor.ActorID, CastInterruptReason.StateChanged);
            }

            _abilityId = 0;
            _castCompleted = false;
        }

        public ActorState_HeroAbility(int stateId) : base(stateId)
        {
        }

        private void OnCastComplete(int abilityId)
        {
            if (abilityId != _abilityId)
                return;

            _castCompleted = true;
            _fsm.SwitchTo((int)ActorStateTypeEnum.IDLE_STATE, null, null);
        }

        private Addon_Ability _abilityAddon;
        private int _abilityId;
        private bool _castCompleted;
    }
}
