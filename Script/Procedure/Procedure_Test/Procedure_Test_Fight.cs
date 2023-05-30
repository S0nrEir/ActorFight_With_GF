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
        private void OnFireActionPerformed(InputAction.CallbackContext ctx)
        {
            // if(ctx.interaction is PressInteraction)
            //     TestFight();
            
            if(ctx.interaction is not PressInteraction)
                return;
            
            if(_load_flag_curr_state != _load_flag_finish)
                return;

            //test ability
            GameEntry.Module.GetModule<Module_Proxy_Actor>().AbilityToSingleTarget(_actor_id_1, _actor_id_2, 1000);
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
            _actor_id_1 = ACTOR_ID_POOL.Gen();
            var entity_1 = await actor_fac.ShowActorAsync<HeroActor>
                (
                    role_meta_id: 1,
                    actor_id: _actor_id_1,
                    asset_path: @"Assets/Res/Prefab/Example/Aquila_Test_001.prefab",
                    grid_x: 0,
                    grid_z: 0,
                    new HeroActorEntityData( _actor_id_1 ) { _role_meta_id = 1 }
                );

            //actor2
            _actor_id_2 = ACTOR_ID_POOL.Gen();
            var entity_2 = await actor_fac.ShowActorAsync<HeroActor>
                (
                    role_meta_id: 2,
                    actor_id: _actor_id_2,
                    asset_path: @"Assets/Res/Prefab/Example/Aquila_Test_002.prefab",
                    grid_x: 1,
                    grid_z: 1,
                    new HeroActorEntityData( _actor_id_2 ) { _role_meta_id = 2 }
                );

            //actor3
            _actor_id_3 = ACTOR_ID_POOL.Gen();
            var entity_3 = await actor_fac.ShowActorAsync<HeroActor>
            (
                role_meta_id:2,
                actor_id:_actor_id_3,
                asset_path:@"Assets/Res/Prefab/Example/Aquila_Test_002.prefab",
                grid_x:1,
                grid_z:1,
                new HeroActorEntityData(_actor_id_3){_role_meta_id = 2}
            );

            //actor4
            _actor_id_4 = ACTOR_ID_POOL.Gen();
            var entity_4 = await actor_fac.ShowActorAsync<HeroActor>
            (
                role_meta_id:2,
                actor_id:_actor_id_4,
                asset_path:@"Assets/Res/Prefab/Example/Aquila_Test_002.prefab",
                grid_x:1,
                grid_z:1,
                new HeroActorEntityData(_actor_id_4){_role_meta_id = 2}
            );
            
            // if ( !( entity_1.Logic is HeroActor ) || !( entity_2.Logic is HeroActor ) )
            //     return;
            
            SetActorPosition(entity_1.Logic as HeroActor,new Vector3(0,0.8f,-3.29f));
            SetActorPosition(entity_2.Logic as HeroActor,new Vector3(0,0.5f,1.6f));
            SetActorPosition(entity_3.Logic as HeroActor,new Vector3(1,0.5f,1.6f));
            SetActorPosition(entity_4.Logic as HeroActor,new Vector3(2,0.5f,1.6f));


            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_1 );
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_2 );
            _load_flag_curr_state = Tools.OrBitValue(_load_flag_curr_state, _load_flag_actor_3);
            _load_flag_curr_state = Tools.OrBitValue(_load_flag_curr_state, _load_flag_actor_4);
            
            OnLoadFinish();
        }
        
        /// <summary>
        /// 临时初始化，设置Actor的位置
        /// </summary>
        private void SetActorPosition(TActorBase actor,Vector3 pos)
        {
            if (actor is null)
            {
                Log.Info("<color=warning>my_actor is null || enemy_actor is null</color>");
                return;
            }
            actor.SetWorldPosition(pos);
        }

        /// <summary>
        /// 场景回调
        /// </summary>
        private void OnSceneLoaded( Scene scene_, LoadSceneMode mode_ )
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
                    (assetName, asset, duration, userData) => 
                    {
                        var action_asset = ( asset as InputActionAsset );
                        if ( action_asset is null || action_asset.actionMaps.Count == 0)
                        {
                            Debug.LogError( $"action_asset is null || action_asset.actionMaps.Count == 0" );
                            return;
                        }
                        var map = action_asset.FindActionMap("gameplay",true); 
                        _fire_action = map.FindAction( "fire", true );
                        _fire_action.performed += OnFireActionPerformed;
                        _fire_action.Enable();
                        action_asset.Enable();
                    },
                    //faild callback
                    (assetName, status, errorMessage, userData) =>
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
    }
}
