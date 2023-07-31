using Aquila.Module;
using UnityEngine;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 法球类actor
    /// </summary>
    public class Actor_Orb : Actor_Base
        //INavMoveBehavior,
        //ITriggerHitBehavior
    {
        //----------------------- pub -----------------------

        /// <summary>
        /// 设置目标并准备就绪
        /// </summary>
        public void SetTargetAndReady( Transform targetTransform )
        {
            
        }


        protected override void AddAddon()
        {
            base.AddAddon();
        }

        protected override void OnInit( object userData )
        {
            base.OnInit( userData );
        }

        public override ActorTypeEnum ActorType => ActorTypeEnum.Orb;
    }

    public class Actor_Orb_EntityData : EntityData
    {
        public Actor_Orb_EntityData( int entity_id ) : base( entity_id, typeof( Actor_Orb ).GetHashCode() )
        {
        }

        /// <summary>
        /// 目标actorID
        /// </summary>
        public int _targetActorID = -1;
    }
}