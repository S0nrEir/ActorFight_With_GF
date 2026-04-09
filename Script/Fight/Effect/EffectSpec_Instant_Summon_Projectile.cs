using Aquila.Event;
using Aquila.Fight.Actor;
using Aquila.Module;
using Aquila.Toolkit;

namespace Aquila.Fight
{
    /// <summary>
    /// 单个召唤物类型投射物 立即
    /// </summary>
    public class EffectSpec_Instant_Summon_Projectile : EffectSpec_Base
    {
        public override async void Apply( Module_ProxyActor.ActorInstance castor, Module_ProxyActor.ActorInstance target )
        {
            base.Apply( castor, target );
            if ( _effectData.GetIntParam1() < 0 )
            {
                Tools.Logger.Warning( $"<color=yellow>EffectSpec_Instant_Summon.Apply--->IntParam1 < 0, effectId:{_effectData.GetEffectId()}</color>" );
                return;
            }

            var roleMetaID = _effectData.GetIntParam1();
            EntityData entityData;
            var entityID = ActorIDPool.Gen();
            //无目标时按位置召唤，有目标时按目标召唤
            if ( target?.Actor == null )
            {
                entityData = new Actor_Bullet_EntityData( entityID );
            }
            else
            {
                entityData = new Actor_Orb_EntityData( entityID )
                {
                    _targetActorID = target.Actor.ActorID,
                    _callerID      = castor.Actor.ActorID,
                    _roleMetaID    = roleMetaID,
                };
            }

            await GameEntry.Module.GetModule<Module_Actor_Fac>().ShowActorAsync( entityID, roleMetaID, entityData, entityID.ToString() );
        }

    }
}