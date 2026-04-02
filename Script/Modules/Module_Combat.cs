using Aquila.Combat;
using Aquila.Extension;
using Aquila.Fight.Addon;
using Cfg.Enum;

namespace Aquila.Module
{
    public class Module_Combat : GameFrameworkModuleBase
    {
        public override void EnsureInit()
        {
            base.EnsureInit();
        }

        public override void Open(object param)
        {
            base.Open(param);
        }

        public override void Close()
        {
            base.Close();
        }

        /// <summary>
        /// 施法请求受理检查：仅做参数、目标、冷却和消耗校验，不执行施法。
        /// </summary>
        public CastAcceptResult RequestCast(CastCmd cmd)
        {
            if (cmd == null)
                return Reject(cmd, CastRejectCode.InvalidCmd, CastRejectFlags.InvalidCmd);

            if (cmd._castorInstanceId <= 0)
                return Reject(cmd, CastRejectCode.InvalidCastorId, CastRejectFlags.InvalidCastorId);

            if (cmd._abilityID <= 0)
                return Reject(cmd, CastRejectCode.InvalidAbilityId, CastRejectFlags.InvalidAbilityId);

            if (cmd._targetInstanceId <= 0)
                return Reject(cmd, CastRejectCode.TargetNotFound, CastRejectFlags.TargetNotFound);

            var actorMgr = GameEntry.Module.GetModule<Module_ActorMgr>();
            if (actorMgr == null)
                return Reject(cmd, CastRejectCode.Unknown, CastRejectFlags.None);

            var castor = actorMgr.Get(cmd._castorInstanceId);
            if (castor == null)
                return Reject(cmd, CastRejectCode.CastorNotFound, CastRejectFlags.CastorNotFound);

            var target = actorMgr.Get(cmd._targetInstanceId);
            if (target == null)
                return Reject(cmd, CastRejectCode.TargetNotFound, CastRejectFlags.TargetNotFound);

            var abilityAddon = castor.GetAddon<Addon_Ability>();
            if (abilityAddon == null)
                return Reject(cmd, CastRejectCode.MissingAbilityAddon, CastRejectFlags.MissingAbilityAddon);

            var canUse = (CastRejectCode)abilityAddon.CanUseAbility(cmd._abilityID);
            if (canUse != CastRejectCode.None)
                return Reject(cmd, canUse, MapCodeToFlag(canUse));

            return CastAcceptResult.Accept(cmd);
        }

        private static CastAcceptResult Reject(CastCmd cmd, CastRejectCode code, CastRejectFlags flags)
        {
            return CastAcceptResult.Reject(cmd, code, flags, BuildLegacyState(code, flags));
        }

        private static CastRejectFlags MapCodeToFlag(CastRejectCode code)
        {
            switch (code)
            {
                case CastRejectCode.InvalidCmd:
                    return CastRejectFlags.InvalidCmd;
                case CastRejectCode.InvalidCastorId:
                    return CastRejectFlags.InvalidCastorId;
                case CastRejectCode.InvalidAbilityId:
                    return CastRejectFlags.InvalidAbilityId;
                case CastRejectCode.CastorNotFound:
                    return CastRejectFlags.CastorNotFound;
                case CastRejectCode.TargetNotFound:
                    return CastRejectFlags.TargetNotFound;
                case CastRejectCode.MissingAbilityAddon:
                    return CastRejectFlags.MissingAbilityAddon;
                case CastRejectCode.AbilitySpecMissing:
                    return CastRejectFlags.AbilitySpecMissing;
                case CastRejectCode.CooldownNotReady:
                    return CastRejectFlags.CooldownNotReady;
                case CastRejectCode.CostNotEnough:
                    return CastRejectFlags.CostNotEnough;
                case CastRejectCode.UnsupportedTargetType:
                    return CastRejectFlags.UnsupportedTargetType;
                default:
                    return CastRejectFlags.None;
            }
        }

        private static int BuildLegacyState(CastRejectCode code, CastRejectFlags flags)
        {
            if (flags != CastRejectFlags.None)
                return (int)flags;

            return (int)code;
        }
    }
}