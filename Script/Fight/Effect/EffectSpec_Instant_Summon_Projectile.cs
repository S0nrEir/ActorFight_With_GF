using Aquila.Event;
using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;
using UnityGameFramework.Runtime;

namespace Aquila.Fight
{
    /// <summary>
    /// 单个召唤物类型投射物 立即
    /// </summary>
    public class EffectSpec_Instant_Summon_Projectile : EffectSpec_Base
    {
        public override async void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target, AbilityResult_Hit result )
        {
            base.Apply( castor, target, result );
            if ( Meta.ExtensionParam.IntParam_1 < 0 )
            {
                Log.Warning( $"<color=yellow>EffectSpec_Instant_Summon.Apply--->Meta.ExtensionParam.IntParam_1 < 0 ,meta id:{Meta.id}</color>" );
                return;
            }

            var roleMetaID = Meta.ExtensionParam.IntParam_1;
            EntityData entityData;
            var entityID = ActorIDPool.Gen();
            //按照位置生成召唤物
            if ( Tools.GetBitValue( result._stateDescription, ( int ) AbilityHitResultTypeEnum.CONTAINS_POSITION ) )
            {
                entityData = new Actor_Bullet_EntityData( entityID )
                {
                    
                };
            }
            //按照目标生成召唤物
            else
            {
                entityData = new Actor_Orb_EntityData( entityID )
                {
                    _targetActorID = result._targetActorID,
                    _callerID = castor.Actor.ActorID,
                    _roleMetaID = roleMetaID,
                };
            }
            
            await GameEntry.Module.GetModule<Module_Actor_Fac>().ShowActorAsync( entityID, roleMetaID, entityData, entityID.ToString() );
        }

    }
}
