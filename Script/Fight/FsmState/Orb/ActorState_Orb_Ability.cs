using Aquila.Combat;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework;
using UnityEngine;

namespace Aquila.Fight.FSM
{
    /// <summary>
    /// 法球技能状态，描述法球命中后的行为
    /// </summary>
    public class ActorState_Orb_Ability : ActorState_Base
    {
        public ActorState_Orb_Ability(int state_id) : base(state_id)
        {
        }

        public override void OnEnter(object param)
        {
            if (!(param is OrbAbilityStateParam))
            {
                Tools.Logger.Error("<color=red>ActorState_Orb_Ability.OnEnter--->param is not OrbAbilityStateParam</color>");
                return;
            }

            var abilityInfo = ((OrbAbilityStateParam)param);
            var id = abilityInfo.AbilityID;
            _castorID = abilityInfo.CastorActorID;
            _targetID = abilityInfo.TargetActorID;
            _position = abilityInfo.Position;
            
            if (!GameEntry.AbilityPool.TryGetAbility(id, out _abilityData))
            {
                Tools.Logger.Error($"<color=red>ActorState_Orb_Ability.OnEnter--->Ability not found in pool, id:{id}</color>");
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            _passedTime += deltaTime;
            if (_passedTime >= _abilityData.GetTimelineDuration())
            {
                var module = GameEntry.Module.GetModule<Module_Combat>();
                var castResult = module.RequestCast(CastCmd.CreateWithSingleTarget(_castorID, _targetID, _abilityData.GetId()));
                if (!castResult.Accepted)
                    Tools.Logger.Warning($"<color=yellow>ActorState_Orb_Ability.OnUpdate()--->RequestCast rejected, castor:{_castorID}, target:{_targetID}, ability:{_abilityData.GetId()}, code:{castResult.PrimaryCode}</color>");

                Clear();
                //触发完直接隐藏
                GameEntry.Entity.HideEntity(_castorID);
            }
        }

        public override void OnLeave(object param)
        {
            _abilityData = default;
            Clear();
        }

        private void Clear()
        {
            _passedTime = 0f;
            _castorID = -1;
            _targetID = -1;
            _position = GameEntry.GlobalVar.InvalidPosition;
        }

        /// <summary>
        /// 技能数据
        /// </summary>
        private AbilityData _abilityData;
        
        /// <summary>
        /// 该状态内经过的时间
        /// </summary>
        private float _passedTime;

        /// <summary>
        /// 施法者ID，为自己
        /// </summary>
        private int _castorID = -1;
        
        /// <summary>
        /// 作用的目标ID
        /// </summary>
        private int _targetID = -1;
        
        /// <summary>
        /// 生效位置
        /// </summary>
        private Vector3 _position = GameEntry.GlobalVar.InvalidPosition;
        
    }

    public class OrbAbilityStateParam : IReference
    {
        public void Clear()
        {
            AbilityID     = -1;
            TargetActorID = -1;
            CastorActorID = -1;
            Position = GameEntry.GlobalVar.InvalidPosition;
        }

        public int TargetActorID = -1;
        public int CastorActorID = -1;
        public int AbilityID     = -1;
        public Vector3 Position = GameEntry.GlobalVar.InvalidPosition;
    }
}
