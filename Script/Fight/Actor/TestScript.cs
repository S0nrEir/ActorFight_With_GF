//using Aquila.Config;
//using Aquila.Fight;
//using Aquila.Fight.Actor;
//using Aquila.Fight.Addon;
//using System.Collections.Generic;
//using UGFExtensions.Await;
//using UnityEngine;
//using UnityEngine.AI;
//using UnityGameFramework.Runtime;

//namespace Aquila
//{

//    public class TestScript : MonoBehaviour
//    {

//        // Start is called before the first frame update
//        void Start()
//        {
//            GameFrameworkMode.GetModule<AccountModule>().Setup( GlobeVar.INVALID_GUID - 1, "test" );
//            GameFrameworkMode.GetModule<FightModule>().Start();
//            GameEntry.Buff.StarFight();
//            CreateActor();
//            CreateTowerActor();
//            //TestBuff();
//            //TestCreateTriggerActor();

//            return;
//            Projectile();
//        }

//        #region TestLogic

//        /// <summary>
//        /// 测子弹
//        /// </summary>
//        private void Bullet(TowerActor parentActor)
//        {
//            if ( parentActor is null )
//                return;

//            var pak = new GC_CREATE_BULLET();
//            pak.Hostguid = pak.Hostguid = GlobeVar.INVALID_GUID - 1;
//            pak.DataID = 67;
//            pak.Posx = Utils.Fight.ClientFloatVarible2ServerInt( parentActor.CachedTransform.position.x );
//            pak.Posz = Utils.Fight.ClientFloatVarible2ServerInt( parentActor.CachedTransform.position.z);
//            pak.ObjID = ACTOR_ID_POOL.Gen();
//            pak.Index = 99;
//            pak.Vecx = 10;
//            pak.Vecz = 10;
//            pak.Forcetype = 0;
//            GameFrameworkMode.GetModule<FightModule>().OnRecvCreateBulletAsync( pak );
//        }

//        private void MoveActor ( TActorBase heroActor )
//        {
//            if (!( heroActor is IPathMoveBehavior ))
//                return;

//            heroActor.SetWorldPosition
//                (
//                    new Vector3( 26.27f, Utils.FightScene.TerrainPositionY( 26.27f, 23.78f, 0 ), 23.78f )
//                );
//            ( heroActor as IPathMoveBehavior ).Move
//                (
//                    new List<float>() { 26.27f, 26.27f },
//                    new List<float>() { 23.78f, 33.4f } 
//                );

//        }

//        /// <summary>
//        /// triggerAddon
//        /// </summary>
//        private void TestCreateTriggerActor ()
//        {
//            GameFrameworkMode.GetModule<FightModule>().SetParcel( TableManager.GetMapConfigByID( 0, 0 ) );
//        }

//        /// <summary>
//        /// 测试buff
//        /// </summary>
//        private void TestBuff ( )
//        {
//            //_heroActor.SetWorldPosition( new Vector3( 28.97f, Utils.FightScene.TerrainPositionY( 28.97f, 53.173f, 0 ), 53.173f ) );
//            GC_CREATE_HERO pak = new GC_CREATE_HERO();
//            pak.DataId = 4;
//            pak.PosX = 289;
//            pak.PosZ = 531;
//            pak.Index = 0;
//            pak.Hostguid = GlobeVar.INVALID_GUID - 1;
//            pak.Forcetype = 0;
//            var objID = ACTOR_ID_POOL.Gen();
//            pak.ObjID = objID;
//            GameFrameworkMode.GetModule<FightModule>().OnRecvCreateHero( pak );

//            GameEntry.Timer.StartCounting( 3f, ( elapsed ) =>
//              {
//                  GameEntry.Buff.OnRecvImpactInfo
//                   (
//                       new List<int> { 1 },
//                       new List<int> { 1 },
//                       objID
//                   );
//              } );

//            GameEntry.Timer.StartCounting( 10f, ( elapsed ) =>
//            {
//                GameEntry.Buff.OnRecvImpactInfo
//                 (
//                     new List<int> { 1 },
//                     new List<int> { 0 },
//                     objID
//                 );
//            } );

