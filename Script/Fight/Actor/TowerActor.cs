using Aquila.Config;
using Aquila.Fight.Addon;
using Aquila.Fight.FSM;
using MRG.Fight.Addon;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 防御塔
    /// </summary>
    public class TowerActor : StaticActor,
        ISwitchStateBehavior,
        IDoAbilityBehavior,
        ITakeDamageBehavior,
        IDieBehavior
    {
        public async void ProjectileTest( Transform target )
        {
            //var entityID = ACTOR_ID_POOL.Gen();
            //var task = await AwaitableExtension.ShowEntity
            //    (
            //        Aquila.GameEntry.Entity,
            //        entityID,
            //        typeof( ProjectileActor ),
            //        @"Assets/Res_MS/Effects/prefab/TempProjectile.prefab",
            //        GameConfig.Entity.GROUP_Other,
            //        GameConfig.Entity.Priority_Actor,
            //        new ProjectileActorEntityData( entityID )
            //    );

            //if ( !OnShowProjectileSucc( task.Logic, target ) )
            //    Log.Error( $"TowerActor.DoAbilityAction---->CreateFaild!" );
        }

        #region private 

        /// <summary>
        /// 投射物创建回调Test
        /// </summary>
        private bool OnShowProjectileSucc( EntityLogic logic, Transform target )
        {
            //if ( target == null )
            //    return false;

            //var projectileActor = logic as ProjectileActor;
            //if ( projectileActor is null )
            //    return false;

            //projectileActor.SetWorldPosition( Vector3.Lerp( CachedTransform.position, target.position, .1f ) );
            //projectileActor.Setup( "TowerActor", -1, logic.Entity.Id, GlobalVar.INVALID_GUID, ForceType );
            //projectileActor.SetDataID( -1 );

            ////set
            //projectileActor.SetWorldPosition( Vector3.Lerp( CachedTransform.position, target.position, .1f ) );
            //var targetID = target.GetComponent<HeroActor>() != null ? target.GetComponent<HeroActor>().ActorID : -1;
            //projectileActor.SetTarget( target, targetID );

            return true;
        }

        /// <summary>
        /// 投射物创建回调
        /// </summary>
        private bool OnShowProjectileSucc()
        {
            return true;
        }
        #endregion

        public override ActorTypeEnum ActorType => ActorTypeEnum.Tower;
        protected override void InitAddons()
        {
            base.InitAddons();

            _FsmAddon = AddAddon<TowerStateAddon>();
            _HPSliderAddon = AddAddon<InfoBoardAddon>();
            _ProcessorAddon = AddAddon<ProcessorAddon>();
            _AnimAddon = AddAddon<AnimAddon>();
            _EffectAddon = AddAddon<EffectAddon>();
        }

        public override void Reset()
        {
            base.Reset();
        }

        protected override void ResetData()
        {
            base.ResetData();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnShow( object userData )
        {
            base.OnShow( userData );
        }

        protected override void OnHide( bool isShutdown, object userData )
        {
            base.OnHide( isShutdown, userData );
        }

        #region impl
        public void Die()
        {
            Debug.Log( $"<color=white>tower {ActorID} died!</color>" );

        }

        public async void DoAbilityAction(  )
        {
        }

        public void SwitchTo( ActorStateTypeEnum stateType, object[] enterParam, object[] existParam )
        {
            _FsmAddon.SwitchTo( stateType, enterParam, existParam );
        }

        public void TakeDamage( int dmg )
        {
            var currHp = _dataAddon.GetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, 0 );
            currHp -= dmg;
            //写入当前hp
            _dataAddon.SetIntDataValue( DataAddonFieldTypeEnum.INT_CURR_HP, currHp );
            if ( currHp <= 0 )
            {
                SwitchTo( ActorStateTypeEnum.DIE_STATE, null, null );
                return;
            }
        }


        #endregion

        private FSMAddon _FsmAddon = null;
        private InfoBoardAddon _HPSliderAddon = null;
        private ProcessorAddon _ProcessorAddon = null;
        private AnimAddon _AnimAddon = null;
        private EffectAddon _EffectAddon = null;
    }

    public class TowerActorEntityData : EntityData
    {
        public TowerActorEntityData( int entityId ) : base( entityId, typeof( TowerActor ).GetHashCode() )
        {
        }
    }
}
