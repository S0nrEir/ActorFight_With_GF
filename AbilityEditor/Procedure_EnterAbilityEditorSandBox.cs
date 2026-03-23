#if UNITY_EDITOR

using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using Aquila.Fight.Actor;
using Aquila.Module;
using GameFramework;
using Aquila.AbilityEditor;
using Aquila.Fight;
using Aquila.Event;
using GameFramework.Resource;
using GameFramework.Event;
using UnityGameFramework.Runtime;

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

            LoadSandBoxAbility(out var abilityData);
            PassActorIDs(owner);
            GameEntry.AbilityEditorSandBox.Init(abilityData);
            ChangeState<Procedure_RunningAbilityEditorSandBox>(owner);
        }

        private async void CreatePlayer()
        {
            _playerEntityID = ActorIDPool.Gen();
            var actor_fac = GameEntry.Module.GetModule<Module_Actor_Fac>();
            var entity = await actor_fac.ShowActorAsync(
                _playerEntityID,
                PLYAER_META_ROLE_ID,
                new HeroActorEntityData(_playerEntityID) { _roleMetaID = PLYAER_META_ROLE_ID },
                "AbilityEditor_Player"
            );
            if (entity != null)
            {
                var actor = entity.Logic as Actor_Hero;
                if (actor != null)
                {
                    actor.SetWorldPosition(new Vector3(18.90956f, -5.782828f, -22.24478f));
                    actor.SetRotation(new Vector3(0, -64.988f, 0));
                }
            }
            _loadFinishSign |= 0b0001;
            MarkLoadFinish(_owner);
        }

        private async void CreateDummy()
        {
            _dummyEntityID = ActorIDPool.Gen();
            var actor_fac = GameEntry.Module.GetModule<Module_Actor_Fac>();
            var entity = await actor_fac.ShowActorAsync(
                _dummyEntityID,
                DUMMY_META_ROLE_ID,
                new HeroActorEntityData(_dummyEntityID) { _roleMetaID = DUMMY_META_ROLE_ID },
                "AbilityEditor_Dummy"
            );
            if (entity != null)
            {
                var actor = entity.Logic as Actor_Hero;
                if (actor != null)
                {
                    actor.SetWorldPosition(new Vector3(5.289566f, -5.782828f, -21.05478f));
                    actor.SetRotation(new Vector3(0, -261.314f, 0));
                }
            }
            _loadFinishSign |= 0b0010;
            MarkLoadFinish(_owner);
        }

        private void PassActorIDs(IFsm<IProcedureManager> owner)
        {
            var playerVar = ReferencePool.Acquire<VarInt32>();
            playerVar.Value = _playerEntityID;
            owner.SetData<VarInt32>(KEY_PLAYER_ENTITY_ID, playerVar);

            var dummyVar = ReferencePool.Acquire<VarInt32>();
            dummyVar.Value = _dummyEntityID;
            owner.SetData<VarInt32>(KEY_DUMMY_ENTITY_ID, dummyVar);
        }

        /// <summary>
        /// 预加载项目完成事件处理
        /// </summary>
        private void OnPreloadItemComplete( object sender, GameEventArgs e )
        {
            var args = e as PreloadItemCompleteEventArgs;
            if ( args == null )
                return;

            switch ( args.ItemType )
            {
                case PreloadItemType.HPBar:
                    _loadFinishSign |= 0b0100;
                    break;
                
                case PreloadItemType.DamageNumber:
                    _loadFinishSign |= 0b1000;
                    break;
            }
            

            // 检查所有InfoBoard项目是否加载完成
            if ((_loadFinishSign & INFOBOARD_LOAD_FLAG ) == INFOBOARD_LOAD_FLAG )
            {
                if ( !_actorCreationStarted )
                {
                    _actorCreationStarted = true;
                    CreateDummy();
                    CreatePlayer();
                }
            }

            // MarkLoadFinish( _owner );
        }

        private bool LoadSandBoxAbility(out AbilityData abilityData)
        {
            abilityData = default;
            string sandBoxDir = System.IO.Path.GetFullPath(SANDBOX_ABILITY_PATH);
            if (!GameEntry.AbilityPool.LoadSandBoxAbility(sandBoxDir, out var tempAbilityData))
            {
                UnityGameFramework.Runtime.Log.Error("[EnterSandBox] LoadSandBoxAbility failed");
                return false;
            }

            abilityData = tempAbilityData;
            return true;
        }

        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            _owner = null;
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            _owner = procedureOwner;
            GameEntry.Event.Subscribe( PreloadItemCompleteEventArgs.EventID, OnPreloadItemComplete );
            GameEntry.LuBan.LoadDataTable();
            GameEntry.AbilityPool.Init();
            
            // 先加载InfoBoard项目，等待完成后再创建Actor
            GameEntry.InfoBoard.Preload();
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe( PreloadItemCompleteEventArgs.EventID, OnPreloadItemComplete );
            
            _loadFinishSign = 0b0000;
            _actorCreationStarted = false;
            _playerEntityID = -1;
            _dummyEntityID  = -1;
            _owner          = null;
        }

        private IFsm<IProcedureManager> _owner = null;
        private const int ALL_LOAD_FLAG = 0b1111;
        private const int INFOBOARD_LOAD_FLAG = 0b1100;
        private int _loadFinishSign = 0b0000;
        private bool _actorCreationStarted = false;
        private int _playerEntityID = -1;
        private int _dummyEntityID  = -1;
        
        public const string KEY_PLAYER_ENTITY_ID = "SandBox_PlayerEntityID";
        public const string KEY_DUMMY_ENTITY_ID  = "SandBox_DummyEntityID";
        public const int DUMMY_META_ROLE_ID      = 999998;
        public const int PLYAER_META_ROLE_ID     = 999999;
        public static readonly string SANDBOX_ABILITY_PATH = "Assets/AbilityEditor/SandBox";
    }
}
#endif