//            GameEntry.Timer.StartCounting( 15f, ( elapsed ) =>
//            {
//                GameEntry.Buff.OnRecvImpactInfo
//                 (
//                     new List<int> { 1 },
//                     new List<int> { 1 },
//                     objID
//                 );
//            } );
//        }

//        /// <summary>
//        /// 抛物线测试
//        /// </summary>
//        private void Projectile ()
//        {

//            var dis = Vector3.Distance( transform.position, target.position );
//            needDirection = ( target.position - transform.position ).normalized;
//            //每单位时间的移动距离（速度）,前半段的速度
//            var needTime = dis / speed / 2f;
//            verticalSpeed = g * needTime;
//        }

//        private void DamageNum ( HeroActor heroActor )
//        {
//            if (heroActor == null)
//                return;

//            GameEntry.Timer.StartTick( 3f, ( elapsed ) =>
//            {
//                if (heroActor.TryGetAddon<InfoBoardAddon>( out var addon ))
//                {
//                    addon.ShowDamageNum( 3f, "-500" );
//                }
//            } );
//        }

//        private float x;
//        private float z;

//        /// <summary>
//        /// 测试特效
//        /// </summary>
//        private void EffectTest ( HeroActor actor )
//        {
//            GameEntry.Timer.StartTick( 10f, ( deltaTime ) =>
//            {
//                GC_Skill_Info_Stct stct = new GC_Skill_Info_Stct()
//                {
//                    cdTime = 3000,
//                    senderID = _heroActor.ActorID,
//                    targetID = _heroActor.ActorID,
//                    skillID = 1
//                };
//                _heroActor.DoAbilityAction( stct );
//            } );
//        }

//        /// <summary>
//        /// 投射物刷帧位移
//        /// </summary>
//        private void ProjectileFixedUpdate ()
//        {
//            verticalSpeed = verticalSpeed - g * Time.fixedDeltaTime;
//            //横向位移
//            transform.Translate( needDirection * speed * Time.fixedDeltaTime, Space.World );
//            //y轴位移
//            transform.Translate( Vector3.up * verticalSpeed * Time.fixedDeltaTime, Space.World );
//            Debug.Log( $"<color=white>{verticalSpeed}</color>" );
//            //当verticalSpeed为正数时，距离一直向上，为负数时，向下,实际上就是一直给向上的力（y轴方向上的位移偏移），并在这个过程中不断衰减它，
//            //Debug.Log( $"{transform.position.y}" );
//            if (verticalSpeed <= 0 && !flag)
//            {
//                Debug.Log( $"<color=orange>{transform.position.y}</color>" );
//                flag = true;
//            }
//        }

//        public float _horizontalSpd = 30f;
//        public Transform target;
//        private float verticalSpeed;
//        public float g = 9.8f;
//        public Vector3 needDirection;
//        public float speed = 2f;

//        #endregion

//        private void FixedUpdate ()
//        {
//            //ProjectileFixedUpdate();
//        }

//        #region Create


//        private async void CreateTowerActor ()
//        {
//            var roleBaseMeta = TableManager.GetRoleBaseAttrByID( 58, 0 );
//            if (roleBaseMeta is null)
//            {
//                Debug.LogError( "roleBaseMeta is null,id:" + 0 );
//                return;
//            }

//            var modelMeta = TableManager.GetCharModelByID( roleBaseMeta.CharModelID, 0 );
//            if (modelMeta is null)
//            {
//                Debug.LogError( "modelMeta is null,id:" + roleBaseMeta.CharModelID );
//                return;
//            }

//            var entityID = ACTOR_ID_POOL.Gen();
//            _createTowerID = entityID;
//            var taskResult = await AwaitableExtension.ShowEntityAsync
//                (
//                    MRG.GameEntry.Entity,
//                    entityID,
//                    typeof( TowerActor ),
//                    modelMeta.ResPath,
//                    GameConfig.Entity.GROUP_TowerActor,
//                    GameConfig.Entity.Priority_Actor,
//                    new HeroActorEntityData( entityID )
//                );
//            _towerActor = taskResult.Logic as TowerActor;
//            //silu02
//            //_towerActor.SetWorldPosition( new Vector3( 30.89933f, Utils.FightScene.TerrainPositionY( 30.89933f, 48f, 0 ), 48f ) );

