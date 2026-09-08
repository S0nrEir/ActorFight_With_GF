using Aquila.Module;
using Cfg.Enum;

namespace Aquila.Fight.Actor
{
    /// <summary>
    /// 集中管理 Actor 行为限制查询。
    /// </summary>
    public static class ActorActionRestrictionRules
    {
        public static bool IsCastBlocked(
            Module_ProxyActor.ActorInstance castor,
            out ActorTagType blockedMainType,
            out int blockedSubType)
        {
            blockedMainType = ActorTagType.Max;
            blockedSubType = (int)ActorTagSubType_Ability.None;

            if (castor == null || castor.Actor == null || GameEntry.Tag == null)
                return false;

            blockedMainType = ActorTagType.Ability;
            blockedSubType = (int)ActorTagSubType_Ability.Stun;

            return GameEntry.Tag.HasTag(
                castor.Actor.ActorID,
                ActorTagType.Ability,
                (ushort)ActorTagSubType_Ability.Stun);
        }
    }
}
