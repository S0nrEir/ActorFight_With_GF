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

            //todo:生成actor的接口改一下，现在拿到元数据了还要在里面根据id再拿一次，太蠢了
            var roleMeta = GameEntry.LuBan.Tables.RoleMeta.Get( Meta.ExtensionParam.IntParam_1 );
            if ( roleMeta is null )
            {
                Log.Warning( $"<color=yellow>EffectSpec_Instant_Summon.Apply--->role meta is null, role meta id:{Meta.ExtensionParam.IntParam_1}</color>" );
                return;
            }

            var roleType = roleMeta.RoleType;
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
            if ( roleType != Cfg.Enum.RoleType.Orb && roleType != Cfg.Enum.RoleType.Bullet )
            {
                Log.Warning( $"EffectSpec_Instance_Summon_Projectile.Apply()--->roleType != Cfg.Enum.RoleType.Orb || roleType != Cfg.Enum.RoleType.Bullet,roleType:{roleType}" );
                return;
            }
            await GameEntry.Module.GetModule<Module_Actor_Fac>().ShowActorAsync
                (
                    Tools.Actor.RoleTypeEnum2SystemType( roleType ),
                    roleMetaID,
                    entityID,
                    Tools.Actor.DefaultOrbAssetPath(),
                    entityData
                );
        }

    }
}
