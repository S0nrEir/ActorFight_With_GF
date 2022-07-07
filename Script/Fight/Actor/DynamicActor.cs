using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquila.Fight.Actor
{

    public abstract class DynamicActor : TActorBase
    {
        public override ActorTypeEnum ActorType => ActorTypeEnum.STATIC;

    }
}

