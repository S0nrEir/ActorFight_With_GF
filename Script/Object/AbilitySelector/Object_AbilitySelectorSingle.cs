using System.Collections;
using System.Collections.Generic;
using Aquila.Combat;
using Aquila.Toolkit;
using Cfg.Enum;
using GameFramework;
using UnityEngine;

namespace Aquila.ObjectPool
{
    public class Object_AbilitySelectorSingle : Object_AbilitySelectorBase
    {
        protected override void OnConfirm()
        {
            if (!TryPickActor(out var actor) || !IsLegalTarget(actor))
            {
                Tools.Logger.Info(Tools.Fight.UsingAbilityFaildDescription_l10n((int)CastRejectFlags.TargetNotFound));
                ReleaseSelf();
                return;
            }

            SubmitAndRelease(CastCmd.CreateWithSingleTarget(_castorId, actor.Actor.ActorID, _abilityId));
        }

        public static Object_AbilitySelectorSingle Gen(string name, GameObject go)
        {
            var obj = ReferencePool.Acquire<Object_AbilitySelectorSingle>();
            obj.Initialize(name, go);
            return obj;
        }
    }

}
