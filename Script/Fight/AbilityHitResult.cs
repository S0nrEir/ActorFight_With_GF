using UnityGameFramework.Runtime;

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
            _stateDescription  = 0b_0000_0000_0000_0000;
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
        public void SetState(AbilityHitResultTypeEnum type)
        {
            if (type == AbilityHitResultTypeEnum.INVALID)
            {
                Log.Warning("<color=yellow>type == AbilityResultDescTypeEnum.INVALID</color>");
                return;
            }

            var int_type = (ushort)type;
            Toolkit.Tools.SetBitValue_i64(_stateDescription, int_type , true);
        }

        /// <summary>
        /// 技能使用结果的状态描述
        /// </summary>
        public uint _stateDescription;

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
    
    /// <summary>
    /// 技能命中结果
    /// </summary>
    public enum AbilityHitResultTypeEnum
    {
        /// <summary>
        /// 无效
        /// </summary>
        INVALID = -1,
        
        /// <summary>
        /// 命中
        /// </summary>
        HIT = 0,
        
        /// <summary>
        /// 施法者不存在
        /// </summary>
        NO_CASTOR,
        
        /// <summary>
        /// 没有目标
        /// </summary>
        NO_TARGET,
        
        /// <summary>
        /// 技能冷却中
        /// </summary>
        CD_NOT_OK,
        
        /// <summary>
        /// 无法使用
        /// </summary>
        CANT_USE,
        
        /// <summary>
        /// 技能消耗不够
        /// </summary>
        COST_NOT_ENOUGH,
        
        /// <summary>
        /// 造成了暴击
        /// </summary>
        CRITICAL,
    }
}
