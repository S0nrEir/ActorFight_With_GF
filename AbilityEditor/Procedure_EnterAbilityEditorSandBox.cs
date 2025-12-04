#if UNITY_EDITOR

using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using Aquila.Fight.Actor;
using Aquila.Module;
using GameFramework;

namespace Aquila.Procedure
{
    /// <summary>
    /// 进入技能编辑器沙盒流程
    /// </summary>
    public class Procedure_EnterAbilityEditorSandBox : ProcedureBase
    {
        private void MarkLoadFinish(IFsm<IProcedureManager> owner)
        {
            if (_loadFinishSign != ALL_LOAD_FLAG)
                return;
            
            ChangeState<Procedure_RunningAbilityEditorSandBox>(owner);
        }

        /// <summary>
        /// 创建木桩 / create dummy
        /// </summary>
        private async void CreateDummy()
        {
            var dummyActorEntityData = ReferencePool.Acquire<HeroActorEntityData>();
            dummyActorEntityData._roleMetaID = 4001;
            _dummyEntityID = ActorIDPool.Gen();
            dummyActorEntityData.Init(_dummyEntityID,typeof( Actor_Hero ).GetHashCode());
            var entity = await GameEntry.Module.GetModule<Module_Actor_Fac>().ShowActorAsync(_dummyEntityID,4001,dummyActorEntityData,_dummyEntityID.ToString());

            if (entity is null)
            {
                Debug.LogError("Failed to create dummy!");
                return;
            }

            _loadFinishSign |= 0b0010;
            (entity.Logic as Actor_Hero)?.SetWorldPosition(new Vector3(5.289566f,-5.782828f,-21.05478f));
            (entity.Logic as Actor_Hero)?.SetRotation(new Vector3(0,-261.314f,0));
            MarkLoadFinish(_owner);
        }

        /// <summary>
        /// 创建玩家 / create player
        /// </summary>
        private async void CreatePlayer()
        {
            var playerActorEntityData = ReferencePool.Acquire<HeroActorEntityData>();
            playerActorEntityData._roleMetaID = 4000;
            _playerEntityID = ActorIDPool.Gen();
            playerActorEntityData.Init(_playerEntityID,typeof( Actor_Hero ).GetHashCode());
            var entity = await GameEntry.Module.GetModule<Module_Actor_Fac>().ShowActorAsync(_playerEntityID, 4000, playerActorEntityData, _playerEntityID.ToString());
            
            if (entity is null)
            {
                Debug.LogError("Failed to create player!");
                return;
            }

            (entity.Logic as Actor_Hero)?.SetWorldPosition(new Vector3(18.90956f,-5.782828f,-22.24478f));
            (entity.Logic as Actor_Hero)?.SetRotation(new Vector3(0,-64.988f,0));
            _loadFinishSign |= 0b0001;
            MarkLoadFinish(_owner);
        }

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnInit(procedureOwner);
            _owner = null;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //create dummy / player.
            CreateDummy();
            CreatePlayer();
            _owner = procedureOwner;
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            _loadFinishSign = 0b0000;
            _playerEntityID = -1;
            _dummyEntityID = -1;
            _owner = null;
        }

        /// <summary>
        /// 切换到运行沙盒流程
        /// </summary>
        public void SwitchToRunningState(IFsm<IProcedureManager> procedureOwner)
        {
            ChangeState<Procedure_RunningAbilityEditorSandBox>(procedureOwner);
        }

        private IFsm<IProcedureManager> _owner = null;
        private const int ALL_LOAD_FLAG = 0b0011;
        private int _loadFinishSign = 0b0000;
        private int _playerEntityID = -1;
        private int _dummyEntityID = -1;
    }
}
#endif