//            //chenbao06
//            _heroActor.SetWorldPosition( new Vector3( 33.56f, Utils.FightScene.TerrainPositionY( 28.97f, 36.27f, 0 ), 36.27f ) );

//            _towerActor.Setup( "Actor", 1, ACTOR_ID_POOL.Gen(), GlobeVar.INVALID_GUID, 0 );
//            _towerActor.SetDataID( 58 );
//            _towerActor.Reset();
//            OnCreateTowerActor( taskResult.Logic as TowerActor );
//        }

//        private async void CreateActor ()
//        {
//            var roleBaseMeta = TableManager.GetRoleBaseAttrByID( 0, 0 );
//            if (roleBaseMeta is null)
//            {
//                Debug.LogError( "roleBaseMeta is null,id:" + 0 );
//                return;
//            }

//            var modelMeta = TableManager.GetCharModelByID( roleBaseMeta.CharModelID, 0 );
//            if (modelMeta is null)
//            {
//                Debug.LogError( "modelMeta is null,id:" + roleBaseMeta.CharModelID );
//                return;
//            }

//            var entityID = ACTOR_ID_POOL.Gen();
//            _createHeroID = entityID;
//            var taskResult = await AwaitableExtension.ShowEntityAsync
//                (
//                    MRG.GameEntry.Entity,
//                    entityID,
//                    typeof( HeroActor ),
//                    modelMeta.ResPath,
//                    GameConfig.Entity.GROUP_HeroActor,
//                    GameConfig.Entity.Priority_Actor,
//                    new HeroActorEntityData( entityID )
//                );
//            _heroActor = taskResult.Logic as HeroActor;
//            //silu02
//            //_heroActor.SetWorldPosition( new Vector3( 28.97f, Utils.FightScene.TerrainPositionY( 28.97f, 53.173f, 0 ), 53.173f ) );
//            //chenbao06
//            //_heroActor.SetWorldPosition( new Vector3( 33.56f, Utils.FightScene.TerrainPositionY( 28.97f, 36.27f, 0 ), 36.27f ) );
//            //chenbao06，位置更高
//            _heroActor.SetWorldPosition( new Vector3( 33.56f, 7f , 36.27f ) );

//            _heroActor.Setup( "Actor", 0, ACTOR_ID_POOL.Gen(), GlobeVar.INVALID_GUID, 0 );
//            _heroActor.SetDataID( 0 );
//            _heroActor.Reset();
//            _heroActor.GetComponent<NavMeshAgent>().enabled = false;
//            OnHeroActorCreate( _heroActor );
//        }

//        #endregion


//        #region OnCreate
//        private void OnCreateTowerActor ( TowerActor tower )
//        {
//            //GameEntry.Timer.StartTick( 5f, ( elapsed ) =>
//            //{
//            //    if ( _heroActor != null )
//            //        tower.ProjectileTest( _heroActor.CachedTransform );
//            //} );
//            GameEntry.Timer.StartTick( 3f, ( elapsed ) =>
//              {
//                  Bullet( tower );
//              } );
//        }

//        private void OnHeroActorCreate ( TActorBase actor )
//        {
//            if (!( actor is HeroActor ))
//                return;

//            //EffectTest( actor as HeroActor );
//            //DamageNum( actor as HeroActor );
//            MoveActor( actor );
//        }
//        #endregion

//        private int _createTowerID = 0;
//        private int _createHeroID = 0;
//        private bool flag = false;
//        private HeroActor _heroActor = null;
//        private TowerActor _towerActor = null;

//        //public static readonly Vector3 _leftDownWorldPos = new Vector3( 60.4f, Config.GameConfig.Scene.SCENE_LAND_Y, 47.6f );
//        //public static readonly Vector3 _rightTopWorldPos = new Vector3( 25.1f, Config.GameConfig.Scene.SCENE_LAND_Y, 31.1f );

//        public static readonly Vector3 _leftDownWorldPos = new Vector3( 60.4f, 39f, 47.6f );
//        public static readonly Vector3 _rightTopWorldPos = new Vector3( 25.1f, 39f, 31.1f );
//    }


//}
