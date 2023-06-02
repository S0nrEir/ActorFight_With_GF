using Aquila.Fight.Actor;
using Aquila.Fight.Addon;
using Aquila.Module;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Aquila.Fight.Addon
{
    /// <summary>
    /// 信息面板组件
    /// </summary>
    public class Addon_InfoBoard : Addon_Base
    {
        public override AddonTypeEnum AddonType => AddonTypeEnum.INFO_BOARD;

        public override void OnAdd()
        {
        }

        public override void Init(Module_Proxy_Actor.ActorInstance instance)
        {
            base.Init(instance);
        }
    }
}
