using Aquial.UI;
using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using Aquila.UI;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 战斗测试流程
    /// </summary>
    public class Procedure_Test_Fight : ProcedureBase
    {
        /// <summary>
        /// 退出状态，这里会回到preload
        /// </summary>
        public void Exit()
        {
            ChangeState<Procedure_Prelaod>( _owner );
        }

        /// <summary>
        /// 该流程加载是否完成
        /// </summary>
        private void OnLoadFinish()
        {
            if ( _loadFlagCurrState != _loadFlagFinish )
                return;

            var param = ReferencePool.Acquire<Form_AbilityParam>();
            param._mainActorID = _actorID1;
            param._enemyActorID = new int[] { _actorID2, _actorID3, _actorID4 };
            param._abilityID = GameEntry.LuBan.Tables.RoleMeta.Get( 1 ).AbilityBaseID;
            GameEntry.UI.OpenForm( FormIdEnum.AbilityForm, param );
            Log.Info( "<color=white>all set load finish</color>" );
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        private void LoadScene()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadSceneAsync( "TestFightScene" );
        }

        /// <summary>
        /// 加载actor
        /// </summary>
        private async void LoadActor()
        {
            var actor_fac = GameEntry.Module.GetModule<Module_Actor_Fac>();
            //actor1
            _actorID1 = ActorIDPool.Gen();
            var entity_1 = await actor_fac.ShowActorAsync<Actor_Hero>
                (
                    1,
                    _actorID1,
                    @"Assets/Res/Prefab/Character/TestCharacter_001.prefab",
                    grid_x: 0,
                    grid_z: 0,
                    new HeroActorEntityData( _actorID1 ) { _roleMetaID = 1 }
                );

            //actor2
            _actorID2 = ActorIDPool.Gen();
            var entity_2 = await actor_fac.ShowActorAsync<Actor_Hero>
                (
                    2,
                    _actorID2,
                    @"Assets/Res/Prefab/Character/TestCharacter_002.prefab",
                    grid_x: 1,
                    grid_z: 1,
                    new HeroActorEntityData( _actorID2 ) { _roleMetaID = 2 }
                );

            //actor3
            _actorID3 = ActorIDPool.Gen();
            var entity_3 = await actor_fac.ShowActorAsync<Actor_Hero>
            (
                2,
                _actorID3,
                @"Assets/Res/Prefab/Character/TestCharacter_002.prefab",
                grid_x: 1,
                grid_z: 1,
                new HeroActorEntityData( _actorID3 ) { _roleMetaID = 2 }
            );

            //actor4
            _actorID4 = ActorIDPool.Gen();
            var entity_4 = await actor_fac.ShowActorAsync<Actor_Hero>
            (
                2,
                _actorID4,
                @"Assets/Res/Prefab/Character/TestCharacter_002.prefab",
                grid_x: 1,
                grid_z: 1,
                new HeroActorEntityData( _actorID4 ) { _roleMetaID = 2 }
            );

            // if ( !( entity_1.Logic is Actor_Hero ) || !( entity_2.Logic is Actor_Hero ) )
            //     return;

            SetActorTransform( entity_1.Logic as Actor_Hero, new Vector3( 0, 0.8f, -3.29f ), Vector3.zero );
            SetActorTransform( entity_2.Logic as Actor_Hero, new Vector3( -2.87f, 0.5f, 1.6f ), new Vector3( 0, 180f, 0 ) );
            SetActorTransform( entity_3.Logic as Actor_Hero, new Vector3( -0.34f, 0.5f, 1.6f ), new Vector3( 0, 180f, 0 ) );
            SetActorTransform( entity_4.Logic as Actor_Hero, new Vector3( 2, 0.5f, 1.6f ), new Vector3( 0, 180f, 0 ) );


            _loadFlagCurrState = Tools.OrBitValue( _loadFlagCurrState, _loadFlagActor1 );
            _loadFlagCurrState = Tools.OrBitValue( _loadFlagCurrState, _loadFlagActor2 );
            _loadFlagCurrState = Tools.OrBitValue( _loadFlagCurrState, _loadFlagActor3 );
            _loadFlagCurrState = Tools.OrBitValue( _loadFlagCurrState, _loadFlagActor4 );

            OnLoadFinish();
        }

        /// <summary>
        /// 临时初始化，设置Actor的位置
        /// </summary>
        private void SetActorTransform( Actor_Hero actor, Vector3 position, Vector3 rotation )
        {
            if ( actor is null )
            {
                Log.Info( "<color=warning>my_actor is null || enemy_actor is null</color>" );
                return;
            }
            actor.SetWorldPosition( position );
            actor.SetRotation( rotation );
        }

        /// <summary>
        /// 场景回调
        /// </summary>
        private void OnSceneLoaded( Scene scene, LoadSceneMode mode )
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _loadFlagCurrState = Tools.OrBitValue( _loadFlagCurrState, _loadFlagScene );
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            base.OnEnter( procedureOwner );
            _owner = procedureOwner;
            GameEntry.Module.GetModule<Module_ProxyActor>().Open(null);
            FightOnEnter();
        }

        protected override void OnLeave( IFsm<IProcedureManager> procedureOwner, bool isShutdown )
        {
            GameEntry.UI.CloseAll();
            GameEntry.Module.GetModule<Module_ProxyActor>().Close();
            _owner = null;
            base.OnLeave( procedureOwner, isShutdown );
        }

        private void FightOnEnter()
        {
            _loadFlagCurrState = 0b_0000;
            // base.OnEnter( procedureOwner );
            //加载场景，加载4个测试用的战斗actor
            LoadScene();
            LoadActor();
        }

        private int _actorID1 = 0;
        private int _actorID2 = 0;
        private int _actorID3 = 0;
        private int _actorID4 = 0;


        /// <summary>
        /// 加载actor1
        /// </summary>
        private int _loadFlagActor1 = 0b_0000_0001;

        /// <summary>
        /// 加载actor2
        /// </summary>
        private int _loadFlagActor2 = 0b_0000_0010;

        /// <summary>
        /// 加载actor3
        /// </summary>
        private int _loadFlagActor3 = 0b_0000_0100;

        /// <summary>
        /// 加载actor4
        /// </summary>
        private int _loadFlagActor4 = 0b_0000_1000;

        /// <summary>
        /// 加载场景
        /// </summary>
        private int _loadFlagScene = 0b_0001_0000;

        /// <summary>
        /// 加载完成
        /// </summary>
        private const int _loadFlagFinish = 0b_0001_1111;

        /// <summary>
        /// 当前的加载状态
        /// </summary>
        private int _loadFlagCurrState = 0b_0000;

        /// <summary>
        /// 流程持有者
        /// </summary>
        private IFsm<IProcedureManager> _owner = null;

        /// <summary>
        /// 测试技能ID
        /// </summary>
#pragma warning disable 0414 // 删除未使用的私有成员
        private int _testAbilityMetaID = 1002;
#pragma warning restore 0414 // 删除未使用的私有成员
    }
}
