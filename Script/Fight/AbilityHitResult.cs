using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua.Cast;

namespace Aquila.Fight
{
    /// <summary>
    /// 描述技能的命中结果
    /// </summary>
    public struct AbilityHitResult
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            _state_description = 0b_0000_0000_0000_0000;
            _dealed_damage     = 0f;
            _target_actor_id   = 0;
            _castor_actor_id   = 0;
        }

        /// <summary>
        /// 增加造成的伤害
        /// </summary>
        public void AddDealedDamage(float dmg)
        {
            _dealed_damage += dmg;
        }

        /// <summary>222
        /// 设置状态
        /// </summary>
        public void SetState(AbilityResultDescTypeEnum type)
        {
            if (type == AbilityResultDescTypeEnum.INVALID)
            {
                Log.Warning("<color=yellow>type == AbilityResultDescTypeEnum.INVALID</color>");
                return;
            }

            var int_type = (uint)type;
            Toolkit.Tools.SetBitValue_U32(_state_description, int_type , true);
        }

        /// <summary>
        /// 技能使用结果的状态描述
        /// </summary>
        public uint _state_description;

        /// <summary>
        /// 造成的伤害
        /// </summary>
        public float _dealed_damage;

        /// <summary>
        /// 目标id
        /// </summary>
        public int _target_actor_id;
        
        /// <summary>
        /// 施法者id
        /// </summary>
        public int _castor_actor_id;
    }
}
