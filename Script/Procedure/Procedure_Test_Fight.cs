using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.ToolKit;
using GameFramework.Fsm;
using GameFramework.Procedure;
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

        private async void LoadActor()
        {
            var actor_fac = GameEntry.Module.GetModule<Module_Actor_Fac>();
            //actor1
            var actor_id = ACTOR_ID_POOL.Gen();
            var actor_1 = await actor_fac.ShowActorAsync<HeroActor>
                (
                    role_meta_id: _temp_role_meta_id_1,
                    actor_id: actor_id,
                    asset_path: @"Assets/Res/Prefab/Aquila_001.prefab",
                    grid_x: 0,
                    grid_z: 0,
                    new HeroActorEntityData( actor_id )
                );

            //actor2
            actor_id = ACTOR_ID_POOL.Gen();
            var actor_2 = await actor_fac.ShowActorAsync<HeroActor>
                (
                    role_meta_id: _temp_role_meta_id_1,
                    actor_id: actor_id,
                    asset_path: @"Assets/Res/Prefab/Aquila_001.prefab",
                    grid_x: 1,
                    grid_z: 1,
                    new HeroActorEntityData( actor_id )
                );
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_1 );
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_actor_2 );
            OnLoadFinish();
        }

        /// <summary>
        /// 场景回调
        /// </summary>
        private void OnSceneLoaded( Scene scene_, LoadSceneMode mode_ )
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _load_flag_curr_state = Tools.OrBitValue( _load_flag_curr_state, _load_flag_scene );
        }

        //@override:
        protected override void OnEnter( IFsm<IProcedureManager> procedureOwner )
        {
            _load_flag_curr_state = 0b_0000;
            base.OnEnter( procedureOwner );
            //加载场景，加载两个测试用的战斗actor
            LoadScene();
            LoadActor();
        }

        /// <summary>
        /// 加载actor1
        /// </summary>
        private int _load_flag_actor_1 = 0b_0001;

        /// <summary>
        /// 加载actor2
        /// </summary>
        private int _load_flag_actor_2 = 0b_0010;

        /// <summary>
        /// 加载场景
        /// </summary>
        private int _load_flag_scene = 0b_0100;

        /// <summary>
        /// 加载完成
        /// </summary>
        private const int _load_flag_finish = 0b_1000;

        /// <summary>
        /// 当前的加载状态
        /// </summary>
        private int _load_flag_curr_state = 0b_0000;

        /// <summary>
        /// 角色属性表id
        /// </summary>
        private int _temp_role_meta_id_1 = 1;

        /// <summary>
        /// 角色属性表id
        /// </summary>
        private int _temp_role_meta_id_2 = 2;
    }
}
