using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using GameFramework.Fsm;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityGameFramework.Runtime;

namespace Aquila.Procedure
{
    /// <summary>
    /// 战斗测试流程
    /// </summary>
    public class Procedure_Test_Fight : ProcedureBase
    {
        private void OnFireActionPerformed( InputAction.CallbackContext ctx )
        {
            // if(ctx.interaction is PressInteraction)
            //     TestFight();

            if ( ctx.interaction is not PressInteraction )
                return;

            if ( _load_flag_curr_state != _load_flag_finish )
                return;

            //test ability
            //GameEntry.Module.GetModule<Module_Proxy_Actor>().AbilityToSingleTarget(_actor_id_1, _actor_id_2, 1000);
            GameEntry.Module.GetModule<Module_ProxyActor>().Ability2SingleTarget( _actor_id_1, _actor_id_2, _testAbilityMetaID );
        }

        private void TestFight()
        {
        }

        /// <summary>
        /// 该流程加载是否完成
        /// </summary>
        private void OnLoadFinish()
        {
            if ( _load_flag_curr_state != _load_flag_finish )
                return;

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
            _actor_id_1 = ActorIDPool.Gen();
            var entity_1 = await actor_fac.ShowActorAsync<Actor_Hero>
                (
                    1,
                    _actor_id_1,
                    @"Assets/Res/Prefab/Character/TestCharacter_001.prefab",
                    grid_x: 0,
                    grid_z: 0,
                    new HeroActorEntityData( _actor_id_1 ) { _roleMetaID = 1 }
                );

            //actor2
            _actor_id_2 = ActorIDPool.Gen();
            var entity_2 = await actor_fac.ShowActorAsync<Actor_Hero>
                (
                    2,
                    _actor_id_2,
                    @"Assets/Res/Prefab/Character/TestCharacter_002.prefab",
                    grid_x: 1,
                    grid_z: 1,
                    new HeroActorEntityData( _actor_id_2 ) { _roleMetaID = 2 }
                );

            //actor3
            _actor_id_3 = ActorIDPool.Gen();
            var entity_3 = await actor_fac.ShowActorAsync<Actor_Hero>
            (
                2,
                _actor_id_3,
                @"Assets/Res/Prefab/Character/TestCharacter_002.prefab",
                grid_x: 1,
                grid_z: 1,
                new HeroActorEntityData( _actor_id_3 ) { _roleMetaID = 2 }
            );

            //actor4
            _actor_id_4 = ActorIDPool.Gen();
            var entity_4 = await actor_fac.ShowActorAsync<Actor_Hero>
            (
                2,
                _actor_id_4,
                @"Assets/Res/Prefab/Character/TestCharacter_002.prefab",
                grid_x: 1,
                grid_z: 1,
                new HeroActorEntityData( _actor_id_4 ) { _roleMetaID = 2 }
            );

            // if ( !( entity_1.Logic is Actor_Hero ) || !( entity_2.Logic is Actor_Hero ) )
            //     return;

            SetActorTransform( entity_1.Logic as Actor_Hero, new Vector3( 0, 0.8f, -3.29f ), Vector3.zero );
            SetActorTransform( entity_2.Logic as Actor_Hero, new Vector3( -2.87f, 0.5f, 1.6f ), new Vector3( 0, 180f, 0 ) );
            SetActorTransform( entity_3.Logic as Actor_Hero, new Vector3( -0.34f, 0.5f, 1.6f ), new Vector3( 0, 180f, 0 ) );
            SetActorTransform( entity_4.Logic as Actor_Hero, new Vector3( 2, 0.5f, 1.6f ), new Vector3( 0, 180f, 0 ) );


            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_1 );
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_2 );
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_3 );
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_4 );

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
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_scene );
        }

        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            _load_flag_curr_state = 0b_0000;
            // base.OnEnter( procedureOwner );
            //加载场景，加载4个测试用的战斗actor
            LoadScene();
            LoadActor();
            //加载临时输入配置
            GameEntry.Resource.LoadAsset( @"Assets/Samples/Input System/1.3.0/Simple Demo/SimpleControls.inputactions", new LoadAssetCallbacks
                (
                    //succ callback
                    ( assetName, asset, duration, userData ) =>
                    {
                        var action_asset = ( asset as InputActionAsset );
                        if ( action_asset is null || action_asset.actionMaps.Count == 0 )
                        {
                            Debug.LogError( $"action_asset is null || action_asset.actionMaps.Count == 0" );
                            return;
                        }
                        var map = action_asset.FindActionMap( "gameplay", true );
                        _fire_action = map.FindAction( "fire", true );
                        _fire_action.performed += OnFireActionPerformed;
                        _fire_action.Enable();
                        action_asset.Enable();
                    },
                    //faild callback
                    ( assetName, status, errorMessage, userData ) =>
                    {
                    }
                )
            );
        }

        private int _actor_id_2 = 0;
        private int _actor_id_1 = 0;
        private int _actor_id_3 = 0;
        private int _actor_id_4 = 0;

        private InputAction _fire_action = null;

        /// <summary>
        /// 加载actor1
        /// </summary>
        private int _load_flag_actor_1 = 0b_0000_0001;

        /// <summary>
        /// 加载actor2
        /// </summary>
        private int _load_flag_actor_2 = 0b_0000_0010;

        /// <summary>
        /// 加载actor3
        /// </summary>
        private int _load_flag_actor_3 = 0b_0000_0100;

        /// <summary>
        /// 加载actor4
        /// </summary>
        private int _load_flag_actor_4 = 0b_0000_1000;

        /// <summary>
        /// 加载场景
        /// </summary>
        private int _load_flag_scene = 0b_0001_0000;

        /// <summary>
        /// 加载完成
        /// </summary>
        private const int _load_flag_finish = 0b_0001_1111;

        /// <summary>
        /// 当前的加载状态
        /// </summary>
        private int _load_flag_curr_state = 0b_0000;

        /// <summary>
        /// 测试技能ID
        /// </summary>
        private int _testAbilityMetaID = 1000;
    }
}