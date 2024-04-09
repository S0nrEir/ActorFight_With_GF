using System.Collections;
using System.Collections.Generic;
using Aquila.Module;
using Cfg.Fight;
using GameFramework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGameFramework.Runtime;

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
                Log.Error($"<color=red>ActorState_Orb_Ability.OnEnter--->param is not OrbAbilityStateParam</color>");
                return;
            }

            var abilityInfo = ((OrbAbilityStateParam)param);
            var id = abilityInfo.AbilityID;
            _castorID = abilityInfo.CastorActorID;
            _targetID = abilityInfo.TargetActorID;
            _position = abilityInfo.Position;
            _abilityMeta = GameEntry.LuBan.Tables.Ability.Get(id);
            if(_abilityMeta is null)
                Log.Error($"<color=red>ActorState_Orb_Ability.OnEnter--->_abilityMeta is null,id:{id}</color>");
        }

        public override void OnUpdate(float deltaTime)
        {
            _passedTime += deltaTime;
            if (deltaTime >= _abilityMeta.Triggers[0].TriggerTime)
            {
                var module = GameEntry.Module.GetModule<Module_ProxyActor>();
                //法球类型的触发直接用第一个effect
                module.AffectAbility(0,_castorID,_targetID,_abilityMeta.id,_position);
                Clear();
                //触发完直接隐藏
                GameEntry.Entity.HideEntity(_castorID);
            }
        }

        public override void OnLeave(object param)
        {
            _abilityMeta = null;
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
        private Table_AbilityBase _abilityMeta = null;
        
        /// <summary>
        /// 该状态内经过的时间
        /// </summary>
        private float _passedTime = 0f;

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
