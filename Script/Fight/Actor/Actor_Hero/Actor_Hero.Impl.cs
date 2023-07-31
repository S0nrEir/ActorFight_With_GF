using Aquila.Fight.FSM;

namespace Aquila.Fight.Actor
{
    //hero actor的各个实现
    public partial class Actor_Hero : 
        ISwitchStateBehavior,
        IDieBehavior
    {
        /// <summary>
        /// 死亡
        /// </summary>
        public void Die()
        {
            SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
        }
        
        /// <summary>
        /// 切换状态
        /// </summary>
        public void SwitchTo( ActorStateTypeEnum stateType, object enterParam, object existParam )
        {
            _fsmAddon.SwitchTo( stateType, enterParam, existParam );
        }
    }
}
